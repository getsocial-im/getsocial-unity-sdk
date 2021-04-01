#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GetSocialSdk.Core
{
    internal class GetSocialStateController
    {
        public static THApplicationInfo Info;
        private readonly ILocalStorage _localStorage = Dependencies.GetLocalStorage();
        private readonly IMetaDataReader _metaDataReader = Dependencies.GetMetaDataReader();

        private string _sdkLanguage;
        private readonly List<ListenerWrapper> _onUserChangesListeners = new List<ListenerWrapper>();
        private int _counter = 0;
        public string SdkLanguage
        {
            get { return _sdkLanguage; }
            set { _sdkLanguage = Localization.ValidateLanguageCode(value); }
        }
        public List<THInviteProvider> InviteChannels;
        public readonly Stack<Action> OnInitializeListeners = new Stack<Action>();
        public CurrentUser User;

        public string SessionId { get; private set; }

        public bool IsInitialized { get; private set; }

        public long ServerTimeDiff { get; private set; }

        internal string UploadEndpoint { get; private set; }

        internal Action<ReferralData> ReferralDataListener;

        /// <summary>
        /// !IMPORTANT! Call from the UI thread only.
        /// </summary>
        public THSuperProperties SuperProperties
        {
            get
            {
                var props = new THSuperProperties
                {
                    SdkLanguage = SdkLanguage,
                    DeviceLanguage = Application.systemLanguage.ToLanguageCode(),
                    AppName = Application.productName,
                    SdkRuntime = THSdkRuntime.UNITY,
                    SdkRuntimeVersion = Application.unityVersion,
                    SdkWrapperVersion = BuildConfig.UnitySdkVersion,
                    SdkVersion = NativeBuildConfig.SdkVersion,
                    LocalTime = DateTime.Now.ToUnixTimestamp().ToString(),
                    DeviceTimezone = DateTime.Now.GetTimeZone(),
                    AppPackageName = Application.identifier,
                    AppVersionPublic = Application.version,
                    DeviceOs = SystemInfo.operatingSystemFamily.ToRpcModel(),
                    DeviceModel = SystemInfo.deviceModel, // probably would be just PC
                    DeviceOsVersion = SystemInfo.operatingSystem, // 'Windows <version> 64 bit', and if the CPU is 32-bit - 'Windows <version>'.
                    AppVersionInternal = Application.buildGUID,

                    // todo This one is implemented for Windows only, don't forget to implement in for Linux/Mac
                    DeviceIdfa = SystemInfo.deviceUniqueIdentifier, // details are here: https://docs.unity3d.com/ScriptReference/SystemInfo-deviceUniqueIdentifier.html
                    DeviceCarrier = "",
                    DeviceManufacturer = "",
                    DeviceIdfv = "",
                    DeviceNetworkType = "",
                    DeviceNetworkSubType = "",
                    DeviceJailbroken = false
                };

                return props;
            }
        }

        private string _appId;

        /// <summary>
        /// !IMPORTANT! Call from the UI thread only.
        /// </summary>
        public string AppId
        {
            get { return _appId ?? LoadSavedAppId() ?? LoadAppIdFromMetaData(); }
            private set
            {
                _appId = value;
                _localStorage.Set(LocalStorageKeys.AppId, value);
            }
        }

        public GetSocialStateController()
        {
            SdkLanguage = Application.systemLanguage.ToLanguageCode();
        }

        public void Uninitialize()
        {
            _localStorage.Delete(LocalStorageKeys.UserPassword, LocalStorageKeys.UserId);
            IsInitialized = false;
            User = null;
        }

        public void ClearUser()
        {
            _localStorage.Delete(LocalStorageKeys.AppId, LocalStorageKeys.UserId, LocalStorageKeys.UserPassword);
        }

        public void Initialized(THSdkAuthResponseAllInOne response, string appId)
        {
            Info = response.SdkAuthResponse.ApplicationInfo;
            ServerTimeDiff = response.SdkAuthResponse.ServerTime - DateTime.Now.ToUnixTimestamp();
            IsInitialized = true;
            AppId = appId;
            SaveSession(response.SdkAuthResponse.SessionId, response.SdkAuthResponse.User, response.SdkAuthResponse.UploadEndpoint);
            InviteChannels = response.InviteProviders.Providers;
            while (OnInitializeListeners.Any())
            {
                OnInitializeListeners.Pop().SafeCall();
            }
        }

        /// <summary>
        /// !IMPORTANT! Call from the UI thread only.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="user"></param>
        public void SaveSession(string sessionId, THPrivateUser user, string uploadEndpoint)
        {
            SessionId = sessionId;
            User = user.ToCurrentUser();

            _localStorage.Set(LocalStorageKeys.UserId, user.Id);
            _localStorage.Set(LocalStorageKeys.UserPassword, user.Password);

            foreach (var listener in _onUserChangesListeners)
            {
               listener.Listener.Invoke(User); 
            }

            UploadEndpoint = uploadEndpoint;
        }

        public UserCredentials LoadUserCredentials(string appId)
        {
            // check app id
            if (!appId.Equals(LoadSavedAppId()))
            {
                GetSocialLogs.W("Stored AppId [ " + LoadSavedAppId() + " ] is different than current one [" + appId + "]. New user will be created.");
                ClearUser();
                return new UserCredentials();
            }
            // check userId and password
            var userId = _localStorage.GetString(LocalStorageKeys.UserId);
            string password = _localStorage.GetString(LocalStorageKeys.UserPassword);
            if (userId == null || password == null)
            {
                GetSocialLogs.W("Invalid user credentials, userId = [" + userId + "], password = [ " + password + " ]. New user will be created.");
                ClearUser();
                return new UserCredentials();
            }
            return new UserCredentials
            {
                Id = userId,
                Password = password
            };
        }

        public string LoadAppIdFromMetaData()
        {
            return _metaDataReader.GetString(MetaDataKeys.AppId);
        }

        private string LoadSavedAppId()
        {
            return _localStorage.GetString(LocalStorageKeys.AppId);
        }

        public string AddOnUserChangesListener(OnCurrentUserChangedListener listener)
        {
            var key = (_counter++).ToString();
            _onUserChangesListeners.Add(new ListenerWrapper { Listener = listener, Key = key});
            return key;
        }

        public void RemoveOnUserChangedListener(string listenerId)
        {
            _onUserChangesListeners.RemoveAll(wrapper => wrapper.Key.Equals(listenerId));
        }
        private class ListenerWrapper
        {
            public OnCurrentUserChangedListener Listener { get; set; }
            public string Key { get; set; }
        }
    }

}
#endif

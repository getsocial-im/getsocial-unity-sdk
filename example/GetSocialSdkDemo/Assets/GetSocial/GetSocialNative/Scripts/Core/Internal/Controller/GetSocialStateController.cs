#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GetSocialSdk.Core
{
    internal class GetSocialStateController
    {
        private readonly ILocalStorage _localStorage = Dependencies.GetLocalStorage();
        private readonly IMetaDataReader _metaDataReader = Dependencies.GetMetaDataReader();

        private string _sdkLanguage;
        public string SdkLanguage
        {
            get { return _sdkLanguage; }
            set { _sdkLanguage = Localization.ValidateLanguageCode(value); }
        }
        public List<THInviteProvider> InviteChannels;
        public Action OnInit;
        public Action OnUserChanged;
        public Action<GetSocialError> OnError;
        public THPrivateUser User;

        public string SessionId { get; private set; }

        public bool IsInitialized { get; private set; }

        public long ServerTimeDiff { get; private set; }

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

        public bool IsNewInstall
        {
            get
            {
                const string isNewInstallKey = "is_new_install";
                var isNewInstall = _localStorage.GetString(isNewInstallKey) != null;
                if (isNewInstall)
                {
                    _localStorage.Set(isNewInstallKey, true.ToString().ToLower());
                }

                //TODO add system change when merged
                return isNewInstall;
            }
        }

        public GetSocialStateController()
        {
            SdkLanguage = Application.systemLanguage.ToLanguageCode();
        }

        public void ClearUser()
        {
            _localStorage.Delete(LocalStorageKeys.AppId, LocalStorageKeys.UserId, LocalStorageKeys.UserPassword);
        }

        public void Initialized(THSdkAuthResponseAllInOne response, string appId)
        {
            ServerTimeDiff = response.SdkAuthResponse.ServerTime - DateTime.Now.ToUnixTimestamp();
            IsInitialized = true;
            AppId = appId;
            SaveSession(response.SdkAuthResponse.SessionId, response.SdkAuthResponse.User);
            InviteChannels = response.InviteProviders.Providers;
            OnInit.SafeCall();
            OnInit = null;
        }

        /// <summary>
        /// !IMPORTANT! Call from the UI thread only.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="user"></param>
        public void SaveSession(string sessionId, THPrivateUser user)
        {
            SessionId = sessionId;
            User = user;

            _localStorage.Set(LocalStorageKeys.UserId, user.Id);
            _localStorage.Set(LocalStorageKeys.UserPassword, user.Password);

            OnUserChanged.SafeCall();
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
    }
}
#endif

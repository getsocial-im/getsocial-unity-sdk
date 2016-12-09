/**
 *     Copyright 2015-2016 GetSocial B.V.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#if UNITY_ANDROID

using System;
using UnityEngine;
using System.Collections.Generic;

namespace GetSocialSdk.Core
{
    sealed class GetSocialNativeBridgeAndroid : IGetSocialNativeBridge
    {
        private static GetSocialNativeBridgeAndroid instance;

        private AndroidJavaObject getSocialJavaObject;

        #region initialization
        private GetSocialNativeBridgeAndroid()
        {
            InitializeGetSocial();
        }

        public static IGetSocialNativeBridge GetInstance()
        {
            if(instance == null)
            {
                instance = new GetSocialNativeBridgeAndroid();
            }
            return instance;
        }
        #endregion

        #region IGetSocial implementation
        public void OnResume()
        {
            getSocialJavaObject.Call("onResume");
        }

        public void OnPause()
        {
            getSocialJavaObject.Call("onPause");
        }

        public bool IsInitialized
        {
            get { return getSocialJavaObject.Call<bool>("isInitialized"); }
        }

        #region current_user
        public string UserGuid
        {
            get
            {
                return getSocialJavaObject.Call<string>("getUserGuid");
            }
        }

        public string UserDisplayName
        {
            get
            {
                return getSocialJavaObject.Call<string>("getUserDisplayName");
            }
        }

        public string UserAvatarUrl
        {
            get
            {
                return getSocialJavaObject.Call<string>("getUserAvatarUrl");
            }
        }

        public bool IsUserAnonymous
        {
            get
            {
                return getSocialJavaObject.Call<bool>("isUserAnonymous");
            }
        }

        public string[] UserIdentities
        {
            get
            {
                return getSocialJavaObject.Call<string[]>("getUserIdentities");
            }
        }

        public bool UserHasIdentityForProvider(string provider)
        {
            return getSocialJavaObject.Call<bool>("userHasIdentityForProvider", provider); 
        }

        public string GetUserIdForProvider(string provider)
        {
            return getSocialJavaObject.Call<string>("getUserIdForProvider", provider);
        }

        public void SetDisplayName(string displayName, Action onSuccess, Action<string> onFailure)
        {
            getSocialJavaObject.Call("setUserDisplayName", displayName, new OperationVoidCallbackProxy(onSuccess, onFailure));
        }

        public void SetAvatarUrl(string avatarUrl, Action onSuccess, Action<string> onFailure)
        {
            getSocialJavaObject.Call("setUserAvatarUrl", avatarUrl, new OperationVoidCallbackProxy(onSuccess, onFailure));
        }

        public void AddUserIdentity(string serializedIdentity, Action<CurrentUser.AddIdentityResult> onComplete, Action<string> onFailure,
                                    CurrentUser.OnAddIdentityConflictDelegate onConflict)
        {
            getSocialJavaObject.Call("addUserIdentity", serializedIdentity, new AddUserIdentityObserverProxy(onComplete, onFailure, onConflict));
        }

        public void RemoveUserIdentity(string provider, Action onSuccess, Action<string> onFailure)
        {
            getSocialJavaObject.Call("removeUserIdentity", provider, new OperationVoidCallbackProxy(onSuccess, onFailure));
        }

        public void ResetUser(Action onSuccess, Action<string> onFailure)
        {
            getSocialJavaObject.Call("resetUser", new OperationVoidCallbackProxy(onSuccess, onFailure));
        }

        public void FollowUser(string guid, Action onSuccess, Action<string> onFailure)
        {
            getSocialJavaObject.Call("followUserByGuid", guid, new OperationVoidCallbackProxy(onSuccess, onFailure));
        }

        public void FollowUser(string provider, string id, Action onSuccess, Action<string> onFailure)
        {
            getSocialJavaObject.Call("followUserOnProvider", provider, id, new OperationVoidCallbackProxy(onSuccess, onFailure));
        }

        public void UnfollowUser(string guid, Action onSuccess, Action<string> onFailure)
        {
            getSocialJavaObject.Call("unfollowUserByGuid", guid, new OperationVoidCallbackProxy(onSuccess, onFailure));
        }

        public void UnfollowUser(string provider, string id, Action onSuccess, Action<string> onFailure)
        {
            getSocialJavaObject.Call("unfollowUserOnProvider", provider, id, new OperationVoidCallbackProxy(onSuccess, onFailure));
        }

        public void GetFollowing(int offset, int count, Action<List<User>> onSuccess, Action<string> onFailure)
        {
            getSocialJavaObject.Call("getFollowing", offset, count, 
                new OperationCallbackProxy<List<User>>(onSuccess, onFailure, ParseUtils.ParseUserList));
        }

        public void GetFollowers(int offset, int count, Action<List<User>> onSuccess, Action<string> onFailure)
        {
            getSocialJavaObject.Call("getFollowers", offset, count, 
                new OperationCallbackProxy<List<User>>(onSuccess, onFailure, ParseUtils.ParseUserList));
        }
        #endregion

        public IConfiguration Configuration
        {
            get { return ConfigurationAndroid.GetInstance(getSocialJavaObject); }
        }

        public int UnreadNotificationsCount
        {
            get { return getSocialJavaObject.Call<int>("getNumberOfUnreadNotifications"); }
        }

        public string Version
        {
            get { return getSocialJavaObject.Call<string>("getVersion"); }
        }

        public string ApiVersion
        {
            get { return getSocialJavaObject.Call<string>("getApiVersion"); }
        }

        public string Environment
        {
            get { return getSocialJavaObject.Call<string>("getEnvironment"); }
        }

        public void Init(string key, Action onSuccess, Action onFailure = null)
        {
            getSocialJavaObject.Call("init", key, new OperationVoidCallbackProxy(
                () =>
                {
                    if(onSuccess != null)
                    {
                        InstantiateUnityLifecycleHelper(); // Start listening onResume/onPause
                        onSuccess();
                    }
                },
                error =>
                {
                    if(onFailure != null)
                    {
                        onFailure();
                    }
                }
            ));
        }

        // Start listening for Android onPause/onResume
        private void InstantiateUnityLifecycleHelper()
        {
            // Just in case
            var aliveLifecycleHelpers = UnityEngine.Object.FindObjectsOfType<UnityLifecycleHelper>();
            for(int i = 0; i < aliveLifecycleHelpers.Length; i++)
            {
                UnityEngine.Object.Destroy(aliveLifecycleHelpers[i]);
            }

            if(Debug.isDebugBuild)
            {
                Debug.Log("Instantiating " + typeof(UnityLifecycleHelper).Name + " to listen onPause/onResume callbacks");
            }
            
            var go = new GameObject { name = "GetSocialAndroidUnityLifecycleHelper" };
            var lifecycleHelper = go.AddComponent<UnityLifecycleHelper>();
            lifecycleHelper.Init(OnPause, OnResume);
            
            UnityEngine.Object.DontDestroyOnLoad(lifecycleHelper);
        }

        public void RegisterPlugin(string providerId, IPlugin plugin)
        {
            if(plugin is IInvitePlugin)
            {
                IInvitePlugin invitePlugin = (IInvitePlugin)plugin;
                getSocialJavaObject.Call("registerInvitePlugin", providerId, new UnityInvitePluginProxy(invitePlugin));
            }
            else
            {
                Debug.LogWarning("For now, GetSocial supports only Invite plugins");
            }
        }

        public void ShowView(string serializedViewBuilder, ViewBuilder.OnViewActionDelegate onViewAction = null)
        {
            Debug.LogWarning("ShowView: " + onViewAction);
            getSocialJavaObject.Call("showSerializedView", serializedViewBuilder,
                new ViewBuilderActionObserverProxy(onViewAction));
        }

        public void CloseView(bool saveViewState)
        {
            getSocialJavaObject.Call("closeView", saveViewState);
        }

        public void RestoreView()
        {
            getSocialJavaObject.Call("restoreView");
        }

        public void Save(string state, Action onSuccess = null, Action<string> onFailure = null)
        {
            getSocialJavaObject.Call("save", state);
            if(onSuccess != null)
            {
                onSuccess();
            }
        }

        public void GetLastSave(Action<string> onSuccess, Action<string> onFailure = null)
        {
            getSocialJavaObject.Call("getLastSave", new OperationStringCallbackProxy(onSuccess, onFailure));
        }

        public void PostActivity(string text, byte[] image, string buttonText, string actionId, string[] tags,
                                 Action<string> onSuccess, Action onFailure)
        {
            var tagsString = AndroidUtils.MergeActivityTags(tags);
            getSocialJavaObject.Call("postActivity", text, image, buttonText, actionId, tagsString,
                new OperationStringCallbackProxy(onSuccess, error => onFailure()));
        }

        public void SetOnWindowStateChangeListener(Action onOpen, Action onClose)
        {
            getSocialJavaObject.Call("setOnWindowStateChangeListener",
                new OnWindowStateChangeListenerProxy(onOpen, onClose));
        }

        public void SetOnInviteFriendsListener(Action onInviteFriendsIntent, Action<int> onFriendsInvited)
        {
            getSocialJavaObject.Call("setOnInviteFriendsListener",
                new InviteFriendsListenerProxy(onInviteFriendsIntent, onFriendsInvited));
        }

        public void SetOnUserActionPerformedListener(OnUserActionPerformed onUserActionPerformed)
        {
            getSocialJavaObject.Call("setOnActionPerformListener",
                new OnUserActionPerformedListenerProxy(onUserActionPerformed));
        }

        public void SetOnUserAvatarClickListener(OnUserAvatarClick onUserAvatarClick)
        {
            getSocialJavaObject.Call("setOnUserAvatarClickHandler",
                new OnUserAvatarClickListenerProxy(onUserAvatarClick));
        }

        public void SetOnAppAvatarClickListener(OnAppAvatarClick onAppAvatarClick)
        {
            getSocialJavaObject.Call("setOnAppAvatarClickHandler", new OnAppAvatarClickHandlerProxy(onAppAvatarClick));
        }

        public void SetOnActivityActionClickListener(OnActivityActionClick onActivityActionClick)
        {
            getSocialJavaObject.Call("setOnActivityActionClickListener",
                new OnActivityActionClickHandlerProxy(onActivityActionClick));
        }

        public void SetOnInviteButtonClickListener(OnInviteButtonClick onInviteButtonClick)
        {
            getSocialJavaObject.Call("setOnInviteButtonClickListener",
                new OnInviteButtonClickHandlerProxy(onInviteButtonClick));
        }

        public void SetOnUserGeneratedContentListener(OnUserGeneratedContent onUserGeneratedContent)
        {
            getSocialJavaObject.Call("setOnUserGeneratedContentListener",
                new OnUserGeneratedContentListenerProxy(onUserGeneratedContent));
        }

        public void SetOnReferralDataReceivedListener(OnReferralDataReceived onReferralDataReceived)
        {
            getSocialJavaObject.Call("setOnReferralDataReceivedListener",
                new OnReferralDataReceivedHandlerProxy(onReferralDataReceived));
        }

        public void SetOnUnreadNotificationsCountChangeListener(Action<int> onUnreadNotificationsCountChange)
        {
            getSocialJavaObject.Call("setOnUnreadNotificationsCountChangeListener",
                new OnUnreadNotificationsCountChangedListenerProxy(onUnreadNotificationsCountChange));
        }

        public void SetLanguage(string languageCode)
        {
            getSocialJavaObject.Call("setLanguage", languageCode);
        }

        public string[] GetSupportedInviteProviders()
        {
            return getSocialJavaObject.Call<string[]>("getSupportedInviteProviders");
        }

        public void InviteFriendsUsingProvider(string provider, string subject = null, string text = null,
                                               byte[] image = null, IDictionary<string, string> referralData = null)
        {
            string referralDataJson = null;

            if(referralData != null)
            {
                referralDataJson = new JSONObject(referralData).ToString();
            }
            getSocialJavaObject.Call("inviteFriendsUsingProvider", provider, subject, text, image, referralDataJson);
        }

        public void GetLeaderboard(string leaderboardId, Action<string> onSuccess, Action<string> onFailure = null)
        {
            getSocialJavaObject.Call("getLeaderboard", leaderboardId, new OperationStringCallbackProxy(onSuccess, onFailure));
        }

        public void GetLeaderboards(HashSet<string> leaderboardIds, Action<string> onSuccess,
                                    Action<string> onFailure = null)
        {
            var javaList = AndroidUtils.ConvertToArrayList(new List<string>(leaderboardIds));
            getSocialJavaObject.Call("getLeaderboards", javaList, new OperationStringCallbackProxy(onSuccess, onFailure));
        }

        public void GetLeaderboards(int offset, int count, Action<string> onSuccess, Action<string> onFailure = null)
        {
            getSocialJavaObject.Call("getLeaderboards", offset, count, new OperationStringCallbackProxy(onSuccess, onFailure));
        }

        public void GetLeaderboardScores(string leaderboardId, int offset, int count, LeaderboardScoreType scoreType,
                                         Action<string> onSuccess, Action<string> onFailure = null)
        {
            getSocialJavaObject.Call("getLeaderboardScores", leaderboardId, offset, count, (int)scoreType,
                new OperationStringCallbackProxy(onSuccess, onFailure));
        }

        public void SubmitLeaderboardScore(string leaderboardId, int score, Action<int> onSuccess = null,
                                           Action onFailure = null)
        {
            getSocialJavaObject.Call("submitLeaderboardScore", leaderboardId, score,
                new SubmitLeaderboardScoreListenerProxy(onSuccess, onFailure));
        }

        #endregion

        #region private methods
        private void InitializeGetSocial()
        {
            using(AndroidJavaObject clazz = new AndroidJavaClass("im.getsocial.sdk.core.unity.GetSocialUnityBridge"))
            {
                getSocialJavaObject = clazz.CallStatic<AndroidJavaObject>("initBridge");
            }

            if(getSocialJavaObject.IsJavaNull())
            {
                throw new Exception("Failed to instantiate Android GetSocial SDK");
            }

            if(!AndroidUtils.IsUnityBuildOfGetSocialAndroidSDK())
            {
                throw new Exception(
                    "Wrong version of GetSocial Android SDK is included into the build. BuildConfig.TARGET_PLATFORM != \"UNITY\"");
            }

            // Call OnResume manually first time
            OnResume();
            MainThreadExecutor.Init();
        }
        #endregion
    }
}

#endif

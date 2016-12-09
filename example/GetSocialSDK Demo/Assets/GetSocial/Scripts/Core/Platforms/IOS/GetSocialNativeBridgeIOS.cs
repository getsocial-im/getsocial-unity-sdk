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

#if UNITY_IOS
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GetSocialSdk.Core
{
    class GetSocialNativeBridgeIOS : IGetSocialNativeBridge
    {
        public delegate bool IsAvailableForDeviceDelegate();

        private static IGetSocialNativeBridge instance;

        #region initialization

        private GetSocialNativeBridgeIOS()
        {
        }

        public static IGetSocialNativeBridge GetInstance()
        {
            if(instance == null)
            {
                instance = new GetSocialNativeBridgeIOS();
            }

            MainThreadExecutor.Init();

            return instance;
        }

        #endregion

        public bool IsInitialized
        {
            get { return _isInitialized(); }
        }

        public IConfiguration Configuration
        {
            get { return ConfigurationIOS.GetInstance(); }
        }

        public int UnreadNotificationsCount
        {
            get { return _getUnreadNotificationsCount(); }
        }

        public string Version
        {
            get { return _getVersion(); }
        }

        public string ApiVersion
        {
            get { return _getApiVersion(); }
        }

        public string Environment
        {
            get { return _getEnvironment(); }
        }

        #region current_user

        public string UserGuid
        {
            get { return _getUserGuid(); }
        }

        public string UserDisplayName
        {
            get { return _getUserDisplayName(); }
        }

        public string UserAvatarUrl { get { return _getUserAvatarUrl(); } }

        public bool IsUserAnonymous { get { return _isUserAnonymous(); } }

        public string[] UserIdentities
        {
            get
            {
                // TODO Replace with JSON array serialization
                var providers = _getUserIdentities();
                if(string.IsNullOrEmpty(providers))
                {
                    return new string[] { };
                }
                return providers.Split(',');
            }
        }

        public bool UserHasIdentityForProvider(string provider)
        {
            return _userHasIdentityForProvider(provider);
        }

        public string GetUserIdForProvider(string provider)
        {
            return _userHasIdentityForProvider(provider) ? _getUserIdForProvider(provider) : null;
        }

        public void SetDisplayName(string displayName, Action onSuccess, Action<string> onFailure)
        {
            _setUserDisplayName(displayName, onSuccess.GetPointer(), onFailure.GetPointer(), CompleteCallback, FailureCallback);
        }

        public void SetAvatarUrl(string avatarUrl, Action onSuccess, Action<string> onFailure)
        {
            _setUserAvatarUrl(avatarUrl, onSuccess.GetPointer(), onFailure.GetPointer(), CompleteCallback, FailureCallback);
        }

        public void AddUserIdentity(string serializedIdentity, Action<CurrentUser.AddIdentityResult> onComplete, Action<string> onFailure,
                               CurrentUser.OnAddIdentityConflictDelegate onConflict)
        {
            _addUserIdentity(serializedIdentity,
                onComplete.GetPointer(),
                onFailure.GetPointer(),
                onConflict.GetPointer(),
                OnAddUserIdentityCompleteProxy.OnAddUserIdentityComplete,
                FailureCallback,
                OnAddUserIdentityConflictProxy.OnUserIdentityConflict);
        }

        public void RemoveUserIdentity(string provider, Action onSuccess, Action<string> onFailure)
        {
            _removeUserIdentity(provider, onSuccess.GetPointer(), onFailure.GetPointer(), CompleteCallback,
                FailureCallback);
        }

        public void ResetUser(Action onSuccess, Action<string> onFailure)
        {
            _resetUser(onSuccess.GetPointer(), onFailure.GetPointer(), CompleteCallback, FailureCallback);
        }

        public void FollowUser(String guid, Action onSuccess, Action<string> onFailure)
        {
            _followUser(guid, onSuccess.GetPointer(), onFailure.GetPointer(), CompleteCallback, FailureCallback);
        }

        public void FollowUser(string provider, string userId, Action onSuccess, Action<string> onFailure)
        {
            _followUserOnProvider(provider, userId, onSuccess.GetPointer(), onFailure.GetPointer(), CompleteCallback, FailureCallback);
        }

        public void UnfollowUser(String guid, Action onSuccess, Action<string> onFailure)
        {
            _unfollowUser(guid, onSuccess.GetPointer(), onFailure.GetPointer(), CompleteCallback, FailureCallback);
        }

        public void UnfollowUser(string provider, string userId, Action onSuccess, Action<string> onFailure)
        {
            _unfollowUserOnProvider(provider, userId, onSuccess.GetPointer(), onFailure.GetPointer(), CompleteCallback, FailureCallback);
        }

        public void GetFollowing(int offset, int count, Action<List<User>> onSuccess, Action<string> onFailure)
        {
            _getFollowingUsers(offset, count, onSuccess.GetPointer(), onFailure.GetPointer(), UserListCallback.OnUserListReceived, FailureCallback);
        }

        public void GetFollowers(int offset, int count, Action<List<User>> onSuccess, Action<string> onFailure)
        {
            _getFollowerUsers(offset, count, onSuccess.GetPointer(), onFailure.GetPointer(), UserListCallback.OnUserListReceived, FailureCallback);
        }

        #endregion

        public void Init(string key, Action onSuccess = null, Action onFailure = null)
        {
            if(!GetSocialSettings.IsAutoRegisrationForPushesEnabledIOS)
            {
                _disableAutoRegistrationForPushNotifications();
            }
            _initWithKey(key, CompleteCallback, onSuccess.GetPointer(), onFailure.GetPointer());
        }

        public void RegisterPlugin(string providerId, IPlugin plugin)
        {
            if(plugin is IInvitePlugin)
            {
                _registerInvitePlugin(providerId, plugin.GetPointer(), UnityInvitePluginProxy.InviteFriends,
                    UnityPluginProxy.IsAvailableForDevice);
            }
            else
            {
                Debug.LogWarning("For now, GetSocial support only Invite plugins");
            }
        }

        public void ShowView(string serializedViewBuilder, ViewBuilder.OnViewActionDelegate onViewAction = null)
        {
            _showView(serializedViewBuilder, onViewAction.GetPointer(), OnViewActionProxy.OnViewBuilderActionCallback);
        }

        public void CloseView(bool saveViewState)
        {
            _closeView(saveViewState);
        }

        public void RestoreView()
        {
            _restoreView();
        }

        public void Save(string state, Action onSuccess = null, Action<string> onFailure = null)
        {
            _save(state, onSuccess.GetPointer(), onFailure.GetPointer(), CompleteCallback, FailureCallback);
        }

        public void GetLastSave(Action<string> onSuccess, Action<string> onFailure)
        {
            _getLastSave(onSuccess.GetPointer(), onFailure.GetPointer(), StringResultCallaback, FailureCallback);
        }

        public void PostActivity(string text, byte[] image, string buttonText, string actionId, string[] tags,
                            Action<string> onSuccess, Action onFailure)
        {
            var tagsCount = tags != null ? tags.Length : 0;
            var base64image = image != null ? Convert.ToBase64String(image) : null;

            _postActivity(text, base64image, buttonText, actionId, tagsCount, tags, onSuccess.GetPointer(),
                onFailure.GetPointer(), StringResultCallaback, CompleteCallback);
        }

        public void SetOnWindowStateChangeListener(Action onOpen, Action onClose)
        {
            Action<bool> onWindowStateChangedAction = (isOpen) =>
            {
                if(isOpen)
                {
                    onOpen();
                }
                else
                {
                    onClose();
                }
            };
            _setOnWindowStateChangeListener(onWindowStateChangedAction.GetPointer(), OnWindowStateChangeListenerProxy.OnWindowStateChange);
        }

        public void SetOnInviteFriendsListener(Action onInviteFriendsIntent, Action<int> onFriendsInvited)
        {
            _setOnInviteFriendsListener(onInviteFriendsIntent.GetPointer(), onFriendsInvited.GetPointer(),
                InviteFriendsListenerProxy.OnInviteFriendsIntent, InviteFriendsListenerProxy.OnFriendsInvited);
        }

        public void SetOnUserActionPerformedListener(OnUserActionPerformed onUserActionPerformed)
        {
            _setOnUserActionPerformedListener(onUserActionPerformed.GetPointer(), OnUserActionPerformedListenerProxy.OnUserActionPerformedCallback);
        }

        public void SetOnUserAvatarClickListener(OnUserAvatarClick onUserAvatarClick)
        {
            _setOnUserAvatarClickListener(onUserAvatarClick.GetPointer(),
                OnUserAvatarClickProxy.OnUserAvatarClickCallback);
        }

        public void SetOnAppAvatarClickListener(OnAppAvatarClick onAppAvatarClick)
        {
            _setOnAppAvatarClickListener(onAppAvatarClick.GetPointer(),
                OnAppAvatarClickListenerProxy.OnAppAvatarClickCallback);
        }

        public void SetOnActivityActionClickListener(OnActivityActionClick onActivityActionClick)
        {
            _setOnActivityActionClickListener(onActivityActionClick.GetPointer(),
                OnActivityActionClickListenerProxy.OnActivityActionClickCallback);
        }

        public void SetOnInviteButtonClickListener(OnInviteButtonClick onInviteButtonClick)
        {
            _setOnInviteButtonClickListener(onInviteButtonClick.GetPointer(),
                OnInviteButtonClickListenerProxy.OnInviteButtonClickCallback);
        }

        public void SetOnUserGeneratedContentListener(OnUserGeneratedContent onUserGeneratedContent)
        {
            _setOnUserGeneratedContentListener(onUserGeneratedContent.GetPointer(),
                OnUserGeneratedContentListenerProxy.OnUserGeneratedContentCallback);
        }

        public void SetOnReferralDataReceivedListener(OnReferralDataReceived onReferralDataReceived)
        {
            _setOnReferralDataReceivedListener(onReferralDataReceived.GetPointer(),
                OnReferralDataReceivedListenerProxy.OnReferralDataReceivedCallback);
        }

        public void SetOnUnreadNotificationsCountChangeListener(Action<int> onUnreadNotificationsCountChange)
        {
            _setOnUnreadNotificationsCountChangeListener(onUnreadNotificationsCountChange.GetPointer(),
                OnUnreadNotificationsCountChangedListenerProxy.OnUnreadNotificationsCountChange);
        }

        public void SetLanguage(string languageCode)
        {
            _setLanguage(languageCode);
        }

        public string[] GetSupportedInviteProviders()
        {
            // TODO Replace with JSON array serialization
            var providersString = _getSupportedInviteProviders();
            if(string.IsNullOrEmpty(providersString))
            {
                return new string[] { };
            }
            return providersString.Split(',');
        }

        public void InviteFriendsUsingProvider(string provider, String subject = null, string text = null,
                                          byte[] image = null, IDictionary<string, string> referralData = null)
        {
            var base64image = image != null ? Convert.ToBase64String(image) : null;
            string referralDataJSON = null;

            if(referralData != null)
            {
                referralDataJSON = new JSONObject(referralData).ToString();
            }
            _inviteFriendsUsingProvider(provider, subject, text, base64image, referralDataJSON);
        }

        public void GetLeaderboard(string leaderboardId, Action<string> onSuccess, Action<string> onFailure = null)
        {
            _getLeaderboard(leaderboardId, onSuccess.GetPointer(), onFailure.GetPointer(), StringResultCallaback);
        }

        public void GetLeaderboards(HashSet<string> leaderboardIds, Action<string> onSuccess,
                               Action<string> onFailure = null)
        {
            var leaderboardIdsArray = new string[leaderboardIds.Count];
            leaderboardIds.CopyTo(leaderboardIdsArray);

            _getLeaderboards(leaderboardIdsArray.Length, leaderboardIdsArray, onSuccess.GetPointer(),
                onFailure.GetPointer(), StringResultCallaback);
        }

        public void GetLeaderboards(int offset, int count, Action<string> onSuccess, Action<string> onFailure = null)
        {
            _getLeaderboardsWithOffset(offset, count, onSuccess.GetPointer(), onFailure.GetPointer(),
                StringResultCallaback);
        }

        public void GetLeaderboardScores(string leaderboardId, int offset, int count, LeaderboardScoreType scoreType,
                                    Action<string> onSuccess, Action<string> onFailure = null)
        {
            _getLeaderboardScores(leaderboardId, offset, count, (int)scoreType, onSuccess.GetPointer(),
                onFailure.GetPointer(), StringResultCallaback);
        }

        public void SubmitLeaderboardScore(string leaderboardId, int score, Action<int> onSuccess, Action onFailure)
        {
            _submitLeaderboardScore(leaderboardId, score, onSuccess.GetPointer(), onFailure.GetPointer(),
                SubmitLeaderboardScoreListenerProxy.OnRankChange, CompleteCallback);
        }

        #region init_internal

        [DllImport("__Internal")]
        private static extern void _initWithKey(string key, CompleteCallbackDelegate actionExecutor,
                                           IntPtr onSuccessActionPtr, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        private static extern bool _isInitialized();

        [DllImport("__Internal")]
        private static extern void _disableAutoRegistrationForPushNotifications();

        #endregion

        [DllImport("__Internal")]
        private static extern int _getUnreadNotificationsCount();

        [DllImport("__Internal")]
        private static extern string _getVersion();

        [DllImport("__Internal")]
        private static extern string _getApiVersion();

        [DllImport("__Internal")]
        private static extern string _getEnvironment();

        [DllImport("__Internal")]
        private static extern void _registerInvitePlugin(string providerId, IntPtr pluginPtr,
                                                    UnityInvitePluginProxy.InviteFriendsDelegate inviteFriends,
                                                    UnityPluginProxy.IsAvailableForDeviceDelegate isAvailableForDevice);

        [DllImport("__Internal")]
        private static extern void _showView(string serializedViewBuilder, IntPtr onViewActionPtr,
                                        OnViewActionProxy.ExecuteViewActionDelegate executeViewAction);

        [DllImport("__Internal")]
        private static extern void _closeView(bool saveViewState);

        [DllImport("__Internal")]
        private static extern void _restoreView();

        #region current_user_internal

        [DllImport("__Internal")]
        private static extern string _getUserGuid();

        [DllImport("__Internal")]
        private static extern string _getUserDisplayName();

        [DllImport("__Internal")]
        private static extern void _setUserDisplayName(string displayName, IntPtr onSuccessPtr, IntPtr onFailurePtr,
                                                  CompleteCallbackDelegate executeCompleteAction, FailureCallbackDelegate failureCallback);

        [DllImport("__Internal")]
        private static extern string _getUserAvatarUrl();

        [DllImport("__Internal")]
        private static extern void _setUserAvatarUrl(string avatarUrl, IntPtr onSuccessPtr, IntPtr onFailurePtr,
                                                CompleteCallbackDelegate executeCompleteAction, FailureCallbackDelegate failureCallback);

        [DllImport("__Internal")]
        private static extern bool _isUserAnonymous();

        [DllImport("__Internal")]
        private static extern string _getUserIdentities();

        [DllImport("__Internal")]
        private static extern bool _userHasIdentityForProvider(string provider);

        [DllImport("__Internal")]
        private static extern string _getUserIdForProvider(string provider);

        [DllImport("__Internal")]
        private static extern void _addUserIdentity(string serializedIdentityInfo,
                                               IntPtr onCompletePtr,
                                               IntPtr onFailurePtr,
                                               IntPtr onConflictPtr,
                                               OnAddUserIdentityCompleteProxy.OnAddUserIdentityCompleteDelegate onAddUserIdentityComplete,
                                               FailureCallbackDelegate failureCallback,
                                               OnAddUserIdentityConflictProxy.OnAddUserIdentityConflictDelegate onConflict);

        [DllImport("__Internal")]
        private static extern void _removeUserIdentity(string providerId, IntPtr onSuccessPtr, IntPtr onFailurePtr,
                                                  CompleteCallbackDelegate completeCallback, FailureCallbackDelegate failureCallback);

        [DllImport("__Internal")]
        private static extern void _resetUser(IntPtr onSuccessPtr, IntPtr onFailurePtr,
                                         CompleteCallbackDelegate completeCallback, FailureCallbackDelegate failureCallback);

        // follow-unfollow
        [DllImport("__Internal")]
        private static extern void _followUser(string guid, IntPtr onSuccessPtr, IntPtr onFailurePtr,
                                          CompleteCallbackDelegate completeCallback, FailureCallbackDelegate failureCallback);

        [DllImport("__Internal")]
        private static extern void _followUserOnProvider(string provider, string userId, IntPtr onSuccessPtr, IntPtr onFailurePtr,
                                                    CompleteCallbackDelegate completeCallback, FailureCallbackDelegate failureCallback);

        [DllImport("__Internal")]
        private static extern void _unfollowUser(string guid, IntPtr onSuccessPtr, IntPtr onFailurePtr,
                                            CompleteCallbackDelegate completeCallback, FailureCallbackDelegate failureCallback);

        [DllImport("__Internal")]
        private static extern void _unfollowUserOnProvider(string provider, string userId, IntPtr onSuccessPtr, IntPtr onFailurePtr,
                                                      CompleteCallbackDelegate completeCallback, FailureCallbackDelegate failureCallback);

        // get users
        [DllImport("__Internal")]
        private static extern void _getFollowingUsers(int offset, int count, IntPtr onSuccessPtr, IntPtr onFailurePtr,
                                                 UserListCallback.GetUserListCallbackDelegate getUserListCallback, FailureCallbackDelegate failureCallback);

        [DllImport("__Internal")]
        private static extern void _getFollowerUsers(int offset, int count, IntPtr onSuccessPtr, IntPtr onFailurePtr,
                                                UserListCallback.GetUserListCallbackDelegate getUserListCallback, FailureCallbackDelegate failureCallback);

        #endregion

        [DllImport("__Internal")]
        private static extern void _save(string state, IntPtr onSuccessPtr, IntPtr onFailurePtr,
                                    CompleteCallbackDelegate executeSuccessAction, FailureCallbackDelegate failureCallback);

        [DllImport("__Internal")]
        private static extern void _getLastSave(IntPtr onSuccessPtr, IntPtr onFailurePtr,
                                           StringResultCallbackDelegate executeSuccessAction, FailureCallbackDelegate executeCompleteAction);

        [DllImport("__Internal")]
        private static extern void _postActivity(string text, string base64image, string buttonText, string actionId,
                                            int tagsCount, string[] tags, IntPtr onSuccessPtr, IntPtr onFailurePtr,
                                            StringResultCallbackDelegate executeSuccessAction, CompleteCallbackDelegate executeCompleteAction);

        [DllImport("__Internal")]
        private static extern void _setOnWindowStateChangeListener(IntPtr onWindowStateChangedPtr,
                                                              OnWindowStateChangeListenerProxy.OnWindowStateChangeDelegate onWIndowStateChange);

        [DllImport("__Internal")]
        private static extern void _setOnInviteFriendsListener(IntPtr onInviteFriendsIntentPtr,
                                                          IntPtr onFriendsInvitedPtr, InviteFriendsListenerProxy.OnInviteFriendsIntentDelegate onInviteFriendsIntent,
                                                          InviteFriendsListenerProxy.OnFriendsInvitedDelegate onFriendsInvited);

        [DllImport("__Internal")]
        private static extern void _setOnUserActionPerformedListener(IntPtr onUserActionPerformedPtr, OnUserActionPerformedListenerProxy.OnUserActionPerformedDelegate onUserActionPerformedCallback);

        [DllImport("__Internal")]
        private static extern void _setOnUserAvatarClickListener(IntPtr onUserAvatarClickPtr,
                                                            OnUserAvatarClickProxy.OnUserAvatarClickDelegate onUserAvatarClick);

        [DllImport("__Internal")]
        private static extern void _setOnAppAvatarClickListener(IntPtr onGameAvatarClickPtr,
                                                           OnAppAvatarClickListenerProxy.OnAppAvatarClickDelegate onGameAvatarClick);

        [DllImport("__Internal")]
        private static extern void _setOnActivityActionClickListener(IntPtr onActivityActionClickPtr,
                                                                OnActivityActionClickListenerProxy.OnActivityActionClickDelegate onActivityActionClick);

        [DllImport("__Internal")]
        private static extern void _setOnInviteButtonClickListener(IntPtr onInviteButtonClickPtr,
                                                              OnInviteButtonClickListenerProxy.OnInviteButtonClickDelegate onInviteButtonClick);

        [DllImport("__Internal")]
        private static extern void _setOnUserGeneratedContentListener(IntPtr onUserGeneratedContent,
                                                                 OnUserGeneratedContentListenerProxy.OnUserGeneratedContentDelegate onUserGeneratedContentCallback);

        [DllImport("__Internal")]
        private static extern void _setOnReferralDataReceivedListener(IntPtr onReferralDataReceivedPtr,
                                                                 OnReferralDataReceivedListenerProxy.OnReferralDataReceivedDelegate onReferralDataReceived);

        [DllImport("__Internal")]
        private static extern void _setOnUnreadNotificationsCountChangeListener(IntPtr onOpenProfilePtr,
                                                                           OnUnreadNotificationsCountChangedListenerProxy.OnUnreadNotificationsCountChangedListenerDelegate
                onUnreadNotificationCountChange);

        [DllImport("__Internal")]
        private static extern void _setLanguage(string languageCode);

        [DllImport("__Internal")]
        private static extern string _getSupportedInviteProviders();

        [DllImport("__Internal")]
        private static extern void _inviteFriendsUsingProvider(string provider, string subject, string text,
                                                          string base64image, string referralDataJSON);

        #region leaderboards_internal

        [DllImport("__Internal")]
        private static extern void _getLeaderboard(string leaderboardId, IntPtr onSuccessPtr, IntPtr onFailurePtr,
                                              StringResultCallbackDelegate callback);

        [DllImport("__Internal")]
        private static extern void _getLeaderboards(int leaderboardsCount, string[] leaderboardIdsArray,
                                               IntPtr onSuccessPtr, IntPtr onFailurePtr, StringResultCallbackDelegate callback);

        [DllImport("__Internal")]
        private static extern void _getLeaderboardsWithOffset(int offset, int count, IntPtr onSuccessPtr,
                                                         IntPtr onFailurePtr, StringResultCallbackDelegate callback);

        [DllImport("__Internal")]
        private static extern void _getLeaderboardScores(string leaderboardId, int offset, int count, int scoreType,
                                                    IntPtr onSuccessPtr, IntPtr onFailurePtr, StringResultCallbackDelegate callback);

        [DllImport("__Internal")]
        private static extern void _submitLeaderboardScore(string leaderboardId, int score, IntPtr onSuccessPtr,
                                                      IntPtr onFailurePtr, SubmitLeaderboardScoreListenerProxy.OnRankChangeDelegate onRankChange,
                                                      CompleteCallbackDelegate failureCallback);

        #endregion

        #region execute_actions_internal

        [DllImport("__Internal")]
        internal static extern void _executeCompleteCallback(IntPtr callbackPtr);

        [DllImport("__Internal")]
        internal static extern void _executeErrorCallback(IntPtr callbackPtr, string errorMessage);

        [DllImport("__Internal")]
        internal static extern void _executeInvitedFriendsCallback(IntPtr onSuccessPtr, string requestId,
                                                              int invitedFriendsCount, string[] invitedFriends);

        [DllImport("__Internal")]
        internal static extern void _executeAddUserIndentityConflictResolver(IntPtr callbackPtr, int resolutionStrategy);

        [DllImport("__Internal")]
        internal static extern void _executeOnUserPerformedActionFinalize(IntPtr callbackPtr, bool shouldPerformAction);

        #endregion

        #region common_native_callbacks

        internal delegate void CompleteCallbackDelegate(IntPtr actionPtr);

        internal delegate void StringResultCallbackDelegate(IntPtr actionPtr, string data);

        internal delegate void BoolResultCallbackDelegate(IntPtr actionPtr, bool result);

        internal delegate void FailureCallbackDelegate(IntPtr actionPtr, string error);

        [MonoPInvokeCallback(typeof(CompleteCallbackDelegate))]
        public static void CompleteCallback(IntPtr actionPtr)
        {
#if DEVELOPMENT_BUILD
            Debug.Log("CompleteCallback");
#endif
            if(actionPtr != IntPtr.Zero)
            {
                var action = actionPtr.Cast<Action>();
                action();
            }
        }

        [MonoPInvokeCallback(typeof(StringResultCallbackDelegate))]
        public static void StringResultCallaback(IntPtr actionPtr, string data)
        {
#if DEVELOPMENT_BUILD
            Debug.Log("StringResultCallaback: " + data);
#endif
            if(actionPtr != IntPtr.Zero)
            {
                var action = actionPtr.Cast<Action<string>>();
                action(data);
            }
        }

        [MonoPInvokeCallback(typeof(BoolResultCallbackDelegate))]
        public static void BoolResultCallback(IntPtr actionPtr, bool result)
        {
#if DEVELOPMENT_BUILD
            Debug.Log("BoolResultCallback: " + result);
#endif
            if(actionPtr != IntPtr.Zero)
            {
                var action = actionPtr.Cast<Action<bool>>();
                action(result);
            }
        }

        [MonoPInvokeCallback(typeof(FailureCallbackDelegate))]
        public static void FailureCallback(IntPtr actionPtr, string errorMessage)
        {
#if DEVELOPMENT_BUILD
            Debug.Log("FailureCallback: " + errorMessage);
#endif
            if(actionPtr != IntPtr.Zero)
            {
                var action = actionPtr.Cast<Action<string>>();
                action(errorMessage);
            }
        }

        #endregion
    }
}
#endif

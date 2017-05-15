#if UNITY_IOS
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace GetSocialSdk.Core
{
    class GetSocialNativeBridgeIOS : IGetSocialNativeBridge
    {
        static IGetSocialNativeBridge _instance;

        public static IGetSocialNativeBridge Instance
        {
            get { return _instance ?? (_instance = new GetSocialNativeBridgeIOS()); }
        }

        public bool IsInitialized
        {
            get { return _isInitialized(); }
        }

        public void Init(Action onSuccess, Action<GetSocialError> onFailure)
        {
            _init(Callbacks.ActionCallback, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public string GetNativeSdkVersion()
        {
            return _getNativeSdkVersion();
        }

        public string GetLanguage()
        {
            return _getLanguage();
        }

        public bool SetLanguage(string languageCode)
        {
            return _setLanguage(languageCode);
        }

        #region invites

        public InviteChannel[] InviteChannels
        {
            get
            {
                var channelsJson = _getInviteChannels();
                GetSocialDebugLogger.D("Invite channels: " + channelsJson);
                return GSJsonUtils.ParseChannelsList(channelsJson);
            }
        }

        public void SendInvite(string channelId, Action onComplete, Action onCancel,
            Action<GetSocialError> onFailure)
        {
            _sendInvite(channelId, Callbacks.ActionCallback, onComplete.GetPointer(), onCancel.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void SendInvite(string channelId, InviteContent customInviteContent,
            Action onComplete, Action onCancel, Action<GetSocialError> onFailure)
        {
            _sendInviteCustom(channelId, customInviteContent.ToJson(),
                Callbacks.ActionCallback, onComplete.GetPointer(), onCancel.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void SendInvite(string channelId, InviteContent customInviteContent,
            CustomReferralData customReferralData,
            Action onComplete, Action onCancel, Action<GetSocialError> onFailure)
        {
            _sendInviteCustomAndReferralData(channelId, customInviteContent.ToJson(), customReferralData.ToJson(),
                Callbacks.ActionCallback, onComplete.GetPointer(), onCancel.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public bool RegisterInviteChannelPlugin(string channelId, InviteChannelPlugin inviteProviderPlugin)
        {
            return _registerInviteProviderPlugin(channelId, inviteProviderPlugin.GetPointer(),
                InviteChannelPluginCallbacks.IsAvailableForDevice,
                InviteChannelPluginCallbacks.PresentChannelInterface);
        }

        #endregion

        public void GetReferralData(Action<ReferralData> onSuccess, Action<GetSocialError> onFailure)
        {
            _getReferralData(
                Callbacks.FetchReferralDataCallback, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer()
            );
        }

        public void RegisterForPushNotifications()
        {
            _registerForPushNotifications();
        }

        public void SetNotificationActionListener(Func<NotificationAction, bool> listener)
        {
            _setNotificationActionListener(listener.GetPointer(), Callbacks.NotificationActionListener);
        }

        public bool SetGlobalErrorListener(Action<GetSocialError> onError)
        {
            return _setGlobalErrorListener(onError.GetPointer(), GlobalErrorCallback.OnGlobalError);
        }

        public bool RemoveGlobalErrorListener()
        {
            return _removeGlobalErrorListener();
        }

        #region user_management

        public bool SetOnUserChangedListener(Action listener)
        {
            return _setOnUserChangedListener(listener.GetPointer(), Callbacks.ActionCallback);
        }

        public bool RemoveOnUserChangedListener()
        {
            return _removeOnUserChangedListener();
        }

        public string UserId
        {
            get { return _getUserId(); }
        }

        public bool IsUserAnonymous
        {
            get { return _isUserAnonymous(); }
        }

        public Dictionary<string, string> UserAuthIdentities
        {
            get
            {
                var identitiesJson = _getAuthIdentities();
                return GSJsonUtils.ParseDictionary(identitiesJson);
            }
        }

        public string DisplayName 
        {
            get { return _getUserDisplayName(); }
        }

        public void SetDisplayName(string displayName, Action onComplete, Action<GetSocialError> onFailure)
        {
            _setUserDisplayName(displayName, Callbacks.ActionCallback, onComplete.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public string AvatarUrl
        {
            get { return _getUserAvatarUrl(); }
        }

        public void SetAvatarUrl(string avatarUrl, Action onComplete, Action<GetSocialError> onFailure)
        {
            _setUserAvatarUrl(avatarUrl, Callbacks.ActionCallback, onComplete.GetPointer(), Callbacks.FailureCallback,
                onFailure.GetPointer());
        }

        public void SetPublicProperty(string key, string value, Action onSuccess, Action<GetSocialError> onFailure)
        {
            _setPublicProperty(key, value, Callbacks.ActionCallback, onSuccess.GetPointer(), Callbacks.FailureCallback,
                onFailure.GetPointer());
        }

        public void SetPrivateProperty(string key, string value, Action onSuccess, Action<GetSocialError> onFailure)
        {
            _setPrivateProperty(key, value, Callbacks.ActionCallback, onSuccess.GetPointer(), Callbacks.FailureCallback,
                onFailure.GetPointer());
        }

        public void RemovePublicProperty(string key, Action onSuccess, Action<GetSocialError> onFailure)
        {
            _removePublicProperty(key, Callbacks.ActionCallback, onSuccess.GetPointer(), Callbacks.FailureCallback,
                onFailure.GetPointer());
        }

        public void RemovePrivateProperty(string key, Action onSuccess, Action<GetSocialError> onFailure)
        {
            _removePrivateProperty(key, Callbacks.ActionCallback, onSuccess.GetPointer(), Callbacks.FailureCallback,
                onFailure.GetPointer());
        }

        public string GetPublicProperty(string key)
        {
            return _getPublicProperty(key);
        }

        public string GetPrivateProperty(string key)
        {
            return _getPrivateProperty(key);
        }

        public bool HasPublicProperty(string key)
        {
            return _hasPublicProperty(key);
        }

        public bool HasPrivateProperty(string key)
        {
            return _hasPrivateProperty(key);
        }

        public void AddAuthIdentity(AuthIdentity authIdentity, Action onComplete, Action<GetSocialError> onFailure, Action<ConflictUser> onConflict)
        {
            _addAuthIdentity(authIdentity.ToJson(),
                Callbacks.ActionCallback, onComplete.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer(),
                UserConflictCallback.OnUserAuthConflict, onConflict.GetPointer());
        }

        public void RemoveAuthIdentity(string providerId, Action onSuccess, Action<GetSocialError> onFailure)
        {
            _removeAuthIdentity(providerId,
                Callbacks.ActionCallback, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void SwitchUser(AuthIdentity authIdentity, Action onSuccess, Action<GetSocialError> onFailure)
        {
            _switchUser(authIdentity.ToJson(),
                Callbacks.ActionCallback, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        #endregion

        #region social_graph

        public void AddFriend (string userId, Action<int> onSuccess, Action<GetSocialError> onFailure)
        {
            _addFriend (userId,
                Callbacks.IntCallback, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void RemoveFriend (string userId, Action<int> onSuccess, Action<GetSocialError> onFailure)
        {
            _removeFriend (userId,
                Callbacks.IntCallback, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void IsFriend (string userId, Action<bool> onSuccess, Action<GetSocialError> onFailure)
        {
            _isFriend (userId,
                Callbacks.BoolCallback, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void GetFriendsCount(Action<int> onSuccess, Action<GetSocialError> onFailure)
        {
            _getFriendsCount(Callbacks.IntCallback, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void GetFriends (int offset, int limit, Action<List<PublicUser>> onSuccess, Action<GetSocialError> onFailure)
        {
            _getFriends (offset, limit,
                ActivityFeedCallbacks.OnUsersListReceived, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        #endregion

        #region activity_feed

        public void GetAnnouncements(string feed, Action<List<ActivityPost>> onSuccess,
            Action<GetSocialError> onFailure)
        {
            _getAnnouncements(feed,
                ActivityFeedCallbacks.OnActivityPostListReceived, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void GetActivities(ActivitiesQuery query, Action<List<ActivityPost>> onSuccess,
            Action<GetSocialError> onFailure)
        {
            _getActivitiesWithQuery(query.ToJson(),
                ActivityFeedCallbacks.OnActivityPostListReceived, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void GetActivity(string activityId, Action<ActivityPost> onSuccess, Action<GetSocialError> onFailure)
        {
            _getActivityById(activityId,
                ActivityFeedCallbacks.OnActivityPostReceived, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void PostActivityToFeed(string feed, ActivityPostContent content, Action<ActivityPost> onSuccess,
            Action<GetSocialError> onFailure)
        {
            _postActivityToFeed(feed, content.ToJson(),
                ActivityFeedCallbacks.OnActivityPostReceived, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void PostCommentToActivity(string activityId, ActivityPostContent comment, Action<ActivityPost> onSuccess,
            Action<GetSocialError> onFailure)
        {
            _postCommentToActivity(activityId, comment.ToJson(),
                ActivityFeedCallbacks.OnActivityPostReceived, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void LikeActivity(string activityId, bool isLiked, Action<ActivityPost> onSuccess,
            Action<GetSocialError> onFailure)
        {
            _likeActivity(activityId, isLiked,
                ActivityFeedCallbacks.OnActivityPostReceived, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void GetActivityLikers(string activityId, int offset, int limit, Action<List<PublicUser>> onSuccess,
            Action<GetSocialError> onFailure)
        {
            _getActivityLikers(activityId, offset, limit,
                ActivityFeedCallbacks.OnUsersListReceived, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        #endregion


        #region Access Helpers

        public void Reset()
        {
            _resetInternal();
        }

        public void SetHadesConfiguration(int hadesConfigurationType)
        {
            _setHadesConfigurationInternal(hadesConfigurationType);
        }

        public int GetCurrentHadesConfiguration()
        {
            return _getCurrentHadesConfigurationInternal();
        }

        #endregion


        #region external_init

        [DllImport("__Internal")]
        static extern void _init(VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern bool _isInitialized();

        #endregion

        [DllImport("__Internal")]
        static extern string _getNativeSdkVersion();

        [DllImport("__Internal")]
        static extern bool _setLanguage(string languageCode);

        [DllImport("__Internal")]
        static extern string _getLanguage();

        [DllImport("__Internal")]
        static extern bool _setGlobalErrorListener(IntPtr onErrorActionPtr, GlobalErrorCallbackDelegate errorCallback);

        [DllImport("__Internal")]
        static extern bool _removeGlobalErrorListener();

        #region external_invites

        [DllImport("__Internal")]
        static extern string _getInviteChannels();

        [DllImport("__Internal")]
        static extern void _sendInvite(string channelId,
            VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr, IntPtr onCancelActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _sendInviteCustom(string channelId, string customInviteContentJson,
            VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr, IntPtr onCancelActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _sendInviteCustomAndReferralData(string channelId, string customInviteContentJson,
            string customReferralDataJson,
            VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr, IntPtr onCancelActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern bool _registerInviteProviderPlugin(string channelId, IntPtr pluginPtr,
            InviteChannelPluginCallbacks.IsAvailableForDeviceDelegate isAvailableForDeviceDelegate,
            InviteChannelPluginCallbacks.PresentChannelInterfaceDelegate presentChannelInterfaceDelegate);

        [DllImport("__Internal")]
        static extern void _getReferralData(
            FetchReferralDataCallbackDelegate fetchReferralDataCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        // Invite Callbacks
        [DllImport("__Internal")]
        public static extern void _executeInviteSuccessCallback(IntPtr inviteSuccessCallbackPtr);

        [DllImport("__Internal")]
        public static extern void _executeInviteCancelledCallback(IntPtr inviteCancelledCallbackPtr);

        [DllImport("__Internal")]
        public static extern void _executeInviteFailedCallback(IntPtr inviteFailedCallbackPtr);

        #endregion

        #region push_notifications

        [DllImport("__Internal")]
        static extern bool _registerForPushNotifications();

        [DllImport("__Internal")]
        static extern bool _setNotificationActionListener(IntPtr listenerPointer,
            NotificationActionListenerDelegate listener);

        #endregion

        #region external_user_management

        [DllImport("__Internal")]
        static extern bool _setOnUserChangedListener(IntPtr listenerPointer, VoidCallbackDelegate onUserChanged);

        [DllImport("__Internal")]
        static extern bool _removeOnUserChangedListener();

        [DllImport("__Internal")]
        static extern string _getUserId();

        [DllImport("__Internal")]
        static extern bool _isUserAnonymous();

        [DllImport("__Internal")]
        static extern string _getAuthIdentities();

        [DllImport("__Internal")]
        static extern void _setUserDisplayName(string displayName, VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern string _getUserDisplayName();

        [DllImport("__Internal")]
        static extern void _setUserAvatarUrl(string avatarUrl, VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern string _getUserAvatarUrl();

        [DllImport("__Internal")]
        static extern void _setPublicProperty(string key, string value, VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _setPrivateProperty(string key, string value, VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _removePublicProperty(string key, VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _removePrivateProperty(string key, VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern string _getPublicProperty(string key);

        [DllImport("__Internal")]
        static extern string _getPrivateProperty(string key);

        [DllImport("__Internal")]
        static extern bool _hasPublicProperty(string key);

        [DllImport("__Internal")]
        static extern bool _hasPrivateProperty(string key);

        [DllImport("__Internal")]
        static extern void _addAuthIdentity(string identity,
            VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr,
            OnUserConflictDelegate conflictCallBack, IntPtr onConflictActionPtr);

        [DllImport("__Internal")]
        static extern void _switchUser(string identity,
            VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _removeAuthIdentity(string providerId,
            VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        #endregion

        #region social_graph

        [DllImport("__Internal")]
        static extern void _addFriend(string userId,
            IntCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _removeFriend(string userId,
            IntCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _isFriend(string userId,
            BoolCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _getFriendsCount(IntCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _getFriends(int offset, int limit,
            StringCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        #endregion

        #region activity_feed_internal

        [DllImport("__Internal")]
        static extern void _getAnnouncements(string feed,
            StringCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _getActivitiesWithQuery(string query,
            StringCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _getActivityById(string id,
            StringCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _postActivityToFeed(string feed, string activity,
            StringCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _postCommentToActivity(string id, string comment,
            StringCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _likeActivity(string id, bool isLiked,
            StringCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _getActivityLikers(string id, int offset, int limit,
            StringCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        #endregion

        #region external_access_helpers

        [DllImport("__Internal")]
        static extern void _resetInternal();

        [DllImport("__Internal")]
        static extern void _setHadesConfigurationInternal(int hadesConfigurationType);

        [DllImport("__Internal")]
        static extern int _getCurrentHadesConfigurationInternal();

        #endregion
    }
}

#endif
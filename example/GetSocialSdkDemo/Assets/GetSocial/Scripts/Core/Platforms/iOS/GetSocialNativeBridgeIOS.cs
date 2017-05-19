#if UNITY_IOS
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

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
            get { return _gs_isInitialized(); }
        }

        public void Init(Action onSuccess, Action<GetSocialError> onFailure)
        {
           _gs_init(Callbacks.ActionCallback, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public string GetNativeSdkVersion()
        {
            return _gs_getNativeSdkVersion();
        }

        public string GetLanguage()
        {
            return _gs_getLanguage();
        }

        public bool SetLanguage(string languageCode)
        {
            return _gs_setLanguage(languageCode);
        }

        #region invites

        public InviteChannel[] InviteChannels
        {
            get
            {
                var channelsJson = _gs_getInviteChannels();
                GetSocialDebugLogger.D("Invite channels: " + channelsJson);
                return GSJsonUtils.ParseChannelsList(channelsJson);
            }
        }

        public void SendInvite(string channelId, Action onComplete, Action onCancel,
            Action<GetSocialError> onFailure)
        {
           _gs_sendInvite(channelId, Callbacks.ActionCallback, onComplete.GetPointer(), onCancel.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void SendInvite(string channelId, InviteContent customInviteContent,
            Action onComplete, Action onCancel, Action<GetSocialError> onFailure)
        {
           _gs_sendInviteCustom(channelId, customInviteContent.ToJson(),
                Callbacks.ActionCallback, onComplete.GetPointer(), onCancel.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void SendInvite(string channelId, InviteContent customInviteContent,
            CustomReferralData customReferralData,
            Action onComplete, Action onCancel, Action<GetSocialError> onFailure)
        {
           _gs_sendInviteCustomAndReferralData(channelId, customInviteContent.ToJson(), customReferralData.ToJson(),
                Callbacks.ActionCallback, onComplete.GetPointer(), onCancel.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public bool RegisterInviteChannelPlugin(string channelId, InviteChannelPlugin inviteProviderPlugin)
        {
            return _gs_registerInviteProviderPlugin(channelId, inviteProviderPlugin.GetPointer(),
                InviteChannelPluginCallbacks.IsAvailableForDevice,
                InviteChannelPluginCallbacks.PresentChannelInterface);
        }

        #endregion

        public void GetReferralData(Action<ReferralData> onSuccess, Action<GetSocialError> onFailure)
        {
           _gs_getReferralData(
                Callbacks.FetchReferralDataCallback, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer()
            );
        }

        public void RegisterForPushNotifications()
        {
           _gs_registerForPushNotifications();
        }

        public void SetNotificationActionListener(Func<NotificationAction, bool> listener)
        {
           _gs_setNotificationActionListener(listener.GetPointer(), Callbacks.NotificationActionListener);
        }

        public bool SetGlobalErrorListener(Action<GetSocialError> onError)
        {
            return _gs_setGlobalErrorListener(onError.GetPointer(), GlobalErrorCallback.OnGlobalError);
        }

        public bool RemoveGlobalErrorListener()
        {
            return _gs_removeGlobalErrorListener();
        }

        #region user_management

        public bool SetOnUserChangedListener(Action listener)
        {
            return _gs_setOnUserChangedListener(listener.GetPointer(), Callbacks.ActionCallback);
        }

        public bool RemoveOnUserChangedListener()
        {
            return _gs_removeOnUserChangedListener();
        }

        public string UserId
        {
            get { return _gs_getUserId(); }
        }

        public bool IsUserAnonymous
        {
            get { return _gs_isUserAnonymous(); }
        }

        public Dictionary<string, string> UserAuthIdentities
        {
            get
            {
                var identitiesJson = _gs_getAuthIdentities();
                return GSJsonUtils.ParseDictionary(identitiesJson);
            }
        }

        public string DisplayName 
        {
            get { return _gs_getUserDisplayName(); }
        }

        public void SetDisplayName(string displayName, Action onComplete, Action<GetSocialError> onFailure)
        {
           _gs_setUserDisplayName(displayName, Callbacks.ActionCallback, onComplete.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public string AvatarUrl
        {
            get { return _gs_getUserAvatarUrl(); }
        }

        public void SetAvatarUrl(string avatarUrl, Action onComplete, Action<GetSocialError> onFailure)
        {
           _gs_setUserAvatarUrl(avatarUrl, Callbacks.ActionCallback, onComplete.GetPointer(), Callbacks.FailureCallback,
                onFailure.GetPointer());
        }

        public void SetPublicProperty(string key, string value, Action onSuccess, Action<GetSocialError> onFailure)
        {
           _gs_setPublicProperty(key, value, Callbacks.ActionCallback, onSuccess.GetPointer(), Callbacks.FailureCallback,
                onFailure.GetPointer());
        }

        public void SetPrivateProperty(string key, string value, Action onSuccess, Action<GetSocialError> onFailure)
        {
           _gs_setPrivateProperty(key, value, Callbacks.ActionCallback, onSuccess.GetPointer(), Callbacks.FailureCallback,
                onFailure.GetPointer());
        }

        public void RemovePublicProperty(string key, Action onSuccess, Action<GetSocialError> onFailure)
        {
           _gs_removePublicProperty(key, Callbacks.ActionCallback, onSuccess.GetPointer(), Callbacks.FailureCallback,
                onFailure.GetPointer());
        }

        public void RemovePrivateProperty(string key, Action onSuccess, Action<GetSocialError> onFailure)
        {
           _gs_removePrivateProperty(key, Callbacks.ActionCallback, onSuccess.GetPointer(), Callbacks.FailureCallback,
                onFailure.GetPointer());
        }

        public string GetPublicProperty(string key)
        {
            return _gs_getPublicProperty(key);
        }

        public string GetPrivateProperty(string key)
        {
            return _gs_getPrivateProperty(key);
        }

        public bool HasPublicProperty(string key)
        {
            return _gs_hasPublicProperty(key);
        }

        public bool HasPrivateProperty(string key)
        {
            return _gs_hasPrivateProperty(key);
        }

        public void AddAuthIdentity(AuthIdentity authIdentity, Action onComplete, Action<GetSocialError> onFailure, Action<ConflictUser> onConflict)
        {
           _gs_addAuthIdentity(authIdentity.ToJson(),
                Callbacks.ActionCallback, onComplete.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer(),
                UserConflictCallback.OnUserAuthConflict, onConflict.GetPointer());
        }

        public void RemoveAuthIdentity(string providerId, Action onSuccess, Action<GetSocialError> onFailure)
        {
           _gs_removeAuthIdentity(providerId,
                Callbacks.ActionCallback, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void SwitchUser(AuthIdentity authIdentity, Action onSuccess, Action<GetSocialError> onFailure)
        {
           _gs_switchUser(authIdentity.ToJson(),
                Callbacks.ActionCallback, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        #endregion

        #region social_graph

        public void AddFriend (string userId, Action<int> onSuccess, Action<GetSocialError> onFailure)
        {
           _gs_addFriend (userId,
                Callbacks.IntCallback, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void RemoveFriend (string userId, Action<int> onSuccess, Action<GetSocialError> onFailure)
        {
           _gs_removeFriend (userId,
                Callbacks.IntCallback, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void IsFriend (string userId, Action<bool> onSuccess, Action<GetSocialError> onFailure)
        {
           _gs_isFriend (userId,
                Callbacks.BoolCallback, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void GetFriendsCount(Action<int> onSuccess, Action<GetSocialError> onFailure)
        {
           _gs_getFriendsCount(Callbacks.IntCallback, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void GetFriends (int offset, int limit, Action<List<PublicUser>> onSuccess, Action<GetSocialError> onFailure)
        {
           _gs_getFriends (offset, limit,
                ActivityFeedCallbacks.OnUsersListReceived, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        #endregion

        #region activity_feed

        public void GetAnnouncements(string feed, Action<List<ActivityPost>> onSuccess,
            Action<GetSocialError> onFailure)
        {
           _gs_getAnnouncements(feed,
                ActivityFeedCallbacks.OnActivityPostListReceived, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void GetActivities(ActivitiesQuery query, Action<List<ActivityPost>> onSuccess,
            Action<GetSocialError> onFailure)
        {
           _gs_getActivitiesWithQuery(query.ToJson(),
                ActivityFeedCallbacks.OnActivityPostListReceived, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void GetActivity(string activityId, Action<ActivityPost> onSuccess, Action<GetSocialError> onFailure)
        {
           _gs_getActivityById(activityId,
                ActivityFeedCallbacks.OnActivityPostReceived, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void PostActivityToFeed(string feed, ActivityPostContent content, Action<ActivityPost> onSuccess,
            Action<GetSocialError> onFailure)
        {
           _gs_postActivityToFeed(feed, content.ToJson(),
                ActivityFeedCallbacks.OnActivityPostReceived, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void PostCommentToActivity(string activityId, ActivityPostContent comment, Action<ActivityPost> onSuccess,
            Action<GetSocialError> onFailure)
        {
           _gs_postCommentToActivity(activityId, comment.ToJson(),
                ActivityFeedCallbacks.OnActivityPostReceived, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void LikeActivity(string activityId, bool isLiked, Action<ActivityPost> onSuccess,
            Action<GetSocialError> onFailure)
        {
           _gs_likeActivity(activityId, isLiked,
                ActivityFeedCallbacks.OnActivityPostReceived, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void GetActivityLikers(string activityId, int offset, int limit, Action<List<PublicUser>> onSuccess,
            Action<GetSocialError> onFailure)
        {
           _gs_getActivityLikers(activityId, offset, limit,
                ActivityFeedCallbacks.OnUsersListReceived, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        #endregion


        #region Access Helpers

        public void Reset()
        {
           _gs_resetInternal();
        }

        public void SetHadesConfiguration(int hadesConfigurationType)
        {
           _gs_setHadesConfigurationInternal(hadesConfigurationType);
        }

        public int GetCurrentHadesConfiguration()
        {
            return _gs_getCurrentHadesConfigurationInternal();
        }

        #endregion


        #region external_init

        [DllImport("__Internal")]
        static extern void _gs_init(VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern bool _gs_isInitialized();

        #endregion

        [DllImport("__Internal")]
        static extern string _gs_getNativeSdkVersion();

        [DllImport("__Internal")]
        static extern bool _gs_setLanguage(string languageCode);

        [DllImport("__Internal")]
        static extern string _gs_getLanguage();

        [DllImport("__Internal")]
        static extern bool _gs_setGlobalErrorListener(IntPtr onErrorActionPtr, GlobalErrorCallbackDelegate errorCallback);

        [DllImport("__Internal")]
        static extern bool _gs_removeGlobalErrorListener();

        #region external_invites

        [DllImport("__Internal")]
        static extern string _gs_getInviteChannels();

        [DllImport("__Internal")]
        static extern void _gs_sendInvite(string channelId,
            VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr, IntPtr onCancelActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_sendInviteCustom(string channelId, string customInviteContentJson,
            VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr, IntPtr onCancelActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_sendInviteCustomAndReferralData(string channelId, string customInviteContentJson,
            string customReferralDataJson,
            VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr, IntPtr onCancelActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern bool _gs_registerInviteProviderPlugin(string channelId, IntPtr pluginPtr,
            InviteChannelPluginCallbacks.IsAvailableForDeviceDelegate isAvailableForDeviceDelegate,
            InviteChannelPluginCallbacks.PresentChannelInterfaceDelegate presentChannelInterfaceDelegate);

        [DllImport("__Internal")]
        static extern void _gs_getReferralData(
            FetchReferralDataCallbackDelegate fetchReferralDataCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        // Invite Callbacks
        [DllImport("__Internal")]
        public static extern void _gs_executeInviteSuccessCallback(IntPtr inviteSuccessCallbackPtr);

        [DllImport("__Internal")]
        public static extern void _gs_executeInviteCancelledCallback(IntPtr inviteCancelledCallbackPtr);

        [DllImport("__Internal")]
        public static extern void _gs_executeInviteFailedCallback(IntPtr inviteFailedCallbackPtr);

        #endregion

        #region push_notifications

        [DllImport("__Internal")]
        static extern bool _gs_registerForPushNotifications();

        [DllImport("__Internal")]
        static extern bool _gs_setNotificationActionListener(IntPtr listenerPointer,
            NotificationActionListenerDelegate listener);

        #endregion

        #region external_user_management

        [DllImport("__Internal")]
        static extern bool _gs_setOnUserChangedListener(IntPtr listenerPointer, VoidCallbackDelegate onUserChanged);

        [DllImport("__Internal")]
        static extern bool _gs_removeOnUserChangedListener();

        [DllImport("__Internal")]
        static extern string _gs_getUserId();

        [DllImport("__Internal")]
        static extern bool _gs_isUserAnonymous();

        [DllImport("__Internal")]
        static extern string _gs_getAuthIdentities();

        [DllImport("__Internal")]
        static extern void _gs_setUserDisplayName(string displayName, VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern string _gs_getUserDisplayName();

        [DllImport("__Internal")]
        static extern void _gs_setUserAvatarUrl(string avatarUrl, VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern string _gs_getUserAvatarUrl();

        [DllImport("__Internal")]
        static extern void _gs_setPublicProperty(string key, string value, VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_setPrivateProperty(string key, string value, VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_removePublicProperty(string key, VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_removePrivateProperty(string key, VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern string _gs_getPublicProperty(string key);

        [DllImport("__Internal")]
        static extern string _gs_getPrivateProperty(string key);

        [DllImport("__Internal")]
        static extern bool _gs_hasPublicProperty(string key);

        [DllImport("__Internal")]
        static extern bool _gs_hasPrivateProperty(string key);

        [DllImport("__Internal")]
        static extern void _gs_addAuthIdentity(string identity,
            VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr,
            OnUserConflictDelegate conflictCallBack, IntPtr onConflictActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_switchUser(string identity,
            VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_removeAuthIdentity(string providerId,
            VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        #endregion

        #region social_graph

        [DllImport("__Internal")]
        static extern void _gs_addFriend(string userId,
            IntCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_removeFriend(string userId,
            IntCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_isFriend(string userId,
            BoolCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_getFriendsCount(IntCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_getFriends(int offset, int limit,
            StringCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        #endregion

        #region activity_feed_internal

        [DllImport("__Internal")]
        static extern void _gs_getAnnouncements(string feed,
            StringCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_getActivitiesWithQuery(string query,
            StringCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_getActivityById(string id,
            StringCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_postActivityToFeed(string feed, string activity,
            StringCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_postCommentToActivity(string id, string comment,
            StringCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_likeActivity(string id, bool isLiked,
            StringCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_getActivityLikers(string id, int offset, int limit,
            StringCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        #endregion

        #region external_access_helpers

        [DllImport("__Internal")]
        static extern void _gs_resetInternal();

        [DllImport("__Internal")]
        static extern void _gs_setHadesConfigurationInternal(int hadesConfigurationType);

        [DllImport("__Internal")]
        static extern int _gs_getCurrentHadesConfigurationInternal();

        #endregion
    }
}

#endif
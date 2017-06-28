using System;
using System.Collections.Generic;
using System.Reflection;

namespace GetSocialSdk.Core
{
    class GetSocialNativeBridgeMock : IGetSocialNativeBridge
    {
        const string Mock = "mock";

        static IGetSocialNativeBridge _instance;

        static readonly Dictionary<string, string> EmptyIdentities = new Dictionary<string, string>();
        static readonly InviteChannel[] EmptyChannels = { };

        static readonly GetSocialError FailedInEditorError =
            new GetSocialError();

        public static IGetSocialNativeBridge Instance
        {
            get { return _instance ?? (_instance = new GetSocialNativeBridgeMock()); }
        }

        public void WhenInitialized(Action action)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), action);
        }

        public bool IsInitialized
        {
            get { return false; }
        }

        public void Init(Action onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), onSuccess, onFailure);
            onFailure(FailedInEditorError);
        }

        public string GetNativeSdkVersion()
        {
            return "Not available in Editor";
        }

        public string GetLanguage()
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod());
            return null;
        }

        public bool SetLanguage(string languageCode)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), languageCode);
            return false;
        }

        public string Language { get; set; }

        public InviteChannel[] InviteChannels
        {
            get { return EmptyChannels; }
        }

        public void SendInvite(string channelId, Action onComplete, Action onCancel,
            Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), channelId, onComplete, onCancel, onFailure);
        }

        public void SendInvite(string channelId, InviteContent customInviteContent, Action onComplete, Action onCancel,
            Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), channelId, customInviteContent,
                onComplete, onCancel, onFailure);
        }

        public void SendInvite(string channelId, InviteContent customInviteContent,
            CustomReferralData customReferralData,
            Action onComplete, Action onCancel, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), channelId, customInviteContent, customReferralData,
                onComplete, onCancel, onFailure);
        }

        public bool RegisterInviteChannelPlugin(string channelId, InviteChannelPlugin inviteChannelPlugin)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), channelId, inviteChannelPlugin);
            return false;
        }

        public void GetReferralData(Action<ReferralData> onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), onSuccess, onFailure);
        }

        public void RegisterForPushNotifications()
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod());
        }

        public void SetNotificationActionListener(Func<NotificationAction, bool> listener)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), listener);
        }

        public bool SetOnUserChangedListener(Action listener)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), listener);
            return true;
        }

        public bool RemoveOnUserChangedListener()
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod());
            return true;
        }

        public bool SetGlobalErrorListener(Action<GetSocialError> onError)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), onError);
            return false;
        }

        public bool RemoveGlobalErrorListener()
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod());
            return false;
        }

        public string UserId
        {
            get { return string.Empty; }
        }

        public bool IsUserAnonymous
        {
            get { return true; }
        }

        public Dictionary<string, string> UserAuthIdentities
        {
            get { return EmptyIdentities; }
        }

        public Dictionary<string, string> AllPublicProperties {
            get { return new Dictionary<string, string>(); }
        }

        public Dictionary<string, string> AllPrivateProperties {
            get { return new Dictionary<string, string>(); }
        }

        public string DisplayName
        {
            get { return ""; }
        }

        public void SetDisplayName(string displayName, Action onComplete, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), displayName, onComplete, onFailure);
        }

        public string AvatarUrl
        {
            get { return ""; }
        }

        public void SetAvatarUrl(string avatarUrl, Action onComplete, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), avatarUrl, onComplete, onFailure);
        }

        public void SetPublicProperty(string key, string value, Action onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), key, value, onSuccess, onFailure);
        }

        public void SetPrivateProperty(string key, string value, Action onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), key, value, onSuccess, onFailure);
        }

        public void RemovePublicProperty(string key, Action onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), key, onSuccess, onFailure);
        }

        public void RemovePrivateProperty(string key, Action onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), key, onSuccess, onFailure);
        }

        public string GetPublicProperty(string key)
        {
            return "";
        }

        public string GetPrivateProperty(string key)
        {
            return "";
        }

        public bool HasPublicProperty(string key)
        {
            return false;
        }

        public bool HasPrivateProperty(string key)
        {
            return false;
        }

        public void AddAuthIdentity(AuthIdentity authIdentity, Action onComplete, Action<GetSocialError> onFailure, Action<ConflictUser> onConflict)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), authIdentity, onComplete,
                onFailure, onComplete);
        }

        public void GetUserById(string userId, Action<PublicUser> onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), userId, onSuccess, onFailure);
        }

        public void RemoveAuthIdentity(string providerId, Action onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), providerId, onSuccess, onFailure);
        }

        public void SwitchUser(AuthIdentity authIdentity, Action onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(),  authIdentity, onSuccess,
                onFailure);
        }

        public void AddFriend (string userId, Action<int> onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(),  userId, onSuccess,
                onFailure);
        }

        public void RemoveFriend (string userId, Action<int> onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(),  userId, onSuccess,
                onFailure);
        }

        public void IsFriend (string userId, Action<bool> onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(),  userId, onSuccess,
                onFailure);
        }

        public void GetFriendsCount(Action<int> onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), onSuccess,
                onFailure);
        }

        public void GetFriends (int offset, int limit, Action<List<PublicUser>> onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), offset, limit, onSuccess,
                onFailure);
        }

        public void GetSuggestedFriends(int offset, int limit, Action<List<SuggestedFriend>> onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), offset, limit, onSuccess,
                onFailure);
        }

        public void GetAnnouncements(string feed, Action<List<ActivityPost>> onSuccess,
            Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), feed, onSuccess, onFailure);
        }

        public void GetActivities(ActivitiesQuery query, Action<List<ActivityPost>> onSuccess,
            Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), query, onSuccess, onFailure);
        }

        public void GetActivity(string activityId, Action<ActivityPost> onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), activityId, onSuccess, onFailure);
        }

        public void PostActivityToFeed(string feed, ActivityPostContent content, Action<ActivityPost> onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), content, onSuccess, onFailure);
        }

        public void PostCommentToActivity(string activityId, ActivityPostContent comment, Action<ActivityPost> onSuccess,
            Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), activityId, comment, onSuccess, onFailure);
        }

        public void LikeActivity(string activityId, bool isLiked, Action<ActivityPost> onSuccess,
            Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), activityId, isLiked, onSuccess, onFailure);
        }

        public void GetActivityLikers(string activityId, int offset, int limit, Action<List<PublicUser>> onSuccess,
            Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), offset, limit, onSuccess, onFailure);
        }

        public void Reset()
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod());
        }

        public void SetHadesConfiguration(int hadesConfigurationType)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), hadesConfigurationType);
        }

        public int GetCurrentHadesConfiguration()
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod());
            return 0;
        }
    }
}
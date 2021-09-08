using System;
using System.Collections.Generic;

namespace GetSocialSdk.Core
{
    internal interface IGetSocialBridge
    {
        #region Init
        string GetNativeSdkVersion();

        bool IsInitialized();
        void AddOnInitializedListener(Action action);
        void Init(string appId);
        void Init(Identity identity, Action onSuccess, Action<GetSocialError> onError);
        void Handle(GetSocialAction action);
        bool IsTestDevice();
        string GetDeviceId();

        #endregion
        #region CurrentUser

        string AddOnCurrentUserChangedListener(OnCurrentUserChangedListener listener);
        void RemoveOnCurrentUserChangedListener(string listenerId);
        CurrentUser GetCurrentUser();
        void SwitchUser(Identity identity, Action success, Action<GetSocialError> failure);
        void AddIdentity(Identity identity, Action success, Action<ConflictUser> conflict,
            Action<GetSocialError> failure);
        void RemoveIdentity(string providerId, Action callback, Action<GetSocialError> failure);
        void ResetUser(Action success, Action<GetSocialError> failure);
        void ResetUserWithoutInit(Action success, Action<GetSocialError> failure);
        void UpdateDetails(UserUpdate userUpdate, Action callback, Action<GetSocialError> failure);
        void Refresh(Action callback, Action<GetSocialError> failure);

        #endregion
        #region Language

        bool SetLanguage(string language);
        string GetLanguage();
        
        #endregion
        #region Communities

        #region Activities

        void GetAnnouncements(AnnouncementsQuery query, Action<List<Activity>> onSuccess, Action<GetSocialError> onFailure);

        void GetActivities(PagingQuery<ActivitiesQuery> query, Action<PagingResult<Activity>> onSuccess,
            Action<GetSocialError> onFailure);

        void GetActivity(string id, Action<Activity> onSuccess, Action<GetSocialError> onFailure);

        void PostActivity(ActivityContent content, PostActivityTarget target, Action<Activity> onSuccess,
            Action<GetSocialError> onFailure);

        void UpdateActivity(string id, ActivityContent content, Action<Activity> onSuccess,
            Action<GetSocialError> onFailure);
        void RemoveActivities(RemoveActivitiesQuery query, Action onSuccess, Action<GetSocialError> onFailure);


        void AddReaction(string reaction, string activityId, Action success, Action<GetSocialError> failure);
        void RemoveReaction(string reaction, string activityId, Action success, Action<GetSocialError> failure);

        void GetReactions(PagingQuery<ReactionsQuery> query, Action<PagingResult<UserReactions>> onSuccess,
            Action<GetSocialError> onFailure);
        void GetTags(TagsQuery query, Action<List<string>> onSuccess, Action<GetSocialError> onFailure);
        void ReportActivity(string activityId, ReportingReason reason, string explanation, Action onSuccess, Action<GetSocialError> onError);

        void AddVotes(HashSet<string> pollOptionIds, string activityId, Action onSuccess, Action<GetSocialError> onError);
        void SetVotes(HashSet<string> pollOptionIds, string activityId, Action onSuccess, Action<GetSocialError> onError);
        void RemoveVotes(HashSet<string> pollOptionIds, string activityId, Action onSuccess, Action<GetSocialError> onError);
        void GetVotes(PagingQuery<VotesQuery> query, Action<PagingResult<UserVotes>> onSuccess, Action<GetSocialError> onError);

        #endregion

        #region Users

        void GetUsers(PagingQuery<UsersQuery> query, Action<PagingResult<User>> onSuccess,
            Action<GetSocialError> onFailure);
        
        void GetUsers(UserIdList list, Action<Dictionary<string, User>> onSuccess,
            Action<GetSocialError> onFailure);
        
        void GetUsersCount(UsersQuery query, Action<int> success, Action<GetSocialError> error);
        #endregion

        #region Friends

        void AddFriends(UserIdList userIds, Action<int> success, Action<GetSocialError> failure);
        void RemoveFriends(UserIdList userIds, Action<int> success, Action<GetSocialError> failure);
        void GetFriends(PagingQuery<FriendsQuery> query, Action<PagingResult<User>> success, Action<GetSocialError> failure);
        void GetFriendsCount(FriendsQuery query, Action<int> success,
            Action<GetSocialError> failure);
        void AreFriends(UserIdList userIds, Action<Dictionary<string,bool>> success, Action<GetSocialError> failure);
        void IsFriend(UserId userId, Action<bool> success, Action<GetSocialError> failure);
        void SetFriends(UserIdList userIds, Action<int> success, Action<GetSocialError> failure);
        void GetSuggestedFriends(SimplePagingQuery query, Action<PagingResult<SuggestedFriend>> success, Action<GetSocialError> failure);
        #endregion

        #region Topics

        void GetTopic(string topic, Action<Topic> success, Action<GetSocialError> failure);

        void GetTopics(PagingQuery<TopicsQuery> pagingQuery, Action<PagingResult<Topic>> success, Action<GetSocialError> failure);

        void GetTopicsCount(TopicsQuery query, Action<int> success, Action<GetSocialError> failure);

        #endregion

        #region Groups

        void CreateGroup(GroupContent content, Action<Group> success, Action<GetSocialError> failure);

        void UpdateGroup(string groupId, GroupContent content, Action<Group> success, Action<GetSocialError> failure);

        void RemoveGroups(List<string> groupIds, Action success, Action<GetSocialError> failure);

        void GetGroupMembers(PagingQuery<MembersQuery> pagingQuery, Action<PagingResult<GroupMember>> success, Action<GetSocialError> failure);

        void GetGroups(PagingQuery<GroupsQuery> pagingQuery, Action<PagingResult<Group>> success, Action<GetSocialError> failure);

        void GetGroupsCount(GroupsQuery query, Action<int> success, Action<GetSocialError> failure);

        void GetGroup(string groupId, Action<Group> success, Action<GetSocialError> failure);

        void AddGroupMembers(AddGroupMembersQuery query, Action<List<GroupMember>> success, Action<GetSocialError> failure);

        void JoinGroup(JoinGroupQuery query, Action<GroupMember> success, Action<GetSocialError> failure);

        void UpdateGroupMembers(UpdateGroupMembersQuery query, Action<List<GroupMember>> success, Action<GetSocialError> failure);

        void RemoveGroupMembers(RemoveGroupMembersQuery query, Action success, Action<GetSocialError> failure);

        void AreGroupMembers(string groupId, UserIdList userIdList, Action<Dictionary<String, Membership>> success, Action<GetSocialError> failure);

        #endregion

        #region Follow/Unfollow

        void Follow(FollowQuery query, Action<int> success, Action<GetSocialError> failure);

        void Unfollow(FollowQuery query, Action<int> success, Action<GetSocialError> failure);

        void IsFollowing(UserId userId, FollowQuery query, Action<Dictionary<string, bool>> success, Action<GetSocialError> failure);

        void GetFollowers(PagingQuery<FollowersQuery> query, Action<PagingResult<User>> success, Action<GetSocialError> failure);

        void GetFollowersCount(FollowersQuery query, Action<int> success, Action<GetSocialError> failure);

        #endregion

        #region Chat
        void SendChatMessage(ChatMessageContent content, ChatId target, Action<ChatMessage> success, Action<GetSocialError> failure);
        void GetChatMessages(ChatMessagesPagingQuery pagingQuery, Action<ChatMessagesPagingResult> success, Action<GetSocialError> failure);
        void GetChats(SimplePagingQuery pagingQuery, Action<PagingResult<Chat>> success, Action<GetSocialError> failure);
        void GetChat(ChatId chatId, Action<Chat> success, Action<GetSocialError> failure);

        #endregion

        #endregion

        #region Invites

        void GetAvailableChannels(Action<List<InviteChannel>> success, Action<GetSocialError> failure);

        void Send(InviteContent customInviteContent, string channelId, Action success, Action cancel, Action<GetSocialError> failure);

        void Create(InviteContent customInviteContent, Action<Invite> success, Action<GetSocialError> failure);
        void CreateLink(Dictionary<string, object> linkParams, Action<string> success, Action<GetSocialError> failure);

        void GetReferredUsers(PagingQuery<ReferralUsersQuery> query, Action<PagingResult<ReferralUser>> success, Action<GetSocialError> failure);

        void GetReferrerUsers(PagingQuery<ReferralUsersQuery> query, Action<PagingResult<ReferralUser>> success, Action<GetSocialError> failure);

        void SetReferrer(UserId userId, string eventName, Dictionary<string, string> customData, Action success, Action<GetSocialError> failure);

        void SetOnReferralDataReceivedListener(Action<ReferralData> action);

#if UNITY_EDITOR
        void TriggerOnReferralDataReceivedListener(ReferralData referralData);
#endif

        #endregion

        #region Promo Codes

        void CreatePromoCode(PromoCodeContent content, Action<PromoCode> success, Action<GetSocialError> failure);
        void GetPromoCode(string code, Action<PromoCode> success, Action<GetSocialError> failure);
        void ClaimPromoCode(string code, Action<PromoCode> success, Action<GetSocialError> failure);

        #endregion

        #region Custom Events

        bool TrackPurchase(PurchaseData purchaseData);
        bool TrackCustomEvent(string eventName, Dictionary<string, string> eventData);

        #endregion

        #region Notifications

        void GetNotifications(PagingQuery<NotificationsQuery> pagingQuery, Action<PagingResult<Notification>> success, Action<GetSocialError> failure);
        void CountNotifications(NotificationsQuery query, Action<int> success, Action<GetSocialError> failure);
        void SetStatus(string newStatus, List<string> notificationIds, Action success, Action<GetSocialError> failure);
        void SetPushNotificationsEnabled(bool enabled, Action success, Action<GetSocialError> failure);
        void ArePushNotificationsEnabled(Action<bool> success, Action<GetSocialError> failure);
        void Send(NotificationContent content, SendNotificationTarget target, Action success, Action<GetSocialError> failure);
        void RegisterDevice();
        void SetOnNotificationReceivedListener(Action<Notification> listener);
        void SetOnNotificationClickedListener(Action<Notification, NotificationContext> listener);
        void SetOnTokenReceivedListener(Action<string> listener);

        #endregion

        #region Helpers

        void HandleOnStartUnityEvent();
        #endregion

    }
}

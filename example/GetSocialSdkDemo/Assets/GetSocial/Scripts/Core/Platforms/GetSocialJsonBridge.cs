using System;
using System.Collections.Generic;
using GetSocialSdk.MiniJSON;
using UnityEngine;

namespace GetSocialSdk.Core
{
    internal class GetSocialJsonBridge: IGetSocialBridge
    {
        private IMethodCaller MethodCaller { get; }

        public GetSocialJsonBridge(IMethodCaller methodCaller)
        {
            MethodCaller = methodCaller;
        }

        public void AddOnInitializedListener(Action action)
        {
            MethodCaller.RegisterListener("GetSocial.addOnInitializedListener", any => action());
        }

        public void Init(string appId)
        {
            CallSync<string>("GetSocial.init", appId);
        }
        public void Init(Identity identity, Action onSuccess, Action<GetSocialError> onError)
        {
            CallAsyncVoid("GetSocial.initWithIdentity", GSJson.Serialize(identity), onSuccess, onError);
        }

        public void Handle(GetSocialAction action)
        {
            CallSync<string>("GetSocial.handleAction", GSJson.Serialize(action));
        }

        public bool IsTestDevice()
        {
            return CallSync<bool>("GetSocial.isTestDevice");
        }

        public string GetDeviceId()
        {
            return CallSync<string>("GetSocial.getDeviceId");
        }

        public string AddOnCurrentUserChangedListener(OnCurrentUserChangedListener listener)
        {
            Action<CurrentUser> wrapper = some => listener(some);
            return MethodCaller.RegisterListener("GetSocial.addOnCurrentUserChangedListener", Json(wrapper));
        }

        public void RemoveOnCurrentUserChangedListener(string listenerId)
        {
            CallSync<string>("GetSocial.removeOnCurrentUserChangedListener", listenerId);
        }

        public CurrentUser GetCurrentUser()
        {
            return CallSync<CurrentUser>("GetSocial.getCurrentUser");
        }

        public void SwitchUser(Identity identity, Action success, Action<GetSocialError> failure)
        {
            CallAsyncVoid("GetSocial.switchUser",  GSJson.Serialize(identity), success, failure);
        }

        public void AddIdentity(Identity identity, Action success, Action<ConflictUser> conflict, Action<GetSocialError> failure)
        {
            MethodCaller.CallAsync("CurrentUser.addIdentity", GSJson.Serialize(identity), (string result) => {
                if (result == null || result.Length == 0) {
                    success();
                } else {
                    var obj = GSJson.Deserialize(result);
                    if (obj != null && obj.GetType().IsGenericDictionary())
                    {
                        var dictionary = obj as Dictionary<string, object>;
                        if (dictionary.Count == 1 && dictionary.ContainsKey("result"))
                        {
                            success();
                        } else {
                            conflict(GSJson.ToObject<ConflictUser>(obj));
                        }
                    } else {
                        conflict(GSJson.ToObject<ConflictUser>(obj));
                    }
                }

            }, Json(failure));            
        }

        public void RemoveIdentity(string providerId, Action callback, Action<GetSocialError> failure)
        {
            CallAsyncVoid("CurrentUser.removeIdentity", providerId, callback, failure);
        }

        public void ResetUser(Action success, Action<GetSocialError> failure)
        {
            CallAsyncVoid("GetSocial.resetUser",  "", success, failure);
        }
        public void ResetUserWithoutInit(Action success, Action<GetSocialError> failure)
        {
            CallAsyncVoid("GetSocial.resetUserWithoutInit", "", success, failure);
        }

        public void UpdateDetails(UserUpdate userUpdate, Action callback, Action<GetSocialError> failure)
        {
            CallAsyncVoid("CurrentUser.updateDetails",  GSJson.Serialize(userUpdate), callback, failure);
        }

        public bool SetLanguage(string language)
        {
            return CallSync<bool>("GetSocial.setLanguage", language);
        }

        public string GetNativeSdkVersion()
        {
            // todo
            return "";
        }

        public bool IsInitialized()
        {
            return CallSync<bool>("GetSocial.isInitialized");
        }

        public string GetLanguage()
        {
            return CallSync<string>("GetSocial.getLanguage");
        }

        public void GetAnnouncements(AnnouncementsQuery query, Action<List<Activity>> onSuccess, Action<GetSocialError> onFailure)
        {
            CallAsync("Communities.getAnnouncements", GSJson.Serialize(query), onSuccess, onFailure);
        }

        public void GetActivities(PagingQuery<ActivitiesQuery> query, Action<PagingResult<Activity>> onSuccess, Action<GetSocialError> onFailure)
        {
            CallAsync("Communities.getActivities", GSJson.Serialize(query), onSuccess, onFailure);
        }

        public void GetActivity(string id, Action<Activity> onSuccess, Action<GetSocialError> onFailure)
        {
            CallAsync("Communities.getActivity", id, onSuccess, onFailure);
        }

        public void PostActivity(ActivityContent content, PostActivityTarget target, Action<Activity> onSuccess, Action<GetSocialError> onFailure)
        {
            CallAsync("Communities.postActivity", GSJson.Serialize(new PostActivityBody{Target = target, Content = content}), onSuccess, onFailure);
        }
        
        public void UpdateActivity(string id, ActivityContent content, Action<Activity> onSuccess, Action<GetSocialError> onFailure)
        {
            CallAsync("Communities.updateActivity", GSJson.Serialize(new UpdateActivityBody{Target = id, Content = content}), onSuccess, onFailure);
        }

        public void AddReaction(string reaction, string activityId, Action success, Action<GetSocialError> failure)
        {
            CallAsyncVoid("Communities.addReaction", GSJson.Serialize(new AddReactionBody{ Reaction = reaction, ActivityId = activityId}), success, failure);
        }
        
        public void RemoveReaction(string reaction, string activityId, Action success, Action<GetSocialError> failure)
        {
            CallAsyncVoid("Communities.removeReaction", GSJson.Serialize(new AddReactionBody{ Reaction = reaction, ActivityId = activityId}), success, failure);
        }
        public void GetReactions(PagingQuery<ReactionsQuery> query, Action<PagingResult<UserReactions>> onSuccess, Action<GetSocialError> onFailure)
        {
            CallAsync("Communities.getReactions", GSJson.Serialize(query), onSuccess, onFailure);
        }
        public void GetTags(TagsQuery query, Action<List<string>> onSuccess, Action<GetSocialError> onFailure)
        {
            CallAsync("Communities.getTags", GSJson.Serialize(query), onSuccess, onFailure);
        }
        public void ReportActivity(string activityId, ReportingReason reason, string explanation, Action onSuccess, Action<GetSocialError> onError)
        {
            CallAsyncVoid("Communities.reportActivity", GSJson.Serialize(new ReportActivityBody{ Id = activityId, Reason = reason, Explanation = explanation}), onSuccess, onError);
        }

        public void GetUsers(PagingQuery<UsersQuery> query, Action<PagingResult<User>> onSuccess, Action<GetSocialError> onFailure)
        {
            CallAsync("Communities.getUsers", GSJson.Serialize(query), onSuccess, onFailure);
        }

        public void GetUsers(UserIdList list, Action<Dictionary<string, User>> onSuccess, Action<GetSocialError> onFailure)
        {
            CallAsync("Communities.getUsersByIds", GSJson.Serialize(list), onSuccess, onFailure);
        }

        public void GetUsersCount(UsersQuery query, Action<int> success, Action<GetSocialError> error)
        {
            CallAsync("Communities.getUsersCount", GSJson.Serialize(query), success, error);
        }

        public void AddFriends(UserIdList userIds, Action<int> success, Action<GetSocialError> failure)
        {
            CallAsync("Communities.addFriends", GSJson.Serialize(userIds), success, failure);
        }

        public void RemoveFriends(UserIdList userIds, Action<int> success, Action<GetSocialError> failure)
        {
            CallAsync("Communities.removeFriends", GSJson.Serialize(userIds), success, failure);
        }

        public void GetFriends(PagingQuery<FriendsQuery> query, Action<PagingResult<User>> success, Action<GetSocialError> failure)
        {
            CallAsync("Communities.getFriends", GSJson.Serialize(query), success, failure);
        }

        public void GetFriendsCount(FriendsQuery query, Action<int> success,
            Action<GetSocialError> failure)
        {
            CallAsync("Communities.getFriendsCount", GSJson.Serialize(query), success, failure);
        }

        public void AreFriends(UserIdList userIds, Action<Dictionary<string, bool>> success, Action<GetSocialError> failure)
        {
            CallAsync("Communities.areFriends", GSJson.Serialize(userIds), success, failure);
        }

        public void IsFriend(UserId userId, Action<bool> success, Action<GetSocialError> failure)
        {
            CallAsync("Communities.isFriend", GSJson.Serialize(userId), success, failure);
        }

        public void SetFriends(UserIdList userIds, Action<int> success, Action<GetSocialError> failure)
        {
            CallAsync("Communities.setFriends", GSJson.Serialize(userIds), success, failure);
        }

        public void GetFriendsCount(FriendsQuery query, Action<PagingResult<User>> success, Action<GetSocialError> failure)
        {
            CallAsync("Communities.getFriendsCount", GSJson.Serialize(query), success, failure);
        }

        public void GetSuggestedFriends(SimplePagingQuery query, Action<PagingResult<SuggestedFriend>> success, Action<GetSocialError> failure)
        {
            CallAsync("Communities.getSuggestedFriends", GSJson.Serialize(query), success, failure);
        }

        public void GetTopic(string topic, Action<Topic> success, Action<GetSocialError> failure)
        {
            CallAsync("Communities.getTopic", topic, success, failure);
        }

        public void GetTopics(PagingQuery<TopicsQuery> pagingQuery, Action<PagingResult<Topic>> success, Action<GetSocialError> failure)
        {
            CallAsync("Communities.getTopics", GSJson.Serialize(pagingQuery), success, failure);
        }

        public void GetTopicsCount(TopicsQuery query, Action<int> success, Action<GetSocialError> failure)
        {
            CallAsync("Communities.getTopicsCount", GSJson.Serialize(query), success, failure);
        }

        public void CreateGroup(GroupContent content, Action<Group> success, Action<GetSocialError> failure)
        {
            CallAsync("Communities.createGroup", GSJson.Serialize(content), success, failure);
        }

        public void UpdateGroup(string groupId, GroupContent content, Action<Group> success, Action<GetSocialError> failure)
        {
            CallAsync("Communities.updateGroup", GSJson.Serialize(new UpdateGroupBody { GroupId = groupId, Content = content }), success, failure);
        }

        public void RemoveGroups(List<string> groupIds, Action success, Action<GetSocialError> failure)
        {
            CallAsyncVoid("Communities.removeGroups", GSJson.Serialize(groupIds), success, failure);
        }

        public void GetGroupMembers(PagingQuery<MembersQuery> pagingQuery, Action<PagingResult<GroupMember>> success, Action<GetSocialError> failure)
        {
            CallAsync("Communities.getGroupMembers", GSJson.Serialize(pagingQuery), success, failure);
        }

        public void GetGroups(PagingQuery<GroupsQuery> pagingQuery, Action<PagingResult<Group>> success, Action<GetSocialError> failure)
        {
            CallAsync("Communities.getGroups", GSJson.Serialize(pagingQuery), success, failure);
        }

        public void GetGroupsCount(GroupsQuery query, Action<int> success, Action<GetSocialError> failure)
        {
            CallAsync("Communities.getGroupsCount", GSJson.Serialize(query), success, failure);
        }

        public void GetGroup(string groupId, Action<Group> success, Action<GetSocialError> failure)
        {
            CallAsync("Communities.getGroup", (groupId), success, failure);
        }

        public void UpdateGroupMembers(UpdateGroupMembersQuery query, Action<List<GroupMember>> success, Action<GetSocialError> failure)
        {
            CallAsync("Communities.updateGroupMembers", GSJson.Serialize(query), success, failure);
        }

        public void AddGroupMembers(AddGroupMembersQuery query, Action<List<GroupMember>> success, Action<GetSocialError> failure)
        {
            CallAsync("Communities.addGroupMembers", GSJson.Serialize(query), success, failure);
        }

        public void JoinGroup(JoinGroupQuery query, Action<GroupMember> success, Action<GetSocialError> failure)
        {
            CallAsync("Communities.updateGroupMembers", GSJson.Serialize(query.InternalQuery), (List<GroupMember> groupMembers) => {
                success(groupMembers.ToArray()[0]);
            }, failure);
        }

        public void RemoveGroupMembers(RemoveGroupMembersQuery query, Action success, Action<GetSocialError> failure)
        {
            CallAsyncVoid("Communities.removeGroupMembers", GSJson.Serialize(query), success, failure);
        }

        public void AreGroupMembers(string groupId, UserIdList userIdList, Action<Dictionary<string, MemberRole>> success, Action<GetSocialError> failure)
        {
            CallAsync("Communities.areGroupMembers", GSJson.Serialize(new AreGroupMembersBody { GroupId = groupId, UserIdList = userIdList }), success, failure);
        }

        public void Follow(FollowQuery query, Action<int> success, Action<GetSocialError> failure)
        {
            CallAsync("Communities.follow", GSJson.Serialize(query), success, failure);
        }

        public void Unfollow(FollowQuery query, Action<int> success, Action<GetSocialError> failure)
        {
            CallAsync("Communities.unfollow", GSJson.Serialize(query), success, failure);
        }

        public void IsFollowing(UserId userId, FollowQuery query, Action<Dictionary<string, bool>> success, Action<GetSocialError> failure)
        {
            CallAsync("Communities.isFollowing", GSJson.Serialize(new IsFollowingBody { UserId = userId, Query = query }), success, failure);
        }

        public void GetFollowers(PagingQuery<FollowersQuery> query, Action<PagingResult<User>> success, Action<GetSocialError> failure)
        {
            CallAsync("Communities.getFollowers", GSJson.Serialize(query), success, failure);
        }

        public void GetFollowersCount(FollowersQuery query, Action<int> success, Action<GetSocialError> failure)
        {
            CallAsync("Communities.getFollowersCount", GSJson.Serialize(query), success, failure);
        }

        #region Invites
        public void GetAvailableChannels(Action<List<InviteChannel>> success, Action<GetSocialError> failure)
        {
            CallAsync("Invites.getAvailableChannels", null, success, failure);
        }

        public void Send(InviteContent customInviteContent, string channelId, Action success, Action cancel, Action<GetSocialError> failure)
        {
            CallAsync("Invites.send", GSJson.Serialize(new SendInviteBody { ChannelId = channelId, Content = customInviteContent }), (string result) => {
                if ("cancel".Equals(result))
                {
                    cancel();
                }
                else
                {
                    success();
                }
            }, failure);
        }

        public void Create(InviteContent customInviteContent, Action<Invite> success, Action<GetSocialError> failure)
        {
            CallAsync("Invites.create", GSJson.Serialize(customInviteContent), success, failure);
        }

        public void CreateLink(Dictionary<string, object> linkParams, Action<string> success, Action<GetSocialError> failure)
        {
            linkParams = ImageToBase64(linkParams);
            CallAsync("Invites.createLink", GSJson.Serialize(linkParams), success, failure);
        }
        
        private static Dictionary<string, object> ImageToBase64(Dictionary<string, object> linkParams)
        {
            if (linkParams == null)
            {
                return null;
            }
            var result = new Dictionary<string, object>();
            foreach (var kv in linkParams)
            {
                if (kv.Value is Texture2D)
                {
                    result[kv.Key + "64"] = (kv.Value as Texture2D).TextureToBase64();
                } else
                {
                    result[kv.Key] = kv.Value;
                }
            }
            return result;
        }

        public void GetReferredUsers(PagingQuery<ReferralUsersQuery> query, Action<PagingResult<ReferralUser>> success, Action<GetSocialError> failure)
        {
            CallAsync("Invites.getReferredUsers", GSJson.Serialize(query), success, failure);
        }

        public void GetReferrerUsers(PagingQuery<ReferralUsersQuery> query, Action<PagingResult<ReferralUser>> success, Action<GetSocialError> failure)
        {
            CallAsync("Invites.getReferrerUsers", GSJson.Serialize(query), success, failure);
        }

        public void SetReferrer(UserId userId, string eventName, Dictionary<string, string> customData, Action success, Action<GetSocialError> failure)
        {
            CallAsyncVoid("Invites.setReferrer", GSJson.Serialize(new SetReferrerBody { UserId = userId, EventName = eventName, CustomData = customData }), success, failure);
        }

        public void SetOnReferralDataReceivedListener(Action<ReferralData> listener)
        {
            Action<ReferralData> wrapper = some => listener(some);
            MethodCaller.RegisterListener("Invites.setOnReferralDataReceivedListener", Json(wrapper));
        }

        #endregion

        #region Promo Codes

        public void CreatePromoCode(PromoCodeContent content, Action<PromoCode> success, Action<GetSocialError> failure)
        {
            CallAsync("PromoCodes.create", GSJson.Serialize(content), success, failure);
        }

        public void GetPromoCode(string code, Action<PromoCode> success, Action<GetSocialError> failure)
        {
            CallAsync("PromoCodes.get", code, success, failure);
        }

        public void ClaimPromoCode(string code, Action<PromoCode> success, Action<GetSocialError> failure)
        {
            CallAsync("PromoCodes.claim", code, success, failure);
        }

        #endregion

        #region Analytics

        public bool TrackPurchase(PurchaseData purchaseData)
        {
            return CallSync<bool>("Analytics.trackPurchase", GSJson.Serialize(purchaseData));
        }

        public bool TrackCustomEvent(string eventName, Dictionary<string, string> eventData)
        {
            return CallSync<bool>("Analytics.trackCustomEvent", GSJson.Serialize(new TrackCustomEventBody { EventName = eventName, EventData = eventData }));
        }

        #endregion

        #region Push notifications

        public void GetNotifications(PagingQuery<NotificationsQuery> pagingQuery, Action<PagingResult<Notification>> success, Action<GetSocialError> failure)
        {
            CallAsync("Notifications.get", GSJson.Serialize(pagingQuery), success, failure);
        }

        public void CountNotifications(NotificationsQuery query, Action<int> success, Action<GetSocialError> failure) {
            CallAsync("Notifications.count", GSJson.Serialize(query), success, failure);
        }

        public void SetStatus(string newStatus, List<string> notificationIds, Action success, Action<GetSocialError> failure) {
            CallAsyncVoid("Notifications.setStatus", GSJson.Serialize(new SetStatusBody { Status = newStatus, NotificationIds = notificationIds }), success, failure);
        }

        public void SetPushNotificationsEnabled(bool enabled, Action success, Action<GetSocialError> failure) {
            CallAsyncVoid("Notifications.setEnabled", GSJson.Serialize(enabled), success, failure);
        }

        public void ArePushNotificationsEnabled(Action<bool> success, Action<GetSocialError> failure) {
            CallAsync("Notifications.areEnabled", "", success, failure);
        }

        public void Send(NotificationContent content, SendNotificationTarget target, Action success, Action<GetSocialError> failure) {
            CallAsyncVoid("Notifications.send", GSJson.Serialize(new SendNotificationBody { Target = target, Content = content }), success, failure) ;
        }

        public void RegisterDevice() {
            CallSync<bool>("Notifications.registerDevice", "");
        }

        public void SetOnNotificationReceivedListener(Action<Notification> listener) {
            MethodCaller.RegisterListener("Notifications.setOnNotificationReceivedListener", Json(listener));
        }

        public void SetOnNotificationClickedListener(Action<Notification, NotificationContext> listener)
        {
            Action<NotificationListenerBody> wrapper = body =>
            {
                listener(body.Notification, body.Context);
            };
            MethodCaller.RegisterListener("Notifications.setOnNotificationClickedListener", Json(wrapper));
        }

        public void SetOnTokenReceivedListener(Action<string> listener) 
        {
            MethodCaller.RegisterListener("Notifications.setOnTokenReceivedListener", listener);
        }

        public void HandleOnStartUnityEvent()
        {
            MethodCaller.Call("GetSocial.handleOnStartUnityEvent", "");
        }

        #endregion

        private void CallAsync<R>(string method, string body, Action<R> success, Action<GetSocialError> failure)
        {
            MethodCaller.CallAsync(method, body, Json(success), Json(failure));
        }

        private void CallAsyncVoid(string method, string body, Action success, Action<GetSocialError> failure)
        {
            MethodCaller.CallAsync(method, body, any => success(), Json(failure));
        }

        private static Action<string> Json<T>(Action<T> callback)
        {
            return s => callback(FromJson<T>(s));
        }

        public static T FromJson<T>(string json)
        {
            if (json == null)
            {
#if UNITY_2018_1_OR_NEWER
                return default;
#else
                return default(T);
#endif
            }
            var obj = GSJson.Deserialize(json);
            if (obj != null && obj.GetType().IsGenericDictionary())
            {
                var dictionary = obj as Dictionary<string, object>;
                
                if (dictionary.Count == 1 && dictionary.ContainsKey("result"))
                {
                    return (T) Convert.ChangeType(dictionary["result"], typeof(T));
                }
            }
            return GSJson.ToObject<T>(obj);
        }

        private R CallSync<R>(string method, string body = "")
        {
            return FromJson<R>(MethodCaller.Call(method, body));
        }

    }

    internal interface IMethodCaller
    {
        string Call(string method, string body);
        void CallAsync(string method, string body, Action<string> success, Action<string> failure);
        string RegisterListener(string method, Action<string> listener);
    }

    public class PostActivityBody
    {
        [JsonSerializationKey("target")]
        public PostActivityTarget Target;
        [JsonSerializationKey("content")]
        public ActivityContent Content;
    }
    
    public class UpdateActivityBody
    {
        [JsonSerializationKey("target")]
        public string Target;
        [JsonSerializationKey("content")]
        public ActivityContent Content;
    }

    public class AddReactionBody
    {
        [JsonSerializationKey("reaction")]
        public string Reaction;
        [JsonSerializationKey("activityId")]
        public string ActivityId;
    }

    public class SendNotificationBody
    {
        [JsonSerializationKey("target")]
        public SendNotificationTarget Target;
        [JsonSerializationKey("content")]
        public NotificationContent Content;
    }

    public class UpdateGroupBody
    {
        [JsonSerializationKey("groupId")]
        public string GroupId;
        [JsonSerializationKey("content")]
        public GroupContent Content;
    }

    public class RemoveGroupMembersBody
    {
        [JsonSerializationKey("groupId")]
        public string GroupId;
        [JsonSerializationKey("query")]
        public RemoveGroupMembersQuery Query;
    }

    public class AreGroupMembersBody
    {
        [JsonSerializationKey("groupId")]
        public string GroupId;
        [JsonSerializationKey("userIdList")]
        public UserIdList UserIdList;
    }

    public class IsFollowingBody
    {
        [JsonSerializationKey("userId")]
        public UserId UserId;
        [JsonSerializationKey("query")]
        public FollowQuery Query;
    }

    public class SendInviteBody
    {
        [JsonSerializationKey("channelId")]
        public string ChannelId;
        [JsonSerializationKey("content")]
        public InviteContent Content;
    }

    public class SetReferrerBody
    {
        [JsonSerializationKey("userId")]
        public UserId UserId;
        [JsonSerializationKey("eventName")]
        public string EventName;
        [JsonSerializationKey("customData")]
        public Dictionary<string, string> CustomData;
    }

    public class TrackCustomEventBody
    {
        [JsonSerializationKey("eventName")]
        public string EventName;
        [JsonSerializationKey("eventData")]
        public Dictionary<string, string> EventData;
    }

    public class SetStatusBody
    {
        [JsonSerializationKey("status")]
        public string Status;
        [JsonSerializationKey("notificationIds")]
        public List<string> NotificationIds;
    }

    public class NotificationListenerBody
    {
        [JsonSerializationKey("notification")]
        public Notification Notification;
        [JsonSerializationKey("context")]
        public NotificationContext Context;
    }

    public class ReportActivityBody
    {
        [JsonSerializationKey("activityId")]
        public string Id;
        [JsonSerializationKey("reason")]
        public ReportingReason Reason;
        [JsonSerializationKey("explanation")]
        public string Explanation;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace GetSocialSdk.Core
{
    public static class Communities
    {
        #region Activities
        public static void GetAnnouncements(AnnouncementsQuery query, Action<List<Activity>> onSuccess,
            Action<GetSocialError> onFailure)
        {
            GetSocialFactory.Bridge.GetAnnouncements(query, onSuccess, onFailure);
        }

        public static void GetActivities(PagingQuery<ActivitiesQuery> query, Action<PagingResult<Activity>> onSuccess,
            Action<GetSocialError> onFailure)
        {
            GetSocialFactory.Bridge.GetActivities(query, onSuccess, onFailure);
        }

        public static void GetActivity(string id, Action<Activity> onSuccess, Action<GetSocialError> onFailure)
        {
            GetSocialFactory.Bridge.GetActivity(id, onSuccess, onFailure);
        }

        public static void PostActivity(ActivityContent content, PostActivityTarget target, Action<Activity> onSuccess,
            Action<GetSocialError> onFailure)
        {
            GetSocialFactory.Bridge.PostActivity(content, target, onSuccess, onFailure);
        }

        public static void UpdateActivity(string id, ActivityContent content, Action<Activity> onSuccess,
            Action<GetSocialError> onFailure)
        {
            GetSocialFactory.Bridge.UpdateActivity(id, content, onSuccess, onFailure);
        }

        public static void RemoveActivities(RemoveActivitiesQuery query, Action onSuccess,
    Action<GetSocialError> onFailure)
        {
            GetSocialFactory.Bridge.RemoveActivities(query, onSuccess, onFailure);
        }

        public static void AddReaction(string reaction, string activityId, Action success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.AddReaction(reaction, activityId, success, failure);
        }
        
        public static void RemoveReaction(string reaction, string activityId, Action success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.RemoveReaction(reaction, activityId, success, failure);
        }

        public static void GetReactions(PagingQuery<ReactionsQuery> query, Action<PagingResult<UserReactions>> onSuccess,
            Action<GetSocialError> onFailure)
        {
            GetSocialFactory.Bridge.GetReactions(query, onSuccess, onFailure);
        }
        
        public static void GetTags(TagsQuery query, Action<List<string>> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.GetTags(query, success, failure);
        }

        public static void ReportActivity(string activityId, ReportingReason reason, string explanation, Action onSuccess, Action<GetSocialError> onError)
        {
            GetSocialFactory.Bridge.ReportActivity(activityId, reason, explanation, onSuccess, onError);
        }
        #endregion

        #region Polls
        public static void AddVotes(HashSet<string> pollOptionIds, string activityId, Action onSuccess, Action<GetSocialError> onError)
        {
            GetSocialFactory.Bridge.AddVotes(pollOptionIds, activityId, onSuccess, onError);
        }

        public static void SetVotes(HashSet<string> pollOptionIds, string activityId, Action onSuccess, Action<GetSocialError> onError)
        {
            GetSocialFactory.Bridge.SetVotes(pollOptionIds, activityId, onSuccess, onError);
        }

        public static void RemoveVotes(HashSet<string> pollOptionIds, string activityId, Action onSuccess, Action<GetSocialError> onError)
        {
            GetSocialFactory.Bridge.RemoveVotes(pollOptionIds, activityId, onSuccess, onError);
        }

        public static void GetVotes(PagingQuery<VotesQuery> query, Action<PagingResult<UserVotes>> onSuccess, Action<GetSocialError> onError)
        {
            GetSocialFactory.Bridge.GetVotes(query, onSuccess, onError);
        }
        #endregion

        #region Users
        public static void GetUsers(PagingQuery<UsersQuery> query, Action<PagingResult<User>> onSuccess,
            Action<GetSocialError> onFailure)
        {
            GetSocialFactory.Bridge.GetUsers(query, onSuccess, onFailure);
        }
        
        public static void GetUsersCount(UsersQuery query, Action<int> success, Action<GetSocialError> error)
        {
            GetSocialFactory.Bridge.GetUsersCount(query, success, error);
        }

        public static void GetUsers(UserIdList list, Action<Dictionary<string, User>> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.GetUsers(list, success, failure);
        }

        public static void GetUser(UserId userId, Action<User> success, Action<GetSocialError> failure)
        {
            GetUsers(UserIdList.CreateWithProvider(userId.Provider, userId.Id), result =>
            {
                if (result.Count == 1)
                {
                    success(result.First().Value);
                }
                else
                {
                    failure(new GetSocialError(ErrorCodes.IllegalArgument, $"User does not exist: {userId}"));
                }
            }, failure);
        }
        #endregion

        #region Friends
        public static void SetFriends(UserIdList userIds, Action<int> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.SetFriends(userIds, success, failure);
        }
        public static void AddFriends(UserIdList userIds, Action<int> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.AddFriends(userIds, success, failure);
        }

        public static void RemoveFriends(UserIdList userIds, Action<int> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.RemoveFriends(userIds, success, failure);
        }

        public static void GetFriends(PagingQuery<FriendsQuery> query, Action<PagingResult<User>> success,
            Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.GetFriends(query, success, failure);
        }

        public static void GetFriendsCount(FriendsQuery query, Action<int> success,
            Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.GetFriendsCount(query, success, failure);
        }
        
        public static void AreFriends(UserIdList userIds, Action<Dictionary<string, bool>> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.AreFriends(userIds, success, failure);
        }
        
        public static void IsFriend(UserId userId, Action<bool> success, Action<GetSocialError> failure)
        {
            AreFriends(UserIdList.CreateWithProvider(userId.Provider, userId.Id), result =>
            {
                if (result.ContainsKey(userId.Id))
                {
                    success(result[userId.Id]);
                }
                else
                {
                    failure(new GetSocialError(ErrorCodes.IllegalArgument, $"User does not exist: {userId}"));
                }
            }, failure);
        }

        public static void GetSuggestedFriends(SimplePagingQuery query, Action<PagingResult<SuggestedFriend>> success,
            Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.GetSuggestedFriends(query, success, failure);
        }
        #endregion
        #region Topics

        public static void GetTopic(string topic, Action<Topic> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.GetTopic(topic, success, failure);
        }

        public static void GetTopics(PagingQuery<TopicsQuery> pagingQuery, Action<PagingResult<Topic>> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.GetTopics(pagingQuery, success, failure);
        }

        public static void GetTopicsCount(TopicsQuery query, Action<int> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.GetTopicsCount(query, success, failure);
        }

        #endregion

        #region Groups

        public static void CreateGroup(GroupContent content, Action<Group> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.CreateGroup(content, success, failure);
        }

        public static void UpdateGroup(string groupId, GroupContent content, Action<Group> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.UpdateGroup(groupId, content, success, failure);
        }

        public static void RemoveGroups(List<string> groupIds, Action success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.RemoveGroups(groupIds, success, failure);
        }

        public static void GetGroupMembers(PagingQuery<MembersQuery> pagingQuery, Action<PagingResult<GroupMember>> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.GetGroupMembers(pagingQuery, success, failure);
        }

        public static void GetGroups(PagingQuery<GroupsQuery> pagingQuery, Action<PagingResult<Group>> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.GetGroups(pagingQuery, success, failure);
        }

        public static void GetGroupsCount(GroupsQuery query, Action<int> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.GetGroupsCount(query, success, failure);
        }

        public static void GetGroup(string groupId, Action<Group> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.GetGroup(groupId, success, failure);
        }

        public static void UpdateGroupMembers(UpdateGroupMembersQuery query, Action<List<GroupMember>> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.UpdateGroupMembers(query, success, failure);
        }

        public static void AddGroupMembers(AddGroupMembersQuery query, Action<List<GroupMember>> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.AddGroupMembers(query, success, failure);
        }

        public static void JoinGroup(JoinGroupQuery query, Action<GroupMember> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.JoinGroup(query, success, failure);
        }

        public static void RemoveGroupMembers(RemoveGroupMembersQuery query, Action success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.RemoveGroupMembers(query, success, failure);
        }

        public static void AreGroupMembers(string groupId, UserIdList userIdList, Action<Dictionary<String, Membership>> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.AreGroupMembers(groupId, userIdList, success, failure);
        }

        #endregion

        #region Chat
        public static void SendChatMessage(ChatMessageContent content, ChatId target, Action<ChatMessage> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.SendChatMessage(content, target, success, failure);
        }

        public static void GetChatMessages(ChatMessagesPagingQuery pagingQuery, Action<ChatMessagesPagingResult> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.GetChatMessages(pagingQuery, success, failure);
        }

        public static void GetChats(SimplePagingQuery pagingQuery, Action<PagingResult<Chat>> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.GetChats(pagingQuery, success, failure);
        }

        public static void GetChat(ChatId chatId, Action<Chat> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.GetChat(chatId, success, failure);
        }

        #endregion

        #region Follow/Unfollow

        public static void Follow(FollowQuery query, Action<int> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.Follow(query, success, failure);
        }

        public static void Unfollow(FollowQuery query, Action<int> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.Unfollow(query, success, failure);
        }

        public static void IsFollowing(UserId userId, FollowQuery query, Action<Dictionary<string, bool>> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.IsFollowing(userId, query, success, failure);
        }

        public static void GetFollowers(PagingQuery<FollowersQuery> query, Action<PagingResult<User>> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.GetFollowers(query, success, failure);
        }

        public static void GetFollowersCount(FollowersQuery query, Action<int> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.GetFollowersCount(query, success, failure);
        }

        #endregion

    }
}

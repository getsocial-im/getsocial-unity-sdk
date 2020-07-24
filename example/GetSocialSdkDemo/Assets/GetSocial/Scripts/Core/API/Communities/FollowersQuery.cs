using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Builder for a query to retrieve groups.
    /// </summary>
    public sealed class FollowersQuery 
    {
        [JsonSerializationKey("ids")]
        internal CommunitiesIds Ids;

        FollowersQuery(CommunitiesIds ids)
        {
            this.Ids = ids;
        }
        
        /// <summary>
        /// Get followers of topic with ID. 
        /// </summary>
        /// <param name="topicId">Topic ID.</param>
        /// <returns>new query.</returns>
        public static FollowersQuery OfTopic(string topicId) 
        {
            return new FollowersQuery(CommunitiesIds.Topic(topicId));
        }

        /// <summary>
        /// Get followers of a group.
        /// </summary>
        /// <param name="groupId">Group ID.</param>
        /// <returns>new query.</returns>
        public static FollowersQuery OfGroup(string groupId) 
        {
            return new FollowersQuery(CommunitiesIds.Group(groupId));
        }

        /// <summary>
        /// Get followers of a user.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <returns>new query.</returns>
        public static FollowersQuery OfUser(UserId userId) 
        {
            return new FollowersQuery(CommunitiesIds.User(userId));
        }
    }
}

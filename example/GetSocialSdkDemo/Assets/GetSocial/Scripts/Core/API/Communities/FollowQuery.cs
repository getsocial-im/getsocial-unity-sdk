using System.Collections.Generic;
using System.Linq;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Builder for a query to follow entities.
    /// </summary>
    public sealed class FollowQuery 
    {
        [JsonSerializationKey("ids")]
        internal CommunitiesIds Ids;

        FollowQuery(CommunitiesIds ids)
        {
            this.Ids = ids;
        }
        
        /// <summary>
        /// Follow topics in the list.
        /// </summary>
        /// <param name="topicIds">List of topic IDs.</param>
        /// <returns>new query</returns>
        public static FollowQuery Topics(params string[] topicIds) 
        {
            return new FollowQuery(CommunitiesIds.Topics(topicIds.ToList()));
        }
        
        /// <summary>
        /// Follow topics in the list.
        /// </summary>
        /// <param name="topicIds">List of topic IDs.</param>
        /// <returns>new query</returns>
        public static FollowQuery Topics(List<string> topicIds) 
        {
            return new FollowQuery(CommunitiesIds.Topics(topicIds));
        }


        /// <summary>
        /// Follow groups in the list.
        /// </summary>
        /// <param name="groupIds">List of group IDs.</param>
        /// <returns>new query.</returns>
        public static FollowQuery Groups(List<string> groupIds) 
        {
            return new FollowQuery(CommunitiesIds.Groups(groupIds));
        }
        
        /// <summary>
        /// Follow groups in the list.
        /// </summary>
        /// <param name="groupIds">List of group IDs.</param>
        /// <returns>new query.</returns>
        public static FollowQuery Groups(params string[] groupIds) 
        {
            return new FollowQuery(CommunitiesIds.Groups(groupIds.ToList()));
        }
        
        /// <summary>
        /// Follow users in the list.
        /// </summary>
        /// <param name="userIdList">User IDs.</param>
        /// <returns>new query.</returns>
        public static FollowQuery Users(UserIdList userIdList) 
        {
            return new FollowQuery(CommunitiesIds.Users(userIdList));
        }
    }
}

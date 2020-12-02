using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public class PostActivityTarget
    {
        [JsonSerializationKey("ids")] internal CommunitiesIds Ids;

        private PostActivityTarget(CommunitiesIds ids) {
            Ids = ids;
        }

        /// <summary>
        /// Post comment to the activity.
        /// </summary>
        /// <param name="activityId">Activity ID.</param>
        /// <returns>new target.</returns>
        public static PostActivityTarget Comment(string activityId) {
            return new PostActivityTarget(CommunitiesIds.Activity(activityId));
        }

        /// <summary>
        /// Post activity in topic.
        /// </summary>
        /// <param name="topicId">Topic ID.</param>
        /// <returns>new target.</returns>
        public static PostActivityTarget Topic(string topicId) {
            return new PostActivityTarget(CommunitiesIds.Topic(topicId));
        }

        /// <summary>
        /// Post activity in group.
        /// </summary>
        /// <param name="groupId">Group ID.</param>
        /// <returns>new target.</returns>
        public static PostActivityTarget Group(string groupId) {
            return new PostActivityTarget(CommunitiesIds.Group(groupId));
        }

        /// <summary>
        /// Post to the feed of current user.
        /// </summary>
        /// <returns>new target.</returns>
        public static PostActivityTarget Timeline()
        {
            return new PostActivityTarget(CommunitiesIds.User(UserId.CurrentUser()));
        }
 
    }
}
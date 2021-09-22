using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core {
    /// <summary>
    /// Builder for a query to retrieve activity posts or comments.
    /// </summary>
    public sealed class ActivitiesQuery {
        [JsonSerializationKey ("ids")]
        internal CommunitiesIds Ids;

        [JsonSerializationKey ("author")]
        internal UserId Author;

        [JsonSerializationKey ("tag")]
        internal string Tag;

        [JsonSerializationKey("pollStatus")]
        internal int InternalPollStatus = PollStatus.All;

        [JsonSerializationKey("trending")]
        internal bool InternalTrending = false;

        private ActivitiesQuery (CommunitiesIds ids) {
            Ids = ids;
        }

        /// <summary>
        /// Get activities in a topic.
        /// </summary>
        /// <param name="topic">The name of the topic.</param>
        /// <returns>New query.</returns>
        public static ActivitiesQuery ActivitiesInTopic (string topic) {
            return new ActivitiesQuery (CommunitiesIds.Topic (topic));
        }

        /// <summary>
        /// Get comments to a certain activity.
        /// </summary>
        /// <param name="activityId">ID of the activity.</param>
        /// <returns>New query.</returns>
        public static ActivitiesQuery CommentsToActivity (string activityId) {
            return new ActivitiesQuery (CommunitiesIds.Activity (activityId));
        }

        /// <summary>
        /// Get activities in user's timeline.
        /// </summary>
        /// <returns>New query.</returns>
        public static ActivitiesQuery Timeline () {
            return new ActivitiesQuery (CommunitiesIds.App ("timeline"));
        }

        /// <summary>
        /// Get activities in user feed.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <returns>New query.</returns>
        public static ActivitiesQuery FeedOf (UserId userId) {
            return new ActivitiesQuery (CommunitiesIds.User (userId));
        }

        /// <summary>
        /// Get activities in a group.
        /// </summary>
        /// <param name="group">Name of the group.</param>
        /// <returns>New query.</returns>
        public static ActivitiesQuery ActivitiesInGroup (string group) {
            return new ActivitiesQuery (CommunitiesIds.Group (group));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ActivitiesQuery Everywhere () {
            return new ActivitiesQuery (CommunitiesIds.Everywhere ());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ActivitiesQuery InAllTopics () {
            return new ActivitiesQuery (CommunitiesIds.Topics (new System.Collections.Generic.List<string>()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ActivitiesQuery ByUser (UserId user) {
            Author = user;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public ActivitiesQuery WithTag (string tag) {
            Tag = tag;
            return this;
        }

        public ActivitiesQuery WithPollStatus(int pollStatus)
        {
            InternalPollStatus = pollStatus;
            return this;
        }

        public ActivitiesQuery OnlyTrending(bool trending)
        {
            this.InternalTrending = trending;
            return this;
        }
    }
}
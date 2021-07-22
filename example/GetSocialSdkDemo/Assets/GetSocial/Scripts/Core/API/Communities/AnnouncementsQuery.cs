using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Builder for a query to retrieve announcements.
    /// </summary>
    public sealed class AnnouncementsQuery 
    {
        [JsonSerializationKey("ids")]
        internal CommunitiesIds Ids;

        [JsonSerializationKey("pollStatus")]
        internal int InternalPollStatus = PollStatus.All;

        private AnnouncementsQuery(CommunitiesIds ids)
        {
            Ids = ids;
        }
        
        /// <summary>
        /// Get announcements for topic.
        /// </summary>
        /// <param name="topic"></param>
        /// <returns>new query.</returns>
        public static AnnouncementsQuery InTopic(string topic) {
            return new AnnouncementsQuery(CommunitiesIds.Topic(topic));
        }

        /// <summary>
        /// Get announcements for all feeds and user timeline.
        /// </summary>
        /// <returns>new query.</returns>
        public static AnnouncementsQuery Timeline() {
            return new AnnouncementsQuery(CommunitiesIds.App("timeline"));
        }

        /// <summary>
        /// Get announcements for a group.
        /// </summary>
        /// <param name="group"></param>
        /// <returns>new query.</returns>
        public static AnnouncementsQuery InGroup(string group)
        {
            return new AnnouncementsQuery(CommunitiesIds.Group(group));
        }

        /// <summary>
        /// Get announcements for user feed.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>new query.</returns>
        public static AnnouncementsQuery InUserFeed(UserId userId)
        {
            return new AnnouncementsQuery(CommunitiesIds.User(userId)); 
        }

        public AnnouncementsQuery WithPollStatus(int pollStatus)
        {
            InternalPollStatus = pollStatus;
            return this;
        }
    }
}
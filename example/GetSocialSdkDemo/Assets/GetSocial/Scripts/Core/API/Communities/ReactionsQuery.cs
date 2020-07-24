using System;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public class ReactionsQuery
    {
        [JsonSerializationKey("ids")]
        internal CommunitiesIds Ids;

        [JsonSerializationKey("reaction")]
        internal string Reaction;

        private ReactionsQuery(CommunitiesIds ids) {
            Ids = ids;
        }
        
        /// <summary>
        /// Get all users reacted to activity with ID.
        /// </summary>
        /// <param name="activityId">Activity ID.</param>
        /// <returns></returns>
        public static ReactionsQuery ForActivity(string activityId) {
            return new ReactionsQuery(CommunitiesIds.Activity(activityId));
        }

        /// <summary>
        /// Get only users reacted with specific reaction.
        /// </summary>
        /// <param name="reaction">name of the reaction to filter by.</param>
        /// <returns>the same query instance.</returns>
        public ReactionsQuery FilterByReaction(string reaction) {
            Reaction = reaction;
            return this;
        }
    }
}
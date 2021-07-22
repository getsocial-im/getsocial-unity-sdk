using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public class VotesQuery
    {
        [JsonSerializationKey("ids")]
        internal CommunitiesIds Ids;

        [JsonSerializationKey("pollOptionId")]
        internal string PollOptionId;

        private VotesQuery(CommunitiesIds ids) {
            Ids = ids;
        }

        /// <summary>
        /// Get all users voted to activity with ID.
        /// </summary>
        /// <param name="activityId">Activity ID.</param>
        /// <returns></returns>
        public static VotesQuery ForActivity(string activityId) {
            return new VotesQuery(CommunitiesIds.Activity(activityId));
        }

        /// <summary>
        /// Get only users voted for a specific poll option.
        /// </summary>
        /// <param name="reaction">id of poll option.</param>
        /// <returns>the same query instance.</returns>
        public VotesQuery WithPollOptionId(string pollOptionId) {
            PollOptionId = pollOptionId;
            return this;
        }
    }
}
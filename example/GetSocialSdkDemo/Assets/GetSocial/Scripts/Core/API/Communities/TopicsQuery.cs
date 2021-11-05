using System.Collections.Generic;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Builder for a query to retrieve topics.
    /// </summary>
    public sealed class TopicsQuery 
    {
        [JsonSerializationKey("searchTerm")]
        internal string SearchTerm;

        [JsonSerializationKey("followerId")]
        internal UserId FollowerId;

        [JsonSerializationKey("trending")]
        internal bool InternalTrending = false;

        [JsonSerializationKey("labels")]
        internal List<string> Labels;

        [JsonSerializationKey("properties")]
        internal Dictionary<string, string> Properties;

        TopicsQuery(string searchTerm)
        {
            this.SearchTerm = searchTerm;
        }
        
        /// <summary>
        /// Find topics by name or description.
        /// </summary>
        /// <param name="searchTerm">topics name/description or part of it.</param>
        /// <returns>new query</returns>
        public static TopicsQuery Find(string searchTerm) {
            return new TopicsQuery(searchTerm);
        }

        /// <summary>
        /// Get all topics.
        /// </summary>
        /// <returns>new query.</returns>
        public static TopicsQuery All() {
            return new TopicsQuery("");
        }

        /// <summary>
        /// Get topics followed by a certain user.
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>new query.</returns>
        public static TopicsQuery FollowedBy(UserId userId)
        {
            var query = All();
            query.FollowerId = userId;
            return query;
        }

        public TopicsQuery OnlyTrending(bool trending)
        {
            this.InternalTrending = trending;
            return this;
        }

        public TopicsQuery WithLabels(List<string> labels)
        {
            this.Labels = labels;
            return this;
        }

        public TopicsQuery WithProperties(Dictionary<string, string> properties)
        {
            this.Properties = properties;
            return this;
        }

        public override string ToString()
        {
            return $"SearchTerm: {SearchTerm}, FollowerId: {FollowerId}, Trending: {InternalTrending}, Labels: {Labels}, Properties: {Properties}";
        }
    }
}

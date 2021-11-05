using System.Collections.Generic;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Builder for a query to retrieve groups.
    /// </summary>
public sealed class GroupsQuery 
    {
        [JsonSerializationKey("searchTerm")]
        internal string SearchTerm;

        [JsonSerializationKey("followerId")]
        internal UserId FollowerId;

        [JsonSerializationKey("memberId")]
        internal UserId MemberId;

        [JsonSerializationKey("trending")]
        internal bool InternalTrending = false;

        [JsonSerializationKey("labels")]
        internal List<string> Labels;

        [JsonSerializationKey("properties")]
        internal Dictionary<string, string> Properties;

        GroupsQuery(string searchTerm)
        {
            this.SearchTerm = searchTerm;
        }
        
        public static GroupsQuery Find(string searchTerm) {
            return new GroupsQuery(searchTerm);
        }

        public static GroupsQuery All() {
            return new GroupsQuery(null);
        }

        public GroupsQuery FollowedBy(UserId userId) {
            this.FollowerId = userId;
            return this;
        }

        public GroupsQuery ByMember(UserId userId) {
            this.MemberId = userId;
            return this;
        }

        public GroupsQuery OnlyTrending(bool trending)
        {
            this.InternalTrending = trending;
            return this;
        }

        public GroupsQuery WithLabels(List<string> labels)
        {
            this.Labels= labels;
            return this;
        }

        public GroupsQuery WithProperties(Dictionary<string, string> properties)
        {
            this.Properties = properties;
            return this;
        }

        public override string ToString()
        {
            return $"SearchTerm: {SearchTerm}, FollowerId: {FollowerId}, MemberId: {MemberId}, Trending: {InternalTrending}, Labels: {Labels}, Properties: {Properties}";
        }

    }
}

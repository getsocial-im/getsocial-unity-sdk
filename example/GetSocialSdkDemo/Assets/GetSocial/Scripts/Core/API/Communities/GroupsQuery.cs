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

    }
}

using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Builder for a query to retrieve group members.
    /// </summary>
    public sealed class GroupMembersQuery 
    {
        [JsonSerializationKey("groupId")]
        internal string GroupId;

        [JsonSerializationKey("role")]
        internal MembershipRole Role;

        [JsonSerializationKey("status")]
        internal MembershipStatus Status;

        GroupMembersQuery(string groupId)
        {
            this.GroupId = groupId;
        }
        
        public static GroupMembersQuery OfGroup(string groupId) {
            return new GroupMembersQuery(groupId);
        }

        public GroupMembersQuery WithStatus(MembershipStatus status)
        {
            Status = status;
            return this;
        }

        public GroupMembersQuery WithRole(MembershipRole role)
        {
            Role = role;
            return this;
        }
    }
}
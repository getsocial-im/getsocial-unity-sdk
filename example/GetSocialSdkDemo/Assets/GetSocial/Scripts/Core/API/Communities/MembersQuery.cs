using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Builder for a query to retrieve group members.
    /// </summary>
    public sealed class MembersQuery 
    {
        [JsonSerializationKey("groupId")]
        internal string GroupId;

        [JsonSerializationKey("role")]
        internal MemberRole? Role;

        [JsonSerializationKey("status")]
        internal MemberStatus? Status;

        MembersQuery(string groupId)
        {
            this.GroupId = groupId;
        }
        
        public static MembersQuery OfGroup(string groupId) {
            return new MembersQuery(groupId);
        }

        public MembersQuery WithStatus(MemberStatus status)
        {
            Status = status;
            return this;
        }

        public MembersQuery WithRole(MemberRole role)
        {
            Role = role;
            return this;
        }
    }
}
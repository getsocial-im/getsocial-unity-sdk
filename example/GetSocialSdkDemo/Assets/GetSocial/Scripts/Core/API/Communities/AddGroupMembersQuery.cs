using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Builder for a query to update group members.
    /// </summary>
    public sealed class AddGroupMembersQuery
    {
        [JsonSerializationKey("internalQuery")]
        internal UpdateGroupMembersQuery InternalQuery;

        public AddGroupMembersQuery(string groupId, UserIdList userIdList)
        {
            this.InternalQuery = new UpdateGroupMembersQuery(groupId, userIdList);
        }

        public AddGroupMembersQuery WithMemberRole(MemberRole memberRole)
        {
            this.InternalQuery.Role = memberRole;
            return this;
        }

        public AddGroupMembersQuery WithMemberStatus(MemberStatus memberStatus)
        {
            this.InternalQuery.Status = memberStatus;
            return this;
        }

    }
}
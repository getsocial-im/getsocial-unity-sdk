using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Builder for a query to update group members.
    /// </summary>
    public sealed class UpdateGroupMembersQuery 
    {
        [JsonSerializationKey("groupId")]
        internal string GroupId;

        [JsonSerializationKey("userIdList")]
        internal UserIdList UserIdList;

        [JsonSerializationKey("status")]
        internal MemberStatus Status = MemberStatus.Member;

        [JsonSerializationKey("role")]
        internal MemberRole? Role = MemberRole.Member;

        [JsonSerializationKey("invitationToken")]
        internal string InvitationToken;

        public UpdateGroupMembersQuery(string groupId, UserIdList userIdList)
        {
            this.UserIdList = userIdList;
            this.GroupId = groupId;
        }

        public UpdateGroupMembersQuery WithMemberStatus(MemberStatus memberStatus)
        {
            this.Status = memberStatus;
            return this;
        }

        public UpdateGroupMembersQuery WithMemberRole(MemberRole memberRole)
        {
            this.Role = memberRole;
            return this;
        }

        public override string ToString()
        {
            return $"GroupID: {GroupId}, UserIdList: {UserIdList}, Role: {Role}, Status: {Status}, InvitationToken: {InvitationToken}";
        }

    }
}
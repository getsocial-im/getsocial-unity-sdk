using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Builder for a query to update group members.
    /// </summary>
    public sealed class JoinGroupQuery
    {
        [JsonSerializationKey("internalQuery")]
        internal UpdateGroupMembersQuery InternalQuery;

        public JoinGroupQuery(string groupId)
        {
            this.InternalQuery = new UpdateGroupMembersQuery(groupId, UserId.CurrentUser().ToUserIdList());
            this.InternalQuery.Status = MemberStatus.ApprovalPending;
            this.InternalQuery.Role = MemberRole.Member;
        }

        public JoinGroupQuery WithInvitationToken(string invitationToken)
        {
            this.InternalQuery.InvitationToken = invitationToken;
            return this;
        }
    }
}
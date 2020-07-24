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
        internal MembershipStatus Status;

        public UpdateGroupMembersQuery(string groupId, UserIdList userIdList, MembershipStatus status)
        {
            this.UserIdList = userIdList;
            this.GroupId = groupId;
            this.Status = status;
        }
        
    }
}
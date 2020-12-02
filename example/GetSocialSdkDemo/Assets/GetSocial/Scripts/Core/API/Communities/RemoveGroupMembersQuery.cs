using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Builder for a query to remove group members.
    /// </summary>
    public sealed class RemoveGroupMembersQuery 
    {
        [JsonSerializationKey("groupId")]
        internal string GroupId;

        [JsonSerializationKey("userIdList")]
        internal UserIdList UserIdList;

        public RemoveGroupMembersQuery(string groupId, UserIdList userIdList)
        {
            this.UserIdList = userIdList;
            this.GroupId = groupId;
        }
    }
}
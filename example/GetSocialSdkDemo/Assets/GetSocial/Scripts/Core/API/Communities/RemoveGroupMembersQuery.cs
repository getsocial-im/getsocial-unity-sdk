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

        RemoveGroupMembersQuery(UserIdList userIdList, string groupId)
        {
            this.UserIdList = userIdList;
            this.GroupId = groupId;
        }
        
        public static RemoveGroupMembersQuery Users(UserIdList userIdList, string groupId) {
            return new RemoveGroupMembersQuery(userIdList, groupId);
        }
    }
}
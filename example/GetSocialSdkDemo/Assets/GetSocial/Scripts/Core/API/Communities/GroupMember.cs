using System.Collections.Generic;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public class GroupMember: User
    {
        [JsonSerializationKey("membership")]
        public MembershipInfo Membership { get; internal set; }
    }
}
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public class GroupMember: User
    {
        [JsonSerializationKey("membership")]
        public Membership Membership { get; internal set; }

        public override string ToString()
        {
            return $"User: {base.ToString()}, Membership: {Membership}";
        }
    }
}
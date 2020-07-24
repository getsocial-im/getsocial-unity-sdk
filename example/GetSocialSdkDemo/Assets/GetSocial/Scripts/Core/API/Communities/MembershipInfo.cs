using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// MembershipInfo entity.
    /// </summary>
    public sealed class MembershipInfo
    {
        /// <summary>
        /// Membership role.
        /// </summary>
        [JsonSerializationKey("role")]
        public MembershipRole Role { get; internal set; }

        [JsonSerializationKey("status")]
        public MembershipStatus Status { get; internal set; }

        [JsonSerializationKey("createdAt")]
        public long CreatedAt { get; internal set; }

        [JsonSerializationKey("invitationToken")]
        public string InvitationToken { get; internal set; }

        public MembershipInfo()
        {

        }
    }
}
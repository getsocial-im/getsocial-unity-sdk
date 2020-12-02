using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// MembershipInfo entity.
    /// </summary>
    public sealed class Membership
    {
        /// <summary>
        /// Membership role.
        /// </summary>
        [JsonSerializationKey("role")]
        public MemberRole Role { get; internal set; }

        [JsonSerializationKey("status")]
        public MemberStatus Status { get; internal set; }

        [JsonSerializationKey("createdAt")]
        public long CreatedAt { get; internal set; }

        [JsonSerializationKey("invitationToken")]
        public string InvitationToken { get; internal set; }

        public Membership()
        {

        }

        public override string ToString()
        {
            return $"CreatedAt: {CreatedAt}, Role: {Role}, Status: {Status}, InvitationToken: {InvitationToken}";
        }

    }
}
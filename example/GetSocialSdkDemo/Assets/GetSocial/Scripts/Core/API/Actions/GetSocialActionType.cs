namespace GetSocialSdk.Core
{
    public static class GetSocialActionType
    {
        /// <summary>
        /// Custom action.
        /// </summary>
        public const string Custom = "custom";

        /// <summary>
        /// Profile with provided identifier should be opened.
        /// </summary>
        public const string OpenProfile = "open_profile";

        /// <summary>
        /// Activity with provided identifier should be opened.
        /// </summary>
        public const string OpenActivity = "open_activity";
            
        /// <summary>
        /// Open Smart Invites action.
        /// </summary>
        public const string OpenInvites = "open_invites";
            
        /// <summary>
        /// Open URL.
        /// </summary>
        public const string OpenUrl = "open_url";
        
        /// <summary>
        /// Add friend.
        /// </summary>
        public const string AddFriend = "add_friend";
        
        /// <summary>
        /// Claim Promo Code.
        /// </summary>
        public const string ClaimPromoCode = "claim_promo_code";

        /// <summary>
        /// Add Group Member.
        /// </summary>
        public const string AddGroupMember = "add_group_member";

        /// <summary>
        /// Open Chat.
        /// </summary>
        public const string OpenChat = "open_chat";

    }
}
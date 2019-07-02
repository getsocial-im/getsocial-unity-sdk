namespace GetSocialSdk.Core
{
    /// <summary>
    /// Invite placeholder tags that will be replaced with their value if included in invite.
    /// </summary>
    public static class InviteTextPlaceholders
    {
        /// <summary>
        /// This tag is replaced with url to download the app.
        /// </summary>
        public const string PlaceholderAppInviteUrl = "[APP_INVITE_URL]";

        /// <summary>
        /// Tag is replaced with current user name.
        /// </summary>
        public static string PlaceholderUserName = "[USER_NAME]";

        /// <summary>
        /// Tag is replaced with promo code from LinkParams if any present.
        /// </summary>
        public static string PlaceholderPromoCode = "[PROMO_CODE]";
    }
}
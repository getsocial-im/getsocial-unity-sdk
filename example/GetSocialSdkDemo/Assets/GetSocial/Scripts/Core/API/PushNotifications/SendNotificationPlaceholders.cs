namespace GetSocialSdk.Core
{
    public static class SendNotificationPlaceholders
    {
        public static class Receivers
        {
            /// <summary>
            /// Send notifications to all friends.
            /// </summary>
            public const string Friends = "friends";

            /// <summary>
            /// Send notifications to all users that were invited by you.
            /// </summary>
            public const string ReferredUsers = "referred_users";

            /// <summary>
            /// Send notification to user who invited you - if exists.
            /// </summary>
            public const string Referrer = "referrer";
        }

        public static class CustomText
        {
            /// <summary>
            ///  Will be replaced with your actual display name in the notification text.
            /// </summary>
            public const string SenderDisplayName = "[SENDER_DISPLAY_NAME]";

            /// <summary>
            /// Will be replaced with received actual display name in the notification text.
            /// </summary>
            public const string ReceiverDisplayName = "[RECEIVER_DISPLAY_NAME]";
        }
    }
}
namespace GetSocialSdk.Core
{
    public static class GetSocialActionKeys
    {
        /// <summary>
        /// TopicId, UserId and ActivityId are mutually exclusive. 
        /// If present TopicId - open topic feed. If UserId - open users feed. If ActivityID - open activity.
        /// CommendId is optional and could be addition to ActivityId only.
        /// </summary>
        public static class OpenActivity
        {
            public const string ActivityId = "$activity_id";
            public const string CommentId = "$comment_id";
            public const string TopicId = "$topic_id";
            public const string UserId = "$user_id";

        }

        public static class OpenProfile
        {
            public const string UserId = "$user_id";
        }

        public static class OpenUrl
        {
            public const string Url = "$url";
        }
        
        public static class AddFriend
        {
            public const string UserId = "$user_id";
        }

        
        public static class ClaimPromoCode
        {
            public const string PromoCode = "$promo_code";
        }

        public static class AddGroupMember
        {
            public const string GroupId = "$group_id";
            public const string UserId = "$user_id";
            public const string InvitationToken = "$invitation_token";
            public const string Role = "$role";
            public const string Status = "$status";
        }

        public static class OpenChat
        {
            public const string ChatId = "$chat_id";
        }

    }
}
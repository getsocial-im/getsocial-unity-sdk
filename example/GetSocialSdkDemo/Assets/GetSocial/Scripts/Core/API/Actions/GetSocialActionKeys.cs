namespace GetSocialSdk.Core
{
    public static class GetSocialActionKeys
    {
        /// <summary>
        /// FeedName and ActivityId are mutually exclusive.
        /// CommendId is optional and could be addition to ActivityId only.
        /// </summary>
        public static class OpenActivity
        {
            public const string ActivityId = "$activity_id";
            public const string CommentId = "$comment_id";
            
            public const string FeedName = "$feed_name";
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

    }
}
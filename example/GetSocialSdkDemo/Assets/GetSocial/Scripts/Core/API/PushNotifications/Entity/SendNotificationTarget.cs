
using System;
using System.Collections.Generic;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public sealed class NotificationReceiverPlaceholders
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

        /// <summary>
        /// Send notification to all users who follows the current user.
        /// </summary>
        public const string Followers = "followers";
    }

    public sealed class SendNotificationTarget
    {

        [JsonSerializationKey("userIdList")]
        internal UserIdList UserIdList;

        [JsonSerializationKey("placeholderIds")]
        internal List<string> PlaceholderIds;

        internal SendNotificationTarget(UserIdList userIdList)
        {
            UserIdList = userIdList;
            PlaceholderIds = new List<string>();
        }

        internal SendNotificationTarget()
        {
            PlaceholderIds = new List<string>();
        }

        public static SendNotificationTarget Placeholder(string placeholder)
        {
            return new SendNotificationTarget().AddPlaceholder(placeholder);
        }

        public static SendNotificationTarget Users(UserIdList userIdList)
        {
            return new SendNotificationTarget(userIdList);
        }

        public SendNotificationTarget AddPlaceholder(string placeholder)
        {
            PlaceholderIds.Add(placeholder);
            return this;
        }

    }
}
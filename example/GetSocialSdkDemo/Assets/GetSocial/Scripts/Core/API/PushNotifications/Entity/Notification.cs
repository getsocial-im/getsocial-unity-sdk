using System.Collections.Generic;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core {
    public class Notification {

        public static class NotificationTypes {

            /// <summary>
            /// Someone commented on your activity.
            /// <summary>
            public const string Comment = "comment";

            /// <summary>
            /// Someone liked your activity.
            /// <summary>
            public const string LikeActivity = "activity_like";

            /// <summary>
            /// Someone liked your comment.
            /// <summary>
            public const string LikeComment = "comment_like";

            /// <summary>
            /// Someone commented on the activity where you've commented before.
            /// <summary>
            public const string CommentedInSameThread = "related_comment";

            /// <summary>
            /// You became friends with another user.
            /// <summary>
            public const string NewFriendship = "friends_add";

            /// <summary>
            /// Someone accepted your invite.
            /// <summary>
            public const string InviteAccepted = "invite_accept";

            /// <summary>
            /// Someone mentioned you in comment.
            /// <summary>
            public const string MentionInComment = "comment_mention";

            /// <summary>
            /// Someone mentioned you in activity.
            /// <summary>
            public const string MentionInActivity = "activity_mention";

            /// <summary>
            /// Someone replied to your comment.
            /// <summary>
            public const string ReplyToComment = "comment_reply";
            //endregion

            /// <summary>
            /// Smart targeting Push Notifications.
            /// <summary>
            public const string Targeting = "targeting";

            /// <summary>
            /// Notifications sent from the Dashboard when using "Test Push Notifications".
            /// <summary>
            public const string Direct = "direct";

            /// <summary>
            /// Notification sent from SDK.
            /// <summary>
            public const string Sdk = "custom";
        }

        [JsonSerializationKey("id")]
        public string Id { get; internal set; }

        [JsonSerializationKey("action")]
        public GetSocialAction Action { get; internal set; }

        [JsonSerializationKey("actionButtons")]
        public List<NotificationButton> ActionButtons { get; internal set; }

        [JsonSerializationKey("status")]
        public string Status { get; internal set; }

        [JsonSerializationKey("type")]
        public string Type { get; internal set; }

        [JsonSerializationKey("createdAt")]
        public long CreatedAt { get; internal set; }

        [JsonSerializationKey("title")]
        public string Title { get; internal set; }

        [JsonSerializationKey("text")]
        public string Text { get; internal set; }

        [JsonSerializationKey("mediaAttachment")]
        public MediaAttachment Attachment { get; internal set; }

        [JsonSerializationKey("sender")]
        public User Sender { get; internal set; }

        [JsonSerializationKey("customization")]
        public NotificationCustomization Customization { get; internal set; }

        public override string ToString () {
            return string.Format ("Id: {0}, Action: {1}, ActionButtons: {2}, Status: {3}, NotificationType: {4}, CreatedAt: {5}, Title: {6}, Text: {7}, ImageUrl: {8}, VideoUrl: {9}, Sender: {10}, Customization: {11}", Id, Action, ActionButtons.ToDebugString (), Status, Type, CreatedAt, Title, Text, Attachment?.ImageUrl, Attachment?.VideoUrl, Sender, Customization);
        }

        internal Notification() {}
    }
}
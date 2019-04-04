using System;
using System.Collections.Generic;
#if UNITY_ANDROID
using UnityEngine;
#endif

#if UNITY_IOS
using System.Linq;
#endif

namespace GetSocialSdk.Core
{
    public class Notification : IConvertableFromNative<Notification>
    {
        /// <summary>
        /// Enumeration that allows you to have convenient switch for your action.
        /// </summary>
        /// 
        [Obsolete("Use GetSocialActionType")]
        public enum Type
        {
            /// <summary>
            /// Custom action.
            /// </summary>
            Custom = 0,
            
            /// <summary>
            /// Profile with provided identifier should be opened.
            /// </summary>
            OpenProfile = 1,
            
            /// <summary>
            /// Activity with provided identifier should be opened.
            /// </summary>
            OpenActivity = 2,
            
            /// <summary>
            /// Open Smart Invites action.
            /// </summary>
            OpenInvites = 3,
            
            /// <summary>
            /// Open URL.
            /// </summary>
            OpenUrl = 4,
        }

        public static class NotificationTypes
        {

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

        /// <summary>
        /// Contains all predefined keys for <see cref="ActionData"/> dictionary.
        /// </summary>
        [Obsolete("Use GetSocialActionKeys instead")]
        public static class Key
        {
            public static class OpenActivity
            {
                public const string ActivityId = "$activity_id";
                public const string CommentId = "$comment_id";
            }

            public static class OpenProfile
            {
                public const string UserId = "$user_id";
            }
        }
        
        public string Id { get; private set; }
        public GetSocialAction NotificationAction { get; private set; }
        public List<ActionButton> ActionButtons { get; private set; } 

#pragma warning disable 0618
        [Obsolete("Use NotificationAction")]
        public Type Action { get; private set; }
#pragma warning restore 0618

        [Obsolete("Use NotificationAction")]
        public Dictionary<string, string> ActionData
        {
            get { return NotificationAction.Data; }
        }

        [Obsolete("Use Status")]
        public bool WasRead
        {
            get { return !Status.Equals(NotificationStatus.Unread); }
        }

        public string Status { get; private set; }
        public string NotificationType { get; private set; }
        public long CreatedAt { get; private set; }
        public string Title { get; private set; }
        public string Text { get; private set; }
        public string ImageUrl { get; private set; }
        public string VideoUrl { get; private set; }
        public UserReference Sender { get; private set; }

        public override string ToString()
        {
            return string.Format("Id: {0}, Action: {1}, ActionButtons: {2}, Status: {3}, NotificationType: {4}, CreatedAt: {5}, Title: {6}, Text: {7}, ImageUrl: {8}, VideoUrl: {9}, Sender: {10}" 
                , Id, NotificationAction, ActionButtons.ToDebugString(), Status, NotificationType, CreatedAt, Title, Text, ImageUrl, VideoUrl, Sender);
        }
#if UNITY_ANDROID
        public Notification ParseFromAJO(AndroidJavaObject ajo)
        {
            Id = ajo.CallStr("getId");
            Status = ajo.CallStr("getStatus");
            NotificationType = ajo.CallStr("getType");
#pragma warning disable 618
            Action = (Type) ajo.CallInt("getActionType");
#pragma warning restore 618
            CreatedAt = ajo.CallLong("getCreatedAt");
            Title = ajo.CallStr("getTitle");
            Text = ajo.CallStr("getText");
            NotificationAction = new GetSocialAction().ParseFromAJO(ajo.CallAJO("getAction"));
            ActionButtons = ajo.CallAJO("getActionButtons").FromJavaList().ConvertAll(item =>
            {
                using (item)
                {
                    return new ActionButton().ParseFromAJO(item);
                }
            });
            ImageUrl = ajo.CallStr("getImageUrl");
            VideoUrl = ajo.CallStr("getVideoUrl");
            Sender = new UserReference().ParseFromAJO(ajo.CallAJO("getSender"));
            return this;
        }

#elif UNITY_IOS
        public Notification ParseFromJson(Dictionary<string, object> dictionary)
        {
            Title = dictionary["Title"] as string;
            Id = dictionary["Id"] as string;
            Status = dictionary["Status"] as string;
            CreatedAt = (long) dictionary["CreatedAt"];
            Text = dictionary["Text"] as string;
            ImageUrl = dictionary["ImageUrl"] as string;
            VideoUrl = dictionary["VideoUrl"] as string;
            NotificationAction =
                new GetSocialAction().ParseFromJson(dictionary["Action"] as Dictionary<string, object>);
            ActionButtons = ((List<object>) dictionary["ActionButtons"]).ConvertAll(item =>
                new ActionButton().ParseFromJson((Dictionary<string, object>) item));
            NotificationType = dictionary["Type"] as string;
#pragma warning disable 618
            Action = (Type) (long) dictionary["OldAction"];
#pragma warning restore 618
            Sender = new UserReference().ParseFromJson(dictionary["Sender"] as Dictionary<string, object>);
            return this;
        }
#endif

        
    }
}
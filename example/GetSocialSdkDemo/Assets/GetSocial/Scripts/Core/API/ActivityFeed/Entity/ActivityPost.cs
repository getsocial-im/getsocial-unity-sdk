using System;
using System.Collections.Generic;

#if UNITY_ANDROID
using System.Linq;
using UnityEngine;
#endif

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Activity post entity. Contains all information about post, its author and content.
    /// </summary>
    public sealed class ActivityPost : IGetSocialBridgeObject<ActivityPost>
    {
        /// <summary>
        /// Type of Activity Feed content
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// Activity Feed Post
            /// </summary>
            Post = 0,
            /// <summary>
            /// Activity Feed Comment
            /// </summary>
            Comment = 1
        }

        /// <summary>
        /// Gets the activity identifier.
        /// </summary>
        /// <value>The activity identifier.</value>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the activity text.
        /// </summary>
        /// <value>The activity text.</value>
        public string Text { get; private set; }

        /// <summary>
        /// Gets a value indicating whether activity has text.
        /// </summary>
        /// <value><c>true</c> if activity has text; otherwise, <c>false</c>.</value>
        public bool HasText
        {
            get { return !string.IsNullOrEmpty(Text); }
        }

        /// <summary>
        /// Gets the image URL or null if activity has no image.
        /// </summary>
        /// <value>The image URL or null if activity has no image.</value>
        public string ImageUrl { get; private set; }

        /// <summary>
        /// Gets a value indicating whether activity has image.
        /// </summary>
        /// <value><c>true</c> if activity has image; otherwise, <c>false</c>.</value>
        public bool HasImage
        {
            get { return !string.IsNullOrEmpty(ImageUrl); }
        }

        /// <summary>
        /// Date of post creation.
        /// </summary>
        /// <value>Date of post creation.</value>
        public DateTime CreatedAt { get; private set; }

        /// <summary>
        /// Gets the button title.
        /// </summary>
        /// <value>The button title or <c>null</c> if post has no button.</value>
        public string ButtonTitle { get; private set; }

        /// <summary>
        /// Gets the button action id.
        /// </summary>
        /// <value>The button action id or <c>null</c> if post has no button.</value>
        public string ButtonAction { get; private set; }

        /// <summary>
        /// Gets a value indicating whether activity has button.
        /// </summary>
        /// <value><c>true</c> if activity has button; otherwise, <c>false</c>.</value>
        public bool HasButton
        {
            get { return ButtonTitle != null; }
        }

        /// <summary>
        /// Gets the post author.
        /// </summary>
        /// <value>The post author.</value>
        public PostAuthor Author { get; private set; }

        /// <summary>
        /// Gets the number of comments.
        /// </summary>
        /// <value>The number of cemments.</value>
        public int CommentsCount { get; private set; }

        /// <summary>
        /// Gets the likes count.
        /// </summary>
        /// <value>The likes count.</value>
        public int LikesCount { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this post is liked by me.
        /// </summary>
        /// <value><c>true</c> if this post is liked by me; otherwise, <c>false</c>.</value>
        public bool IsLikedByMe { get; private set; }

        /// <summary>
        /// Gets the sticky post start date.
        /// </summary>
        /// <value>The sticky post start date.</value>
        public DateTime StickyStart { get; private set; }

        /// <summary>
        /// Gets the sticky post end date.
        /// </summary>
        /// <value>The sticky post end date.</value>
        public DateTime StickyEnd { get; private set; }

        /// <summary>
        /// Determines if the post is sticky at the specified dateTime.
        /// </summary>
        /// <returns><c>true</c> if post is sticky at the specified dateTime; otherwise, <c>false</c>.</returns>
        /// <param name="dateTime">Datetime to check.</param>
        public bool IsStickyAt(DateTime dateTime)
        {
            return dateTime.Ticks > StickyStart.Ticks && dateTime.Ticks < StickyEnd.Ticks;
        }

        /// <summary>
        /// List of mentions in activity post.
        /// </summary>
        public List<Mention> Mentions { get; private set; }

        /// <summary>
        /// Feed name that this activity belongs to.
        /// </summary>
        public string FeedId { get; private set;  }

        public override string ToString()
        {
            return string.Format(
                "Id: {0}, Text: {1}, HasText: {2}, ImageUrl: {3}, HasImage: {4}, CreatedAt: {5}, ButtonTitle: {6}, ButtonAction: {7}, HasButton: {8}, Author: {9}, CommentsCount: {10}, LikesCount: {11}, IsLikedByMe: {12}, StickyStart: {13}, StickyEnd: {14}, FeedId: {15}, Mentions: {16}",
                Id, Text, HasText, ImageUrl, HasImage, CreatedAt, ButtonTitle, ButtonAction, HasButton, Author,
                CommentsCount, LikesCount, IsLikedByMe, StickyStart, StickyEnd, FeedId, Mentions);
        }

#if UNITY_ANDROID

        public AndroidJavaObject ToAJO()
        {
            throw new NotImplementedException();
        }

        public ActivityPost ParseFromAJO(AndroidJavaObject ajo)
        {
            using (ajo)
            {
                Id = ajo.CallStr("getId");
                Text = ajo.CallStr("getText");
                ImageUrl = ajo.CallStr("getImageUrl");
                ButtonTitle = ajo.CallStr("getButtonTitle");
                ButtonAction = ajo.CallStr("getButtonAction");
                CreatedAt = DateUtils.FromUnixTime(ajo.CallLong("getCreatedAt"));
                Author = new PostAuthor().ParseFromAJO(ajo.CallAJO("getAuthor"));
                CommentsCount = ajo.CallInt("getCommentsCount");
                LikesCount = ajo.CallInt("getLikesCount");
                IsLikedByMe = ajo.CallBool("isLikedByMe");

                StickyStart = DateUtils.FromUnixTime(ajo.CallLong("getStickyStart"));
                StickyEnd = DateUtils.FromUnixTime(ajo.CallLong("getStickyEnd"));
                FeedId = ajo.CallStr("getFeedId");
                Mentions = ajo.CallAJO("getMentions").FromJavaList().ConvertAll(mentionAjo =>
                {
                    using (mentionAjo)
                    {
                        return new Mention().ParseFromAJO(mentionAjo);
                    }
                }).ToList();
            }
            return this;
        }

#elif UNITY_IOS

        public string ToJson()
        {
            throw new NotImplementedException();
        }

        public ActivityPost ParseFromJson(Dictionary<string, object> json)
        {
            Id = (string) json["Id"];
            Text = json["Text"] as string;

            ImageUrl = json["ImageUrl"] as string;
            ButtonTitle = json["ButtonTitle"] as string;
            ButtonAction = json["ButtonAction"] as string;
            CreatedAt = DateUtils.FromUnixTime((long) json["CreatedAt"]);

            var authorDic = json["Author"] as Dictionary<string, object>;
            Author = new PostAuthor().ParseFromJson(authorDic);

            CommentsCount = (int) (long) json["CommentsCount"];
            LikesCount = (int) (long) json["LikesCount"];
            IsLikedByMe = (bool) json["IsLikedByMe"];

            StickyStart = DateUtils.FromUnixTime((long) json["StickyStart"]);
            StickyEnd = DateUtils.FromUnixTime((long) json["StickyEnd"]);
            Mentions = GSJsonUtils.ParseList<Mention>(json["Mentions"] as string);
            FeedId = json["FeedId"] as string;
            
            return this;
        }
#endif
    }
}
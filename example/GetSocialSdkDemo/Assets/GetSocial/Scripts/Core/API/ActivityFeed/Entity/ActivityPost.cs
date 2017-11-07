using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    /// <summary>
    ///
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
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

        public UnityEngine.AndroidJavaObject ToAJO()
        {
            throw new NotImplementedException();
        }

        public ActivityPost ParseFromAJO(UnityEngine.AndroidJavaObject ajo)
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
                    using (ajo)
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

        public ActivityPost ParseFromJson(Dictionary<string, object> jsonDic)
        {
            Id = (string) jsonDic[IdFieldName];
            Text = jsonDic[TextFieldName] as string;

            ImageUrl = jsonDic[ImageUrlFieldName] as string;
            ButtonTitle = jsonDic[ButtonTitleFieldName] as string;
            ButtonAction = jsonDic[ButtonActionFieldName] as string;
            CreatedAt = DateUtils.FromUnixTime((long) jsonDic[CreatedAtFieldName]);

            var authorDic = jsonDic[AuthorFieldName] as Dictionary<string, object>;
            Author = new PostAuthor().ParseFromJson(authorDic);

            CommentsCount = (int) (long) jsonDic[CommentsCountFieldName];
            LikesCount = (int) (long) jsonDic[LikesCountFieldName];
            IsLikedByMe = (bool) jsonDic[IsLikedByMeFieldName];

            StickyStart = DateUtils.FromUnixTime((long) jsonDic[StickyStartFieldName]);
            StickyEnd = DateUtils.FromUnixTime((long) jsonDic[StickyEndFieldName]);
            Mentions = GSJsonUtils.ParseList<Mention>(jsonDic[MentionsFieldName] as string);
            FeedId = jsonDic[FeedIdFieldName] as string;
            
            return this;
        }

        static string IdFieldName
        {
            get { return ReflectionUtils.GetMemberName((ActivityPost c) => c.Id); }
        }

        static string TextFieldName
        {
            get { return ReflectionUtils.GetMemberName((ActivityPost c) => c.Text); }
        }

        static string ImageUrlFieldName
        {
            get { return ReflectionUtils.GetMemberName((ActivityPost c) => c.ImageUrl); }
        }

        static string ButtonTitleFieldName
        {
            get { return ReflectionUtils.GetMemberName((ActivityPost c) => c.ButtonTitle); }
        }

        static string ButtonActionFieldName
        {
            get { return ReflectionUtils.GetMemberName((ActivityPost c) => c.ButtonAction); }
        }

        static string CreatedAtFieldName
        {
            get { return ReflectionUtils.GetMemberName((ActivityPost c) => c.CreatedAt); }
        }

        static string AuthorFieldName
        {
            get { return ReflectionUtils.GetMemberName((ActivityPost c) => c.Author); }
        }

        static string CommentsCountFieldName
        {
            get { return ReflectionUtils.GetMemberName((ActivityPost c) => c.CommentsCount); }
        }

        static string LikesCountFieldName
        {
            get { return ReflectionUtils.GetMemberName((ActivityPost c) => c.LikesCount); }
        }

        static string IsLikedByMeFieldName
        {
            get { return ReflectionUtils.GetMemberName((ActivityPost c) => c.IsLikedByMe); }
        }

        static string StickyStartFieldName
        {
            get { return ReflectionUtils.GetMemberName((ActivityPost c) => c.StickyStart); }
        }

        static string StickyEndFieldName
        {
            get { return ReflectionUtils.GetMemberName((ActivityPost c) => c.StickyEnd); }
        }

        static string MentionsFieldName
        {
            get { return ReflectionUtils.GetMemberName((ActivityPost c) => c.Mentions); }
        }

        static string FeedIdFieldName
        {
            get { return ReflectionUtils.GetMemberName((ActivityPost c) => c.FeedId); }
        }

#endif
    }
}
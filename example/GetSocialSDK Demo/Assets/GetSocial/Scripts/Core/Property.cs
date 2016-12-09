/**
 *     Copyright 2015-2016 GetSocial B.V.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace GetSocialSdk.Core
{
    /// <summary>
    /// List of properties that influence the behavior and UI of the SDK.
    /// Learn more about SDK customizations from the <a href="http://docs.getsocial.im/#customizing-the-appearance">documentation</a>.
    /// </summary>
    public static class Property
    {
        /// <summary>
        /// The width of the window.
        /// </summary>
        public const string WindowWidth = "window.width"; // dimension
        /// <summary>
        /// The height of the window.
        /// </summary>
        public const string WindowHeight = "window.height"; // dimension
        /// <summary>
        /// The color of the window background.
        /// </summary>
        public const string WindowBgColor = "window.bg-color"; // color
        /// <summary>
        /// The window background image.
        /// </summary>
        public const string WindowBgImage = "window.bg-image"; // drawable
        /// <summary>
        /// The window overlay image.
        /// </summary>
        public const string WindowOverlayImage = "window.overlay-image"; // drawable
        /// <summary>
        /// The color of the window tint.
        /// </summary>
        public const string WindowTintColor = "window.tint-color"; // color
        /// <summary>
        /// The window animation style.
        /// </summary>
        public const string WindowAnimationStyle = "window.animation-style"; // animation style

        /// <summary>
        /// The height of the header.
        /// </summary>
        public const string HeaderHeight = "header.height"; // dimension
        /// <summary>
        /// The color of the header background.
        /// </summary>
        public const string HeaderBgColor = "header.bg-color"; // color
        /// <summary>
        /// The top padding of the header.
        /// </summary>
        public const string HeaderPaddingTop = "header.padding-top"; // dimension
        /// <summary>
        /// The bottom padding of the header.
        /// </summary>
        public const string HeaderPaddingBottom = "header.padding-bottom"; // dimension

        /// <summary>
        /// The title text style.
        /// </summary>
        public const string TitleTextStyle = "title.text-style"; // text style
        /// <summary>
        /// The title margin top.
        /// </summary>
        [System.Obsolete("Use HeaderPaddingTop instead")]
        public const string TitleMarginTop = "title.margin-top"; // dimension

        /// <summary>
        /// The width of the back button.
        /// </summary>
        public const string BackButtonWidth = "back-button.width"; // dimension
        /// <summary>
        /// The height of the back button.
        /// </summary>
        public const string BackButtonHeight = "back-button.height"; // dimension
        /// <summary>
        /// The back button margin left.
        /// </summary>
        public const string BackButtonMarginLeft = "back-button.margin-left"; // dimension
        /// <summary>
        /// The back button margin top.
        /// </summary>
        public const string BackButtonMarginTop = "back-button.margin-top"; // dimension
        /// <summary>
        /// The back button background image normal.
        /// </summary>
        public const string BackButtonBgImageNormal = "back-button.bg-image-normal"; // drawable
        /// <summary>
        /// The back button background image pressed.
        /// </summary>
        public const string BackButtonBgImagePressed = "back-button.bg-image-pressed"; // drawable

        /// <summary>
        /// The width of the close button.
        /// </summary>
        public const string CloseButtonWidth = "close-button.width"; // dimension
        /// <summary>
        /// The height of the close button.
        /// </summary>
        public const string CloseButtonHeight = "close-button.height"; // dimension
        /// <summary>
        /// The close button margin right.
        /// </summary>
        public const string CloseButtonMarginRight = "close-button.margin-right"; // dimension
        /// <summary>
        /// The close button margin top.
        /// </summary>
        public const string CloseButtonMarginTop = "close-button.margin-top"; // dimension
        /// <summary>
        /// The close button background image normal.
        /// </summary>
        public const string CloseButtonBgImageNormal = "close-button.bg-image-normal"; // drawable
        /// <summary>
        /// The close button background image pressed.
        /// </summary>
        public const string CloseButtonBgImagePressed = "close-button.bg-image-pressed"; // drawable

        /// <summary>
        /// The content margin top.
        /// </summary>
        public const string ContentMarginTop = "content.margin-top"; // dimension
        /// <summary>
        /// The content margin left.
        /// </summary>
        public const string ContentMarginLeft = "content.margin-left"; // dimension
        /// <summary>
        /// The content margin right.
        /// </summary>
        public const string ContentMarginRight = "content.margin-right"; // dimension
        /// <summary>
        /// The content margin bottom.
        /// </summary>
        public const string ContentMarginBottom = "content.margin-bottom"; // dimension
        /// <summary>
        /// The content text style.
        /// </summary>
        public const string ContentTextStyle = "content.text-style"; // text style

        /// <summary>
        /// The entity name text style.
        /// </summary>
        public const string EntityNameTextStyle = "entity-name.text-style"; // text style

        /// <summary>
        /// The timestamp text style.
        /// </summary>
        public const string TimestampTextStyle = "timestamp.text-style"; // text style

        /// <summary>
        /// The badge text style.
        /// </summary>
        public const string BadgeTextStyle = "badge.text-style"; // text style
        /// <summary>
        /// The badge background image.
        /// </summary>
        public const string BadgeBgImage = "badge.bg-image"; // drawable
        /// <summary>
        /// The badge background image insets.
        /// </summary>
        public const string BadgeBgImageInsets = "badge.bg-image-insets"; // insets
        /// <summary>
        /// The badge text insets.
        /// </summary>
        public const string BadgeTextInsets = "badge.text-insets"; //insets

        /// <summary>
        /// The link text style.
        /// </summary>
        public const string LinkTextStyle = "link.text-style"; // text style

        /// <summary>
        /// The verified account background image.
        /// </summary>
        public const string VerifiedAccountBgImage = "verified-account-badge.bg-image"; // drawable

        /// <summary>
        /// The invite friends button text style.
        /// </summary>
        public const string InviteFriendsButtonTextStyle = "invite-friends-button.text-style"; // text style
        /// <summary>
        /// The invite friends button text Y offset normal.
        /// </summary>
        public const string InviteFriendsButtonTextYOffsetNormal = "invite-friends-button.text-y-offset-normal"; // dimension
        /// <summary>
        /// The invite friends button text Y offset pressed.
        /// </summary>
        public const string InviteFriendsButtonTextYOffsetPressed = "invite-friends-button.text-y-offset-pressed"; // dimension
        /// <summary>
        /// The color of the invite friends button bar.
        /// </summary>
        public const string InviteFriendsButtonBarColor = "invite-friends-button.bar-color"; // color
        /// <summary>
        /// The invite friends button background image normal.
        /// </summary>
        public const string InviteFriendsButtonBgImageNormal = "invite-friends-button.bg-image-normal"; // drawable
        /// <summary>
        /// The invite friends button background image pressed.
        /// </summary>
        public const string InviteFriendsButtonBgImagePressed = "invite-friends-button.bg-image-pressed"; // drawable
        /// <summary>
        /// The invite friends button background image pressed insets.
        /// </summary>
        public const string InviteFriendsButtonBgImagePressedInsets = "invite-friends-button.bg-image-pressed-insets"; // insets
        /// <summary>
        /// The invite friends button background image normal insets.
        /// </summary>
        public const string InviteFriendsButtonBgImageNormalInsets = "invite-friends-button.bg-image-normal-insets"; // insets

        /// <summary>
        /// The activity action button text style.
        /// </summary>
        public const string ActivityActionButtonTextStyle = "activity-action-button.text-style"; // text style
        /// <summary>
        /// The activity action button text Y offset normal.
        /// </summary>
        public const string ActivityActionButtonTextYOffsetNormal = "activity-action-button.text-y-offset-normal"; // dimension
        /// <summary>
        /// The activity action button text Y offset pressed.
        /// </summary>
        public const string ActivityActionButtonTextYOffsetPressed = "activity-action-button.text-y-offset-pressed"; // dimension
        /// <summary>
        /// The activity action button background image normal.
        /// </summary>
        public const string ActivityActionButtonBgImageNormal = "activity-action-button.bg-image-normal"; // drawable
        /// <summary>
        /// The activity action button background image pressed.
        /// </summary>
        public const string ActivityActionButtonBgImagePressed = "activity-action-button.bg-image-pressed"; // drawable
        /// <summary>
        /// The activity action button background image normal insets.
        /// </summary>
        public const string ActivityActionButtonBgImageNormalInsets = "activity-action-button.bg-image-normal-insets"; // insets
        /// <summary>
        /// The activity action button background image pressed insets.
        /// </summary>
        public const string ActivityActionButtonBgImagePressedInsets = "activity-action-button.bg-image-pressed-insets"; // insets

        /// <summary>
        /// The activity image aspect ratio.
        /// </summary>
        public const string ActivityImageAspectRatio = "activity-image.aspect-ratio"; // aspect ratio
        /// <summary>
        /// The activity image radius.
        /// </summary>
        public const string ActivityImageRadius = "activity-image.radius"; // dimension
        /// <summary>
        /// The activity image default image.
        /// </summary>
        public const string ActivityImageDefaultImage = "activity-image.default-image"; //drawable

        /// <summary>
        /// The overscroll text style.
        /// </summary>
        public const string OverscrollTextStyle = "overscroll.text-style"; // text style

        /// <summary>
        /// The placeholder title text style.
        /// </summary>
        public const string PlaceholderTitleTextStyle = "placeholder-title.text-style"; // text style
        /// <summary>
        /// The placeholder content text style.
        /// </summary>
        public const string PlaceholderContentTextStyle = "placeholder-content.text-style"; // text style

        /// <summary>
        /// The input field text style.
        /// </summary>
        public const string InputFieldTextStyle = "input-field.text-style"; // text style
        /// <summary>
        /// The color of the input field background.
        /// </summary>
        public const string InputFieldBgColor = "input-field.bg-color"; // color
        /// <summary>
        /// The color of the input field hint.
        /// </summary>
        public const string InputFieldHintColor = "input-field.hint-color"; // color
        /// <summary>
        /// The size of the input field border.
        /// </summary>
        public const string InputFieldBorderSize = "input-field.border-size"; // dimension
        /// <summary>
        /// The color of the input field border.
        /// </summary>
        public const string InputFieldBorderColor = "input-field.border-color"; // color
        /// <summary>
        /// The input field radius.
        /// </summary>
        public const string InputFieldRadius = "input-field.radius"; // dimension

        /// <summary>
        /// The color of the activity input bar background.
        /// </summary>
        public const string ActivityInputBarBgColor = "activity-input-bar.bg-color"; // color
        /// <summary>
        /// The color of the comment input bar background.
        /// </summary>
        public const string CommentInputBarBgColor = "comment-input-bar.bg-color"; // color

        /// <summary>
        /// The size of the avatar border.
        /// </summary>
        public const string AvatarBorderSize = "avatar.border-size"; // dimension
        /// <summary>
        /// The color of the avatar border.
        /// </summary>
        public const string AvatarBorderColor = "avatar.border-color"; // color
        /// <summary>
        /// The avatar radius.
        /// </summary>
        public const string AvatarRadius = "avatar.radius"; // dimension
        /// <summary>
        /// The avatar default image.
        /// </summary>
        public const string AvatarDefaultImage = "avatar.default-image"; // drawable

        /// <summary>
        /// The like button background image normal.
        /// </summary>
        public const string LikeButtonBgImageNormal = "like-button.bg-image-normal"; // drawable
        /// <summary>
        /// The like button background image selected.
        /// </summary>
        public const string LikeButtonBgImageSelected = "like-button.bg-image-selected"; // drawable

        /// <summary>
        /// The color of the divider background.
        /// </summary>
        public const string DividerBgColor = "divider.bg-color"; // color
        /// <summary>
        /// The height of the divider.
        /// </summary>
        public const string DividerHeight = "divider.height"; // dimension

        /// <summary>
        /// The load more button text style.
        /// </summary>
        public const string LoadMoreButtonTextStyle = "load-more-button.text-style"; // text style
        /// <summary>
        /// The load more button text Y offset normal.
        /// </summary>
        public const string LoadMoreButtonTextYOffsetNormal = "load-more-button.text-y-offset-normal"; // dimension
        /// <summary>
        /// The load more button text Y offset pressed.
        /// </summary>
        public const string LoadMoreButtonTextYOffsetPressed = "load-more-button.text-y-offset-pressed"; // dimension
        /// <summary>
        /// The load more button background image normal.
        /// </summary>
        public const string LoadMoreButtonBgImageNormal = "load-more-button.bg-image-normal"; // drawable
        /// <summary>
        /// The load more button background image pressed.
        /// </summary>
        public const string LoadMoreButtonBgImagePressed = "load-more-button.bg-image-pressed"; // drawable
        /// <summary>
        /// The load more button background image normal insets.
        /// </summary>
        public const string LoadMoreButtonBgImageNormalInsets = "load-more-button.bg-image-normal-insets"; // insets
        /// <summary>
        /// The load more button background image pressed insets.
        /// </summary>
        public const string LoadMoreButtonBgImagePressedInsets = "load-more-button.bg-image-pressed-insets"; // insets

        /// <summary>
        /// The post button background image normal.
        /// </summary>
        public const string PostButtonBgImageNormal = "post-button.bg-image-normal"; // drawable
        /// <summary>
        /// The post button background image pressed.
        /// </summary>
        public const string PostButtonBgImagePressed = "post-button.bg-image-pressed"; // drawable

        /// <summary>
        /// The segment bar text style normal.
        /// </summary>
        public const string SegmentBarTextStyleNormal = "segment-bar.text-style-normal"; // text style
        /// <summary>
        /// The segment bar text style selected.
        /// </summary>
        public const string SegmentBarTextStyleSelected = "segment-bar.text-style-selected"; // text style
        /// <summary>
        /// The color of the segment bar border.
        /// </summary>
        public const string SegmentBarBorderColor = "segment-bar.border-color"; // color
        /// <summary>
        /// The size of the segment bar border.
        /// </summary>
        public const string SegmentBarBorderSize = "segment-bar.border-size"; // dimension
        /// <summary>
        /// The segment bar border radius.
        /// </summary>
        public const string SegmentBarBorderRadius = "segment-bar.border-radius"; // dimension
        /// <summary>
        /// The segment bar background color normal.
        /// </summary>
        public const string SegmentBarBgColorNormal = "segment-bar.bg-color-normal"; // color
        /// <summary>
        /// The segment bar background color selected.
        /// </summary>
        public const string SegmentBarBgColorSelected = "segment-bar.bg-color-selected"; // color
        /// <summary>
        /// The segment bar background image normal.
        /// </summary>
        public const string SegmentBarBgImageNormal = "segment-bar.bg-image-normal"; // drawable
        /// <summary>
        /// /*The segment bar background image pressed.*/
        /// </summary>
        public const string SegmentBarBgImagePressed = "segment-bar.bg-image-pressed"; // drawable

        /// <summary>
        /// The loading indicator background image.
        /// </summary>
        public const string LoadingIndicatorBgImage = "loading-indicator.bg-image"; // drawable

        /// <summary>
        /// The notification icon comment background image.
        /// </summary>
        public const string NotificationIconCommentBgImage = "notification-icon-comment.bg-image"; // drawable
        /// <summary>
        /// The notification icon like background image.
        /// </summary>
        public const string NotificationIconLikeBgImage = "notification-icon-like.bg-image"; // drawable

        /// <summary>
        /// The no network placeholder background image.
        /// </summary>
        public const string NoNetworkPlaceholderBgImage = "no-network-placeholder.bg-image"; // drawable
        /// <summary>
        /// The no activity placeholder background image.
        /// </summary>
        public const string NoActivityPlaceholderBgImage = "no-activity-placeholder.bg-image"; // drawable
        /// <summary>
        /// The invite provider placeholder background image.
        /// </summary>
        public const string InviteProviderPlaceholderBgImage = "invite-provider-placeholder.bg-image"; // drawable

        /// <summary>
        /// The list item background color odd.
        /// </summary>
        public const string ListItemBgColorOdd = "list-item.bg-color-odd"; // color
        /// <summary>
        /// The list item background color even.
        /// </summary>
        public const string ListItemBgColorEven = "list-item.bg-color-even"; // color
        /// <summary>
        /// The notification list item background color read.
        /// </summary>
        public const string NotificationListItemBgColorRead = "notification-list-item.bg-color-read"; // color

        /// <summary>
        /// The notification list item background color unread.
        /// </summary>
        public const string NotificationListItemBgColorUnread = "notification-list-item.bg-color-unread"; // color
    }
}

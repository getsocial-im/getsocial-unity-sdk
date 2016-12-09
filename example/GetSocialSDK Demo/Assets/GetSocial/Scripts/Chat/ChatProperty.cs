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

#if ENABLE_GETSOCIAL_CHAT

namespace GetSocialSdk.Chat
{
    /// <summary>
    /// List of properties that influence the behavior and Chat UI of the SDK.
    /// Learn more about SDK customizations from the <a href="http://docs.getsocial.im/#customizing-the-appearance">documentation</a>.
    /// </summary>
    public static class ChatProperty
    {
        /// <summary>
        /// The chat tooltip background image.
        /// </summary>
        public const string ChatTooltipBgImage = "chat-tooltip.bg-image"; // drawable
        /// <summary>
        /// The chat tooltip text style.
        /// </summary>
        public const string ChatTooltipTextStyle = "chat-tooltip.text-style"; // text style

        /// <summary>
        /// My chat message text style.
        /// </summary>
        public const string MyChatMessageTextStyle = "my-chat-message.text-style"; //text style
        /// <summary>
        /// My chat message background image normal.
        /// </summary>
        public const string MyChatMessageBgImageNormal = "my-chat-message.bg-image-normal"; //drawable
        /// <summary>
        /// My chat message background image pressed.
        /// </summary>
        public const string MyChatMessageBgImagePressed = "my-chat-message.bg-image-pressed"; //drawable
        /// <summary>
        /// My chat message background image normal insets.
        /// </summary>
        public const string MyChatMessageBgImageNormalInsets = "my-chat-message.bg-image-normal-insets"; //insets
        /// <summary>
        /// My chat message background image pressed insets.
        /// </summary>
        public const string MyChatMessageBgImagePressedInsets = "my-chat-message.bg-image-pressed-insets"; //insets
        /// <summary>
        /// My chat message text insets.
        /// </summary>
        public const string MyChatMessageTextInsets = "my-chat-message.text-insets"; //insets

        /// <summary>
        /// Their chat message text style.
        /// </summary>
        public const string TheirChatMessageTextStyle = "their-chat-message.text-style"; //text style
        /// <summary>
        /// Their chat message background image normal.
        /// </summary>
        public const string TheirChatMessageBgImageNormal = "their-chat-message.bg-image-normal"; //drawable
        /// <summary>
        /// Their chat message background image pressed.
        /// </summary>
        public const string TheirChatMessageBgImagePressed = "their-chat-message.bg-image-pressed"; //drawable
        /// <summary>
        /// Their chat message background image normal insets.
        /// </summary>
        public const string TheirChatMessageBgImageNormalInsets = "their-chat-message.bg-image-normal-insets"; //insets
        /// <summary>
        /// Their chat message background image pressed insets.
        /// </summary>
        public const string TheirChatMessageBgImagePressedInsets = "their-chat-message.bg-image-pressed-insets"; //insets
        /// <summary>
        /// Their chat message text insets.
        /// </summary>
        public const string TheirChatMessageTextInsets = "their-chat-message.text-insets"; //insets

        /// <summary>
        /// The start chat button text style.
        /// </summary>
        public const string StartChatButtonTextStyle = "start-chat-button.text-style"; // text style
        /// <summary>
        /// The start chat button text Y offset normal.
        /// </summary>
        public const string StartChatButtonTextYOffsetNormal = "start-chat-button.text-y-offset-normal"; // dimension
        /// <summary>
        /// The start chat button text Y offset pressed.
        /// </summary>
        public const string StartChatButtonTextYOffsetPressed = "start-chat-button.text-y-offset-pressed"; // dimension
        /// <summary>
        /// The start color of the chat button bar.
        /// </summary>
        public const string StartChatButtonBarColor = "start-chat-button.bar-color"; // color
        /// <summary>
        /// The start chat button background image normal.
        /// </summary>
        public const string StartChatButtonBgImageNormal = "start-chat-button.bg-image-normal"; // drawable
        /// <summary>
        /// The start chat button background image pressed.
        /// </summary>
        public const string StartChatButtonBgImagePressed = "start-chat-button.bg-image-pressed"; // drawable
        /// <summary>
        /// The start chat button background image normal insets.
        /// </summary>
        public const string StartChatButtonBgImageNormalInsets = "start-chat-button.bg-image-normal-insets"; // insets
        /// <summary>
        /// The start chat button background image pressed insets.
        /// </summary>
        public const string StartChatButtonBgImagePressedInsets = "start-chat-button.bg-image-pressed-insets"; // insets
        /// <summary>
        /// The no chat messages placeholder background image.
        /// </summary>
        public const string NoChatMessagesPlaceholderBgImage = "no-chat-messages-placeholder.bg-image"; // drawable
        /// <summary>
        /// The color of the chat input bar background.
        /// </summary>
        public const string ChatInputBarBgColor = "chat-input-bar.bg-color"; // color
        /// <summary>
        /// The chat presence online image.
        /// </summary>
        public const string ChatPresenceOnlineImage = "presence-online.bg-image"; // drawable
        /// <summary>
        /// The chat presence offline image.
        /// </summary>
        public const string ChatPresenceOfflineImage = "presence-offline.bg-image"; // drawable
        /// <summary>
        /// The chat presence online text style.
        /// </summary>
        public const string ChatPresenceOnlineTextStyle = "presence-online.text-style"; // test style
        /// <summary>
        /// The chat presence offline text style.
        /// </summary>
        public const string ChatPresenceOfflineTextStyle = "presence-offline.text-style"; // text style
    }
}
#endif

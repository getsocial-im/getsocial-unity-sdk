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
using GetSocialSdk.Core;

namespace GetSocialSdk.Chat
{
    /// <summary>
    /// Chat message content.
    /// </summary>
    public sealed class ChatMessageContent 
    {
        internal readonly string messageText;

        internal ChatMessageContent(string messageText)
        {
            this.messageText = messageText;
        }

        /// <summary>
        /// Creates message content to send with the text.
        /// </summary>
        /// <returns>Text message content to send as chat message.</returns>
        /// <param name="messageText">Message text.</param>
        public static ChatMessageContent CreateWithText(string messageText)
        {
            Check.Argument.IsNotNull(messageText, "messageText", "Message text cannot be null or empty");

            return new ChatMessageContent(messageText);
        }

        /// <summary>
        /// Gets a value indicating whether message content has text.
        /// </summary>
        /// <value><c>true</c> if message content has text; otherwise, <c>false</c>.</value>
        public bool HasText
        {
            get
            {
                return messageText != null;
            }
        }

        /// <summary>
        /// Gets the message text.
        /// </summary>
        /// <value>The message text.</value>
        public string MessageText
        {
            get
            {
                return messageText;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="GetSocialSdk.Chat.ChatMessageContent"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="GetSocialSdk.Chat.ChatMessageContent"/>.</returns>
        public override string ToString()
        {
            return string.Format("[ChatMessageContent: HasText={0}, MessageText={1}]", HasText, MessageText);
        }
    }
}
#endif
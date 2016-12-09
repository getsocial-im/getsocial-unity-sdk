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
using System;

namespace GetSocialSdk.Chat
{
    /// <summary>
    /// Represents chat message.
    /// </summary>
    public sealed class ChatMessage
    {
        internal ChatMessage(User sender, string guid, DateTime timestamp, bool wasSentByMe, ChatMessageContent content)
        {
            this.sender = sender;
            this.guid = guid;
            this.timestamp = timestamp;
            this.wasSentByMe = wasSentByMe;
            this.content = content;
        }

        private readonly User sender;
        private readonly string guid;
        private readonly DateTime timestamp;
        private readonly bool wasSentByMe;
        private readonly ChatMessageContent content;

        /// <summary>
        /// Gets the sender of the message.
        /// </summary>
        /// <value>Returns the user that has sent the message</value>
        public User Sender
        {
            get { return sender; }
        }

        /// <summary>
        /// Gets the GUID of the message.
        /// </summary>
        /// <value>The GUID of the message.</value>
        public string Guid
        {
            get { return guid; }
        }

        /// <summary>
        /// Gets the timestamp of the message.
        /// </summary>
        /// <value>The timestamp of the message.</value>
        public DateTime Timestamp
        {
            get { return timestamp; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="GetSocialSdk.Chat.ChatMessage"/> was sent by me.
        /// </summary>
        /// <value><c>true</c> if was sent by me; otherwise, <c>false</c>.</value>
        public bool WasSentByMe
        {
            get { return wasSentByMe; }
        }

        /// <summary>
        /// Gets the content of the message.
        /// </summary>
        /// <value>The content of the message.</value>
        public ChatMessageContent Content
        {
            get { return content; }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="GetSocialSdk.Chat.ChatMessage"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="GetSocialSdk.Chat.ChatMessage"/>.</returns>
        public override string ToString()
        {
            return string.Format("[ChatMessage: Sender={0}, Guid={1}, Timestamp={2}, WasSentByMe={3}, Content={4}]", Sender, Guid, Timestamp.ToString("U"), WasSentByMe, Content);
        }
    }
}
#endif
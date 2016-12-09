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

using System;
using System.Collections.Generic;

namespace GetSocialSdk.Chat
{
    public interface IChatRoom
    {
        /// <summary>
        /// Gets the unique identifier of the room.
        /// </summary>
        /// <value>The room unique identifier.</value>
        string Id { get; }

        /// <summary>
        /// Gets a value indicating whether this room is unread.
        /// </summary>
        /// <value><c>true</c> if this room is unread; otherwise, <c>false</c>.</value>
        bool IsUnread { get; }

        /// <summary>
        /// Gets the last chat message from this room.
        /// </summary>
        /// <value>The last chat message from this room. Returns null if no messages were sent to this room.</value>
        ChatMessage LastMessage { get; }

        /// <summary>
        /// Gets user messages from this room.
        /// </summary>
        /// <param name="beforeMessage">Offset message.</param>
        /// <param name="limit">Max number messages to get.</param>
        /// <param name="onSuccess">On success callback.</param>
        /// <param name="onFailure">On failure callback.</param>
        void GetMessages(ChatMessage beforeMessage, int limit, Action<List<ChatMessage>> onSuccess, Action<string> onFailure);

        /// <summary>
        /// Marks the room as read. The room will be marked again unread when a new message is received
        /// </summary>
        /// <param name="onSuccess">On success callback</param>
        /// <param name="onFailure">On failure callback</param>
        void MarkAsRead(Action onSuccess, Action<string> onFailure);

        /// <summary>
        /// Sends the user message to this room.
        /// </summary>
        /// <param name="messageContent">Message content to be sent.</param>
        /// <param name="onSuccess">On success callback</param>
        /// <param name="onFailure">On failure callback</param>
        void SendMessage(ChatMessageContent messageContent, Action onSuccess, Action<string> onFailure);

        /// <summary>
        /// Sends the typing status of the current user to this room.
        /// 
        /// Note that if the same status is sent twice in 1 sec interval the receiver is going to receive it only once.
        /// </summary>
        /// <param name="typingStatus">Typing status to send.</param>
        /// <param name="onSuccess">On success callback</param>
        /// <param name="onFailure">On failure callback</param>
        void SetTypingStatus(TypingStatus typingStatus, Action onSuccess, Action<string> onFailure);
    }
}
#endif
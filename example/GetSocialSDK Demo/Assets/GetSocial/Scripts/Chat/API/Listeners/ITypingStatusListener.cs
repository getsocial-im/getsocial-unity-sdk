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
    /// Implement this interface and pass it to <see cref="GetSocialSdk.Chat.GetSocialChat.AddTypingStatusListener"/> to start listening for other room participants typing status
    /// </summary>
    public interface ITypingStatusListener
    {
        /// <summary>
        /// Invoked when typing status is received to one of public rooms
        /// </summary>
        /// <param name="room">Public chat room to which typing status is received.</param>
        /// <param name="user">User who is typing.</param>
        /// <param name="typingStatus">Typing status.</param>
        void OnPublicRoomTypingStatusReceived(IPublicChatRoom room, User user, TypingStatus typingStatus);

        /// <summary>
        /// Invoked when typing status is received to one of private rooms
        /// </summary>
        /// <param name="room">Private chat room to which typing status is received.</param>
        /// <param name="user">User who is typing.</param>
        /// <param name="typingStatus">Typing status.</param>
        void OnPrivateRoomTypingStatusReceived(IPrivateChatRoom room, User user, TypingStatus typingStatus);
    }
}
#endif

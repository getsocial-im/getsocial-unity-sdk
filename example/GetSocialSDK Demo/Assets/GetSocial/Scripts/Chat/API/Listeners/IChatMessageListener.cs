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
    /// Implement this interface and pass it to <see cref="GetSocialSdk.Chat.GetSocialChat.AddMessageListener"/> to start listening for chat messages
    /// </summary>
    public interface IChatMessageListener
    {
        /// <summary>
        /// Invoked when message is received to a public room.
        /// </summary>
        /// <param name="publicRoom">Public room the message arrived to.</param>
        /// <param name="message">The received message.</param>
        void OnPublicRoomMessage(IPublicChatRoom publicRoom, ChatMessage message);

        /// <summary>
        /// Invoked when message is received to a private room.
        /// </summary>
        /// <param name="privateRoom">Public room the message arrived to.</param>
        /// <param name="message">The received message.</param>
        void OnPrivateRoomMessage(IPrivateChatRoom privateRoom, ChatMessage message);
    }
}
#endif
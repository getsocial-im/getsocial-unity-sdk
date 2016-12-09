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
    /// Implement this interface and pass it to <see cref="GetSocialSdk.Chat.GetSocialChat.AddOnUnreadRoomCountChangedListener"/> to start listening for unread rooms count changes.
    /// </summary>
    public interface IUnreadRoomCountChangedListener
    {
        /// <summary>
        /// Invoked when unread count of public chat rooms changes.
        /// </summary>
        /// <param name="count">Count of unread public rooms.</param>
        void OnUnreadPublicRoomsCountChanged(int count);

        /// <summary>
        /// Invoked when unread count of private chat rooms changes.
        /// </summary>
        /// <param name="count">Count of unread private rooms.</param>
        void OnUnreadPrivateRoomsCountChanged(int count);
    }
}
#endif
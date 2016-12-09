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

namespace GetSocialSdk.Chat
{
    /// <summary>
    /// A public chat room.
    /// </summary>
    public interface IPublicChatRoom : IChatRoom
    {
        /// <summary>
        /// Gets the name of the public room.
        /// </summary>
        /// <value>The name of the public room.</value>
        string Name { get; }

        /// <summary>
        /// Gets a value indicating whether the user is subscribed to receive messages for that room
        /// </summary>
        /// <value><c>true</c> if current user is subsribed to this toom; otherwise, <c>false</c>.</value>
        bool IsSubscribed { get; }

        /// <summary>
        /// Subscribes the current user to receive messages for that room.
        /// In order to get messages for public rooms you first need to subscribe.
        /// </summary>
        /// <param name="onSuccess">On success callback</param>
        /// <param name="onFailure">On failure callback</param>
        void Subscribe(Action onSuccess, Action<string> onFailure);

        /// <summary>
        /// Unsubscribes the user from that room. In that case the current user will not receive any messages for that room.
        /// In order to get messages for public rooms you first need to subscribe.
        /// </summary>
        /// <param name="onSuccess">On success callback</param>
        /// <param name="onFailure">On failure callback</param>
        void Unsubscribe(Action onSuccess, Action<string> onFailure);
    }
}
#endif
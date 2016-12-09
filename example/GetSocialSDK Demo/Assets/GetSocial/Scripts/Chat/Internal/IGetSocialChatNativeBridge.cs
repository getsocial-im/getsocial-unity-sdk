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
using GetSocialSdk.Core;

namespace GetSocialSdk.Chat
{
    public interface IGetSocialChatNativeBridge
    {
        bool IsEnabled { get; }

        int UnreadPublicRoomsCount { get; }

        int UnreadPrivateRoomsCount { get; }

        void GetPublicRoom(string name, Action<IPublicChatRoom> onSuccess, Action<string> onFailure);

        void GetAllPrivateRooms(Action<List<IPrivateChatRoom>> onSuccess, Action<string> onFailure);

        void GetPrivateRoom(User user, Action<IPrivateChatRoom> onSuccess, Action<string> onFailure);

        void GetPrivateRoom(string provider, string userId, Action<IPrivateChatRoom> onSuccess, Action<string> onFailure);

        #region listeners
        [Obsolete]
        void SetOnUnreadConversationsCountChangeListener(Action<int> onUnreadConversationsCountChange);

        void SetOnUnreadCountChangedListenerInternal(Action<int> onUnreadPublicRoomsCountChanged,
                                                     Action<int> onUnreadPrivateRoomsCountChanged);

        void SetMessagesListenerInternal(Action<IPublicChatRoom, ChatMessage> onPublicRoomMessage,
                                         Action<IPrivateChatRoom, ChatMessage> onPrivateRoomMessage);

        void SetTypingStatusListenerInternal(Action<IPublicChatRoom, User, TypingStatus> onPublicRoomTypingStatusReceived, 
                                             Action<IPrivateChatRoom, User, TypingStatus> onPrivateRoomTypingStatusReceived);
        #endregion
    }
}
#endif

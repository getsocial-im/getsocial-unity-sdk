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
using System.Reflection;

namespace GetSocialSdk.Chat
{
    class GetSocialChatEditorMock : IGetSocialChatNativeBridge
    {
        private static IGetSocialChatNativeBridge instance;

        #region initialization

        private GetSocialChatEditorMock()
        {
        }

        internal static IGetSocialChatNativeBridge GetInstance()
        {
            if(instance == null)
            {
                instance = new GetSocialChatEditorMock();
            }
            return instance;
        }

        #endregion

        #region IGetSocialChat implementation

        public int UnreadPublicRoomsCount
        {
            get
            {
                return 0;
            }
        }

        public int UnreadPrivateRoomsCount
        {
            get
            {
                return 0;
            }
        }

        public bool IsEnabled
        {
            get
            {
                return false;
            }
        }

        public void GetPublicRoom(string name, Action<IPublicChatRoom> onSuccess, Action<string> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), name, onSuccess, onFailure);
        }

        public void GetAllPrivateRooms(Action<List<IPrivateChatRoom>> onSuccess, Action<string> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), onSuccess, onFailure);
        }

        public void GetPrivateRoom(User user, Action<IPrivateChatRoom> onSuccess, Action<string> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), user, onSuccess, onFailure);
        }

        public void GetPrivateRoom(string provider, string userId, Action<IPrivateChatRoom> onSuccess, Action<string> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), provider, userId, onSuccess, onFailure);
        }

        public void SetOnUnreadConversationsCountChangeListener(Action<int> onUnreadConversationsCountChange)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), onUnreadConversationsCountChange);
        }

        public void SetOnUnreadCountChangedListenerInternal(Action<int> onUnreadPublicRoomsCountChanged,
                                                       Action<int> onUnreadPrivateRoomsCountChanged)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), onUnreadPublicRoomsCountChanged, onUnreadPrivateRoomsCountChanged);
        }

        public void SetMessagesListenerInternal(Action<IPublicChatRoom, ChatMessage> onPublicRoomMessage,
                                           Action<IPrivateChatRoom, ChatMessage> onPrivateRoomMessage)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), onPublicRoomMessage, onPrivateRoomMessage);
        }

        public void SetTypingStatusListenerInternal(Action<IPublicChatRoom, User, TypingStatus> onPublicRoomTypingStatusReceived, 
                                        Action<IPrivateChatRoom, User, TypingStatus> onPrivateRoomTypingStatusReceived)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), onPublicRoomTypingStatusReceived, onPrivateRoomTypingStatusReceived);
        }

        #endregion
    }
}
#endif

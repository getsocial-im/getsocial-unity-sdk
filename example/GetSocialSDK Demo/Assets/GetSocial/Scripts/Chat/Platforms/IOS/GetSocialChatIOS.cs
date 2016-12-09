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
#if UNITY_IOS

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GetSocialSdk.Core;

namespace GetSocialSdk.Chat
{
    class GetSocialChatIOS : IGetSocialChatNativeBridge
    {
        private static IGetSocialChatNativeBridge instance;

        #region initialization

        private GetSocialChatIOS()
        {
        }

        public static IGetSocialChatNativeBridge GetInstance()
        {
            if(instance == null)
            {
                instance = new GetSocialChatIOS();
            }
            return instance;
        }

        #endregion

        #region IGetSocialChat implementation

        public bool IsEnabled
        {
            get
            {
                return _isChatEnabled();
            }
        }

        public int UnreadPublicRoomsCount
        {
            get
            {
                return _getUnreadPublicRoomsCount();
            }
        }

        public int UnreadPrivateRoomsCount
        {
            get
            {
                return _getUnreadPrivateRoomsCount();
            }
        }

        public void GetPublicRoom(string name, Action<IPublicChatRoom> onSuccess, Action<string> onFailure)
        {
            _getPublicRoom(name,
                onSuccess.GetPointer(), 
                onFailure.GetPointer(),
                ChatCallbacks.OnRoomReceivedCallback,
                GetSocialNativeBridgeIOS.FailureCallback);
        }

        public void GetAllPrivateRooms(Action<List<IPrivateChatRoom>> onSuccess, Action<string> onFailure)
        {
            _getAllPrivateRooms(
                onSuccess.GetPointer(),
                onFailure.GetPointer(), 
                ChatCallbacks.OnRoomsReceivedCallback,
                GetSocialNativeBridgeIOS.FailureCallback);
        }

        public void GetPrivateRoom(User user, Action<IPrivateChatRoom> onSuccess, Action<string> onFailure)
        {
            _getPrivateRoom(user.Guid,
                onSuccess.GetPointer(), 
                onFailure.GetPointer(),
                ChatCallbacks.OnRoomReceivedCallback,
                GetSocialNativeBridgeIOS.FailureCallback);
        }

        public void GetPrivateRoom(string provider, string userId, Action<IPrivateChatRoom> onSuccess, Action<string> onFailure)
        {
            _getPrivateRoomForUserOnProvider(provider, userId,
                onSuccess.GetPointer(), 
                onFailure.GetPointer(),
                ChatCallbacks.OnRoomReceivedCallback,
                GetSocialNativeBridgeIOS.FailureCallback);
        }

        public void SetOnUnreadConversationsCountChangeListener(Action<int> onUnreadConversationsCountChange)
        {
            var onUnreadConversationsCountChangePtr = onUnreadConversationsCountChange.GetPointer();
            
            _setOnUnreadConversationsCountChangeListener(onUnreadConversationsCountChangePtr, OnUnreadConversationsCountChangedListenerProxy.OnUnreadConversationsCountChange);
        }

        public void SetOnUnreadCountChangedListenerInternal(Action<int> onUnreadPublicRoomsCountChanged,
                                                            Action<int> onUnreadPrivateRoomsCountChanged)
        {
            _setOnUnreadRoomsCountChangedListener(
                onUnreadPublicRoomsCountChanged.GetPointer(), 
                onUnreadPrivateRoomsCountChanged.GetPointer(),
                ChatCallbacks.OnUnreadConversationsCountChangeCallback);
        }

        public void SetMessagesListenerInternal(Action<IPublicChatRoom, ChatMessage> onPublicRoomMessage,
                                                Action<IPrivateChatRoom, ChatMessage> onPrivateRoomMessage)
        {
            _setOnMessageListener(onPublicRoomMessage.GetPointer(),
                onPrivateRoomMessage.GetPointer(),
                ChatCallbacks.OnMessageReceivedCallback);
        }

        public void SetTypingStatusListenerInternal(Action<IPublicChatRoom, User, TypingStatus> onPublicRoomTypingStatusReceived,
                                                    Action<IPrivateChatRoom, User, TypingStatus> onPrivateRoomTypingStatusReceived)
        {
            _setTypingStatusListener(
                onPublicRoomTypingStatusReceived.GetPointer(),
                onPrivateRoomTypingStatusReceived.GetPointer(),
                ChatCallbacks.OnTypingStatusReceivedCallback);
        }

        #endregion

        [DllImport("__Internal")]
        private static extern bool _isChatEnabled();

        // unread counters
        [DllImport("__Internal")]
        private static extern int _getUnreadPublicRoomsCount();

        [DllImport("__Internal")]
        private static extern int _getUnreadPrivateRoomsCount();

        // get rooms
        [DllImport("__Internal")] 
        private static extern void _getPublicRoom(
            string name, 
            IntPtr onSuccessPtr,
            IntPtr onFailurePtr, 
            ChatCallbacks.OnRoomReceivedDelegate onRoomReceived, 
            GetSocialNativeBridgeIOS.FailureCallbackDelegate onFailure);

        [DllImport("__Internal")] 
        private static extern void _getAllPrivateRooms(
            IntPtr onSuccessPtr,
            IntPtr onFailurePtr,
            ChatCallbacks.OnRoomsReceivedDelegate onRoomsReceived,
            GetSocialNativeBridgeIOS.FailureCallbackDelegate onFailure);

        [DllImport("__Internal")] 
        private static extern void _getPrivateRoom(
            string userGuid,
            IntPtr onSuccessPtr,
            IntPtr onFailurePtr, 
            ChatCallbacks.OnRoomReceivedDelegate onRoomReceived, 
            GetSocialNativeBridgeIOS.FailureCallbackDelegate onFailure);

        [DllImport("__Internal")] 
        private static extern void _getPrivateRoomForUserOnProvider(
            string provider, 
            string userId,
            IntPtr onSuccessPtr,
            IntPtr onFailurePtr, 
            ChatCallbacks.OnRoomReceivedDelegate onRoomReceived, 
            GetSocialNativeBridgeIOS.FailureCallbackDelegate onFailure);

        // listeners
        [DllImport("__Internal")]
        private static extern void _setOnUnreadConversationsCountChangeListener(IntPtr onUnreadConversationsCountChangePtr, 
                                                                                OnUnreadConversationsCountChangedListenerProxy.OnUnreadConversationsCountChangedListenerDelegate onUnreadConversationsCountChange);

        [DllImport("__Internal")]
        private static extern void _setOnUnreadRoomsCountChangedListener(IntPtr onPublicUnreadConversationsCountChangePtr,
                                                                 IntPtr onPrivateUnreadConversationsCountChangePtr,
                                                                 ChatCallbacks.OnUnreadConversationsCountChangedListenerDelegate callback);

        [DllImport("__Internal")] 
        private static extern void _setOnMessageListener(
            IntPtr onPublicRoomMessagePtr, 
            IntPtr onPrivateRoomMessagePtr, 
            ChatCallbacks.OnMessageReceivedDelegate onMessageReceived);

        [DllImport("__Internal")] 
        private static extern void _setTypingStatusListener(IntPtr onPublicRoomTypingStatusReceivedPtr,
                                                            IntPtr onPrivateRoomTypingStatusReceivedPtr,
                                                            ChatCallbacks.OnTypingStatusReceivedDelegate onTypingStatusReceived);
    }
}
#endif
#endif

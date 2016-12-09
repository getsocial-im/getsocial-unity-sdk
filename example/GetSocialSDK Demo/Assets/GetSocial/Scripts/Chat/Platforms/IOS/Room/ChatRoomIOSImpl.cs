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

using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using GetSocialSdk.Core;

namespace GetSocialSdk.Chat
{
    abstract class ChatRoomIOSImpl : IChatRoom
    {
        protected readonly string roomTopic;

        protected ChatRoomIOSImpl(string roomTopic)
        {
            this.roomTopic = roomTopic;
        }

        public override string ToString()
        {
            return string.Format("[ChatRoomIOSImpl: LastMessage={0}, IsUnread={1}, Id={2}]", LastMessage, IsUnread, Id);
        }

        #region IChatRoom implementation
        public string Id
        {
            get
            {
                return _getRoomId(roomTopic);
            }
        }

        public bool IsUnread
        {
            get
            {
                return _isRoomUnread(roomTopic);
            }
        }

        public ChatMessage LastMessage
        {
            get
            {
                string serializedMessage = _getRoomLastMessage(roomTopic);
#if DEVELOPMENT_BUILD
                UnityEngine.Debug.Log("Room Last Message JSON: " + serializedMessage);
#endif
                return ChatJsonUtils.ParseChatMessage(serializedMessage);
            }
        }

        public void GetMessages(ChatMessage offsetMessage, int limit,
                                Action<List<ChatMessage>> onSuccess,
                                Action<string> onFailure)
        {
            _getRoomMessages(roomTopic, offsetMessage.Guid, limit, 
                onSuccess.GetPointer(), 
                onFailure.GetPointer(), 
                ChatRoomCallbacks.OnRoomUserMessagesReceivedCallback,
                GetSocialNativeBridgeIOS.FailureCallback);
        }

        public void MarkAsRead(Action onSuccess, Action<string> onFailure)
        {
            _markRoomAsRead(roomTopic, 
                onSuccess.GetPointer(), 
                onFailure.GetPointer(), 
                GetSocialNativeBridgeIOS.CompleteCallback,
                GetSocialNativeBridgeIOS.FailureCallback);
        }

        public void SendMessage(ChatMessageContent messageContent, Action onSuccess, Action<string> onFailure)
        {
            // For now we support only sending text messages
            _sendUserMessage(roomTopic, messageContent.MessageText,
                onSuccess.GetPointer(), 
                onFailure.GetPointer(), 
                GetSocialNativeBridgeIOS.CompleteCallback,
                GetSocialNativeBridgeIOS.FailureCallback);
        }

        public void SetTypingStatus(TypingStatus typingStatus,
                                     Action onSuccess, Action<string> onFailure)
        {
            _sendTypingStatus(roomTopic, (int)typingStatus,
                onSuccess.GetPointer(),
                onFailure.GetPointer(),
                GetSocialNativeBridgeIOS.CompleteCallback,
                GetSocialNativeBridgeIOS.FailureCallback);
        }

        #endregion

        #region chat_room_internal

        [DllImport("__Internal")]
        static extern string _getRoomId(string roomTopic);

        [DllImport("__Internal")]
        static extern bool _isRoomUnread(string roomTopic);

        [DllImport("__Internal")]
        static extern string _getRoomLastMessage(string roomTopic);

        [DllImport("__Internal")]
        static extern void _getRoomMessages(string roomTopic, string offsetMessageGuid, int limit, 
                                            IntPtr onSuccessPtr, IntPtr onFailurePtr, 
                                            ChatRoomCallbacks.OnRoomMessagesReceivedCallbackDelegate successCallback,
                                            GetSocialNativeBridgeIOS.FailureCallbackDelegate failureCallback);

        [DllImport("__Internal")]
        static extern void _markRoomAsRead(string roomTopic,
                                           IntPtr onSuccessPtr, IntPtr onFailurePtr,
                                           GetSocialNativeBridgeIOS.CompleteCallbackDelegate successCallback,
                                           GetSocialNativeBridgeIOS.FailureCallbackDelegate failureCallback);

        [DllImport("__Internal")]
        static extern void _sendUserMessage(string roomTopic, string message, 
                                            IntPtr onSuccessPtr, IntPtr onFailurePtr,
                                            GetSocialNativeBridgeIOS.CompleteCallbackDelegate successCallback,
                                            GetSocialNativeBridgeIOS.FailureCallbackDelegate failureCallback);

        [DllImport("__Internal")]
        static extern void _sendTypingStatus(string roomTopic, int typingStatus, 
                                             IntPtr onSuccessPtr, IntPtr onFailurePtr,
                                             GetSocialNativeBridgeIOS.CompleteCallbackDelegate successCallback,
                                             GetSocialNativeBridgeIOS.FailureCallbackDelegate failureCallback);

        #endregion

        #region chat_room_public_internal
        [DllImport("__Internal")]
        protected static extern string _getRoomName(string roomTopic);

        [DllImport("__Internal")]
        protected static extern bool _isSubscribedToPublicRoom(string roomTopic);

        [DllImport("__Internal")]
        protected static extern void _subscribeToPublicRoom(string roomTopic, IntPtr onSuccessPtr, IntPtr onFailurePtr, 
                                                            GetSocialNativeBridgeIOS.CompleteCallbackDelegate successCallback,
                                                            GetSocialNativeBridgeIOS.FailureCallbackDelegate failureCallback);

        [DllImport("__Internal")]
        protected static extern void _unsubscribeFromPublicRoom(string roomTopic, IntPtr onSuccessPtr, IntPtr onFailurePtr, 
                                                                GetSocialNativeBridgeIOS.CompleteCallbackDelegate successCallback,
                                                                GetSocialNativeBridgeIOS.FailureCallbackDelegate failureCallback);
        #endregion

        #region chat_room_private_internal
        [DllImport("__Internal")]
        protected static extern string _getOtherUserForPrivateRoom(string roomTopic);
        #endregion
    }
}
#endif
#endif
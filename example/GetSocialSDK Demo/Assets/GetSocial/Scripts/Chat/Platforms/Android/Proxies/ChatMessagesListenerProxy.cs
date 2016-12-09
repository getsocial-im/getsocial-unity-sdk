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
#if UNITY_ANDROID
using System;
using UnityEngine;
using GetSocialSdk.Core;

namespace GetSocialSdk.Chat
{
    class ChatMessagesListenerProxy : JavaInterfaceProxy
    {
        private readonly Action<IPublicChatRoom, ChatMessage> onPublicRoomMessageAction;
        private readonly Action<IPrivateChatRoom, ChatMessage> onPrivateRoomMessageAction;
        
        internal ChatMessagesListenerProxy(Action<IPublicChatRoom, ChatMessage> onPublicRoomMessageAction,
                                           Action<IPrivateChatRoom, ChatMessage> onPrivateRoomMessageAction)
            : base("im.getsocial.sdk.chat.GetSocialChat$RoomMessageListener")
        {
            this.onPublicRoomMessageAction = onPublicRoomMessageAction;
            this.onPrivateRoomMessageAction = onPrivateRoomMessageAction;
        }

        void onPublicRoomMessage(AndroidJavaObject chatRoom, AndroidJavaObject messageAJO)
        {
            var publicChatRoom = new PublicChatRoomAndroidImpl(chatRoom);
            var message = AndroidChatUtils.ChatMessageFromJavaObject(messageAJO);
            MainThreadExecutor.Queue(() => onPublicRoomMessageAction(publicChatRoom, message));
        }

        void onPrivateRoomMessage(AndroidJavaObject chatRoom, AndroidJavaObject messageAJO)
        {
            var privateChatRoom = new PrivateChatRoomAndroidImpl(chatRoom);
            var message = AndroidChatUtils.ChatMessageFromJavaObject(messageAJO);
            MainThreadExecutor.Queue(() => onPrivateRoomMessageAction(privateChatRoom, message));
        }
    }
}
#endif
#endif
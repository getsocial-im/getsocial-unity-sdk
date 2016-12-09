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
    class ChatRoomTypingStatusListenerProxy : JavaInterfaceProxy
    {
        private readonly Action<IPublicChatRoom, User, TypingStatus> onPublicRoomTypingStatusAction;
        private readonly Action<IPrivateChatRoom, User, TypingStatus> onPrivateRoomTypingStatusAction;

        internal ChatRoomTypingStatusListenerProxy(
            Action<IPublicChatRoom, User, TypingStatus> onPublicRoomTypingStatusAction,
            Action<IPrivateChatRoom, User, TypingStatus> onPrivateRoomTypingStatusAction)
            : base("im.getsocial.sdk.chat.GetSocialChat$RoomTypingStatusListener")
        {
            this.onPublicRoomTypingStatusAction = onPublicRoomTypingStatusAction;
            this.onPrivateRoomTypingStatusAction = onPrivateRoomTypingStatusAction;
        }

        void onPublicRoomTypingStatus(AndroidJavaObject chatRoom, 
                                      AndroidJavaObject userAJO, 
                                      AndroidJavaObject typingStatusAJO)
        {
            var publicChatRoom = new PublicChatRoomAndroidImpl(chatRoom);
            var user = AndroidUtils.UserFromJavaObj(userAJO);
            var typingStatus = AndroidChatUtils.TypingStatusFromAJO(typingStatusAJO);

            MainThreadExecutor.Queue(() => onPublicRoomTypingStatusAction(publicChatRoom, user, typingStatus));
        }

        void onPrivateRoomTypingStatus(AndroidJavaObject chatRoom, 
                                       AndroidJavaObject userAJO, 
                                       AndroidJavaObject typingStatusAJO)
        {
            var privateChatRoom = new PrivateChatRoomAndroidImpl(chatRoom);
            var user = AndroidUtils.UserFromJavaObj(userAJO);
            var typingStatus = AndroidChatUtils.TypingStatusFromAJO(typingStatusAJO);

            MainThreadExecutor.Queue(() => onPrivateRoomTypingStatusAction(privateChatRoom, user, typingStatus));
        }
    }
}
#endif
#endif
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
using GetSocialSdk.Core;

namespace GetSocialSdk.Chat
{
    class PublicChatRoomIOSImpl : ChatRoomIOSImpl, IPublicChatRoom
    {
        internal PublicChatRoomIOSImpl(string roomTopic) : base(roomTopic)
        {
        }

        public string Name
        {
            get
            {
                return ChatRoomIOSImpl._getRoomName(roomTopic);
            }
        }

        public bool IsSubscribed
        {
            get
            {
                return ChatRoomIOSImpl._isSubscribedToPublicRoom(roomTopic);
            }
        }

        public void Subscribe(Action onSuccess, Action<string> onFailure)
        {
            ChatRoomIOSImpl._subscribeToPublicRoom(roomTopic, onSuccess.GetPointer(), onFailure.GetPointer(),
                GetSocialNativeBridgeIOS.CompleteCallback, GetSocialNativeBridgeIOS.FailureCallback);
        }

        public void Unsubscribe(Action onSuccess, Action<string> onFailure)
        {
            ChatRoomIOSImpl._unsubscribeFromPublicRoom(roomTopic, onSuccess.GetPointer(), onFailure.GetPointer(),
                GetSocialNativeBridgeIOS.CompleteCallback, GetSocialNativeBridgeIOS.FailureCallback);
        }

        public override string ToString()
        {
            return string.Format("[PublicChatRoomIOSImpl: IsSubscribed={0}, Name={1}, Base={2}]", IsSubscribed, Name, base.ToString());
        }
    }
}

#endif
#endif
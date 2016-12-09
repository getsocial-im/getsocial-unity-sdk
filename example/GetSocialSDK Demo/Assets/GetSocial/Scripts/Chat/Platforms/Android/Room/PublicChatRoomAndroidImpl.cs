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

#if UNITY_ANDROID
#if ENABLE_GETSOCIAL_CHAT
using UnityEngine;
using System;
using GetSocialSdk.Core;

namespace GetSocialSdk.Chat
{
    class PublicChatRoomAndroidImpl : ChatRoomAndroidImpl, IPublicChatRoom
    {
        internal PublicChatRoomAndroidImpl(AndroidJavaObject chatRoomJavaObject) : base(chatRoomJavaObject)
        {
            if(chatRoomJavaObject.GetSimpleClassName() != "PublicChatRoom")
            {
                Debug.LogError("Current AndroidJavaObject is not public chat room but is: " + chatRoomJavaObject.GetSimpleClassName());
            }
        }

        #region IPublicChatRoom implementation
        public void Subscribe(Action onSuccess, Action<string> onFailure)
        {
            AndroidUtils.RunOnUiThread(() =>
                chatRoomJavaObject.Call("subscribe", new OperationVoidCallbackProxy(onSuccess, onFailure)));
        }

        public void Unsubscribe(Action onSuccess, Action<string> onFailure)
        {
            AndroidUtils.RunOnUiThread(() =>
                chatRoomJavaObject.Call("unsubscribe", new OperationVoidCallbackProxy(onSuccess, onFailure)));
        }

        public bool IsSubscribed
        {
            get
            {
                return chatRoomJavaObject.Call<bool>("isSubscribed");
            }
        }
        #endregion


        public override string ToString()
        {
            return string.Format("[PublicChatRoomIOSImpl: IsSubscribed={0}, Name={1}, Base={2}]", IsSubscribed, Name, base.ToString());
        }
    }
}
#endif
#endif
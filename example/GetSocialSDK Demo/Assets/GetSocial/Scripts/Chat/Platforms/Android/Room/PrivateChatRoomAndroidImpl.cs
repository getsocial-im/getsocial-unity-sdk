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
using GetSocialSdk.Core;

namespace GetSocialSdk.Chat
{
    class PrivateChatRoomAndroidImpl : ChatRoomAndroidImpl, IPrivateChatRoom
    {
        internal PrivateChatRoomAndroidImpl(AndroidJavaObject chatRoomJavaObject) : base(chatRoomJavaObject)
        {
            if (chatRoomJavaObject.GetSimpleClassName() != "PrivateChatRoom")
            {
                Debug.LogError("Current AndroidJavaObject is not private chat room but is: " + chatRoomJavaObject.GetSimpleClassName());
            }
        }

        #region IPrivateChatRoom implementation
        public User OtherParticipant
        {
            get
            {
                return AndroidUtils.UserFromJavaObj(chatRoomJavaObject.Call<AndroidJavaObject>("getOtherParticipant"));
            }
        }
        #endregion


        public override string ToString()
        {
            return string.Format("[PrivateChatRoomIOSImpl: OtherParticipant={0}] {1}", OtherParticipant, base.ToString());
        }
    }
}
#endif
#endif
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
using System.Collections.Generic;
using UnityEngine;
using GetSocialSdk.Core;

namespace GetSocialSdk.Chat
{
    class GetSocialChatAndroid : IGetSocialChatNativeBridge
    {
        private static GetSocialChatAndroid instance;

        private AndroidJavaObject getSocialChatJavaObject;

        #region initialization

        private GetSocialChatAndroid()
        {
            using(AndroidJavaObject clazz = new AndroidJavaClass("im.getsocial.sdk.chat.unity.GetSocialChatUnityBridge"))
            {
                getSocialChatJavaObject = clazz.CallStatic<AndroidJavaObject>("getInstance");
            }
            
            if(AndroidUtils.IsJavaNull(getSocialChatJavaObject))
            {
                Debug.LogError(string.Format("Failed to initialize GetSocial Chat module. Please make sure project is configured properly"));
            }
        }

        internal static GetSocialChatAndroid GetInstance()
        {
            if(instance == null)
            {
                instance = new GetSocialChatAndroid();
            }
            return instance;
        }

        #endregion


        #region IGetSocialChat implementation

        public int UnreadPublicRoomsCount
        {
            get
            {
                return GetInstance().getSocialChatJavaObject.Call<int>("getUnreadPublicRoomsCount");
            }
        }

        public int UnreadPrivateRoomsCount
        {
            get
            {
                return GetInstance().getSocialChatJavaObject.Call<int>("getUnreadPrivateRoomsCount");
            }
        }

        public bool IsEnabled
        {
            get
            {
                return GetInstance().getSocialChatJavaObject.Call<bool>("isEnabled");
            }
        }

        public void GetPublicRoom(string name, Action<IPublicChatRoom> onSuccess, Action<string> onFailure)
        {
            var callback = new OperationGenericCallbackProxy<IPublicChatRoom>(onSuccess, onFailure, AndroidChatUtils.PublicChatRoomFromJavaObject);
            GetInstance().getSocialChatJavaObject.Call("getPublicRoom", name, callback);
        }

        public void GetAllPrivateRooms(Action<List<IPrivateChatRoom>> onSuccess, Action<string> onFailure)
        {
            var callback = new OperationGenericCallbackProxy<List<IPrivateChatRoom>>(onSuccess, onFailure, AndroidChatUtils.PublicChatRoomsFromJavaObject);
            GetInstance().getSocialChatJavaObject.Call("getAllPrivateRooms", callback);
        }

        public void GetPrivateRoom(User user, Action<IPrivateChatRoom> onSuccess, Action<string> onFailure)
        {
            var callback = new OperationGenericCallbackProxy<IPrivateChatRoom>(onSuccess, onFailure, AndroidChatUtils.PrivateChatRoomFromJavaObject);
            GetInstance().getSocialChatJavaObject.Call("getPrivateRoom", user.Guid, callback);
        }

        public void GetPrivateRoom(string provider, string userId, Action<IPrivateChatRoom> onSuccess, Action<string> onFailure)
        {
            var callback = new OperationGenericCallbackProxy<IPrivateChatRoom>(onSuccess, onFailure, AndroidChatUtils.PrivateChatRoomFromJavaObject);
            GetInstance().getSocialChatJavaObject.Call("getPrivateRoom", provider, userId, callback);
        }

        public void SetOnUnreadConversationsCountChangeListener(Action<int> onUnreadConversationsCountChange)
        {
            GetInstance().getSocialChatJavaObject.Call("setOnUnreadConversationsCountChangeListener", new OnUnreadConversationsCountChangeListenerProxy(onUnreadConversationsCountChange));
        }

        public void SetOnUnreadCountChangedListenerInternal(Action<int> onUnreadPublicRoomsCountChanged,
                                                            Action<int> onUnreadPrivateRoomsCountChanged)
        {
            GetInstance().getSocialChatJavaObject.Call("setOnUnreadRoomsCountChangedListener",
                new OnUnreadRoomsCountChangedListenerProxy(onUnreadPublicRoomsCountChanged, onUnreadPrivateRoomsCountChanged));
        }

        public void SetMessagesListenerInternal(Action<IPublicChatRoom, ChatMessage> onPublicRoomMessage,
                                                Action<IPrivateChatRoom, ChatMessage> onPrivateRoomMessage)
        {
            GetInstance().getSocialChatJavaObject.Call("setRoomMessageListener",
                new ChatMessagesListenerProxy(onPublicRoomMessage, onPrivateRoomMessage));
        }

        public void SetTypingStatusListenerInternal(Action<IPublicChatRoom, User, TypingStatus> onPublicRoomTypingStatusReceived,
                                                    Action<IPrivateChatRoom, User, TypingStatus> onPrivateRoomTypingStatusReceived)
        {
            GetInstance().getSocialChatJavaObject.Call("setTypingStatusListener",
                new ChatRoomTypingStatusListenerProxy(onPublicRoomTypingStatusReceived, onPrivateRoomTypingStatusReceived));
        }

        #endregion
    }
}
#endif
#endif

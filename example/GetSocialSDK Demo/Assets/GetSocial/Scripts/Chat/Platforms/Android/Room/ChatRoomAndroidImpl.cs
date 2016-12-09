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
using System.Collections.Generic;
using GetSocialSdk.Core;

namespace GetSocialSdk.Chat
{
    /// <summary>
    /// Chat room android implementation forawrds all calls to native.
    /// </summary>
    abstract class ChatRoomAndroidImpl : IChatRoom
    {
        protected readonly AndroidJavaObject chatRoomJavaObject;

        protected ChatRoomAndroidImpl(AndroidJavaObject chatRoomJavaObject)
        {
            this.chatRoomJavaObject = chatRoomJavaObject;
        }

        #region IChatRoom implementation
        public string Id
        {
            get
            {
                return chatRoomJavaObject.Call<string>("getRoomId");
            }
        }

        public string Name
        {
            get
            {
                return chatRoomJavaObject.Call<string>("getName");
            }
        }

        public bool IsUnread
        {
            get
            {
                return chatRoomJavaObject.Call<bool>("isUnread");
            }
        }

        public ChatMessage LastMessage
        {
            get
            {
                try
                {
                    
                    AndroidJavaObject lastMessage = chatRoomJavaObject.Call<AndroidJavaObject>("getLastMessage");
                    return AndroidChatUtils.ChatMessageFromJavaObject(lastMessage);
                }
                catch(Exception e)
                {
#if DEVELOPMENT_BUILD
                    Debug.Log("Last chat message not present. \n" + e.StackTrace + "\nReason: " + e.Message);
#endif
                    return null;
                }
            }
        }

        public void GetMessages(ChatMessage offsetMessage, int limit, 
                                Action<List<ChatMessage>> onSuccess,
                                Action<string> onFailure)
        {
            AndroidUtils.RunOnUiThread(() =>
            {
                chatRoomJavaObject.Call("getMessagesBeforeChatMessageByGuid", 
                    offsetMessage.Guid, limit, 
                    new OperationGenericCallbackProxy<List<ChatMessage>>(onSuccess, onFailure, AndroidChatUtils.ChatMessagesFromJavaObject));
            });
        }

        public void MarkAsRead(Action onSuccess, Action<string> onFailure)
        {
            AndroidUtils.RunOnUiThread(() =>
            {
                chatRoomJavaObject.Call("markAsRead", 
                    new OperationVoidCallbackProxy(onSuccess, onFailure));
            });
        }

        public void SendMessage(ChatMessageContent messageContent, Action onSuccess, Action<string> onFailure)
        {
            AndroidUtils.RunOnUiThread(() =>
            {
                chatRoomJavaObject.Call("sendMessage", 
                    AndroidChatUtils.CreateChatMessageContentAJO(messageContent.MessageText), 
                    new OperationVoidCallbackProxy(onSuccess, onFailure));
            });
        }

        public void SetTypingStatus(TypingStatus typingStatus, Action onSuccess, Action<string> onFailure)
        {
            AndroidUtils.RunOnUiThread(() =>
            {
                chatRoomJavaObject.Call("setTypingStatus", 
                    AndroidChatUtils.GetTypingStatusAJO(typingStatus), 
                    new OperationVoidCallbackProxy(onSuccess, onFailure));
            });
        }
        #endregion

        public override string ToString()
        {
            return string.Format("[ChatRoomIOSImpl: LastMessage={0}, IsUnread={1}, Id={2}]", LastMessage, IsUnread, Id);
        }
    }
}
#endif
#endif
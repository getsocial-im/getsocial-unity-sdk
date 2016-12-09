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
    public static class AndroidChatUtils
    {
        public static ChatMessage ChatMessageFromJavaObject(AndroidJavaObject chatMessageAJO)
        {
            User sender = AndroidUtils.UserFromJavaObj(chatMessageAJO.Call<AndroidJavaObject>("getSender"));
            string guid = chatMessageAJO.Call<string>("getGuid");
            DateTime timestamp = AndroidUtils.DateTimeFromJavaUtilDate(chatMessageAJO.Call<AndroidJavaObject>("getTimestamp"));
            bool wasSentByMe = chatMessageAJO.Call<bool>("wasSentByMe");

            // For now - only text messges supproted
            var msgText = chatMessageAJO.Call<AndroidJavaObject>("getContent").Call<string>("getText");

            chatMessageAJO.Dispose();
            return new ChatMessage(sender, guid, timestamp, wasSentByMe, ChatMessageContent.CreateWithText(msgText));
        }

        public static List<ChatMessage> ChatMessagesFromJavaObject(AndroidJavaObject chatMessagesAJO)
        {
            var result = new List<ChatMessage>();
            int listSize = chatMessagesAJO.Call<int>("size");
            for(var i = 0; i < listSize; i++)
            {
                var messageAJO = chatMessagesAJO.Call<AndroidJavaObject>("get", i);
                result.Add(ChatMessageFromJavaObject(messageAJO));
            }

            chatMessagesAJO.Dispose();
            return result;
        }

        public static List<IPrivateChatRoom> PublicChatRoomsFromJavaObject(AndroidJavaObject roomsAJO)
        {
            var result = new List<IPrivateChatRoom>();
            int listSize = roomsAJO.Call<int>("size");
            for(var i = 0; i < listSize; i++)
            {
                var room = roomsAJO.Call<AndroidJavaObject>("get", i);
                result.Add(PrivateChatRoomFromJavaObject(room));
            }

            return result;
        }

        public static IPublicChatRoom PublicChatRoomFromJavaObject(AndroidJavaObject roomAJO)
        {
            if(roomAJO == null)
            {
                Debug.LogError("Public chat room AndroidJavaObject is null");
            }

            if(roomAJO.GetSimpleClassName() != "PublicChatRoom")
            {
                Debug.LogError("This AndroidJavaObject is not public chat room!");
            }

            return new PublicChatRoomAndroidImpl(roomAJO);
        }

        public static IPrivateChatRoom PrivateChatRoomFromJavaObject(AndroidJavaObject roomAJO)
        {
            if(roomAJO == null)
            {
                Debug.LogError("Private chat room AndroidJavaObject is null");
            }

            if(roomAJO.GetSimpleClassName() != "PrivateChatRoom")
            {
                Debug.LogError("This AndroidJavaObject is not private chat room!");
            }

            return new PrivateChatRoomAndroidImpl(roomAJO);
        }

        public static AndroidJavaObject CreateChatMessageContentAJO(string message)
        {
            using(var msgContentClass = new AndroidJavaClass("im.getsocial.sdk.chat.ChatMessageContent"))
            {
                return msgContentClass.CallStatic<AndroidJavaObject>("createWithText", message);
            }
        }

        public static AndroidJavaObject GetTypingStatusAJO(TypingStatus typingStatus)
        {
            using(var msgContentClass = new AndroidJavaClass("im.getsocial.sdk.chat.TypingStatus"))
            {
                return msgContentClass.CallStatic<AndroidJavaObject>("fromInt", (int) typingStatus);
            }
        }

        public static TypingStatus TypingStatusFromAJO(AndroidJavaObject typingStatusAJO)
        {
            var status = (TypingStatus) typingStatusAJO.Call<int>("getIntValue");
            typingStatusAJO.Dispose();
            return status;
        }
    }
}
#endif
#endif
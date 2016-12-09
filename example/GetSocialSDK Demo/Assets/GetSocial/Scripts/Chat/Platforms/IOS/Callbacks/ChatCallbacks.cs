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
using GetSocialSdk.Core;

namespace GetSocialSdk.Chat
{
    static class ChatCallbacks
    {
        public delegate void OnUnreadConversationsCountChangedListenerDelegate(IntPtr actionPtr, int unreadConversationsCount);

        public delegate void OnRoomReceivedDelegate(IntPtr actionPtr, int roomType, string roomTopic);

        public delegate void OnRoomsReceivedDelegate(IntPtr actionPtr, int roomType, string roomTopicsJsonArray);

        public delegate void OnMessageReceivedDelegate(IntPtr actionPtr, string roomTopic, int roomType, string serializedMesssage);

        public delegate void OnTypingStatusReceivedDelegate(IntPtr actionPtr, string roomTopic, int roomType, string serializedUser, int typingStatus);

        // Room type
        private const int RoomTypePrivate = 0;
        private const int RoomTypePublic = 1;

        [MonoPInvokeCallback(typeof(OnUnreadConversationsCountChangedListenerDelegate))]
        public static void OnUnreadConversationsCountChangeCallback(IntPtr actionPtr, int unreadConversationsCount)
        {
#if DEVELOPMENT_BUILD
            UnityEngine.Debug.Log("OnUnreadConversationsCountChangeCallback: " + unreadConversationsCount);
#endif
            if (actionPtr == IntPtr.Zero) { return; }

            var action = actionPtr.Cast<Action<int>>();
            action(unreadConversationsCount);
        }

        #region get_rooms

        [MonoPInvokeCallback(typeof(OnRoomReceivedDelegate))]
        public static void OnRoomReceivedCallback(IntPtr actionPtr, int roomType, string roomTopic)
        {
#if DEVELOPMENT_BUILD
            UnityEngine.Debug.Log("Room callback: " + roomTopic);
#endif
            if (actionPtr == IntPtr.Zero) { return; }

            if(roomType == RoomTypePublic)
            {
                var action = actionPtr.Cast<Action<IPublicChatRoom>>();
                action(new PublicChatRoomIOSImpl(roomTopic));
            }
            else if(roomType == RoomTypePrivate)
            {
                var action = actionPtr.Cast<Action<IPrivateChatRoom>>();
                action(new PrivateChatRoomIOSImpl(roomTopic));
            }
            else
            {
                throw new InvalidOperationException("Unknown room type: " + roomType);
            }
        }

        [MonoPInvokeCallback(typeof(OnRoomsReceivedDelegate))]
        public static void OnRoomsReceivedCallback(IntPtr actionPtr, int roomType, string roomTopicsJsonArray)
        {
#if DEVELOPMENT_BUILD
            UnityEngine.Debug.Log("Room topics callback: " + roomTopicsJsonArray + ", room type: " + roomType);
#endif
            if(actionPtr == IntPtr.Zero) { return; }

            string[] roomTopics = ChatJsonUtils.ParseTopics(roomTopicsJsonArray);

            if(roomType == RoomTypePublic)
            {
                var action = actionPtr.Cast<Action<List<IPublicChatRoom>>>();
                action(ConvertToPublicRooms(roomTopics));
            }
            else if(roomType == RoomTypePrivate)
            {
                var action = actionPtr.Cast<Action<List<IPrivateChatRoom>>>();
                action(ConvertToPrivateRooms(roomTopics));
            }
            else
            {
                throw new InvalidOperationException("Unknown room type: " + roomType);
            }
        }

        private static List<IPublicChatRoom> ConvertToPublicRooms(string[] roomTopics)
        {
            var result = new List<IPublicChatRoom>(roomTopics.Length);
            foreach(var topic in roomTopics)
            {
                result.Add(new PublicChatRoomIOSImpl(topic));
            }
            return result;
        }

        private static List<IPrivateChatRoom> ConvertToPrivateRooms(string[] roomTopics)
        {
            var result = new List<IPrivateChatRoom>(roomTopics.Length);
            foreach(var topic in roomTopics)
            {
                result.Add(new PrivateChatRoomIOSImpl(topic));
            }
            return result;
        }
        #endregion

        #region room_updates_listener
        [MonoPInvokeCallback(typeof(OnMessageReceivedDelegate))]
        public static void OnMessageReceivedCallback(IntPtr actionPtr, 
                                                     string roomTopic, int roomType, string serializedMessage)
        {
#if DEVELOPMENT_BUILD
            UnityEngine.Debug.Log(string.Format("Received message, roomId: {0}, roomType: {1}, message: {2}",
                                                roomTopic, roomType, serializedMessage));
#endif
            if (actionPtr == IntPtr.Zero) { return; }

            var message = ChatJsonUtils.ParseChatMessage(serializedMessage);

            if(roomType == RoomTypePublic)
            {
                var room = new PublicChatRoomIOSImpl(roomTopic);
                actionPtr.Cast<Action<IPublicChatRoom, ChatMessage>>().Invoke(room, message);
            }
            else if(roomType == RoomTypePrivate)
            {
                var room = new PrivateChatRoomIOSImpl(roomTopic);
                actionPtr.Cast<Action<IPrivateChatRoom, ChatMessage>>().Invoke(room, message);
            }
            else
            {
                throw new InvalidOperationException("Unknown room type: " + roomType);
            }
        }

        [MonoPInvokeCallback(typeof(OnTypingStatusReceivedDelegate))]
        public static void OnTypingStatusReceivedCallback(IntPtr actionPtr, string roomTopic, int roomType, string serializedUser, int typingStatus)
        {
#if DEVELOPMENT_BUILD
            UnityEngine.Debug.Log(string.Format("Received typing status, roomId: {0}, roomType: {1}, user: {2}, typing status: {3}",
                                                roomTopic, roomType, serializedUser, typingStatus));
#endif

            if (actionPtr == IntPtr.Zero) { return; }

            var user = new User(new JSONObject(serializedUser));
            TypingStatus receivedTypingStatus = (TypingStatus) typingStatus;

            if(roomType == RoomTypePublic)
            {
                var room = new PublicChatRoomIOSImpl(roomTopic);
                actionPtr.Cast<Action<IPublicChatRoom, User, TypingStatus>>().Invoke(room, user, receivedTypingStatus);
            }
            else if(roomType == RoomTypePrivate)
            {
                var room = new PrivateChatRoomIOSImpl(roomTopic);
                actionPtr.Cast<Action<IPrivateChatRoom, User, TypingStatus>>().Invoke(room, user, receivedTypingStatus);
            }
            else
            {
                throw new InvalidOperationException("Unknown room type: " + roomType);
            }
        }
        #endregion
    }
}
#endif
#endif

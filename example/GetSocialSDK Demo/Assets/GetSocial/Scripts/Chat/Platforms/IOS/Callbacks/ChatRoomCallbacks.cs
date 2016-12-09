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
    static class ChatRoomCallbacks
    {
        public delegate void OnMessageReceivedCallbackDelegate(IntPtr actionPtr, string serializedMessage);
        public delegate void OnRoomMessagesReceivedCallbackDelegate(IntPtr actionPtr, string serializedMessages);

        [MonoPInvokeCallback(typeof(OnMessageReceivedCallbackDelegate))]
        public static void OnRoomMessageCallback(IntPtr actionPtr, string serializedMessage)
        {
#if DEVELOPMENT_BUILD
            UnityEngine.Debug.Log(System.Reflection.MethodBase.GetCurrentMethod() + serializedMessage);
#endif      
            if (actionPtr == IntPtr.Zero) { return; }
            
            var action = actionPtr.Cast<Action<ChatMessage>>();
            action(ChatJsonUtils.ParseChatMessage(serializedMessage));
        }

        [MonoPInvokeCallback(typeof(OnRoomMessagesReceivedCallbackDelegate))]
        public static void OnRoomUserMessagesReceivedCallback(IntPtr actionPtr, string serializedMessages)
        {
#if DEVELOPMENT_BUILD
            UnityEngine.Debug.Log(System.Reflection.MethodBase.GetCurrentMethod() + serializedMessages);
#endif
            if (actionPtr == IntPtr.Zero) { return; }

            var action = actionPtr.Cast<Action<List<ChatMessage>>>();
            action(ChatJsonUtils.ParseChatMessages(serializedMessages));
        }
    }
}
#endif
#endif
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

using UnityEngine;
using System;
using System.Collections.Generic;
using GetSocialSdk.Core;

namespace GetSocialSdk.Chat
{
    static class ChatJsonUtils
    {
        private const string JsonPropertyUser = "sender";
        private const string JsonPropertyGuid = "guid";
        private const string JsonPropertyTimestamp = "timestamp";
        private const string JsonPropertyWasSentByMe = "wasSentByMe";

        private const string JsonPropertyContent = "rawContent";
        private const string JsonPropertyContentText = "text";

        public static ChatMessage ParseChatMessage(string serializedMessage)
        {
            if (string.IsNullOrEmpty(serializedMessage)) { return null; }

            var jsonMessage = new JSONObject(serializedMessage);

            var sender = new User(jsonMessage[JsonPropertyUser]);
            var timestamp = ParseUtils.UnixTimeStampToDateTime(jsonMessage[JsonPropertyTimestamp].n);

            string guid = null;
            if (jsonMessage.HasField(JsonPropertyGuid))
            {
                guid = jsonMessage[JsonPropertyGuid].str;
            }
            else
            {
                Debug.LogError("Message GUID is not present in received message.");
            }

            var wasSentByMe = jsonMessage[JsonPropertyWasSentByMe].b;

            var text = jsonMessage[JsonPropertyContent][JsonPropertyContentText].str;
            var content = new ChatMessageContent(text);

            return new ChatMessage(sender, guid, timestamp, wasSentByMe, content);
        }
        
        public static List<ChatMessage> ParseChatMessages(string serializedUserMessages)
        {
            var jsonMessages = new JSONObject(serializedUserMessages);
            var messages = new List<ChatMessage>(jsonMessages.list.Count);

            foreach (var jsonMessage in jsonMessages.list)
            {
                messages.Add(ParseChatMessage(jsonMessage.ToString()));
            }

            return messages;
        }

        public static string[] ParseTopics(string topicsJsonArray)
        {
            var json = new JSONObject(topicsJsonArray);
            var topics = new string[json.list.Count];
            for (int i = 0; i < topics.Length; i++)
            {
                topics[i] = json.list[i].str;
            }
            return topics;
        }
    }
}
#endif
#endif
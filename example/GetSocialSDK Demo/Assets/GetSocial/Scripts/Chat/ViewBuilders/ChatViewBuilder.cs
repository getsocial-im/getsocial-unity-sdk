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
using GetSocialSdk.Core;

namespace GetSocialSdk.Chat
{
    /// <summary>
    /// Builder to construct chat window.
    /// </summary>
    public class ChatViewBuilder : ViewBuilder
    {
        private string userId;
        private string providerId;
        private string conversationId;
        private string roomName;

        #region initialization
        private ChatViewBuilder(IGetSocialNativeBridge getSocialImpl) : base(getSocialImpl) {}

        /// <summary>
        /// Must not be invoked directly. Call <see cref="GetSocialChat.CreateChatViewForUserId"/> instead.
        /// </summary>
        public static ChatViewBuilder ConstructWithUserId(IGetSocialNativeBridge getSocialImpl, string userId)
        {
            Check.Argument.IsNotNull(userId, "userId", "Can't create chat view with null userId");

            var chatViewBuilder = new ChatViewBuilder(getSocialImpl);
            chatViewBuilder.userId = userId;

            return chatViewBuilder;
        }

        /// <summary>
        /// Must not be invoked directly. Call <see cref="GetSocialChat.CreateChatViewForUserIdOnProvider"/> instead.
        /// </summary>
        public static ChatViewBuilder ConstructWithUserIdAndProviderId(IGetSocialNativeBridge getSocialImpl, string userId, string providerId)
        {
            Check.Argument.IsNotNull(userId, "userId", "Can't create chat view with null userId");
            Check.Argument.IsNotNull(providerId, "providerId", "Can't create chat view with null providerId");

            var chatViewBuilder = new ChatViewBuilder(getSocialImpl);
            chatViewBuilder.userId = userId;
            chatViewBuilder.providerId = providerId;

            return chatViewBuilder;
        }

        /// <summary>
        /// Must not be invoked directly. Call <see cref="GetSocialChat.CreateChatViewForConversationId"/> instead.
        /// </summary>
        public static ChatViewBuilder ConstructWithConversationId(IGetSocialNativeBridge getSocialImpl, string conversationId)
        {
            Check.Argument.IsNotNull(conversationId, "conversationId", "Can't create chat view with null conversationId");

            var chatViewBuilder = new ChatViewBuilder(getSocialImpl);
            chatViewBuilder.conversationId = conversationId;

            return chatViewBuilder;
        }

        /// <summary>
        /// Must not be invoked directly. Call <see cref="GetSocialChat.CreateChatViewForRoomName"/> instead.
        /// </summary>
        public static ChatViewBuilder ConstructWithRoomName(IGetSocialNativeBridge getSocialImpl, string roomName)
        {
            Check.Argument.IsNotNull(roomName, "roomName", "Can't create chat view with null roomName");

            var chatViewBuilder = new ChatViewBuilder(getSocialImpl);
            chatViewBuilder.roomName = roomName;

            return chatViewBuilder;
        }

        #endregion

        #region implemented/override members of ViewBuilder
        /// <summary>
        /// Sets the title of the chat view.
        /// </summary>
        /// <returns>The <c>ChatViewBuilder</c> instance.</returns>
        /// <param name="title">New title.</param>
        public new ChatViewBuilder SetTitle(string title)
        {
            return (ChatViewBuilder)base.SetTitle(title);
        }

        protected override JSONObject ToJson()
        {
            var json = base.ToJson();

            json.SetFieldOptional(PropertyUserId, userId);
            json.SetFieldOptional(PropertyProvider, providerId);
            json.SetFieldOptional(PropertyConversationId, conversationId);
            json.SetFieldOptional(PropertyRoomName, roomName);

            return json;
        }

        protected override string GetViewId()
        {
            return ViewChat;
        }
        #endregion
    }
}
#endif

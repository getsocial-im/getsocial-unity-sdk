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
    /// Builder to construct window with the list of user chats.
    /// </summary>
    public class ChatListViewBuilder : ViewBuilder
    {
        #region initialization
        private ChatListViewBuilder(IGetSocialNativeBridge getSocialImpl) : base(getSocialImpl) {}

        /// <summary>
        /// Must not be invoked directly. Invoke <c><see cref="GetSocialChat.CreateChatListView"/></c> instead.
        /// </summary>
        public static ChatListViewBuilder Construct(IGetSocialNativeBridge getSocialImpl)
        {
            return new ChatListViewBuilder(getSocialImpl);
        }
        #endregion

        #region implemented/override members of ViewBuilder
        /// <summary>
        /// Sets the title of the chat view.
        /// </summary>un
        /// <returns>The <c>ChatListViewBuilder</c> instance.</returns>
        /// <param name="title">New title.</param>
        public new ChatListViewBuilder SetTitle(string title)
        {
            return (ChatListViewBuilder)base.SetTitle(title);
        }

        protected override string GetViewId()
        {
            return ViewChatList;
        }
        #endregion
    }
}
#endif

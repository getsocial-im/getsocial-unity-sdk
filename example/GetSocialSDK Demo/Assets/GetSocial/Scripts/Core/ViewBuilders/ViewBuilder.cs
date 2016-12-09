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

namespace GetSocialSdk.Core
{
    public abstract class ViewBuilder
    {
        public delegate void OnViewActionDelegate(string actionId, string actionData);

        protected const string ViewActivities = "activities";
        protected const string ViewChat = "chat";
        protected const string ViewChatList = "chatList";
        protected const string ViewNotifications = "notifications";
        protected const string ViewSmartInvite = "smartInvite";
        protected const string ViewUserList = "userList";

        protected const string PropertyViewBuilder = "viewBuilder";
        protected const string PropertyRoomName = "roomName";
        protected const string PropertyUserId = "userId";
        protected const string PropertyLeaderboardId = "leaderboardId";
        protected const string PropertyConversationId = "conversationId";
        protected const string PropertyTitle = "title";
        protected const string PropertyGroup = "group";
        protected const string PropertyTags = "tags";
        protected const string PropertyProvider = "provider";
        protected const string PropertyImage = "image";
        protected const string PropertyText = "text";
        protected const string PropertySubject = "subject";
        protected const string PropertyReferralData = "referralData";

        protected string title;

        private IGetSocialNativeBridge getSocialImpl;

        protected ViewBuilder(IGetSocialNativeBridge getSocialImpl)
        {
            this.getSocialImpl = getSocialImpl;
        }

        /// <summary>
        /// Shows the view to the user with a callback.
        /// </summary>
        public void Show()
        {
            var serializedViewBuilder = ToJson().Print(pretty: GetSocialSettings.IsDebugLogsEnabled);
#if DEVELOPMENT_BUILD
            Log.D("Serialized view builder: {0}", serializedViewBuilder);
#endif
            getSocialImpl.ShowView(serializedViewBuilder, OnViewAction);
        }

        protected virtual void OnViewAction(string actionId, string actionData)
        {
        }

        /// <summary>
        /// Sets the title of the view.
        /// </summary>
        /// <returns>The <c>ViewBuilder</c> instance.</returns>
        /// <param name="title">New title.</param>
        protected virtual ViewBuilder SetTitle(string title)
        {
            this.title = title;
            return this;
        }

        protected virtual JSONObject ToJson()
        {
            var json = new JSONObject();
            
            json.SetField(PropertyViewBuilder, GetViewId());
            json.SetFieldOptional(PropertyTitle, title);
            
            return json;
        }

        protected abstract string GetViewId();
    }
}


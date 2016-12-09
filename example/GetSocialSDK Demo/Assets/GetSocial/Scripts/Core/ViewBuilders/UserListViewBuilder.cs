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

using System;
using UnityEngine;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Builder to friends list window.
    /// </summary>
    public sealed class UserListViewBuilder : ViewBuilder
    {
        /// <summary>
        /// Constants for different user list types
        /// </summary>
        public enum UserListType
        {
            /// <summary>
            /// List of following.
            /// </summary>
            Following = 1,
            /// <summary>
            /// List of followers.
            /// </summary>
            Followers = 2
        }

        private const string UserListViewTypeProperty = "viewType";

        private const string ActionDidSelectedUser = "didSelectUser";
        private const string ActionDidCancel = "didCancel";

        private UserListType userListType = UserListType.Following;
        private Action<User> onUserSelected;
        private Action onCancel;

        #region initialization
        private UserListViewBuilder(IGetSocialNativeBridge getSocialImpl, UserListType userListType, Action<User> onUserSelected, Action onCancel)
            : base(getSocialImpl)
        {
            this.onUserSelected = onUserSelected;
            this.onCancel = onCancel;
            this.userListType = userListType;
        }

        /// <summary>
        /// Must not be invoked directly. Invoke <c><see cref="GetSocial.CreateUserListView"/></c> instead.
        /// </summary>
        public static UserListViewBuilder Construct(IGetSocialNativeBridge getSocialImpl, UserListType type,
                                                    Action<User> onUserSelected, Action onCancel)
        {
            return new UserListViewBuilder(getSocialImpl, type, onUserSelected, onCancel);
        }
        #endregion

        #region implemented/override members of ViewBuilder
        /// <summary>
        /// Sets the title of the friends list window.
        /// </summary>
        /// <returns>The <c>UserListViewBuilder</c> instance.</returns>
        /// <param name="title">New title of friends list window.</param>
        public new UserListViewBuilder SetTitle(string title)
        {
            return (UserListViewBuilder)base.SetTitle(title);
        }

        protected override void OnViewAction(string actionId, string serializedUser)
        {
            switch(actionId)
            {
            case ActionDidSelectedUser:
                if(string.IsNullOrEmpty(serializedUser))
                {
                    Debug.LogError("User is null or empty");
                    onCancel();
                    return;
                }
                var userIdentity = new User(new JSONObject(serializedUser));
                onUserSelected(userIdentity);
                break;
            case ActionDidCancel:
                onCancel();
                break;
            }
        }

        protected override JSONObject ToJson()
        {
            var json = base.ToJson();
            json.SetField(UserListViewTypeProperty, (int)userListType);
            return json;
        }

        protected override string GetViewId()
        {
            return ViewUserList;
        }
        #endregion
    }
}
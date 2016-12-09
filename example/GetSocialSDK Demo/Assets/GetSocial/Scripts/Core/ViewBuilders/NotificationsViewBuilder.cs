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
    /// <summary>
    /// Builder to construct notifications window.
    /// </summary>
    public class NotificationsViewBuilder : ViewBuilder
    {
        #region initialization
        private NotificationsViewBuilder(IGetSocialNativeBridge getSocialImpl) : base(getSocialImpl) {}

        /// <summary>
        /// Must not be invoked directly. Invoke <c><see cref="GetSocial.CreateNotificationsView"/></c> instead.
        /// </summary>
        public static NotificationsViewBuilder Construct(IGetSocialNativeBridge getSocialImpl)
        {
            return new NotificationsViewBuilder(getSocialImpl);
        }
        #endregion


        #region implemented/override members of ViewBuilder
        /// <summary>
        /// Sets the title of the notifications view.
        /// </summary>
        /// <returns>The <c>NotificationsViewBuilder</c> instance.</returns>
        /// <param name="title">New title.</param>
        public new NotificationsViewBuilder SetTitle(string title)
        {
            return (NotificationsViewBuilder)base.SetTitle(title);
        }

        protected override string GetViewId()
        {
            return ViewNotifications;
        }
        #endregion

    }
}


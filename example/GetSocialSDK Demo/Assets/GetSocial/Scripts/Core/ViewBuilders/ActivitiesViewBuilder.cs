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

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Builder to construct activities window.
    /// </summary>
    public sealed class ActivitiesViewBuilder : ViewBuilder
    {
        private string group;
        private string[] tags;

        #region initialization
        private ActivitiesViewBuilder(IGetSocialNativeBridge getSocialImpl) : base (getSocialImpl) {}

        /// <summary>
        /// Must not be invoked directly. Invoke <c><see cref="GetSocial.CreateActivitiesView"/></c> instead.
        /// </summary>
        public static ActivitiesViewBuilder Construct(IGetSocialNativeBridge getSocialImpl)
        {
            return new ActivitiesViewBuilder(getSocialImpl);
        }

        /// <summary>
        /// Must not be invoked directly. Invoke <c><see cref="GetSocial.CreateActivitiesView"/></c> instead.
        /// </summary>
        public static ActivitiesViewBuilder Construct(IGetSocialNativeBridge getSocialImpl, string group, params string[] tags)
        {
            Check.Argument.IsNotNull(group, "group", "Can't create activity view with null group");

            var filteredTags = Array.FindAll(tags, tag => !string.IsNullOrEmpty(tag));

            var activitiesViewBuilder = new ActivitiesViewBuilder(getSocialImpl);
            activitiesViewBuilder.group = group;
            activitiesViewBuilder.tags = filteredTags;

            return activitiesViewBuilder;
        }
        #endregion


        #region implemented/override members of ViewBuilder
        /// <summary>
        /// Sets the title of the activity.
        /// </summary>
        /// <returns>The <c>ViewBuilder</c> instance.</returns>
        /// <param name="title">New title of activity.</param>
        public new ActivitiesViewBuilder SetTitle(string title)
        {
            return (ActivitiesViewBuilder)base.SetTitle(title);
        }

        protected override JSONObject ToJson()
        {
            var json = base.ToJson();

            json.SetFieldOptional(PropertyGroup, group);

            if(tags != null && tags.Length > 0)
            {
                var jsonObjectArray = new JSONObject[tags.Length];
                for (int i = 0; i < tags.Length; i++)
                {
                    jsonObjectArray[i] = JSONObject.CreateStringObject(tags[i]);
                }

                json.SetField(PropertyTags, new JSONObject(jsonObjectArray));
            }

            return json;
        }

        protected override string GetViewId()
        {
            return ViewActivities;
        }
        #endregion
    }
}

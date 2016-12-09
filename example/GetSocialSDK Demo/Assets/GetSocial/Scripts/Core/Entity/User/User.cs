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
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.ObjectModel;
using System.Text;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Immutable class that contains information about GetSocial user
    /// </summary>
    public sealed class User
    {
        #region serialization

        private const string PropertyGuid = "guid";
        private const string PropertyDisplayName = "display_name";
        private const string PropertyAvatar = "avatar";
        private const string PropertyIdentities = "identities";

        #endregion

        private string guid = string.Empty;
        private string displayName = string.Empty;
        private string avatarUrl = string.Empty;
        private Dictionary<string, string> providerIds = new Dictionary<string, string>();

        public User(JSONObject serializedUser)
        {
            Deserialize(serializedUser);
        }

        /// <summary>
        /// Unique Identifier of the user.
        /// </summary>
        public string Guid
        {
            get { return guid; }
        }

        /// <summary>
        /// Display name of the user.
        /// </summary>
        public string DisplayName
        {
            get { return displayName; }
        }

        /// <summary>
        /// The Url of the Avatar of the user. <code>null</code> if no avatar Url is available.
        /// </summary>
        public string AvatarUrl
        {
            get { return avatarUrl; }
        }

        /// <summary>
        /// Indicates if the user has at least one identity available
        /// </summary>
        /// <value><code>true</code> if user does not have any identities; otherwise, <code>false</code>.</value>
        public bool IsAnonymous
        {
            get { return providerIds.Count == 0; }
        }

        /// <summary>
        /// Identities of the user
        /// </summary>
        public ReadOnlyCollection<string> Identities
        {
            get { return Array.AsReadOnly(providerIds.Keys.ToArray()); }
        }

        /// <summary>
        /// Determines whether the user has identity for the specified provider.
        /// </summary>
        /// <returns><code>true</code> if this instance has identity for the specified provider; otherwise, <code>false</code>.</returns>
        public bool HasIdentityForProvider(string provider)
        {
            return providerIds.ContainsKey(provider);
        }

        /// <summary>
        /// Gets the user identifier for the specified provider.
        /// </summary>
        /// <returns>The user identifier for provider, null if user doesn't have the specified provider present</returns>
        public string GetIdForProvider(string provider)
        {
            return HasIdentityForProvider(provider) ? providerIds[provider] : null;
        }

        private void Deserialize(JSONObject serializedEntityFromNativeCode)
        {
            guid = serializedEntityFromNativeCode[PropertyGuid].str;
            displayName = serializedEntityFromNativeCode[PropertyDisplayName].str;
            avatarUrl = serializedEntityFromNativeCode[PropertyAvatar].str;

            providerIds = new Dictionary<string, string>();
            JSONObject providerData = serializedEntityFromNativeCode[PropertyIdentities];

            if(providerData != null)
            {
                foreach(var providerId in providerData.keys)
                {
                    providerIds[providerId] = providerData[providerId].str;
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="GetSocialSdk.Core.User"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="GetSocialSdk.Core.User"/>.</returns>
        public override string ToString()
        {
            return string.Format("[User: Guid={0}, DisplayName={1}, AvatarUrl={2}, IsAnonymous={3}, Identities={4}]"
                , Guid, DisplayName, AvatarUrl, IsAnonymous, FormatIdentities());
        }

        private string FormatIdentities()
        {
            var builder = new StringBuilder();
            foreach(var provider in Identities)
            {
                builder.Append(string.Format("{0}({1})", provider, GetIdForProvider(provider)));
            }
            return builder.Length == 0 ? "none" : builder.ToString();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="GetSocialSdk.Core.User"/>.
        /// </summary>
        /// <returns>A short <see cref="System.String"/> that represents the current <see cref="GetSocialSdk.Core.User"/>.</returns>
        public string ToButtonString()
        {
            return string.Format("{0}\n(id: {1})", DisplayName, Guid);
        }
    }
}

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

using UnityEngine;
using System.Collections.Generic;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Describe the identity of the user. User can have multiple identities.
    /// </summary>
    public class UserIdentity
    {
        /// <summary>
        /// The provider id of Facebook.
        /// </summary>
        public const string ProviderFacebook = "facebook";

        /// <summary>
        /// The provider id Google Plus.
        /// </summary>
        public const string ProviderGooglePlus = "googleplus";

        /// <summary>
        /// The provider id Google Play Games.
        /// </summary>
        public const string ProviderGooglePlay = "googleplay";

        private const string PropertyProviderId = "provider_id";
        private const string PropertyUserId = "user_id";
        private const string PropertyToken = "token";
        private const string PropertyInfo = "info";
        private const string PropertyDisplayName = "display_name";
        private const string PropertyAvatar = "avatar";

        private readonly string providerId;
        private readonly Dictionary<string, string> loginParams;

        private UserIdentity(string providerId, Dictionary<string, string> loginParams)
        {
            this.providerId = providerId;
            this.loginParams = loginParams;
        }

        /// <summary>
        /// Creates the <see cref="UserIdentity"/> instance using an access token.
        /// </summary>
        /// <returns><see cref="UserIdentity"/> instance.</returns>
        /// <param name="providerId">Provider id.</param>
        /// <param name="accessToken">Access token.</param>
        public static UserIdentity Create(string providerId, string accessToken)
        {
            Check.Argument.IsStrNotNullOrEmpty(providerId, "providerId", "Provider must not be null or empty");
            Check.Argument.IsStrNotNullOrEmpty(accessToken, "accessToken", "Token must not be null or empty");

            var loginParams = new Dictionary<string, string>
            {
                { PropertyToken, accessToken }
            };

            return new UserIdentity(providerId, loginParams);
        }

        /// <summary>
        /// Creates the <see cref="UserIdentity"/> instance with with an userId and token.
        /// </summary>
        /// <returns><see cref="UserIdentity"/> instance.</returns>
        /// <param name="providerId">Provider id.</param>
        /// <param name="userId">User id.</param>
        /// <param name="token">
        ///     Token of the user for the specified provider. 
        ///     It's a string, provided by the developer and it will be required by the GetSocial SDK to validate any future intent to add this same identity to another user.
        /// </param>
        public static UserIdentity Create(string providerId, string userId, string token)
        {
            Check.Argument.IsStrNotNullOrEmpty(providerId, "providerId", "Provider must not be null or empty");
            Check.Argument.IsStrNotNullOrEmpty(userId, "userId", "User id must not be null or empty");
            Check.Argument.IsStrNotNullOrEmpty(token, "token", "Token must not be null or empty");

            var loginParams = new Dictionary<string, string>
            {
                { PropertyUserId, userId },
                { PropertyToken, token }
            };
            
            return new UserIdentity(providerId, loginParams);
        }

        /// <summary>
        /// Creates the <see cref="UserIdentity"/> instance using Facebook token.
        /// </summary>
        /// <returns>IdentityInfo instance for adding Facebook identity.</returns>
        /// <param name="token">Facebook access token.</param>
        public static UserIdentity CreateFacebookIdentity(string token)
        {
            return Create(ProviderFacebook, token);
        }

        internal JSONObject Serialize()
        {
            var json = new JSONObject();
            json.SetField(PropertyProviderId, providerId);
            json.SetField(PropertyInfo, new JSONObject(loginParams));
            return json;
        }
    }
}

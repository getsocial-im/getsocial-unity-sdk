using System.Collections.Generic;
using GetSocialSdk.MiniJSON;
using UnityEngine;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// This class is representation of User Auth Identity, that is used by GetSocial framework to identify user
    /// and to manage his accounts.
    /// </summary>
    public class AuthIdentity : IGetSocialBridgeObject<AuthIdentity>
    {
#pragma warning disable 414
        private readonly string _providerId;
        private readonly string _providerUserId;
        private readonly string _accessToken;
#pragma warning restore 414
        
        private AuthIdentity(string providerName, string userId, string accessToken)
        {
            _providerId = providerName;
            _providerUserId = userId;
            _accessToken = accessToken;
        }

        /// <summary>
        /// Create a Facebook identity with specified access token.
        /// </summary>
        /// <param name="accessToken">Token of Facebook user returned from FB SDK.</param>
        /// <value>The instance of AuthIdentity for Facebook user with specified access token</value>
        public static AuthIdentity CreateFacebookIdentity(string accessToken)
        {
            return CreateCustomIdentity(AuthIdentityProvider.Facebook, null, accessToken);
        }

        /// <summary>
        /// Create custom identity.
        /// </summary>
        /// <param name="providerName">Your custom provider name.</param>
        /// <param name="userId">Unique user identifier for your custom provider.</param>
        /// <param name="accessToken">Password of the user for your custom provider.
        /// It's a string, provided by the developer and it will be
        /// required by the GetSocial SDK to validate any future
        /// intent to add this same identity to another user.</param>
        /// <value>The instance of AuthIdentity for your custom provider</value>
        public static AuthIdentity CreateCustomIdentity(string providerName, string userId, string accessToken)
        {
            return new AuthIdentity(providerName, userId, accessToken);
        }

#if UNITY_ANDROID
        public AndroidJavaObject ToAJO()
        {
            var identityClass = new AndroidJavaClass("im.getsocial.sdk.usermanagement.AuthIdentity");
            return identityClass.CallStaticAJO("createCustomIdentity", _providerId, _providerUserId, _accessToken);
        }

        public AuthIdentity ParseFromAJO(AndroidJavaObject ajo)
        {
            throw new System.NotImplementedException();
        }
#elif UNITY_IOS
        public string ToJson()
        {
            var jsonDic = new Dictionary<string, object>
            {
                {"ProviderId", _providerId},
                {"ProviderUserId", _providerUserId},
                {"AccessToken", _accessToken}
            };
            return GSJson.Serialize(jsonDic);
        }

        public AuthIdentity ParseFromJson(Dictionary<string, object> json)
        {
            throw new System.NotImplementedException();
        }
        #endif
    }
}
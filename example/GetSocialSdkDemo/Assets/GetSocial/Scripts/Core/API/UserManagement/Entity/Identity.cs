using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// This class is representation of User Auth Identity, that is used by GetSocial framework to identify user
    /// and to manage his accounts.
    /// </summary>
    public class Identity 
    {
        [JsonSerializationKey("provider")]
        internal readonly string ProviderId;
        [JsonSerializationKey("userId")]
        internal readonly string ProviderUserId;
        [JsonSerializationKey("accessToken")]
        internal readonly string AccessToken;
        
        private Identity(string providerName, string userId, string accessToken)
        {
            ProviderId = providerName;
            ProviderUserId = userId;
            AccessToken = accessToken;
        }

        /// <summary>
        /// Create a Facebook identity with specified access token.
        /// </summary>
        /// <param name="accessToken">Token of Facebook user returned from FB SDK.</param>
        /// <value>The instance of AuthIdentity for Facebook user with specified access token</value>
        public static Identity Facebook(string accessToken)
        {
            return Custom(AuthIdentityProvider.Facebook, null, accessToken);
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
        public static Identity Custom(string providerName, string userId, string accessToken)
        {
            return new Identity(providerName, userId, accessToken);
        }
    }
}
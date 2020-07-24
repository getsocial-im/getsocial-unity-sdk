using System.Collections.Generic;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public class PrivateUser : User
    {
        /// <summary>
        /// Get all private properties of the user. The map is not modifiable.
        /// </summary>
        /// <value>Map of user's private properties.</value>
        [JsonSerializationKey("privateProperties")]
        public Dictionary<string, string> PrivateProperties { get; internal set; }

        /// <summary>
        /// Ban info of the user.
        /// </summary>
        /// <value>Null if user is not banned. If user is banned then contains the description of the ban.</value>
        [JsonSerializationKey("banInfo")]
        public BanInfo BanInfo { get; internal set; }

        public override string ToString()
        {
            return $"{base.ToString()}, PrivateProperties: {PrivateProperties.ToDebugString()}, BanInfo: {BanInfo}";
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace GetSocialSdk.Core
{
    public class UserReference : IGetSocialBridgeObject<UserReference>
    {
        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public string Id { get; protected set; }

        /// <summary>
        /// Gets the user display name.
        /// </summary>
        /// <value>The user display name.</value>
        public string DisplayName { get; protected set; }

        /// <summary>
        /// Gets the user avatar URL.
        /// </summary>
        /// <value>The user avatar URL.</value>
        public string AvatarUrl { get; protected set; }
#if UNITY_IOS
        public string ToJson()
        {
            throw new System.NotImplementedException();
        }

        public UserReference ParseFromJson(Dictionary<string, object> json)
        {
            Id = (string) json["Id"];
            DisplayName = (string) json["DisplayName"];
            AvatarUrl = (string) json["AvatarUrl"];
            return this;
        }
#elif UNITY_ANDROID
        public AndroidJavaObject ToAJO()
        {
            throw new System.NotImplementedException();
        }

        public UserReference ParseFromAJO(AndroidJavaObject ajo)
        {
            Id = ajo.CallStr("getId");
            DisplayName = ajo.CallStr("getDisplayName");
            AvatarUrl = ajo.CallStr("getAvatarUrl");
            return this;
        }
#endif
    }
}
using System;

#if UNITY_ANDROID
using UnityEngine;
#endif

#if UNITY_IOS
using System.Collections.Generic;
#endif

namespace GetSocialSdk.Core
{
    public class UserReference : IConvertableFromNative<UserReference>
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
        public UserReference()
        {

        }

        public override string ToString()
        {
            return string.Format("[UserReference: Id={0}, DisplayName={1}, AvatarUrl={2}]", Id, DisplayName, AvatarUrl);
        }

        internal UserReference(string id, string displayName, string avatarUrl)
        {
            Id = id;
            DisplayName = displayName;
            AvatarUrl = avatarUrl;
        }
#if UNITY_IOS
        public UserReference ParseFromJson(Dictionary<string, object> json)
        {
            if (json == null) return null;
            Id = (string) json["Id"];
            DisplayName = (string) json["DisplayName"];
            AvatarUrl = (string) json["AvatarUrl"];
            return this;
        }
#elif UNITY_ANDROID
        public UserReference ParseFromAJO(AndroidJavaObject ajo)
        {
            if (ajo == null) return null;
            Id = ajo.CallStr("getId");
            DisplayName = ajo.CallStr("getDisplayName");
            AvatarUrl = ajo.CallStr("getAvatarUrl");
            return this;
        }
#endif
    }
}
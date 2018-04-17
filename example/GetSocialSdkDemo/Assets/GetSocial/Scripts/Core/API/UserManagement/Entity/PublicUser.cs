using System;
using System.Collections.Generic;

#if UNITY_ANDROID
using UnityEngine;
#endif

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Immutable properties for a public user.
    /// </summary>
    public class PublicUser : IGetSocialBridgeObject<PublicUser>
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
        
        /// <summary>
        /// Gets the user auth identities.
        /// </summary>
        /// <value>The user auth identities.</value>
        public Dictionary<string, string> Identities { get; protected set; }

        /// <summary>
        /// Gets all public properties.
        /// Returns a copy of origin user properties.
        /// </summary>
        /// <value>User public properties</value>
        public Dictionary<string, string> AllPublicProperties
        {
            get { return new Dictionary<string, string>(_publicProperties); }
        }

#pragma warning disable 414, 649      
        private Dictionary<string, string> _publicProperties;
#pragma warning restore 414, 649

        public override string ToString()
        {
            return string.Format("[PublicUser: Id={0}, DisplayName={1}, Identities={2}, PublicProperties={3}]", Id, DisplayName, Identities.ToDebugString(), _publicProperties.ToDebugString());
        }

#if UNITY_ANDROID

        public AndroidJavaObject ToAJO()
        {
            throw new NotImplementedException();
        }

        public PublicUser ParseFromAJO(AndroidJavaObject ajo)
        {
            // NOTE: Don't forget to call Dispose() in subclasses to avoid leaks!!
            Id = ajo.CallStr("getId");
            DisplayName = ajo.CallStr("getDisplayName");
            AvatarUrl = ajo.CallStr("getAvatarUrl");
            Identities = ajo.CallAJO("getIdentities").FromJavaHashMap();
            _publicProperties = ajo.CallAJO("getAllPublicProperties").FromJavaHashMap();
            return this;
        }

#elif UNITY_IOS
        public string ToJson()
        {
            throw new NotImplementedException();
        }

        public PublicUser ParseFromJson(Dictionary<string, object> json)
        {
            Id = (string) json["Id"];
            DisplayName = (string) json["DisplayName"];
            AvatarUrl = (string) json["AvatarUrl"];

            var identitiesDictionary = json["Identities"] as Dictionary<string, object>;
            Identities = identitiesDictionary.ToStrStrDict();

            var publicProperties = json["PublicProperties"] as Dictionary<string, object>;
            _publicProperties = publicProperties.ToStrStrDict();

            return this;
        }
#endif
    }
}
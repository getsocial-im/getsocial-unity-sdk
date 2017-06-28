using System;
using System.Collections.Generic;
using UnityEngine;

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

        private Dictionary<string, string> _publicProperties;

        private Dictionary<string, string> _internalPublicProperties;

        public bool HasPublicProperty(string key)
        {
            return _publicProperties[key] != null;
        }

        public string GetPublicProperty(string key)
        {
            return _publicProperties[key];
        }

        internal bool HasInternalPublicProperty(string key)
        {
            return _internalPublicProperties[key] != null;
        }

        internal string GetInternalPublicProperty(string key)
        {
            return _internalPublicProperties[key];
        }

        public override string ToString()
        {
            return string.Format("[PublicUser: Id={0}, DisplayName={1}, Identities={2}, PublicProperties={3}]", Id, DisplayName, Identities.ToDebugString(), _publicProperties.ToDebugString());
        }

#if UNITY_ANDROID

        public UnityEngine.AndroidJavaObject ToAJO()
        {
            throw new NotImplementedException();
        }

        public PublicUser ParseFromAJO(UnityEngine.AndroidJavaObject ajo)
        {
            // NOTE: Don't forget to call Dispose() in subclasses to avoid leaks!!
            Id = ajo.CallStr("getId");
            DisplayName = ajo.CallStr("getDisplayName");
            AvatarUrl = ajo.CallStr("getAvatarUrl");
            Identities = ajo.CallAJO("getIdentities").FromJavaHashMap();
            _publicProperties = ajo.GetSafe<AndroidJavaObject>("_publicProperties").FromJavaHashMap();
            _internalPublicProperties = ajo.GetSafe<AndroidJavaObject>("_internalPublicProperties").FromJavaHashMap();
         
            return this;
        }

#elif UNITY_IOS
        public string ToJson()
        {
            throw new NotImplementedException();
        }

        public PublicUser ParseFromJson(Dictionary<string, object> jsonDic)
        {
            Id = (string) jsonDic[IdFieldName];
            DisplayName = (string) jsonDic[DisplayNameFieldName];
            AvatarUrl = (string) jsonDic[AvatarUrlFieldName];

            var identitiesDic = jsonDic[IdentitiesFieldName] as Dictionary<string, object>;
            Identities = identitiesDic.ToStrStrDict();

            var publicProperties = jsonDic["PublicProperties"] as Dictionary<string, object>;
            _publicProperties = publicProperties.ToStrStrDict();

            var intPublicProperties = jsonDic["InternalPublicProperties"] as Dictionary<string, object>;
            _internalPublicProperties = intPublicProperties.ToStrStrDict();

            return this;
        }

        static string IdFieldName
        {
            get { return ReflectionUtils.GetMemberName((PublicUser c) => c.Id); }
        }

        static string DisplayNameFieldName
        {
            get { return ReflectionUtils.GetMemberName((PublicUser c) => c.DisplayName); }
        }

        static string AvatarUrlFieldName
        {
            get { return ReflectionUtils.GetMemberName((PublicUser c) => c.AvatarUrl); }
        }

        static string IdentitiesFieldName
        {
            get { return ReflectionUtils.GetMemberName((PublicUser c) => c.Identities); }
        }
#endif
    }
}
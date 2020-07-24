using System;
using System.Collections.Generic;
using GetSocialSdk.MiniJSON;
using UnityEngine;

namespace GetSocialSdk.Core
{
    public sealed class UserUpdate 
    {
        private const string RemoveValue = "";

        [JsonSerializationKey("displayName")]
        public string DisplayName;
        [JsonSerializationKey("avatarUrl")]
        public string AvatarUrl;
        public Texture2D Avatar {
            get 
            {
                return _avatar64.FromBase64();
            }
            set
            {
                _avatar64 = value.TextureToBase64();
            }
        }
        [JsonSerializationKey("avatar")]
        internal string _avatar64;

        [JsonSerializationKey("publicProperties")]
        internal readonly Dictionary<string, string> _publicProperties = new Dictionary<string, string>();
        [JsonSerializationKey("privateProperties")]
        internal readonly Dictionary<string, string> _privateProperties = new Dictionary<string, string>();

        public UserUpdate AddPublicProperty(string key, string value)
        {
            _publicProperties[key] = value;
            return this;
        }
        public UserUpdate AddPrivateProperty(string key, string value)
        {
            _privateProperties[key] = value;
            return this;
        }

        public UserUpdate AddPublicProperties(Dictionary<string, string> properties)
        {
            _publicProperties.AddAll(properties);
            return this;
        }
        public UserUpdate AddPrivateProperties(Dictionary<string, string> properties)
        {
            _privateProperties.AddAll(properties);
            return this;
        }

        public UserUpdate RemovePublicProperty(string key)
        {
            _publicProperties[key] = RemoveValue;
            return this;
        }
        public UserUpdate RemovePrivateProperty(string key)
        {
            _privateProperties[key] = RemoveValue;
            return this;
        }
    }
}
using System;
using System.Collections.Generic;
using GetSocialSdk.MiniJSON;
using UnityEngine;

namespace GetSocialSdk.Core
{
    public sealed class UserUpdate : IConvertableToNative
    {
        private const string RemoveValue = "";

        private string _displayName;
        private string _avatarUrl;
        private Texture2D _avatar;

        private readonly Dictionary<string, string> _publicProperties = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _privateProperties = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _publicInternalProperties = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _privateInternalProperties = new Dictionary<string, string>();

        public static Builder CreateBuilder()
        {
            return new Builder();
        }

        /// <summary>
        /// Convenience builder for <see cref="UserUpdate"/>.
        /// </summary>
        public class Builder
        {
            private readonly UserUpdate _userUpdate;

            internal Builder()
            {
                _userUpdate = new UserUpdate();
            }

            /// <summary>
            /// Set user's display name.
            /// </summary>
            /// <param name="displayName">New display name</param>
            /// <returns>Instance of <see cref="Builder"/> to chain method calls.</returns>
            public Builder UpdateDisplayName(string displayName)
            {
                _userUpdate._displayName = displayName;
                return this;
            }

            ///<summary>
            /// Set the avatar URL. Can not be used with <see cref="UpdateAvatar"/>.
            /// </summary>
            /// <param name="avatarUrl">New avatar URL.</param>
            /// <returns>Instance of <see cref="Builder"/> to chain method calls.</returns> 
            public Builder UpdateAvatarUrl(string avatarUrl)
            {
                _userUpdate._avatarUrl = avatarUrl;
                _userUpdate._avatar = null;
                return this;
            }

            
            /// <summary>
            /// Set the new avatar. Can not be used with <see cref="UpdateAvatarUrl"/>
            /// </summary>
            /// <param name="avatar">New avatar.</param>
            /// <returns>Instance of <see cref="Builder"/> to chain method calls.</returns>
            public Builder UpdateAvatar(Texture2D avatar)
            {
                _userUpdate._avatar = avatar;
                _userUpdate._avatarUrl = null;
                return this;
            }

            public Builder SetPublicProperty(string key, string value)
            {
                _userUpdate._publicProperties[key] = value;
                return this;
            }

            public Builder RemovePublicProperty(string key)
            {
                _userUpdate._publicProperties[key] = RemoveValue;
                return this;
            }

            public Builder SetPrivateProperty(string key, string value)
            {
                _userUpdate._privateProperties[key] = value;
                return this;
            }

            public Builder RemovePrivateProperty(string key)
            {
                _userUpdate._privateProperties[key] = RemoveValue;
                return this;
            }

            
            //todo Make use of internal properties in future. As of 10.09.2018 those are not used both in iOS and Android.
            Builder SetInternalPublicProperty(string key, string value)
            {
                _userUpdate._publicInternalProperties[key] = value;
                return this;
            }

            Builder RemoveInternalPublicProperty(string key)
            {
                _userUpdate._publicInternalProperties[key] = RemoveValue;
                return this;
            }

            Builder SetInternalPrivateProperty(string key, string value)
            {
                _userUpdate._privateInternalProperties[key] = value;
                return this;
            }

            Builder RemoveInternalPrivateProperty(string key)
            {
                _userUpdate._privateInternalProperties[key] = RemoveValue;
                return this;
            }

            /// <summary>
            /// Creates a new UserUpdate instance.
            /// </summary>
            /// <returns>New UserUpdate instance</returns>
            public UserUpdate Build()
            {
                var userUpdate = new UserUpdate
                {
                    _displayName = _userUpdate._displayName,
                    _avatarUrl = _userUpdate._avatarUrl,
                    _avatar = _userUpdate._avatar
                };
                
                userUpdate._publicProperties.AddAll(_userUpdate._publicProperties);
                userUpdate._privateProperties.AddAll(_userUpdate._privateProperties);
                userUpdate._publicInternalProperties.AddAll(_userUpdate._publicInternalProperties);
                userUpdate._privateInternalProperties.AddAll(_userUpdate._privateInternalProperties);

                return userUpdate;
            }
        }

#if UNITY_ANDROID
        public AndroidJavaObject ToAjo()
        {
            var updateUserBuilder = new AndroidJavaObject("im.getsocial.sdk.usermanagement.UserUpdate$Builder");
            updateUserBuilder.CallAJO("updateDisplayName", _displayName);
            updateUserBuilder.CallAJO("updateAvatar", _avatar);
            updateUserBuilder.CallAJO("updateAvatarUrl", _avatarUrl);
            foreach (var publicProperty in _publicProperties)
            {
                updateUserBuilder.CallAJO("setPublicProperty", publicProperty.Key, publicProperty.Value);
            }

            foreach (var privateProperty in _privateProperties)
            {
                updateUserBuilder.CallAJO("setPrivateProperty", privateProperty.Key, privateProperty.Value);
            }

            foreach (var publicInternalProperty in _publicInternalProperties)
            {
                updateUserBuilder.CallAJO("setInternalPublicProperty", publicInternalProperty.Key,
                    publicInternalProperty.Value);
            }

            foreach (var privateInternalProperty in _privateInternalProperties)
            {
                updateUserBuilder.CallAJO("setInternalPrivateProperty", privateInternalProperty.Key,
                    privateInternalProperty.Value);
            }

            return updateUserBuilder.CallAJO("build");
        }

#elif UNITY_IOS
        public string ToJson()

        {
            var jsonDic = new Dictionary<string, object>
            {
                {"DisplayName", _displayName},
                {"Avatar", _avatar},
                {"AvatarUrl", _avatarUrl},
                {"PublicProperties", _publicProperties},
                {"PrivateProperties", _privateProperties},
                {"PublicInternalProperties", _publicInternalProperties},
                {"PrivateInternalProperties", _privateInternalProperties},
            };
            return GSJson.Serialize(jsonDic);
        }
#endif
    }
}
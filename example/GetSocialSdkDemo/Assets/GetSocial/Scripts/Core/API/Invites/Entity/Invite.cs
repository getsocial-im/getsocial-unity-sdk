using System;
using System.Collections.Generic;
using GetSocialSdk.MiniJSON;
using UnityEngine;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Invite content being sent along with smart invite.
    /// </summary>
    public sealed class Invite
    {
        [JsonSerializationKey("subject")]
        public string Subject { get; private set; }

        [JsonSerializationKey("text")]
        public string Text { get; private set; }

        [JsonSerializationKey("userName")]
        public string UserName { get; private set; }

        [JsonSerializationKey("imageUrl")]
        public string ImageUrl { get; private set; }
#pragma warning disable 0649
        [JsonSerializationKey("image")]
        internal string _image64;
#pragma warning restore 0649
        public Texture2D Image 
        {
            get 
            {
                return _image64.FromBase64();
            }
        }

        [JsonSerializationKey("gifUrl")]
        public string GifUrl { get; private set; }

        [JsonSerializationKey("videoUrl")]
        public string VideoUrl { get; private set; }

        [JsonSerializationKey("referralUrl")]
        public string ReferralUrl { get; private set; }

        [JsonSerializationKey("linkParams")]
        public Dictionary<string, string> LinkParams { get; private set; }

    }
}
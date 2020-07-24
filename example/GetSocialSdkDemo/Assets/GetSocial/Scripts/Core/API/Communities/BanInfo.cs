using System;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// BanInfo entity.
    /// </summary>
    public sealed class BanInfo
    {

        /// <summary>
        /// Ban expiration.
        /// </summary>
        [JsonSerializationKey("expiration")]
        public long Expiration { get; internal set; }

        /// <summary>
        /// Ban reason.
        /// </summary>
        [JsonSerializationKey("reason")]
        public string Reason { get; internal set; }

        public BanInfo()
        {

        }
        
        
        public override string ToString()
        {
            return $"Expiration: {Expiration}, Reason: {Reason}";
        }
        
    }
}
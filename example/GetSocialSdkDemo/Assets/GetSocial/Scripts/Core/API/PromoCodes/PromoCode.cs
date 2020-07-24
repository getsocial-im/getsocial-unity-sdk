using System;
using System.Collections.Generic;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core {
    public sealed class PromoCode {

        /// <summary>
        /// Promo code.
        /// </summary>
        [JsonSerializationKey("code")]
        public string Code { get; internal set; }

        /// <summary>
        /// Attached custom data.
        /// </summary>
        [JsonSerializationKey("data")]
        public Dictionary<string, string> Properties { get; internal set; }

        /// <summary>
        /// Maximum number of claims. If `0` then no limits.
        /// </summary>
        [JsonSerializationKey("maxClaimCount")]
        public uint MaxClaimCount { get; internal set; }

        /// <summary>
        /// Date when Promo Code becomes active.
        /// </summary>
        [JsonSerializationKey("startDate")]
        public long StartDate { get; internal set; }

        /// <summary>
        /// Date when Promo Code is not active anymore. Null if Promo Code is available forever until manually disabled on the Dashboard.
        /// </summary>
        [JsonSerializationKey("endDate")]
        public long EndDate { get; internal set; }

        /// <summary>
        /// Creator basic info.
        /// </summary>
        [JsonSerializationKey("creator")]
        public User Creator { get; internal set; }
        /// <summary>
        /// Current number of claims.
        /// </summary>
        
        [JsonSerializationKey("claimCount")]
        public uint ClaimCount { get; internal set; }

        /// <summary>
        /// Is enabled on the Dashboard.
        /// </summary>
        [JsonSerializationKey("isEnabled")]
        public bool IsEnabled { get; internal set; }

        /// <summary>
        /// Is claimable. True only if Promo Code is enabled and active in the current time.
        /// </summary>
        [JsonSerializationKey("isClaimable")]
        public bool IsClaimable { get; internal set; }

        public override string ToString()
        {
            return $"Code: {Code}, Data: {Properties.ToDebugString()}, Creator: {Creator}, MaxClaims: {MaxClaimCount}, Claims: {ClaimCount}, StartDate: {StartDate}, EndDate: {EndDate}, Enabled: {IsEnabled}, Claimable: {IsClaimable}";
        }
        
    }
}
using System.Collections.Generic;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Referral data of the smart invite.
    /// </summary>
    public sealed class ReferralData
    {
        /// <summary>
        /// The unique Smart Invite link token. There is unique association between
        /// token and attached referral data.
        /// </summary>
        [JsonSerializationKey("token")]
        public string Token { get; internal set; }

        /// <summary>
        /// The referrer user identifier.
        /// </summary>
        [JsonSerializationKey("referrerUserId")]
        public string ReferrerUserId { get; internal set; }

        /// <summary>
        /// The id of the channel where Smart Link was shared.
        /// </summary>
        [JsonSerializationKey("referrerChannelId")]
        public string ReferrerChannelId { get; internal set; }

        /// <summary>
        /// Returns true if the app is installed for the first time on this device. False otherwise.
        /// </summary>
        [JsonSerializationKey("isFirstMatch")]
        public bool IsFirstMatch { get; internal set; }

        /// <summary>
        ///  If GetSocial is able to retrieve extra meta information (e.g. from Google Play, Facebook or depplink) we provide 100% guarantee
        ///  that received data corresponds to the user. In other cases we use fingerprinting to find a best match.
        /// </summary>
        /// <value>true if GetSocial can give 100% guarantee that received referral data corresponds to the user, false in case of the best match.</value>
        [JsonSerializationKey("isGuaranteedMatch")]
        public bool IsGuaranteedMatch { get; internal set; }

        /// <summary>
        /// Returns true if the app is reinstalled on this device. False otherwise.
        /// </summary>
        [JsonSerializationKey("isReinstall")]
        public bool IsReinstall { get; internal set; }

        /// <summary>
        /// Returns true if the app is opened for this link the first time on this device. False otherwise.
        /// </summary>
        [JsonSerializationKey("isFirstMatchLink")]
        public bool IsFirstMatchLink { get; internal set; }

        /// <summary>
        /// Gets the overriden referral link parameters assigned to the Smart Link.
        /// </summary>
        /// <value>The custom referral link parameters.</value>
        [JsonSerializationKey("linkParams")]
        public Dictionary<string, string> LinkParams { get ; internal set; }

        /// <summary>
        /// Gets the original referral link parameters. Overrides from the Smart Link are ignored.
        /// </summary>
        /// <value>The original referral link parameters.</value>
        [JsonSerializationKey("originalLinkParams")]
        public Dictionary<string, string> OriginalLinkParams { get; internal set; }
        
    }
}
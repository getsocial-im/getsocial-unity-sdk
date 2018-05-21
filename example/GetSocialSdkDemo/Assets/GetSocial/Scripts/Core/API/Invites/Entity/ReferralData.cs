using System;

#if UNITY_ANDROID
using UnityEngine;
#endif

#if UNITY_IOS
using System.Collections.Generic;
#endif

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Referral data of the smart invite.
    /// </summary>
    public sealed class ReferralData : IConvertableFromNative<ReferralData>
    {
        /// <summary>
        /// The unique Smart Invite link token. There is unique association between
        /// token and attached referral data.
        /// </summary>
        public string Token { get; private set; }

        /// <summary>
        /// The referrer user identifier.
        /// </summary>
        public string ReferrerUserId { get; private set; }

        /// <summary>
        /// The id of the channel where Smart Link was shared.
        /// </summary>
        public string ReferrerChannelId { get; private set; }

        /// <summary>
        /// Returns true if the app is installed for the first time on this device. False otherwise.
        /// </summary>
        public bool IsFirstMatch { get; private set; }
        
        /// <summary>
        ///  If GetSocial is able to retrieve extra meta information (e.g. from Google Play, Facebook or depplink) we provide 100% guarantee
        ///  that received data corresponds to the user. In other cases we use fingerprinting to find a best match.
        /// </summary>
        /// <value>true if GetSocial can give 100% guarantee that received referral data corresponds to the user, false in case of the best match.</value>
        public bool IsGuaranteedMatch { get; private set; }

        /// <summary>
        /// Returns true if the app is reinstalled on this device. False otherwise.
        /// </summary>
        public bool IsReinstall { get; private set; }

        /// <summary>
        /// Returns true if the app is opened for this link the first time on this device. False otherwise.
        /// </summary>
        public bool IsFirstMatchLink { get; private set; }
        
        /// <summary>
        /// Gets the custom referral data with the parameter overrides from the Smart Link.
        /// </summary>
        /// <value>The custom referral data.</value>
        /// Deprecated, use <see cref="LinkParams"/> instead.
        [Obsolete("Deprecated, use LinkParams instead.")]
#pragma warning disable 0618
        public CustomReferralData CustomReferralData { get; private set; }
#pragma warning restore 0618

        /// <summary>
        /// Gets the overriden link parameters assigned to the Smart Link.
        /// </summary>
        /// <value>The custom link parameters.</value>
        public LinkParams LinkParams { get ; private set; }

        /// <summary>
        /// Gets the original custom referral data. Overrides from the Smart Link are ignored.
        /// </summary>
        /// <value>The original custom referral data.</value>
        /// Deprecated, use <see cref="OriginalLinkParams"/> instead.
        [Obsolete("Deprecated, use OriginalLinkParams instead.")]
#pragma warning disable 0618
        public CustomReferralData OriginalCustomReferralData { get; private set; }
#pragma warning restore 0618

        /// <summary>
        /// Gets the original link parameters. Overrides from the Smart Link are ignored.
        /// </summary>
        /// <value>The original link parameters.</value>
        public LinkParams OriginalLinkParams { get; private set; }

        public override string ToString()
        {
            return string.Format("[ReferralData: Token: {0}, ReferrerUserId={1}, ReferrerChannelId={2}, IsFirstMatch={3}, IsGuaranteedMatch={4}, LinkParams={5}, " +
                                 ", OriginalLinkParams={6}]",
                Token, ReferrerUserId, ReferrerChannelId, IsFirstMatch, IsGuaranteedMatch, LinkParams.ToDebugString(), OriginalLinkParams.ToDebugString());
        }

#if UNITY_ANDROID
        public ReferralData ParseFromAJO(AndroidJavaObject ajo)
        {
            if (ajo.IsJavaNull())
            {
                return null;
            }

            using (ajo)
            {
                Token = ajo.CallStr("getToken");
                ReferrerUserId = ajo.CallStr("getReferrerUserId");
                ReferrerChannelId = ajo.CallStr("getReferrerChannelId");
                IsFirstMatch = ajo.CallBool("isFirstMatch");
                IsGuaranteedMatch = ajo.CallBool("isGuaranteedMatch");
                IsReinstall = ajo.CallBool("isReinstall");
                IsFirstMatchLink = ajo.CallBool("isFirstMatchLink");
                var linkParamsDict = ajo.CallAJO("getLinkParams").FromJavaHashMap();
                LinkParams = new LinkParams(linkParamsDict);
                var originalLinkParamsDict = ajo.CallAJO("getOriginalLinkParams").FromJavaHashMap();
                OriginalLinkParams = new LinkParams(originalLinkParamsDict);
#pragma warning disable 0618
                CustomReferralData = new CustomReferralData(LinkParams);
                OriginalCustomReferralData = new CustomReferralData(OriginalLinkParams);
#pragma warning restore 0618
            }
            return this;
        }
#elif UNITY_IOS

        public ReferralData ParseFromJson(Dictionary<string, object> json)
        {
            Token = json["Token"] as string;
            ReferrerUserId = json["ReferrerUserId"] as string;
            ReferrerChannelId = json["ReferrerChannelId"] as string;
            IsFirstMatch = (bool) json["IsFirstMatch"];
            IsGuaranteedMatch = (bool) json["IsGuaranteedMatch"];
            IsReinstall = (bool) json["IsReinstall"];
            IsFirstMatchLink = (bool) json["IsFirstMatchLink"];
            LinkParams = new LinkParams(json["LinkParams"] as Dictionary<string, object>);
            OriginalLinkParams =
                new LinkParams(json["OriginalLinkParams"] as Dictionary<string, object>);
#pragma warning disable 0618
            CustomReferralData = new CustomReferralData(LinkParams);
            OriginalCustomReferralData = new CustomReferralData(OriginalLinkParams);
#pragma warning restore 0618
            return this;
        }
#endif
    }
}
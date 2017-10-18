using System;
using System.Collections.Generic;
using UnityEngine;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Referral data of the smart invite.
    /// </summary>
    public sealed class ReferralData : IGetSocialBridgeObject<ReferralData>
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
        /// Returns true if the referral data is retrieved on this device for a first time. False otherwise.
        /// </summary>
        public bool IsFirstMatch { get; private set; }
        
        /// <summary>
        ///  If GetSocial is able to retrieve extra meta information (e.g. from Google Play, Facebook or depplink) we provide 100% guarantee
        ///  that received data corresponds to the user. In other cases we use fingerprinting to find a best match.
        /// </summary>
        /// <value>true if GetSocial can give 100% guarantee that received referral data corresponds to the user, false in case of the best match.</value>
        public bool IsGuaranteedMatch { get; private set; }

        /// <summary>
        /// Gets the custom referral data with the parameter overrides from the Smart Link.
        /// </summary>
        /// <value>The custom referral data.</value>
        public CustomReferralData CustomReferralData { get; private set; }

        /// <summary>
        /// Gets the original custom referral data. Overrides from the Smart Link are ignored.
        /// </summary>
        /// <value>The original custom referral data.</value>
        public CustomReferralData OriginalCustomReferralData { get; private set; }

        public override string ToString()
        {
            return string.Format("[ReferralData: Token: {0}, ReferrerUserId={1}, ReferrerChannelId={2}, IsFirstMatch={3}, IsGuaranteedMatch={4}, CustomReferralData={5}, " +
                                 ", OriginalCustomReferralData={6}]",
                Token, ReferrerUserId, ReferrerChannelId, IsFirstMatch, IsGuaranteedMatch, CustomReferralData.ToDebugString(), OriginalCustomReferralData.ToDebugString());
        }

#if UNITY_ANDROID
        public UnityEngine.AndroidJavaObject ToAJO()
        {
            throw new System.NotImplementedException("Referral Data is never passed to Android, only received");
        }

        public ReferralData ParseFromAJO(UnityEngine.AndroidJavaObject ajo)
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
                var customReferralDataDict = ajo.CallAJO("getCustomReferralData").FromJavaHashMap();
                CustomReferralData = new CustomReferralData(customReferralDataDict);
                var originalCustomReferralDataDict = ajo.CallAJO("getOriginalCustomReferralData").FromJavaHashMap();
                OriginalCustomReferralData = new CustomReferralData(originalCustomReferralDataDict);

            }
            return this;
        }
#elif UNITY_IOS
        public string ToJson()
        {
            throw new System.NotImplementedException("Referral Data is never passed to iOS, only received");
        }

        public ReferralData ParseFromJson(Dictionary<string, object> json)
        {
            Token = json[TokenFieldName] as string;
            ReferrerUserId = json[ReferrerUserIdFieldName] as string;
            ReferrerChannelId = json[ReferrerChannelIdFieldName] as string;
            IsFirstMatch = (bool) json[IsFirstMatchFieldName];
            IsGuaranteedMatch = (bool) json[IsGuaranteedMatchName];
            CustomReferralData = new CustomReferralData(json[CustomReferralDataFieldName] as Dictionary<string, object>);
            OriginalCustomReferralData =
                new CustomReferralData(json[OriginalCustomReferralDataFieldName] as Dictionary<string, object>);
            return this;
        }

        static string TokenFieldName
        {
            get { return ReflectionUtils.GetMemberName((ReferralData c) => c.Token); }
        }

        static string ReferrerUserIdFieldName
        {
            get { return ReflectionUtils.GetMemberName((ReferralData c) => c.ReferrerUserId); }
        }

        static string ReferrerChannelIdFieldName
        {
            get { return ReflectionUtils.GetMemberName((ReferralData c) => c.ReferrerChannelId); }
        }

        static string IsFirstMatchFieldName
        {
            get { return ReflectionUtils.GetMemberName((ReferralData c) => c.IsFirstMatch); }
        }
        
        static string IsGuaranteedMatchName
        {
            get { return ReflectionUtils.GetMemberName((ReferralData c) => c.IsGuaranteedMatch); }
        }

        static string CustomReferralDataFieldName
        {
            get { return ReflectionUtils.GetMemberName((ReferralData c) => c.CustomReferralData); }
        }

        static string OriginalCustomReferralDataFieldName
        {
            get { return ReflectionUtils.GetMemberName((ReferralData c) => c.OriginalCustomReferralData); }
        }

#endif
    }
}
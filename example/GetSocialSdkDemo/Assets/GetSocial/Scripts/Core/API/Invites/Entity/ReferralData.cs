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
        /// <value>The referral user identifier.</value>
        public string Token { get; private set; }

        /// <summary>
        /// The referrer user identifier.
        /// </summary>
        /// <value>The referral user identifier.</value>
        public string ReferrerUserId { get; private set; }

        /// <summary>
        /// The id of the channel that was used for the invite.
        /// </summary>
        /// <value>The referral user identifier.</value>
        public string ReferrerChannelId { get; private set; }

        /// <summary>
        /// Returns true if the referral data is retrieved on this device for a first time. False otherwise.
        /// </summary>
        /// <value>The referral user identifier.</value>
        public bool IsFirstMatch { get; private set; }

        /// <summary>
        /// Gets the custom referral data.
        /// </summary>
        /// <value>The custom referral data.</value>
        public CustomReferralData CustomReferralData { get; private set; }

        public override string ToString()
        {
            return string.Format("[ReferralData: Token: {0}, ReferrerUserId={1}, ReferrerChannelId={2}, IsFirstMatch={3}, CustomReferralData={4}]",
                Token, ReferrerUserId, ReferrerChannelId, IsFirstMatch, CustomReferralData.ToDebugString());
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
                var customReferralDataDict = ajo.CallAJO("getCustomReferralData").FromJavaHashMap();
                CustomReferralData = new CustomReferralData(customReferralDataDict);
            }
            return this;
        }
#elif UNITY_IOS
        public string ToJson()
        {
            throw new System.NotImplementedException("Referral Data is never passed to iOS, only received");
        }

        public ReferralData ParseFromJson(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            var dic = json.ToDict();
            Token = dic[TokenFieldName] as string;
            ReferrerUserId = dic[ReferrerUserIdFieldName] as string;
            ReferrerChannelId = dic[ReferrerChannelIdFieldName] as string;
            IsFirstMatch = (bool) dic[IsFirstMatchFieldName];
            CustomReferralData = new CustomReferralData(dic[CustomReferralDataFieldName] as Dictionary<string, object>);
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

        static string CustomReferralDataFieldName
        {
            get { return ReflectionUtils.GetMemberName((ReferralData c) => c.CustomReferralData); }
        }

#endif
    }
}
#if UNITY_EDITOR

using System.Collections.Generic;

namespace GetSocialSdk.Core
{
    public class UnityEditorHelperFunctions
    {
        public static void TriggerOnReferralDataReceivedListener(string Token, string ReferrerUserId, string ReferrerChannelId, bool IsFirstMatch,
                bool IsGuaranteedMatch, bool IsReinstall, bool IsFirstMatchLink, Dictionary<string, string> LinkParams, Dictionary<string, string> OriginalLinkParams)
        {
            var referralData = new ReferralData();
            referralData.ReferrerUserId = Token;
            referralData.ReferrerUserId = ReferrerUserId;
            referralData.ReferrerChannelId= ReferrerChannelId;
            referralData.IsFirstMatch = IsFirstMatch;
            referralData.IsGuaranteedMatch = IsGuaranteedMatch;
            referralData.IsReinstall = IsReinstall;
            referralData.IsFirstMatchLink = IsFirstMatchLink;
            referralData.LinkParams = LinkParams;
            referralData.OriginalLinkParams = OriginalLinkParams;
            GetSocialFactory.Bridge.TriggerOnReferralDataReceivedListener(referralData);
        }
    }
}

#endif


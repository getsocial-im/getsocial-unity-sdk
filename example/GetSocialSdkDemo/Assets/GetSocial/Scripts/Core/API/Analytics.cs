using System.Collections.Generic;

namespace GetSocialSdk.Core
{
    public static class Analytics
    {
        public static bool TrackPurchase(PurchaseData purchaseData)
        {
            return GetSocialFactory.Bridge.TrackPurchase(purchaseData);
        }

        public static bool TrackCustomEvent(string eventName, Dictionary<string, string> eventData)
        {
            return GetSocialFactory.Bridge.TrackCustomEvent(eventName, eventData);
        }

    }
}
#if UNITY_ANDROID
using UnityEngine;
#endif

#if UNITY_IOS
using System.Collections.Generic;
#endif

namespace GetSocialSdk.Core
{
    public sealed class NotificationsSummary : IConvertableFromNative<NotificationsSummary>
    {
        public int SuccessfullySentCount { get; private set; }

#if UNITY_ANDROID
        public NotificationsSummary ParseFromAJO(AndroidJavaObject ajo)
        {
            SuccessfullySentCount = ajo.CallInt("getSuccessfullySentCount");
            return this;
        }
#elif UNITY_IOS
        public NotificationsSummary ParseFromJson(Dictionary<string, object> json)
        {
            SuccessfullySentCount = (int) (long) json["SuccessfullySentCount"];
            return this;
        }
#endif
    }
}
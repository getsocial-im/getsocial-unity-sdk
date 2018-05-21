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
    /// User that is received in the case of conflict when adding auth identity.
    /// </summary>
    public class ConflictUser : PublicUser, IConvertableFromNative<ConflictUser>
    {
#if UNITY_ANDROID
        public new ConflictUser ParseFromAJO(AndroidJavaObject ajo)
        {
            using (ajo)
            {
                base.ParseFromAJO(ajo);
            }
            return this;
        }
#elif UNITY_IOS

        public new ConflictUser ParseFromJson(Dictionary<string, object> json)
        {
            base.ParseFromJson(json);
            return this;
        }
#endif
    }
}
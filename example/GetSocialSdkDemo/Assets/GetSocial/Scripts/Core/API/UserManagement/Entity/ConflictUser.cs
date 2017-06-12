using System.Collections.Generic;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// User that is received in the case of conflict when adding auth identity.
    /// </summary>
    public class ConflictUser : PublicUser, IGetSocialBridgeObject<ConflictUser>
    {
#if UNITY_ANDROID
        public new UnityEngine.AndroidJavaObject ToAJO()
        {
            throw new System.NotImplementedException("ConflictUser is never passed to Android");
        }

        public new ConflictUser ParseFromAJO(UnityEngine.AndroidJavaObject ajo)
        {
            using (ajo)
            {
                base.ParseFromAJO(ajo);
            }
            return this;
        }
#elif UNITY_IOS
        public new string ToJson()
        {
            throw new System.NotImplementedException("ConflictUser is never passed to iOS");
        }

        public new ConflictUser ParseFromJson(Dictionary<string, object> json)
        {
            base.ParseFromJson(json);
            return this;
        }
#endif
    }
}
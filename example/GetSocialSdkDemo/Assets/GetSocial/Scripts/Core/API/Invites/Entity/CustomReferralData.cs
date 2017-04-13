using System.Collections.Generic;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Custom referral data attached to the invite.
    /// </summary>
    public sealed class CustomReferralData : Dictionary<string, string>, IGetSocialBridgeObject<CustomReferralData>
    {
        public CustomReferralData()
        {
        }

        public CustomReferralData(Dictionary<string, string> data) : base(data)
        {
        }

        public CustomReferralData(IDictionary<string, object> data)
        {
            foreach (var kv in data)
            {
                this[kv.Key] = kv.Value as string;
            }
        }

        public override string ToString()
        {
            return string.Format("[CustomReferralData: {0}]", this.ToDebugString());
        }

#if UNITY_ANDROID
        public UnityEngine.AndroidJavaObject ToAJO()
        {
            return new UnityEngine.AndroidJavaObject("im.getsocial.sdk.invites.CustomReferralData", this.ToJavaHashMap());
        }

        public CustomReferralData ParseFromAJO(UnityEngine.AndroidJavaObject ajo)
        {
            Clear();
            var dict = ajo.FromJavaHashMap();
            foreach (var keyValuePair in dict)
            {
                this[keyValuePair.Key] = keyValuePair.Value;
            }
            return this;
        }
#elif UNITY_IOS
        public string ToJson()
        {
            return GSJson.Serialize(this);
        }

        public CustomReferralData ParseFromJson(string json)
        {
            Clear();

            var dic = json.ToDict();
            foreach (var kv in dic)
            {
                this[kv.Key] = kv.Value as string;
            }

            return this;
        }
#endif
    }
}
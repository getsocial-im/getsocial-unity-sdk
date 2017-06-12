using System.Collections.Generic;

namespace GetSocialSdk.Core
{
    public interface IGetSocialBridgeObject<out T>
    {
#if UNITY_ANDROID
        UnityEngine.AndroidJavaObject ToAJO();

        T ParseFromAJO(UnityEngine.AndroidJavaObject ajo);
#elif UNITY_IOS
        string ToJson();

        T ParseFromJson(Dictionary<string, object> json);
#endif
    }
}
#if UNITY_ANDROID
using UnityEngine;
using GetSocialSdk.MiniJSON;
namespace GetSocialSdk.Core
{
    public static class AndroidAJOConverter
    {
        private const string GetSocialJsonClassSignature = "im.getsocial.sdk.json.GetSocialJson";

        public static AndroidJavaObject Convert(object from, string toClass) 
        {
            return new AndroidJavaClass(GetSocialJsonClassSignature).CallStaticAJO("parse", GSJson.Serialize(from), new AndroidJavaClass(toClass));
        }

        public static T Convert<T>(AndroidJavaObject from)
        {
            var str = new AndroidJavaClass(GetSocialJsonClassSignature).CallStatic<string>("toJson", from);
            return GetSocialJsonBridge.FromJson<T>(str);
        }
    }
}
#endif
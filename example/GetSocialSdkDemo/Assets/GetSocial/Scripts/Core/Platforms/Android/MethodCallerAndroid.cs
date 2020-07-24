#if UNITY_ANDROID
using System;
using UnityEngine;

namespace GetSocialSdk.Core
{
    public class MethodCallerAndroid: IMethodCaller
    {
        private const string GetSocialJsonClassSignature = "im.getsocial.sdk.json.GetSocialJson";

        private readonly AndroidJavaClass _json;

        public MethodCallerAndroid()
        {
            _json = new AndroidJavaClass(GetSocialJsonClassSignature);
        }
        
        public string Call(string method, string body)
        {
            return _json.CallStatic<string>("callSync", method, body);
        }
        
        public void CallAsync(string method, string body, Action<string> success, Action<string> failure)
        {
            _json.CallStatic("callAsync", method, body, new StringCallbackAndroid(success), new StringCallbackAndroid(failure));
        }

        public string RegisterListener(string method, Action<string> listener)
        {
            return _json.CallStatic<string>("registerListener", method, new StringCallbackAndroid(listener));
        }
    }
}
#endif
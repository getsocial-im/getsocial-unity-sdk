#if UNITY_IOS
using System;
using System.Runtime.InteropServices;

namespace GetSocialSdk.Core
{
    public class MethodCalleriOS : IMethodCaller
    {
        public string Call(string method, string body)
        {
            return _gs_callSync(method, body);
        }

        public void CallAsync(string method, string body, Action<string> success, Action<string> failure)
        {
            _gs_callAsync(method, body,
                Callbacks.StringCallback, success.GetPointer(),
                Callbacks.StringCallback, failure.GetPointer());
        }

        public string RegisterListener(string method, Action<string> listener)
        {
            return _gs_addListener(method, Callbacks.StringCallback, listener.GetPointer());
        }
        #region external_init

        [DllImport("__Internal")]
        static extern string _gs_callSync(string method, string content);

        [DllImport("__Internal")]
        static extern void _gs_callAsync(string method, string content, StringCallbackDelegate successCallback, IntPtr successPointer,
            StringCallbackDelegate failureCallback, IntPtr failurePointer);

        [DllImport("__Internal")]
        static extern string _gs_addListener(string method, StringCallbackDelegate successCallback, IntPtr successPointer);



        #endregion

    }
}

#endif

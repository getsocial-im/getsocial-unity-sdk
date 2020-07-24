#if UNITY_ANDROID
using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace GetSocialSdk.Core
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    internal class StringCallbackAndroid : JavaInterfaceProxy
    {
        private readonly Action<string> _onSuccess;

        public StringCallbackAndroid(Action<string> onSuccess)
            : base("im.getsocial.sdk.Callback")
        {
            _onSuccess = onSuccess;
        }

        void onSuccess(string value)
        {
            HandleValue(value, _onSuccess);
        }

        // this is needed to handle null, it won't be called with any other value
        void onSuccess(AndroidJavaObject value)
        {
            HandleValue(value.IsJavaNull() ? null : value.ToString(), _onSuccess);
        }
    }
}
#endif

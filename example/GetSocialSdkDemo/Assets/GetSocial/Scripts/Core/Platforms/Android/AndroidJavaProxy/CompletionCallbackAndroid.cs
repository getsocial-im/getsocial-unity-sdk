#if UNITY_ANDROID
using System;
using System.Diagnostics.CodeAnalysis;

namespace GetSocialSdk.Core
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    internal class CompletionCallbackAndroid : JavaInterfaceProxy
    {
        private readonly Action _onSuccess;

        public CompletionCallbackAndroid(Action onSuccess)
            : base("im.getsocial.sdk.CompletionCallback")
        {
            _onSuccess = onSuccess;
        }

        void onSuccess()
        {
            ExecuteOnMainThread(_onSuccess);
        }
    }
}
#endif
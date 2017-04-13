#if UNITY_ANDROID
using UnityEngine;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GetSocialSdk.Core
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    class BoolCallbackProxy : JavaInterfaceProxy
    {
        readonly Action<bool> _onSuccess;
        readonly Action<GetSocialError> _onFailure;

        public BoolCallbackProxy(Action<bool> onSuccess, Action<GetSocialError> onFailure)
            : base("im.getsocial.sdk.Callback")
        {
            _onSuccess = onSuccess;
            _onFailure = onFailure;
        }

        void onSuccess(bool value)
        {
            _onSuccess (value);
        }

        void onFailure(AndroidJavaObject throwable)
        {
            var e = throwable.ToGetSocialError();

            GetSocialDebugLogger.D("On onFailure: " + e.Message);

            ExecuteOnMainThread(() => _onFailure(e));
        }
    }
}

#endif
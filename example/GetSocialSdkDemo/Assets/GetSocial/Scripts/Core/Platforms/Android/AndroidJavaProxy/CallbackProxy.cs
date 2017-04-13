#if UNITY_ANDROID
using UnityEngine;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GetSocialSdk.Core
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    class CallbackProxy<T> : JavaInterfaceProxy
    {
        readonly Action<T> _onSuccess;
        readonly Action<GetSocialError> _onFailure;
        readonly Func<AndroidJavaObject, T> _createObjectFunc;

        public CallbackProxy(Action<T> onSuccess, Action<GetSocialError> onFailure, 
            Func<AndroidJavaObject, T> createObjectFunc)
            : base("im.getsocial.sdk.Callback")
        {
            _onSuccess = onSuccess;
            _onFailure = onFailure;
            _createObjectFunc = createObjectFunc;
        }

        void onSuccess(AndroidJavaObject resultAJO)
        {
            T res = _createObjectFunc(resultAJO);

            GetSocialDebugLogger.D("On success: " + res);

            ExecuteOnMainThread(() => _onSuccess(res));
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
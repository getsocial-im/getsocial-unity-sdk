#if UNITY_ANDROID
using UnityEngine;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GetSocialSdk.Core
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    class CallbackProxy<T> : JavaInterfaceProxy where T : IGetSocialBridgeObject<T>, new()
    {
        readonly Action<T> _onSuccess;
        readonly Action<GetSocialError> _onFailure;

        public CallbackProxy(Action<T> onSuccess, Action<GetSocialError> onFailure)
            : base("im.getsocial.sdk.Callback")
        {
            _onSuccess = onSuccess;
            _onFailure = onFailure;
        }

        void onSuccess(AndroidJavaObject resultAJO)
        {
            T res = new T().ParseFromAJO(resultAJO);

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
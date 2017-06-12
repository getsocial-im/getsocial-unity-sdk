#if UNITY_ANDROID
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GetSocialSdk.Core
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    class ListCallbackProxy<T> : JavaInterfaceProxy where T : IGetSocialBridgeObject<T>, new()
    {
        readonly Action<List<T>> _onSuccess;
        readonly Action<GetSocialError> _onFailure;

        public ListCallbackProxy(Action<List<T>> onSuccess, Action<GetSocialError> onFailure)
            : base("im.getsocial.sdk.Callback")
        {
            _onSuccess = onSuccess;
            _onFailure = onFailure;
        }

        void onSuccess(AndroidJavaObject resultAJO)
        {
            List<T> res = resultAJO.FromJavaList<AndroidJavaObject>().ConvertAll(item => new T().ParseFromAJO(item));

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
#if UNITY_ANDROID
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace GetSocialSdk.Core
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    class ListCallbackProxy<T> : JavaInterfaceProxy where T : IConvertableFromNative<T>, new()
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
            ExecuteOnMainThread(() =>
            {
                var res = resultAJO.FromJavaList().ConvertAll(ajo =>
                {
                    using (ajo)
                    {
                        return new T().ParseFromAJO(ajo);
                    }
                }).ToList();
                _onSuccess(res);
            });
        }

        void onFailure(AndroidJavaObject throwable)
        {
            ExecuteOnMainThread(() =>
            {
                using (throwable)
                {
                    var e = throwable.ToGetSocialError();
                    GetSocialDebugLogger.D("On onFailure: " + e.Message);
                    _onFailure(e);
                }
            });
        }
    }
}

#endif
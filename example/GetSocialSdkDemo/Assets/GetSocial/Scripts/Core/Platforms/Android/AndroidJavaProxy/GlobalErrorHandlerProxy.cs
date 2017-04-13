#if UNITY_ANDROID
using UnityEngine;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GetSocialSdk.Core
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    class GlobalErrorListenerProxy : JavaInterfaceProxy
    {
        readonly Action<GetSocialError> _onError;

        public GlobalErrorListenerProxy(Action<GetSocialError> onError)
            : base("im.getsocial.sdk.GlobalErrorListener")
        {
            _onError = onError;
        }

        void onError(AndroidJavaObject throwable)
        {
            var ex = throwable.ToGetSocialError();
            GetSocialDebugLogger.D(string.Format("Global error handler: {0}", ex.Message));

            ExecuteOnMainThread(() => _onError(ex));
        }
    }
}

#endif
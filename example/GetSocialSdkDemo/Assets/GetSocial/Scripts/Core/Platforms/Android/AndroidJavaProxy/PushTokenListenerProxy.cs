#if UNITY_ANDROID
using UnityEngine;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GetSocialSdk.Core
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    internal class PushTokenListenerProxy : JavaInterfaceProxy
    {
        readonly PushTokenListener _onTokenReady;

        public PushTokenListenerProxy(PushTokenListener listener)
            : base("im.getsocial.sdk.pushnotifications.PushTokenListener")
        {
            _onTokenReady = listener;
        }

        void onTokenReady(string deviceToken)
        {
            HandleValue(deviceToken, value => {
                if (_onTokenReady != null) _onTokenReady(value);
            });
        }
    }
}
#endif
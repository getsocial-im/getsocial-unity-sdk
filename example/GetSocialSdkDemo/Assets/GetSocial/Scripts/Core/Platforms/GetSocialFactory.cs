using System;
using System.Linq;
using UnityEngine;

namespace GetSocialSdk.Core
{
    static class GetSocialFactory
    {

        private static IGetSocialNativeBridge _nativeImplementation;

        internal static IGetSocialNativeBridge Instance
        {
            get { return _nativeImplementation ?? (_nativeImplementation = FindNativeBridge()); }
        }

        private static IGetSocialNativeBridge FindNativeBridge()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return new GetSocialNativeBridgeAndroid();
#elif UNITY_IOS && !UNITY_EDITOR
            return new GetSocialNativeBridgeIOS();
#else
            return new GetSocialNativeUnityBridge();
#endif
        }
    }
}
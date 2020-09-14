#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

#if UNITY_ANDROID
using System;
using UnityEngine;
#endif

namespace GetSocialSdk.Core
{
    public static class GetSocialFacebookSharePluginHelper
    {
        public static void RegisterFacebookSharePlugin()
        {
#if UNITY_IOS && !UNITY_EDITOR
            _gs_registerFacebookSharePlugin();
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
            AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass pluginHelper = new AndroidJavaClass("im.getsocial.sdk.internal.unity.GetSocialFacebookPluginHelper");
            pluginHelper.CallStatic("registerFacebookPlugin", new object[1] {activity});
#endif
        }
    
#if UNITY_IOS && !UNITY_EDITOR
        [DllImport("__Internal")]
        static extern void _gs_registerFacebookSharePlugin();
#endif
    }
}
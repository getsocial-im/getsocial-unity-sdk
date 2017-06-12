#if UNITY_IOS
using System;
using AOT;

namespace GetSocialSdk.Core
{
    delegate void OnPublicUserCallbackDelegate(IntPtr onPublicUserPtr, string publicUserJson);
    
    public static class PublicUserCallback
    {
        [MonoPInvokeCallback(typeof(OnPublicUserCallbackDelegate))]
        public static void OnPublicUser(IntPtr onPublicUserCallbackPtr, string publicUserJson)
        {
            var publicUser = new PublicUser().ParseFromJson(publicUserJson);
            IOSUtils.TriggerCallback(onPublicUserCallbackPtr, publicUser);
        }
    }
}

#endif
#if UNITY_IOS
using System;
using System.Collections.Generic;

namespace GetSocialSdk.Core
{
    delegate void VoidCallbackDelegate(IntPtr actionPtr);

    delegate void StringCallbackDelegate(IntPtr actionPtr, string data);

    delegate void FailureWithDataCallbackDelegate(IntPtr actionPtr, string data, string error);

    public static class Callbacks
    {
        [AOT.MonoPInvokeCallback(typeof(VoidCallbackDelegate))]
        public static void ActionCallback(IntPtr actionPtr)
        {
            GetSocialDebugLogger.D("CompleteCallback");
            if (actionPtr != IntPtr.Zero)
            {
                actionPtr.Cast<Action>().Invoke();
            }
        }

        [AOT.MonoPInvokeCallback(typeof(StringCallbackDelegate))]
        public static void StringCallback(IntPtr actionPtr, string result)
        {
            GetSocialDebugLogger.D("StringResultCallaback: " + result);
            IOSUtils.TriggerCallback(actionPtr, result);
        }

        [AOT.MonoPInvokeCallback(typeof(FailureWithDataCallbackDelegate))]
        public static void FailureWithDataCallback(IntPtr actionPtr, string data, string errorMessage)
        {
            GetSocialDebugLogger.D("FailureWithDataCallback: " + errorMessage + ", data: " + data);
            if (actionPtr != IntPtr.Zero)
            {
                actionPtr.Cast<Action<string, string>>().Invoke(data, errorMessage);
            }
        }
    }
}

#endif
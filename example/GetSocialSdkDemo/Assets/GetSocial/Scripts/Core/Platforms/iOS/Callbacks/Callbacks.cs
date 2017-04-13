#if UNITY_IOS
using System;
using AOT;

namespace GetSocialSdk.Core
{
    delegate void VoidCallbackDelegate(IntPtr actionPtr);

    delegate void StringCallbackDelegate(IntPtr actionPtr, string data);

    delegate void BoolCallbackDelegate(IntPtr actionPtr, bool result);

    delegate void IntCallbackDelegate(IntPtr actionPtr, int result);

    delegate void FailureCallbackDelegate(IntPtr actionPtr, string error);

    delegate void FailureWithDataCallbackDelegate(IntPtr actionPtr, string data, string error);

    delegate void FetchReferralDataCallbackDelegate(IntPtr actionPtr, string referralDataJson);

    delegate bool NotificationActionListenerDelegate(IntPtr funcPtr, string notificationActionJson);

    public static class Callbacks
    {

        [MonoPInvokeCallback(typeof(VoidCallbackDelegate))]
        public static void ActionCallback(IntPtr actionPtr)
        {
            GetSocialDebugLogger.D("CompleteCallback");
            if (actionPtr != IntPtr.Zero)
            {
                actionPtr.Cast<Action>().Invoke();
            }
        }

        [MonoPInvokeCallback(typeof(StringCallbackDelegate))]
        public static void StringCallback(IntPtr actionPtr, string result)
        {
            GetSocialDebugLogger.D("StringResultCallaback: " + result);
            IOSUtils.TriggerCallback(actionPtr, result);
        }

        [MonoPInvokeCallback(typeof(BoolCallbackDelegate))]
        public static void BoolCallback(IntPtr actionPtr, bool result)
        {
            GetSocialDebugLogger.D("BoolCallback: " + result);
            IOSUtils.TriggerCallback(actionPtr, result);
        }

        [MonoPInvokeCallback(typeof(IntCallbackDelegate))]
        public static void IntCallback(IntPtr actionPtr, int result)
        {
            GetSocialDebugLogger.D("IntCallback: " + result);
            IOSUtils.TriggerCallback(actionPtr, result);
        }

        [MonoPInvokeCallback(typeof(FailureCallbackDelegate))]
        public static void FailureCallback(IntPtr actionPtr, string serializedError)
        {
            GetSocialDebugLogger.D("FailureCallback: " + serializedError + ", ptr: " + actionPtr.ToInt32());
            var error = new GetSocialError().ParseFromJson(serializedError);
            IOSUtils.TriggerCallback(actionPtr, error);
        }

        [MonoPInvokeCallback(typeof(FailureWithDataCallbackDelegate))]
        public static void FailureWithDataCallback(IntPtr actionPtr, string data, string errorMessage)
        {
            GetSocialDebugLogger.D("FailureWithDataCallback: " + errorMessage + ", data: " + data);
            if (actionPtr != IntPtr.Zero)
            {
                actionPtr.Cast<Action<string, string>>().Invoke(data, errorMessage);
            }
        }

        [MonoPInvokeCallback(typeof(FetchReferralDataCallbackDelegate))]
        public static void FetchReferralDataCallback(IntPtr actionPtr, string referralData)
        {
            GetSocialDebugLogger.D("OnReferralDataReceived: " + referralData);
            var data = new ReferralData().ParseFromJson(referralData);
            IOSUtils.TriggerCallback(actionPtr, data);
        }

        [MonoPInvokeCallback(typeof(NotificationActionListenerDelegate))]
        public static bool NotificationActionListener(IntPtr funcPtr, string notificationActionJson)
        {
            GetSocialDebugLogger.D("NotificationActionReceived: " + notificationActionJson);
            var notificationAction = new NotificationAction().ParseFromJson(notificationActionJson);
            if (funcPtr != IntPtr.Zero)
            {
                return funcPtr.Cast<Func<NotificationAction, bool>>().Invoke(notificationAction);
            }
            return false;
        }


    }
}

#endif
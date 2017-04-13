#if UNITY_IOS && USE_GETSOCIAL_UI
using System;
using AOT;
using GetSocialSdk.Core;

namespace GetSocialSdk.Ui
{
    delegate void ActivityActionButtonClickedDelegate(IntPtr onButtonClickedPtr, string actionId, string serializedActivityPost);

    public static class ActivityFeedActionButtonCallback
    {
        [MonoPInvokeCallback(typeof(ActivityActionButtonClickedDelegate))]
        public static void OnActionButtonClick(IntPtr onButtonClickedPtr, string actionId, string serializedActivityPost)
        {
            GetSocialDebugLogger.D(string.Format("OnActionButtonClick for action [{0}], post: {1}", actionId, serializedActivityPost));
            var post = new ActivityPost().ParseFromJson(serializedActivityPost);

            if (onButtonClickedPtr != IntPtr.Zero)
            {
                onButtonClickedPtr.Cast<Action<string, ActivityPost>>().Invoke(actionId, post);
            }
        }
    }
}
#endif

#if UNITY_IOS 
using System;
using GetSocialSdk.Core;

namespace GetSocialSdk.Ui
{
    delegate void OnAction(IntPtr onButtonClickedPtr, string serializedAction);

    public static class ActivityFeedActionDelegate
    {
        [AOT.MonoPInvokeCallback(typeof(OnAction))]
        public static void OnActionButtonClick(IntPtr onActionPtr, string serializedAction)
        {
            GetSocialDebugLogger.D(string.Format("OnActionClick: [{0}]", serializedAction));

            if (onActionPtr != IntPtr.Zero)
            {
                var action = GetSocialJsonBridge.FromJson<Action>(serializedAction);
                onActionPtr.Cast<Action<Action>>().Invoke(action);
            }
        }
    }
}
#endif

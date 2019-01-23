#if UNITY_IOS && USE_GETSOCIAL_UI
using System;
using GetSocialSdk.Core;

namespace GetSocialSdk.Ui
{
    internal delegate bool ActionListenerDelegate(IntPtr actionListenerPtr, string action);
    
    public static class ActionListenerCallback
    {
        [AOT.MonoPInvokeCallback(typeof(ActionListenerDelegate))]
        public static bool OnAction(IntPtr actionListenerPtr, string action)
        {
            GetSocialDebugLogger.D(string.Format("OnActionReceived: {0}", action));

            if (actionListenerPtr != IntPtr.Zero)
            {
                return actionListenerPtr.Cast<ActionListener>()
                    .Invoke(new GetSocialAction().ParseFromJson(action.ToDict()));
            }

            return false;
        }
    }
}

#endif
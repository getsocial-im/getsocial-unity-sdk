#if UNITY_IOS 
using System;
using GetSocialSdk.Core;

namespace GetSocialSdk.Ui
{
    internal delegate void ActionListenerDelegate(IntPtr actionListenerPtr, string serializedAction);
    
    public static class ActionListenerCallback
    {
        [AOT.MonoPInvokeCallback(typeof(ActionListenerDelegate))]
        public static void OnAction(IntPtr actionListenerPtr, string serializedAction)
        {
            GetSocialDebugLogger.D(string.Format("OnActionReceived: {0}", serializedAction));

            if (actionListenerPtr != IntPtr.Zero)
            {
                var action = GetSocialJsonBridge.FromJson<GetSocialAction>(serializedAction);
                actionListenerPtr.Cast<ActionListener>().Invoke(action);
            }
        }
    }
}

#endif
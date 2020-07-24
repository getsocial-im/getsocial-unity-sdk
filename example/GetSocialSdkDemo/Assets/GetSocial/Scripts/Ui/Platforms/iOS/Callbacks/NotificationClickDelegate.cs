#if UNITY_IOS 

using System;
using GetSocialSdk.Core;

namespace GetSocialSdk.Ui
{
    
    delegate void OnNotificationClick(IntPtr onNotificationClickedPtr, string serializedNotification, string serializedNotificationContext);
    
    public static class NotificationClickDelegate
    {
        [AOT.MonoPInvokeCallback(typeof(OnNotificationClick))]
        public static void OnNotificationClick(IntPtr onNotificationClickedPtr, string serializedNotification, string serializedNotificationContext)
        {
            GetSocialDebugLogger.D(string.Format("OnNotificationClick for notification: {0}, context: {1}", serializedNotification, serializedNotificationContext));

            if (onNotificationClickedPtr != IntPtr.Zero)
            {
                var notification = GetSocialJsonBridge.FromJson<Notification>(serializedNotification);
                GetSocialDebugLogger.D("Notification: " + notification);
                NotificationContext context = null;
                if (serializedNotification != null)
                {
                    context = GetSocialJsonBridge.FromJson<NotificationContext>(serializedNotificationContext);
                }
                GetSocialDebugLogger.D("Context: " + context);
                onNotificationClickedPtr.Cast<NotificationCenterViewBuilder.NotificationClickListener>().Invoke(notification, context);
            }
        }
        
    }
}

#endif
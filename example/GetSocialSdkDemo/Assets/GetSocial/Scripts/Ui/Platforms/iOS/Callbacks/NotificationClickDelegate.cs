#if UNITY_IOS && USE_GETSOCIAL_UI

using System;
using GetSocialSdk.Core;

namespace GetSocialSdk.Ui
{
    
    delegate bool OnNotificationClick(IntPtr onNotificationClickedPtr, string serializedNotification);
    
    public static class NotificationClickDelegate
    {
        [AOT.MonoPInvokeCallback(typeof(OnNotificationClick))]
        public static bool OnNotificationClick(IntPtr onNotificationClickedPtr, string serializedNotification)
        {
            GetSocialDebugLogger.D(string.Format("OnNotificationClick for notification: {0}", serializedNotification));

            if (onNotificationClickedPtr != IntPtr.Zero)
            {
                var notification = new Notification().ParseFromJson(serializedNotification.ToDict());
                return onNotificationClickedPtr.Cast<NotificationCenterViewBuilder.NotificationClickListener>().Invoke(notification);
            }

            return false;
        }
        
    }
}

#endif
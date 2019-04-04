#if UNITY_IOS && USE_GETSOCIAL_UI

using System;
using GetSocialSdk.Core;

namespace GetSocialSdk.Ui
{
    delegate bool OnNotificationActionButtonClick(IntPtr onNotificationClickedPtr, string serializedActionButton, string serializedNotification);
    
    public static class NotificationActionButtonClickDelegate
    {
        [AOT.MonoPInvokeCallback(typeof(OnNotificationActionButtonClick))]
        public static bool OnActionButtonClick(IntPtr onActionButtonClickedPtr, string serializedActionButton, string serializedNotification)
        {
            GetSocialDebugLogger.D(string.Format("OnActionButtonClick for notification: {0}, actionButton: {1}", serializedNotification, serializedActionButton));

            if (onActionButtonClickedPtr != IntPtr.Zero)
            {
                var notification = new Notification().ParseFromJson(serializedNotification.ToDict());
                var actionButton = new ActionButton().ParseFromJson(serializedActionButton.ToDict());
                
                return onActionButtonClickedPtr.Cast<NotificationCenterViewBuilder.ActionButtonClickListener>().Invoke(notification, actionButton);
            }

            return false;
        }
        
    }
}

#endif
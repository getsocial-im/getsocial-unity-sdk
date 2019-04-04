#if UNITY_ANDROID && USE_GETSOCIAL_UI
using System;
using GetSocialSdk.Core;
using UnityEngine;

namespace GetSocialSdk.Ui
{
    public class NotificationClickListenerProxy : JavaInterfaceProxy
    {
        readonly NotificationCenterViewBuilder.NotificationClickListener _notificationClickListener;

        public NotificationClickListenerProxy(NotificationCenterViewBuilder.NotificationClickListener notificationClickListener)
            : base("im.getsocial.sdk.ui.pushnotifications.NotificationCenterViewBuilder$NotificationClickListener")
        {
            _notificationClickListener = notificationClickListener;
        }

        bool onNotificationClicked(AndroidJavaObject notificationAJO)
        {
            Debug.Log(">>>>>>> XXXX");
            var notification = new Notification().ParseFromAJO(notificationAJO);
            return _notificationClickListener != null && _notificationClickListener(notification);
        }
        
    }
}
#endif
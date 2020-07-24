#if UNITY_ANDROID 
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

        void onNotificationClicked(AndroidJavaObject notificationAJO, AndroidJavaObject contextAJO)
        {
            Debug.Log(">>>>>>> XXXX");
            var notification = AndroidAJOConverter.Convert<Notification>(notificationAJO);
            var context = AndroidAJOConverter.Convert<NotificationContext>(contextAJO);
            ExecuteOnMainThread(() => _notificationClickListener(notification, context));
        }
        
    }
}
#endif
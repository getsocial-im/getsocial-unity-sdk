#if UNITY_ANDROID && USE_GETSOCIAL_UI
using System;
using GetSocialSdk.Core;
using UnityEngine;

namespace GetSocialSdk.Ui
{
    public class NotificationActionButtonClickListenerProxy : JavaInterfaceProxy
    {
        readonly NotificationCenterViewBuilder.ActionButtonClickListener _actionButtonClickListener;

        public NotificationActionButtonClickListenerProxy(NotificationCenterViewBuilder.ActionButtonClickListener actionButtonClickListener)
            : base("im.getsocial.sdk.ui.pushnotifications.NotificationCenterViewBuilder$ActionButtonClickListener")
        {
            _actionButtonClickListener = actionButtonClickListener;
        }

        bool onActionButtonClicked(AndroidJavaObject notificationAJO, AndroidJavaObject actionButtonAJO)
        {
            Debug.Log(">>>>>>> XXXX");
            var notification = new Notification().ParseFromAJO(notificationAJO);
            var actionButton = new ActionButton().ParseFromAJO(actionButtonAJO);
            return _actionButtonClickListener != null &&_actionButtonClickListener(notification, actionButton);
        }
        
    }
}
#endif
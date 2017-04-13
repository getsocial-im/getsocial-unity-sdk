#if UNITY_ANDROID
using UnityEngine;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GetSocialSdk.Core
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    class NotificationActionListenerProxy : JavaInterfaceProxy
    {
        readonly Func<NotificationAction, bool> _onNotificationAction;

        public NotificationActionListenerProxy(Func<NotificationAction, bool> onNotificationAction)
            : base("im.getsocial.sdk.pushnotifications.NotificationActionListener")
        {
            _onNotificationAction = onNotificationAction;
        }

        bool onActionReceived(AndroidJavaObject ajo)
        {
            return _onNotificationAction(new NotificationAction().ParseFromAJO(ajo));
        }
    }
}
#endif
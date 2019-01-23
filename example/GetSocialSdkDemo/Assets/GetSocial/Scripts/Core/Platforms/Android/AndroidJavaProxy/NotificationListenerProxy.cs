#if UNITY_ANDROID
using UnityEngine;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GetSocialSdk.Core
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    internal class NotificationListenerProxy : JavaInterfaceProxy
    {
        readonly NotificationListener _onNotification;

        public NotificationListenerProxy(NotificationListener listener)
            : base("im.getsocial.sdk.pushnotifications.NotificationListener")
        {
            _onNotification = listener;
        }

        bool onNotificationReceived(AndroidJavaObject ajo, bool wasClicked)
        {
            return _onNotification != null && _onNotification(new Notification().ParseFromAJO(ajo), wasClicked);
        }
    }
}
#endif
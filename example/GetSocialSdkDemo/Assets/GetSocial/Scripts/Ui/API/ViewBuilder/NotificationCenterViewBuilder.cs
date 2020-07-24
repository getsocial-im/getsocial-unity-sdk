using GetSocialSdk.Core;
#if UNITY_ANDROID
using UnityEngine;
#endif

#if UNITY_IOS
using GetSocialSdk.MiniJSON;
using System;
using System.Runtime.InteropServices;
#endif

namespace GetSocialSdk.Ui
{
    public sealed class NotificationCenterViewBuilder : ViewBuilder<NotificationCenterViewBuilder>
    {

#pragma warning disable 414

        /// <summary>
        /// Called when a notification is clicked on.
        /// </summary>
        /// <param name="notification">selected notification.</param>
        public delegate void NotificationClickListener(Notification notification, NotificationContext context);
        
        private NotificationsQuery _query;
        private NotificationClickListener _notificationClickListener;

#pragma warning restore 414

        public static NotificationCenterViewBuilder Create(NotificationsQuery query)
        {
            return new NotificationCenterViewBuilder(query);
        }

        internal NotificationCenterViewBuilder(NotificationsQuery query)
        {
            _query = query;
        }

        /// <summary>
        /// Sets notification click listener.
        /// </summary>
        /// <param name="notificationClickListener">listener to set.</param>
        /// <returns>Builder instance.</returns>
        public NotificationCenterViewBuilder SetNotificationClickListener(
            NotificationClickListener notificationClickListener)
        {
            _notificationClickListener = notificationClickListener;
            return this;
        }

        internal override bool ShowInternal()
        {
#if UNITY_ANDROID
            return ShowBuilder(ToAJO());
#elif UNITY_IOS
            return _gs_showNotificationCenterView(_customWindowTitle, GSJson.Serialize(_query),
                NotificationClickDelegate.OnNotificationClick,
                _notificationClickListener.GetPointer(),
                Callbacks.ActionCallback, _onOpen.GetPointer(),
                Callbacks.ActionCallback, _onClose.GetPointer(),
                UiActionListenerCallback.OnUiAction, _uiActionListener.GetPointer());
#else
            return false;
#endif
        }
#if UNITY_ANDROID

        AndroidJavaObject ToAJO()
        {
            var clazz = new AndroidJavaClass("im.getsocial.sdk.ui.pushnotifications.NotificationCenterViewBuilder");
            var notificationCenterViewBuilderAJO = clazz.CallStaticAJO("create", AndroidAJOConverter.Convert(_query, "im.getsocial.sdk.notifications.NotificationsQuery"));

            if (_notificationClickListener != null)
            {
                notificationCenterViewBuilderAJO.CallAJO("setNotificationClickListener",
                    new NotificationClickListenerProxy(_notificationClickListener));
            }

            return notificationCenterViewBuilderAJO;
        }

#elif UNITY_IOS

        [DllImport("__Internal")]
        static extern bool _gs_showNotificationCenterView(string customWindowTitle, string query,
            OnNotificationClick onNotificationClick, IntPtr onNotificationClickPtr,
            Action<IntPtr> onOpenAction, IntPtr onOpenActionPtr,
            Action<IntPtr> onCloseAction, IntPtr onCloseActionPtr,
            Action<IntPtr, int> uiActionListener, IntPtr uiActionListenerPtr);
#endif
    }
}
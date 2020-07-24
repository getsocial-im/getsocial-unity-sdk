using System;
using System.Collections.Generic;

namespace GetSocialSdk.Core
{
    public static class Notifications
    {
        /// <summary>
        /// Get the list of notifications for current user.
        /// </summary>
        /// <param name="pagingQuery">Notifications query.</param>
        /// <param name="success">Callback to be invoked with a list of notifications.</param>
        /// <param name="failure">Called if operation failed.</param>
        public static void Get(PagingQuery<NotificationsQuery> pagingQuery, Action<PagingResult<Notification>> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.GetNotifications(pagingQuery, success, failure);
        }  

        /// <summary>
        /// Get a number of notifications for current user.
        /// </summary>
        /// <param name="query">Notifications count query.</param>
        /// <param name="success">Callback to be invoked with a number of notifications.</param>
        /// <param name="failure">Called if operation failed.</param>
        public static void Count(NotificationsQuery query, Action<int> success, Action<GetSocialError> failure) {
            GetSocialFactory.Bridge.CountNotifications(query, success, failure);
        }

        /// <summary>
        /// Set notifications status to one of <see cref="NotificationStatus"/>.
        /// </summary>
        /// <param name="newStatus">One of <see cref="NotificationStatus"/></param>
        /// <param name="notificationIds">List of notifications IDs to change the read status.</param>
        /// <param name="success">A callback to indicate if this operation was successful.</param>
        /// <param name="failure">Called if operation failed.</param>
        public static void SetStatus(string newStatus, List<string> notificationIds, Action success, Action<GetSocialError> failure) {
            GetSocialFactory.Bridge.SetStatus(newStatus, notificationIds, success, failure);
        }

        /// <summary>
        /// If set to `false` - current user won't receive GetSocial notifications anymore, until called with `true`.
        /// </summary>
        /// <param name="enabled">Disabled or enable PNs.</param>
        /// <param name="success">A callback to indicate if this operation was successful.</param>
        /// <param name="failure">Called if operation failed.</param>
        public static void SetPushNotificationsEnabled(bool enabled, Action success, Action<GetSocialError> failure) {
            GetSocialFactory.Bridge.SetPushNotificationsEnabled(enabled, success, failure);
        }

        /// <summary>
        /// Check if PNs are enabled for current user.
        /// </summary>
        /// <param name="success">Called with `true` if is enabled, with `false` otherwise.</param>
        /// <param name="failure">Called if operation failed.</param>
        public static void ArePushNotificationsEnabled(Action<bool> success, Action<GetSocialError> failure) {
            GetSocialFactory.Bridge.ArePushNotificationsEnabled(success, failure);
        }

        /// <summary>
        /// Send notification to any GetSocial user. Also you can use placeholders in <see cref="SendNotificationPlaceholders"/>.
        /// </summary>
        /// <param name="content">Content of push notification.</param>
        /// <param name="target">List of user IDs or placeholders who will receive the notification.</param>
        /// <param name="success">Notifies if operation was successful.</param>
        /// <param name="failure">Called if operation failed.</param>
        public static void Send(NotificationContent content, SendNotificationTarget target, Action success, Action<GetSocialError> failure) {
            GetSocialFactory.Bridge.Send(content, target, success, failure);
        }

        /// <summary>
        /// If autoRegisterForPush meta property is set to false in the getsocial.json, call this method to register for Push Notifications.
        /// </summary>
        public static void RegisterDevice() {
            GetSocialFactory.Bridge.RegisterDevice();
        }

        /// <summary>
        /// Set a notification listener, you can handle notification while application is in foreground. 
        /// </summary>
        /// <param name="listener">An object that will be notified with received notification.</param>
        public static void SetOnNotificationReceivedListener(Action<Notification> listener) {
            GetSocialFactory.Bridge.SetOnNotificationReceivedListener(listener);
        }

        /// <summary>
        /// Set a notification listener, you can handle a click on notification.
        /// </summary>
        /// <param name="listener">An object that will be notified with clicked notification.</param>
        public static void SetOnNotificationClickedListener(Action<Notification, NotificationContext> listener) {
            GetSocialFactory.Bridge.SetOnNotificationClickedListener(listener);
        }

        /// <summary>
        /// Set a listener to be called when Push Notifications token obtained by GetSocial.
        /// </summary>
        /// <param name="listener">An object that will be notified with push token.</param>
        public static void SetOnTokenReceivedListener(Action<string> listener) {
            GetSocialFactory.Bridge.SetOnTokenReceivedListener(listener);
        }

    }
}
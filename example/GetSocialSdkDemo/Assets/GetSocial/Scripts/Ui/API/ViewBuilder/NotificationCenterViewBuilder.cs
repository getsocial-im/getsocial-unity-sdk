#if USE_GETSOCIAL_UI
using System;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using GetSocialSdk.Core;
using GetSocialSdk.MiniJSON;
#if UNITY_ANDROID
using UnityEngine;
#endif

#if UNITY_IOS
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
        /// <returns>true, if notification is handled, otherwise false.</returns>
        public delegate bool NotificationClickListener(Notification notification);
        
        /// <summary>
        /// Called when an action button is clicked on.
        /// </summary>
        /// <param name="notification">the notification which the action button belongs to.</param>
        /// <param name="actionButton">the button is clicked on.</param>
        /// <returns>true, if notification is handled, otherwise false.</returns>
        public delegate bool ActionButtonClickListener(Notification notification, ActionButton actionButton) ;

        private string[] _notificationTypes;
        private string[] _actionTypes;
        private NotificationClickListener _notificationClickListener;
        private ActionButtonClickListener _actionButtonClickListener;
        
#pragma warning restore 414
        
        /// <summary>
        /// Sets notification types to filter list of notifications.
        /// </summary>
        /// <param name="notificationTypes">notification types</param>
        /// <returns>Builder instance.</returns>
        public NotificationCenterViewBuilder SetFilterByTypes(string[] notificationTypes)
        {
            _notificationTypes = notificationTypes;
            return this;
        }

        /// <summary>
        /// Sets notification action types to filter list of notifications.
        /// </summary>
        /// <param name="actionTypes">action types</param>
        /// <returns>Builder instance.</returns>
        public NotificationCenterViewBuilder SetFilterByActions(string[] actionTypes)
        {
            _actionTypes = actionTypes;
            return this;
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

        /// <summary>
        /// Sets notification action button click listener.
        /// </summary>
        /// <param name="actionButtonClickListener">listener to set.</param>
        /// <returns>Builder instance.</returns>
        public NotificationCenterViewBuilder SetActionButtonClickListener(
            ActionButtonClickListener actionButtonClickListener)
        {
            _actionButtonClickListener = actionButtonClickListener;
            return this;
        }

        internal override bool ShowInternal()
        {
#if UNITY_ANDROID
            return ShowBuilder(ToAJO());
#elif UNITY_IOS
            return _gs_showNotificationCenterView(_customWindowTitle, GSJson.Serialize(_notificationTypes), GSJson.Serialize(_actionTypes),
                NotificationClickDelegate.OnNotificationClick,
                _notificationClickListener.GetPointer(),
                NotificationActionButtonClickDelegate.OnActionButtonClick,
                _actionButtonClickListener.GetPointer(),
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
            var notificationCenterViewBuilderAJO =
                new AndroidJavaObject("im.getsocial.sdk.ui.pushnotifications.NotificationCenterViewBuilder");

            if (_notificationTypes != null && _notificationTypes.Length > 0)
            {
                notificationCenterViewBuilderAJO.CallAJO("setFilterByTypes",
                    _notificationTypes.ToList().ToJavaStringArray());
            }

            if (_actionTypes != null && _actionTypes.Length > 0)
            {
                notificationCenterViewBuilderAJO.CallAJO("setFilterByActions", _actionTypes.ToList().ToJavaStringArray());
            }
            
            if (_notificationClickListener != null)
            {
                notificationCenterViewBuilderAJO.CallAJO("setNotificationClickListener",
                    new NotificationClickListenerProxy(_notificationClickListener));
            }

            if (_actionButtonClickListener != null)
            {
                notificationCenterViewBuilderAJO.CallAJO("setActionButtonClickListener",
                    new NotificationActionButtonClickListenerProxy(_actionButtonClickListener));
            }

            return notificationCenterViewBuilderAJO;
        }

#elif UNITY_IOS

        [DllImport("__Internal")]
        static extern bool _gs_showNotificationCenterView(string customWindowTitle, string notificationTypes,
         string actionTypes, 
            OnNotificationClick onNotificationClick, IntPtr onNotificationClickPtr,
            OnNotificationActionButtonClick onActionButtonClick, IntPtr onActionButtonClickPtr,
            Action<IntPtr> onOpenAction, IntPtr onOpenActionPtr,
            Action<IntPtr> onCloseAction, IntPtr onCloseActionPtr,
            Action<IntPtr, int> uiActionListener, IntPtr uiActionListenerPtr);
#endif
    }
}
#endif

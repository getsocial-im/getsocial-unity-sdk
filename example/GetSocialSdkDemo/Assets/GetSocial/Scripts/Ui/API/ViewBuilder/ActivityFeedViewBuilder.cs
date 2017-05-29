#if USE_GETSOCIAL_UI
using System;
using System.Runtime.InteropServices;
using GetSocialSdk.Core;
using UnityEngine;

namespace GetSocialSdk.Ui
{
    /// <summary>
    /// Use this class to construct activity feed view.
    /// Call <see cref="Show()"/> to present the UI.
    /// </summary>
    public sealed class ActivityFeedViewBuilder : ViewBuilder<ActivityFeedViewBuilder>
    {
        readonly string _feed;

        Action<string, ActivityPost> _onButtonClickListener;
        Action<PublicUser> _onAvatarClickListener;

        internal ActivityFeedViewBuilder()
        {
            _feed = ActivitiesQuery.GlobalFeed;
        }

        internal ActivityFeedViewBuilder(string feed)
        {
            _feed = feed;
        }

        /// <summary>
        /// Register callback to listen when activity action button was clicked.
        /// </summary>
        /// <param name="onButtonClickListener">Called when activity action button was clicked.</param>
        /// <returns><see cref="ActivityFeedViewBuilder" instance./></returns>
        public ActivityFeedViewBuilder SetButtonActionListener(Action<string, ActivityPost> onButtonClickListener)
        {
            _onButtonClickListener = onButtonClickListener;

            return this;
        }

        /// <summary>
        /// Set a listener that will be called when user taps on someones avatar.
        /// </summary>
        /// <param name="onAvatarClickListener"></param>
        /// <returns></returns>
        public ActivityFeedViewBuilder SetAvatarClickListener(Action<PublicUser> onAvatarClickListener)
        {
            _onAvatarClickListener = onAvatarClickListener;

            return this;
        }

        public override bool Show()
        {
#if UNITY_ANDROID
            return ShowBuilder(ToAJO());
#elif UNITY_IOS
            return _gs_showActivityFeedView(_customWindowTitle, _feed,
                ActivityFeedActionButtonCallback.OnActionButtonClick,
                _onButtonClickListener.GetPointer(),
                Callbacks.ActionCallback,
                _onOpen.GetPointer(),
                Callbacks.ActionCallback,
                _onClose.GetPointer(),
                UiActionListenerCallback.OnUiAction,
                _uiActionListener.GetPointer(),
                AvatarClickListenerCallback.OnAvatarClicked,
                _onAvatarClickListener.GetPointer());
#else
            return false;
#endif
        }

#if UNITY_ANDROID

        AndroidJavaObject ToAJO()
        {
            var activityFeedBuilderAJO =
                new AndroidJavaObject("im.getsocial.sdk.ui.activities.ActivityFeedViewBuilder", _feed);

            if (_onButtonClickListener != null)
            {
                activityFeedBuilderAJO.CallAJO("setButtonActionListener",
                    new ActionButtonListenerProxy(_onButtonClickListener));
            }
            if (_onAvatarClickListener != null)
            {
                activityFeedBuilderAJO.CallAJO("setAvatarClickListener",
                    new AvatarClickListenerProxy(_onAvatarClickListener));
            }

            return activityFeedBuilderAJO;
        }

#elif UNITY_IOS

        [DllImport("__Internal")]
        static extern bool _gs_showActivityFeedView(string customWindowTitle, string feed,
            Action<IntPtr, string, string> onActionButtonClick, IntPtr onButtonClickPtr,
            Action<IntPtr> onOpenAction, IntPtr onOpenActionPtr,
            Action<IntPtr> onCloseAction, IntPtr onCloseActionPtr,
            Action<IntPtr, int> uiActionListener, IntPtr uiActionListenerPtr,
            Action<IntPtr, string> avatarClickListener, IntPtr avatarClickListenerPtr);

#endif
    }
}

#endif
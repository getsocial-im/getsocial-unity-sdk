#if USE_GETSOCIAL_UI
using System;
using System.Runtime.InteropServices;
using GetSocialSdk.Core;
using UnityEngine;

namespace GetSocialSdk.Ui
{
    /// <summary>
    /// Use this class to construct activity details view.
    /// Call <see cref="Show()"/> to present the UI.
    /// </summary>
    public sealed class ActivityDetailsViewBuilder : ViewBuilder<ActivityDetailsViewBuilder>
    {
        readonly string _activityId;

        private bool _showActivityFeedView;

        Action<string, ActivityPost> _onButtonClicked;
        Action<PublicUser> _onAvatarClickListener;

        internal ActivityDetailsViewBuilder(string activityId)
        {
            _activityId = activityId;
        }

        /// <summary>
        /// By default is true. If you want to activity details without activity feed view in history,
        /// set it to false.
        /// </summary>
        /// <param name="showFeedView">true to have activity feed view in history, false otherwise</param>
        /// <returns><see cref="ActivityDetailsViewBuilder" instance./></returns>
        public ActivityDetailsViewBuilder SetShowActivityFeedView(bool showFeedView)
        {
            _showActivityFeedView = showFeedView;

            return this;
        }


        /// <summary>
        /// Register callback to listen when activity action button was clicked.
        /// </summary>
        /// <param name="onButtonClicked">Called when activity action button was clicked.</param>
        /// <returns><see cref="ActivityDetailsViewBuilder" instance./></returns>
        public ActivityDetailsViewBuilder SetButtonActionListener(Action<string, ActivityPost> onButtonClicked)
        {
            _onButtonClicked = onButtonClicked;

            return this;
        }

        /// <summary>
        /// Set a listener that will be called when user taps on someones avatar.
        /// </summary>
        /// <param name="onAvatarClickListener"></param>
        /// <returns></returns>
        public ActivityDetailsViewBuilder SetAvatarClickListener(Action<PublicUser> onAvatarClickListener)
        {
            _onAvatarClickListener = onAvatarClickListener;

            return this;
        }


        public override bool Show()
        {
#if UNITY_ANDROID
            return ShowBuilder(ToAJO());
#elif UNITY_IOS
            return _gs_showActivityDetailsView(_customWindowTitle, _activityId, _showActivityFeedView,
                ActivityFeedActionButtonCallback.OnActionButtonClick,
                _onButtonClicked.GetPointer(),
                Callbacks.ActionCallback,
                _onOpen.GetPointer(),
                Callbacks.ActionCallback,
                _onClose.GetPointer(),
                UiActionListenerCallback.OnUiAction,
                _uiActionListener.GetPointer(),
                AvatarClickListenerCallback.OnAvatarClicked,
                _onAvatarClickListener.GetPointer()
                );
#else
            return false;
#endif
        }

#if UNITY_ANDROID

        AndroidJavaObject ToAJO()
        {
            var activityDetailsBuilderAJO =
                new AndroidJavaObject("im.getsocial.sdk.ui.activities.ActivityDetailsViewBuilder", _activityId);

            activityDetailsBuilderAJO.CallAJO("setShowActivityFeedView", _showActivityFeedView);
            if (_onButtonClicked != null)
            {
                activityDetailsBuilderAJO.CallAJO("setButtonActionListener",
                    new ActionButtonListenerProxy(_onButtonClicked));
            }
            if (_onAvatarClickListener != null)
            {
                activityDetailsBuilderAJO.CallAJO("setAvatarClickListener",
                    new AvatarClickListenerProxy(_onAvatarClickListener));
            }

            return activityDetailsBuilderAJO;
        }

#elif UNITY_IOS

        [DllImport("__Internal")]
        static extern bool _gs_showActivityDetailsView(string customWindowTitle, string activityId, bool showFeedView,
            Action<IntPtr, string, string> onActionButtonClick, IntPtr onButtonClickPtr,
            Action<IntPtr> onOpenAction, IntPtr onOpenActionPtr,
            Action<IntPtr> onCloseAction, IntPtr onCloseActionPtr,
            Action<IntPtr, int> uiActionListener, IntPtr uiActionListenerPtr,
            Action<IntPtr, string> avatarClickListener, IntPtr avatarClickListenerPtr);

#endif
    }
}

#endif
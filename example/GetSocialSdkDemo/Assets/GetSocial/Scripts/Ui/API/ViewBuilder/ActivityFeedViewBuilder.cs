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
        
        string _filterUserId;
        bool _readOnly;
        bool _friendsFeed;
        
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
        /// <returns><see cref="ActivityFeedViewBuilder"/> instance.</returns>
        public ActivityFeedViewBuilder SetButtonActionListener(Action<string, ActivityPost> onButtonClickListener)
        {
            _onButtonClickListener = onButtonClickListener;

            return this;
        }

        /// <summary>
        /// Set a listener that will be called when user taps on someones avatar.
        /// </summary>
        /// <param name="onAvatarClickListener"></param>
        /// <returns><see cref="ActivityFeedViewBuilder"/> instance.</returns>
        public ActivityFeedViewBuilder SetAvatarClickListener(Action<PublicUser> onAvatarClickListener)
        {
            _onAvatarClickListener = onAvatarClickListener;

            return this;
        }
        
        
        /// <summary>
        /// Set this to valid user id if you want to display feed of only one user.
        ///  If is not set, normal feed will be shown.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns><see cref="ActivityFeedViewBuilder"/> instance.</returns>
        public ActivityFeedViewBuilder SetFilterByUser(String userId) {
            _filterUserId = userId;
            
            return this;
        }
        
        /// <summary>
        /// Make the feed read-only. UI elements, that allows to post, comment or like are hidden.
        /// </summary>
        /// <param name="readOnly">should feed be read-only</param>
        /// <returns><see cref="ActivityFeedViewBuilder"/> instance.</returns>
        public ActivityFeedViewBuilder SetReadOnly(bool readOnly) {
            _readOnly = readOnly;
            
            return this;
        }
        
        /// <summary>
        /// Display feed with posts of your friends and your own.
        /// </summary>
        /// <param name="showFriendsFeed">display friends feed or not</param>
        /// <returns><see cref="ActivityDetailsViewBuilder"/> instance.</returns>
        public ActivityFeedViewBuilder SetShowFriendsFeed(bool showFriendsFeed)
        {
            _friendsFeed = showFriendsFeed;

            return this;
        }

        internal override bool ShowInternal()
        {
#if UNITY_ANDROID
            return ShowBuilder(ToAJO());
#elif UNITY_IOS
            return _gs_showActivityFeedView(_customWindowTitle, _feed, _filterUserId, _readOnly, _friendsFeed, 
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

            if (_filterUserId != null) {
                activityFeedBuilderAJO.CallAJO("setFilterByUser", _filterUserId);
            }
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
    
            activityFeedBuilderAJO.CallAJO("setReadOnly", _readOnly);
            activityFeedBuilderAJO.CallAJO("setShowFriendsFeed", _friendsFeed);

            return activityFeedBuilderAJO;
        }

#elif UNITY_IOS

        [DllImport("__Internal")]
        static extern bool _gs_showActivityFeedView(string customWindowTitle, string feed, string filterUserId, bool readOnly, bool friendsFeed, 
            Action<IntPtr, string, string> onActionButtonClick, IntPtr onButtonClickPtr,
            Action<IntPtr> onOpenAction, IntPtr onOpenActionPtr,
            Action<IntPtr> onCloseAction, IntPtr onCloseActionPtr,
            Action<IntPtr, int> uiActionListener, IntPtr uiActionListenerPtr,
            Action<IntPtr, string> avatarClickListener, IntPtr avatarClickListenerPtr);

#endif
    }
}

#endif
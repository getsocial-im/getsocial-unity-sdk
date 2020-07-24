﻿using System;
using GetSocialSdk.Core;

#if UNITY_ANDROID
using UnityEngine;
#endif

#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

namespace GetSocialSdk.Ui
{
    /// <summary>
    /// Use this class to construct activity details view.
    /// Call <see cref="Show()"/> to present the UI.
    /// </summary>
    public sealed class ActivityDetailsViewBuilder : ViewBuilder<ActivityDetailsViewBuilder>
    {
#pragma warning disable 414
        readonly string _activityId;
        private string _commentId;

        private bool _showActivityFeedView;

        ActionListener _actionListener;
        Action<User> _onAvatarClickListener;
        Action<string> _onMentionClickListener;
        Action<string> _tagClickListener;
#pragma warning restore 414

        internal ActivityDetailsViewBuilder(string activityId)
        {
            _activityId = activityId;
            _showActivityFeedView = true;
        }

        public static ActivityDetailsViewBuilder Create(string activityId) 
        {
            return new ActivityDetailsViewBuilder(activityId);
        }

        /// <summary>
        /// By default is true. If you want to activity details without activity feed view in history,
        /// set it to false.
        /// </summary>
        /// <param name="showFeedView">true to have activity feed view in history, false otherwise</param>
        /// <returns><see cref="ActivityDetailsViewBuilder"/> instance</returns>
        public ActivityDetailsViewBuilder SetShowActivityFeedView(bool showFeedView)
        {
            _showActivityFeedView = showFeedView;

            return this;
        }


        /// <summary>
        /// Register a callback to listen to activity action button click events.
        /// </summary>
        /// <param name="listener">Called when action button is clicked</param>
        /// <returns><see cref="ActivityFeedViewBuilder"/> instance.</returns>
        public ActivityDetailsViewBuilder SetActionListener(ActionListener listener)
        {
            _actionListener = listener;

            return this;
        }

        /// <summary>
        /// Set a listener that will be called when user taps on someones avatar.
        /// </summary>
        /// <param name="onAvatarClickListener"></param>
        /// <returns><see cref="ActivityDetailsViewBuilder"/> instance.</returns>
        public ActivityDetailsViewBuilder SetAvatarClickListener(Action<User> onAvatarClickListener)
        {
            _onAvatarClickListener = onAvatarClickListener;

            return this;
        }

        /// <summary>
        /// Set a listener that will be called when user taps on mention in activity post.
        /// </summary>
        /// <param name="mentionClickListener">Called with ID of mentioned user.</param>
        /// <returns><see cref="ActivityDetailsViewBuilder"/> instance.</returns>
        public ActivityDetailsViewBuilder SetMentionClickListener(Action<string> mentionClickListener)
        {
            _onMentionClickListener = mentionClickListener;

            return this;
        }

        /// <summary>
        /// Set tag click listener, that will be notified if tag was clicked.
        /// </summary>
        /// <param name="tagClickListener">Called with name of tag that was clicked.</param>
        /// <returns><see cref="ActivityDetailsViewBuilder"/> instance.</returns>
        public ActivityDetailsViewBuilder SetTagClickListener(Action<string> tagClickListener)
        {
            _tagClickListener = tagClickListener;

            return this;
        }

        /// <summary>
        /// Set detailed comment which should be focused. Default is null.
        /// </summary>
        /// <param name="commentId">comment identifier</param>
        /// <returns><see cref="ActivityDetailsViewBuilder"/> instance.</returns>
        public ActivityDetailsViewBuilder SetCommentId(string commentId)
        {
            _commentId = commentId;
            return this;
        }

        internal override bool ShowInternal()
        {
#if UNITY_ANDROID
            return ShowBuilder(ToAJO());
#elif UNITY_IOS
            return _gs_showActivityDetailsView(_customWindowTitle, _activityId, _showActivityFeedView, _commentId,
                Callbacks.ActionCallback, _onOpen.GetPointer(),
                Callbacks.ActionCallback, _onClose.GetPointer(),
                UiActionListenerCallback.OnUiAction, _uiActionListener.GetPointer(),
                AvatarClickListenerCallback.OnAvatarClicked, _onAvatarClickListener.GetPointer(),
                MentionClickListenerCallback.OnMentionClicked, _onMentionClickListener.GetPointer(),
                TagClickListenerCallback.OnTagClicked, _tagClickListener.GetPointer(),
                ActionListenerCallback.OnAction, _actionListener.GetPointer());
#else
            return false;
#endif
        }

#if UNITY_ANDROID

        AndroidJavaObject ToAJO()
        {
            var activityFeedBuilderAJOClass = new AndroidJavaClass("im.getsocial.sdk.ui.communities.ActivityDetailsViewBuilder");
            var activityDetailsBuilderAJO = activityFeedBuilderAJOClass.CallStaticAJO("create", _activityId);

            activityDetailsBuilderAJO.CallAJO("setShowActivityFeedView", _showActivityFeedView);
            if (_actionListener != null)
            {
                activityDetailsBuilderAJO.CallAJO("setActionListener",
                    new ActionListenerProxy(_actionListener));
            }
            if (_onAvatarClickListener != null)
            {
                activityDetailsBuilderAJO.CallAJO("setAvatarClickListener",
                    new AvatarClickListenerProxy(_onAvatarClickListener));
            }
            if (_onMentionClickListener != null)
            {
                activityDetailsBuilderAJO.CallAJO("setMentionClickListener",
                    new MentionClickListenerProxy(_onMentionClickListener));
            }
            if (_tagClickListener != null) 
            {
                activityDetailsBuilderAJO.CallAJO("setTagClickListener",
                    new TagClickListenerProxy(_tagClickListener));   
            }
            if (_commentId != null)
            {
                activityDetailsBuilderAJO.CallAJO("setCommentId", _commentId);
            }
            return activityDetailsBuilderAJO;
        }

#elif UNITY_IOS

        [DllImport("__Internal")]
        static extern bool _gs_showActivityDetailsView(string customWindowTitle, string activityId, bool showFeedView, string commentId,
            Action<IntPtr> onOpenAction, IntPtr onOpenActionPtr,
            Action<IntPtr> onCloseAction, IntPtr onCloseActionPtr,
            Action<IntPtr, int> uiActionListener, IntPtr uiActionListenerPtr,
            Action<IntPtr, string> avatarClickListener, IntPtr avatarClickListenerPtr,
            Action<IntPtr, string> mentionClickListener, IntPtr mentionClickListenerPtr,
            Action<IntPtr, string> tagClickListener, IntPtr tagClickListenerPtr,
            ActionListenerDelegate actionListener, IntPtr actionListenerPtr);
#endif
    }
}
using System;
using GetSocialSdk.Core;

#if UNITY_IOS
using System.Runtime.InteropServices;
using System.Collections.Generic;
using GetSocialSdk.MiniJSON;
#endif

#if UNITY_ANDROID
using UnityEngine;
#endif

namespace GetSocialSdk.Ui
{
    /// <summary>
    /// Use this class to construct activity feed view.
    /// Call <see cref="Show()"/> to present the UI.
    /// </summary>
    public sealed class ActivityFeedViewBuilder : ViewBuilder<ActivityFeedViewBuilder>
    {
#pragma warning disable 414
        readonly ActivitiesQuery _query;
        Action<User> _onAvatarClickListener;
        Action<string> _onMentionClickListener;
        Action<string> _tagClickListener;
        ActionListener _actionListener;
        
#pragma warning restore 414
        
        public static ActivityFeedViewBuilder Create(ActivitiesQuery query)
        {
            return new ActivityFeedViewBuilder(query);
        }


        internal ActivityFeedViewBuilder(ActivitiesQuery query)
        {
            _query = query;
        }

        /// <summary>
        /// Register a callback to listen to activity action button click events.
        /// </summary>
        /// <param name="listener">Called when action button is clicked</param>
        /// <returns><see cref="ActivityFeedViewBuilder"/> instance.</returns>
        public ActivityFeedViewBuilder SetActionListener(ActionListener listener)
        {
            _actionListener = listener;
            
            return this;
        }

        /// <summary>
        /// Set a listener that will be called when user taps on someones avatar.
        /// </summary>
        /// <param name="onAvatarClickListener"></param>
        /// <returns><see cref="ActivityFeedViewBuilder"/> instance.</returns>
        public ActivityFeedViewBuilder SetAvatarClickListener(Action<User> onAvatarClickListener)
        {
            _onAvatarClickListener = onAvatarClickListener;

            return this;
        }

        /// <summary>
        /// Set a listener that will be called when user taps on mention in activity post.
        /// </summary>
        /// <param name="mentionClickListener">Called with ID of mentioned user or one of the shortcuts.
        /// <returns><see cref="ActivityFeedViewBuilder"/> instance.</returns>
        public ActivityFeedViewBuilder SetMentionClickListener(Action<string> mentionClickListener)
        {
            _onMentionClickListener = mentionClickListener;

            return this;
        }

        /// <summary>
        /// Set tag click listener, that will be notified if tag was clicked.
        /// </summary>
        /// <param name="tagClickListener">Called with name of tag that was clicked.</param>
        /// <returns><see cref="ActivityFeedViewBuilder"/> instance.</returns>
        public ActivityFeedViewBuilder SetTagClickListener(Action<string> tagClickListener) {
            _tagClickListener = tagClickListener;

            return this;
        }
        
        
        internal override bool ShowInternal()
        {
#if UNITY_ANDROID
            return ShowBuilder(ToAJO());
#elif UNITY_IOS
            return _gs_showActivityFeedView(_customWindowTitle, GSJson.Serialize(_query),
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
            var activityFeedBuilderAJOClass = new AndroidJavaClass("im.getsocial.sdk.ui.communities.ActivityFeedViewBuilder");
            var activityFeedBuilderAJO = activityFeedBuilderAJOClass.CallStaticAJO("create", AndroidAJOConverter.Convert(_query, "im.getsocial.sdk.communities.ActivitiesQuery"));

            if (_actionListener != null)
            {
                activityFeedBuilderAJO.CallAJO("setActionListener",
                    new ActionListenerProxy(_actionListener));
            }
            if (_onAvatarClickListener != null)
            {
                activityFeedBuilderAJO.CallAJO("setAvatarClickListener",
                    new AvatarClickListenerProxy(_onAvatarClickListener));
            }
            if (_onMentionClickListener != null)
            {
                activityFeedBuilderAJO.CallAJO("setMentionClickListener",
                    new MentionClickListenerProxy(_onMentionClickListener));
            }
            if (_tagClickListener != null) 
            {
                activityFeedBuilderAJO.CallAJO("setTagClickListener",
                    new TagClickListenerProxy(_tagClickListener));   
            }

            return activityFeedBuilderAJO;
        }

#elif UNITY_IOS

        [DllImport("__Internal")]
        static extern bool _gs_showActivityFeedView(string customWindowTitle, string query,
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

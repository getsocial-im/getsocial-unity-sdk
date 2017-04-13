#if UNITY_IOS
using System;
using AOT;

namespace GetSocialSdk.Core
{
    using System.Collections.Generic;

    public static class ActivityFeedCallbacks
    {
        [MonoPInvokeCallback(typeof(StringCallbackDelegate))]
        public static void OnActivityPostReceived(IntPtr actionPtr, string serializedPost)
        {
            GetSocialDebugLogger.D("OnActivityPostReceived: " + serializedPost);
            if (actionPtr != IntPtr.Zero)
            {
                var post = new ActivityPost().ParseFromJson(serializedPost);
                actionPtr.Cast<Action<ActivityPost>>().Invoke(post);
            }
            else
            {
                GetSocialDebugLogger.W("OnActivityPostReceived callback with zero pointer");
            }
        }

        [MonoPInvokeCallback(typeof(StringCallbackDelegate))]
        public static void OnActivityPostListReceived(IntPtr actionPtr, string serializedPostList)
        {
            GetSocialDebugLogger.D("OnActivityPostListReceived: " + serializedPostList);
            var posts = GSJsonUtils.ParseActivityPostsList(serializedPostList);

            if (actionPtr != IntPtr.Zero)
            {
                actionPtr.Cast<Action<List<ActivityPost>>>().Invoke(posts);
            }
            else
            {
                GetSocialDebugLogger.W("OnActivityPostListReceived callback with zero pointer");
            }
        }

        [MonoPInvokeCallback(typeof(StringCallbackDelegate))]
        public static void OnUsersListReceived(IntPtr actionPtr, string serializedList)
        {
            GetSocialDebugLogger.D("OnUsersListReceived: " + serializedList);
            var posts = GSJsonUtils.ParseUsersList(serializedList);

            if (actionPtr != IntPtr.Zero)
            {
                actionPtr.Cast<Action<List<PublicUser>>>().Invoke(posts);
            }
            else
            {
                GetSocialDebugLogger.W("OnUsersListReceived callback with zero pointer");
            }
        }
    }
}

#endif
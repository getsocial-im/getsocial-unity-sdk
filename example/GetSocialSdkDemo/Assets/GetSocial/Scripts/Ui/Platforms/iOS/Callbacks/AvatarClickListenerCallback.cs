#if UNITY_IOS 
using System;
using GetSocialSdk.Core;

namespace GetSocialSdk.Ui
{
    delegate void AvatarClickListenerDelegate(IntPtr avatarClickListenerPtr, string serializedUser);

    public static class AvatarClickListenerCallback
    {
        [AOT.MonoPInvokeCallback(typeof(AvatarClickListenerDelegate))]
        public static void OnAvatarClicked(IntPtr onAvatarClickedPtr, string serializedUser)
        {
            GetSocialDebugLogger.D(string.Format("OnAvatarClicked for user {0}", serializedUser));

            if (onAvatarClickedPtr != IntPtr.Zero)
            {
                var user = GetSocialJsonBridge.FromJson<User>(serializedUser);
                onAvatarClickedPtr.Cast<Action<User>>().Invoke(user);
            }
        }
    }
}
#endif

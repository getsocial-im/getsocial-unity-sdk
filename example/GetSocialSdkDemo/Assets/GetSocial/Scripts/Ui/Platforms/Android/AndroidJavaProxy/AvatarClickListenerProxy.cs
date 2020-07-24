#if UNITY_ANDROID 
using System;
using System.Diagnostics.CodeAnalysis;
using GetSocialSdk.Core;
using UnityEngine;

namespace GetSocialSdk.Ui
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class AvatarClickListenerProxy : JavaInterfaceProxy
    {
        readonly Action<User> _avatarClickListener;

        public AvatarClickListenerProxy(Action<User> avatarClickListener)
            : base("im.getsocial.sdk.ui.AvatarClickListener")
        {
            _avatarClickListener = avatarClickListener;
        }

        void onAvatarClicked(AndroidJavaObject publicUserAjo)
        {
            var publicUser = AndroidAJOConverter.Convert<User>(publicUserAjo);
            ExecuteOnMainThread(() => _avatarClickListener(publicUser));
        }
    }
}
#endif
#if UNITY_ANDROID 
using System;
using UnityEngine;
using System.Diagnostics.CodeAnalysis;
using GetSocialSdk.Core;

namespace GetSocialSdk.Ui
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class MentionClickListenerProxy : JavaInterfaceProxy
    {
        readonly Action<string> _onMentionClickListener;

        public MentionClickListenerProxy(Action<string> onMentionClickListener) : base("im.getsocial.sdk.ui.MentionClickListener")
        {
            _onMentionClickListener = onMentionClickListener;
        }

        void onMentionClicked(string mention)
        {
            ExecuteOnMainThread(() => _onMentionClickListener(mention));
        }
    }
}
#endif
#if UNITY_ANDROID 
using System;
using System.Diagnostics.CodeAnalysis;
using GetSocialSdk.Core;
using UnityEngine;

namespace GetSocialSdk.Ui
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class ActionListenerProxy : JavaInterfaceProxy
    {
        readonly ActionListener _internal;

        public ActionListenerProxy(ActionListener listener)
            : base("im.getsocial.sdk.actions.ActionListener")
        {
            _internal = listener;
        }

        void handleAction(AndroidJavaObject ajoAction)
        {
            var action = AndroidAJOConverter.Convert<GetSocialAction>(ajoAction);
            ExecuteOnMainThread(() => _internal(action));
        }
    }
}

#endif
#if UNITY_ANDROID
using UnityEngine;
using System.Diagnostics.CodeAnalysis;

namespace GetSocialSdk.Core
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    internal class ActionListenerProxy : JavaInterfaceProxy
    {
        readonly ActionListener _actionListener;

        public ActionListenerProxy(ActionListener actionListener)
            : base("im.getsocial.sdk.actions.ActionListener")
        {
            _actionListener = actionListener;
        }

        bool handleAction(AndroidJavaObject ajo)
        {
            return _actionListener != null && _actionListener(new GetSocialAction().ParseFromAJO(ajo));
        }
    }
}
#endif
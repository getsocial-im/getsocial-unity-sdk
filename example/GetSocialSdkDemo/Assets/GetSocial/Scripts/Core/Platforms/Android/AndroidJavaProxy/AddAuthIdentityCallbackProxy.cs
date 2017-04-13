#if UNITY_ANDROID
using UnityEngine;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GetSocialSdk.Core
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    class AddAuthIdentityCallbackProxy : JavaInterfaceProxy
    {
        readonly Action _onComplete;
        readonly Action<GetSocialError> _onFailure;
        readonly Action<ConflictUser> _onConflict;

        public AddAuthIdentityCallbackProxy(Action onComplete, Action<GetSocialError> onFailure, Action<ConflictUser> onConflict)
            : base("im.getsocial.sdk.usermanagement.AddAuthIdentityCallback")
        {
            _onComplete = onComplete;
            _onFailure = onFailure;
            _onConflict = onConflict;
        }

        void onComplete()
        {
            GetSocialDebugLogger.D("AddAuthIdentityCallbackProxy onComplete");
            ExecuteOnMainThread(_onComplete);
        }

        void onFailure(AndroidJavaObject throwableAJO)
        {
            var ex = throwableAJO.ToGetSocialError();
            GetSocialDebugLogger.D("AddAuthIdentityCallbackProxy onFailure: " + ex);

            ExecuteOnMainThread(() => _onFailure(ex));
        }

        void onConflict(AndroidJavaObject conflictUserAJO)
        {
            GetSocialDebugLogger.D("AddAuthIdentityCallbackProxy onConflict");

            var conflictUser = new ConflictUser().ParseFromAJO(conflictUserAJO);
            ExecuteOnMainThread(() => _onConflict(conflictUser));
        }
    }
}

#endif
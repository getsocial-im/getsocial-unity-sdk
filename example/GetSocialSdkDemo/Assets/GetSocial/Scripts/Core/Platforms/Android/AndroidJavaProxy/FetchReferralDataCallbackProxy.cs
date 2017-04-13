#if UNITY_ANDROID
using System;
using UnityEngine;
using System.Diagnostics.CodeAnalysis;

namespace GetSocialSdk.Core
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    class FetchReferralDataCallbackProxy : JavaInterfaceProxy
    {
        private readonly Action<ReferralData> _onSuccess;
        private readonly Action<GetSocialError> _onFailure;

        public FetchReferralDataCallbackProxy(Action<ReferralData> onSuccess, Action<GetSocialError> onFailure)
            : base("im.getsocial.sdk.invites.FetchReferralDataCallback")
        {
            _onSuccess = onSuccess;
            _onFailure = onFailure;
        }

        void onSuccess(AndroidJavaObject referralDataAJO)
        {
            var referralData = new ReferralData().ParseFromAJO(referralDataAJO);

            GetSocialDebugLogger.D("On success: " + referralData);

            ExecuteOnMainThread(() => _onSuccess(referralData));
        }

        void onFailure(AndroidJavaObject throwable)
        {
            var e = throwable.ToGetSocialError();

            GetSocialDebugLogger.D("On onFailure: " + e.Message);

            ExecuteOnMainThread(() => _onFailure(e));
        }
    }
}

#endif
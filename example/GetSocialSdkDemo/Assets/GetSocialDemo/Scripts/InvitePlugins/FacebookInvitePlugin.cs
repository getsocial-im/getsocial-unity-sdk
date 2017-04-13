/**
 *     Copyright 2015-2016 GetSocial B.V.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using GetSocialSdk.Core;
using UnityEngine;
using Facebook.Unity;

#if USE_GETSOCIAL_UI
using GetSocialSdk.Ui;
#endif

public class FacebookInvitePlugin : InviteChannelPlugin
{
    #region IInvitePlugin implementation

    public bool IsAvailableForDevice(InviteChannel inviteChannel)
    {
        return true;
    }

    public void PresentChannelInterface(InviteChannel inviteChannel, InvitePackage invitePackage,
                                         Action onComplete, Action onCancel, Action<GetSocialError> onFailure)
    {
        GetSocialDebugLogger.D(string.Format("FacebookInvitePlugin.PresentChannelInterface(), inviteChannel: {0}, invite package: {1}",
                inviteChannel, invitePackage));

#if UNITY_ANDROID && USE_GETSOCIAL_UI
        // Get Social UI needs to be closed while Facebook activity is opened
        // because othewise it cannot deliever the result back to the app
        GetSocialUi.CloseView(true);
#endif
        SendInvite(invitePackage.ReferralDataUrl, onComplete, onCancel, onFailure);
    }

    #endregion

    static void SendInvite(string referralDataUrl,
                           Action completeCallback,
                           Action cancelCallback,
                           Action<GetSocialError> errorCallback)
    {
        GetSocialDebugLogger.D("Sending Facebook invite with URL: " + referralDataUrl);
        FB.Mobile.AppInvite(new Uri(referralDataUrl), null,
            result =>
            {
#if UNITY_ANDROID && USE_GETSOCIAL_UI
                // Restore the hidden view after we have received the result
                GetSocialUi.RestoreView();
#endif

                GetSocialDebugLogger.D("Sending invite finished: " + result);
                if (result.Cancelled)
                {
                    cancelCallback();
                    return;
                }
                if (!string.IsNullOrEmpty(result.Error))
                {
                    var errorMsg = "Failed sending app invite: " + result.Error;
                    Debug.LogError(errorMsg);
                    errorCallback(new GetSocialError(errorMsg));
                    return;
                }

                completeCallback();
            });
    }
}
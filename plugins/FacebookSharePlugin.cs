/**
 *     Copyright 2015-2018 GetSocial B.V.
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
#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

#if USE_GETSOCIAL_UI
using GetSocialSdk.Ui;
#endif

public class FacebookSharePlugin : InviteChannelPlugin
{
    #region IInvitePlugin implementation

    public bool IsAvailableForDevice(InviteChannel inviteChannel)
    {
        return true;
    }

    public void PresentChannelInterface(InviteChannel inviteChannel, InvitePackage invitePackage,
                                         Action onComplete, Action onCancel, Action<GetSocialError> onFailure)
    {
        GetSocialDebugLogger.D(string.Format("FacebookSharePlugin.PresentChannelInterface(), inviteChannel: {0}, invite package: {1}",
                inviteChannel, invitePackage));

#if USE_GETSOCIAL_UI
        // GetSocialUi needs to be closed while Facebook activity is opened
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
        GetSocialDebugLogger.D("Sharing link on Facebook : " + referralDataUrl);
        FB.Mobile.ShareDialogMode = IsFBAppInstalled() ? ShareDialogMode.NATIVE : ShareDialogMode.WEB; 
        FB.ShareLink(new Uri(referralDataUrl), callback: result => 
        {

#if USE_GETSOCIAL_UI
            // reopen GetSocialUi
            // because othewise it cannot deliever the result back to the app
            GetSocialUi.RestoreView();
#endif
            GetSocialDebugLogger.D("Sharing link finished: " + result);
            if (result.Cancelled)
            {
                cancelCallback();
                return;
            }
            if (!string.IsNullOrEmpty(result.Error))
            {
                var errorMsg = "Failed to share link: " + result.Error;
                Debug.LogError(errorMsg);
                errorCallback(new GetSocialError(errorMsg));
                return;
            }

            completeCallback();
        });
    }

    private static bool IsFBAppInstalled()
    {
#if UNITY_ANDROID
        var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        var packageManager = currentActivity.Call<AndroidJavaObject>("getPackageManager");
        try
        {
            var arguments = new object[]{"com.facebook.katana", 1};
            packageManager.Call<AndroidJavaObject>("getPackageInfo", arguments);
            return true;
        }
        catch (Exception exception)
        {
            GetSocialDebugLogger.D("Facebook app is not installed, open web view, exception: " + exception.Message);
            return false;
        }
#elif UNITY_IOS
        return _gs_checkIfFBAppInstalled();
#else
        return false;
#endif
    }
    
#if UNITY_IOS
        [DllImport("__Internal")]
        static extern bool _gs_checkIfFBAppInstalled();
#endif
    
}
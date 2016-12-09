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
using System.Collections.Generic;
using System;
using GetSocialSdk.Core;
using Facebook.MiniJSON;
using UnityEngine;

public class FacebookInvitePlugin : IInvitePlugin
{
    #region IInvitePlugin implementation
    public bool IsAvailableForDevice()
    {
        return true;
    }

    public void InviteFriends(string subject, string text, string referralDataUrl, byte[] image, Action<string, List<string>> completeCallback, Action cancelCallback, Action<Exception> errorCallback)
    {
        if(FB.IsLoggedIn)
        {
            SendInvite(completeCallback, cancelCallback, errorCallback);
        }
        else
        {
            #if UNITY_ANDROID
            // NOTE: Due to a bug with the facebook plugin for Unity4x for the Android platform we need to:
            // 1) close the window
            // 2) log in to Facebook
            // 3) reopen the view
            // 4) send the invite

            // close the Smart Invites window
            GetSocial.Instance.CloseView(true);
            #endif

            FB.Login(AuthSection.FacebookPermissions, result =>
            {
                #if UNITY_ANDROID
                // reopen the Smart Invites window
                GetSocial.Instance.RestoreView();
                #endif

                if(FB.IsLoggedIn)
                {
                    SendInvite(completeCallback, cancelCallback, errorCallback);
                }
                else
                {
                    // The auth failed or the user cancelled the login
                }
            });
        }
    }
    #endregion

    private void SendInvite(Action<string, List<string>> completeCallback, Action cancelCallback, Action<Exception> errorCallback)
    {
#if UNITY_IOS
        // HACK: App requests are shown below GetSocial view
        GetSocial.Instance.CloseView(true);
#endif
        
        FB.AppRequest(
            message: "This text is never displayed",
            callback: (result) => OnAppRequestFinished(result, completeCallback, cancelCallback, errorCallback));
    }


    #region private methods
    private void OnAppRequestFinished(FBResult result, Action<string, List<string>> completeCallback, Action cancelCallback, Action<Exception> errorCallback)
    {
        if(result != null)
        {
            var responseObject = Json.Deserialize(result.Text) as Dictionary<string, object>;

            if(responseObject.ContainsKey("to") && responseObject.ContainsKey("request"))
            {
                var objectArray = responseObject["to"] as List<object>;
                var requestId = responseObject["request"] as string;

                if(objectArray == null || objectArray.Count == 0)
                {
                    cancelCallback();
                }
                else
                {
                    completeCallback(requestId, objectArray.ConvertAll<string>(val => val.ToString()));
                }
            }
            else
            {
                cancelCallback();
            }
        }
        else
        {
            cancelCallback();
        }
#if UNITY_IOS
        // HACK: App requests are shown below GetSocial view
        GetSocial.Instance.RestoreView();
#endif
    }
    #endregion
}

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

#if UNITY_ANDROID
using System.Collections.Generic;
using System;
using GetSocialSdk.Core;
using UnityEngine;

public class FacebookMessengerInvitePlugin : IInvitePlugin
{
    public const string ProviderName = "facebookmessenger";


    #region IInvitePlugin implementation
    public bool IsAvailableForDevice()
    {
        try
        {
            using(AndroidJavaObject packageManager = AndroidUtils.Activity.Call<AndroidJavaObject>("getPackageManager"))
            {
                using(AndroidJavaObject result = packageManager.Call<AndroidJavaObject>("getPackageInfo", "com.facebook.orca", 0))
                {
                }
            }
            return true;
        }
        catch(Exception e)
        {
#if DEVELOPMENT_BUILD
            Debug.LogWarning("There was an error checking if Facebook Messenger is available for device :" + e.Message);
#endif
        }
        return false;
    }

    public void InviteFriends(string subject, string text, string referralDataUrl, byte[] image, 
                              Action<string, List<string>> completeCallback, 
                              Action cancelCallback, 
                              Action<Exception> errorCallback)
    {
        try
        {
            using(AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent"))
            {
                using(AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent"))
                {
                    intentObject.Call<AndroidJavaObject>("setType", "text/plain");
                    intentObject.Call<AndroidJavaObject>("setPackage", "com.facebook.orca");
                    intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), text);
                    intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));

                    AndroidUtils.Activity.Call("startActivity", intentObject);
                    completeCallback(null, null);
                }
            }
        }
        catch(Exception e)
        {
            errorCallback(e);
        }
    }
    #endregion
}
#endif
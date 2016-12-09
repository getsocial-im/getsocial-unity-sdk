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

#if UNITY_IOS
using System;

namespace GetSocialSdk.Core
{
    public class UnityInvitePluginProxy
    {
        public delegate void InviteFriendsDelegate(IntPtr instancePtr, string subject, string text, string referralDataUrl, string base64image, IntPtr onSuccessPtr, IntPtr onCancelPtr, IntPtr onFailure);

        [MonoPInvokeCallback(typeof(InviteFriendsDelegate))]
        public static void InviteFriends(IntPtr instancePtr, string subject, string text, string referralDataUrl, string base64image, IntPtr onSuccessPtr, IntPtr onCancelPtr, IntPtr onFailurePtr)
        {
            var instance = instancePtr.Cast<IInvitePlugin>();
            var image = base64image != null ? Convert.FromBase64String(base64image) : null;
            
            instance.InviteFriends(
                subject: subject,
                text: text,
                referralDataUrl : referralDataUrl,
                image: image,
                onSuccess: (requestId, invitedFriends) => {
                    int invitedFriendsCount = invitedFriends != null ? invitedFriends.Count : 0;
                    string[] invitedFriendsArray = invitedFriends != null ? invitedFriends.ToArray() : null;
                    GetSocialNativeBridgeIOS._executeInvitedFriendsCallback(onSuccessPtr, requestId, invitedFriendsCount, invitedFriendsArray);
                },   
                onCancel: () => GetSocialNativeBridgeIOS._executeCompleteCallback(onCancelPtr),
                onFailure: (exception) => GetSocialNativeBridgeIOS._executeErrorCallback(onFailurePtr, exception.Message)
            );
        }
    }
}
#endif

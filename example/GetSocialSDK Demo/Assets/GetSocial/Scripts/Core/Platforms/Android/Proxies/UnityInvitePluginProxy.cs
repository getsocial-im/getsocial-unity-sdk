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
using UnityEngine;
using System;
using System.Collections.Generic;

namespace GetSocialSdk.Core
{
    class UnityInvitePluginProxy : JavaInterfaceProxy
    {
        private readonly IInvitePlugin invitePlugin;
  
        internal UnityInvitePluginProxy(IInvitePlugin invitePlugin) : base("im.getsocial.sdk.core.unity.proxy.UnityInvitePlugin")
        {
            this.invitePlugin = invitePlugin;
        }

        bool isAvailableForDevice(AndroidJavaObject context)
        {
            return invitePlugin.IsAvailableForDevice();
        }

        void inviteFriends(AndroidJavaObject context, string subject, string text, string referralDataUrl, string base64image, AndroidJavaObject callback)
        {
            MainThreadExecutor.Queue(() => {
                invitePlugin.InviteFriends(
                               subject: subject, 
                               text: text,
                               image: !string.IsNullOrEmpty(base64image) ? Convert.FromBase64String(base64image) : null,
                               referralDataUrl: referralDataUrl,
                               onSuccess: (requestId, to) => OnComplete(callback, requestId, to),
                               onCancel: () => OnCancel(callback),
                               onFailure: (exception) => OnError(callback, exception));
            });
        }

        private void OnComplete(AndroidJavaObject callback, string requestId, IList<string> to)
        {
            if(!callback.IsJavaNull())
            {
                callback.Call("onComplete", requestId, AndroidUtils.ConvertToArrayList(to));
            }
        }

        private void OnCancel(AndroidJavaObject callback)
        {
            if(!callback.IsJavaNull())
            {
                callback.Call("onCancel");
            }
        }

        private void OnError(AndroidJavaObject callback, Exception exception)
        {
            if(!callback.IsJavaNull())
            {
                int errorCode = exception is GetSocialAPIException ? (int)((GetSocialAPIException)exception).ErrorCode : 0;
                AndroidJavaObject javaException = new AndroidJavaObject("im.getsocial.sdk.core.GetSocialApiException", exception.Message, errorCode);
                callback.Call("onError", javaException);
            }
        }
    }
}
#endif

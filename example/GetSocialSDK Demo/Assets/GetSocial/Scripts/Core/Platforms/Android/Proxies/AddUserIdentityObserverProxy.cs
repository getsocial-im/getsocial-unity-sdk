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

namespace GetSocialSdk.Core
{
    internal sealed class AddUserIdentityObserverProxy : JavaInterfaceProxy
    {
        private readonly Action<CurrentUser.AddIdentityResult> onCompleteAction;
        private readonly Action<string> onFailureAction;
        private readonly CurrentUser.OnAddIdentityConflictDelegate onConflictDelegate;

        internal AddUserIdentityObserverProxy(Action<CurrentUser.AddIdentityResult> onComplete, Action<string> onFailure, CurrentUser.OnAddIdentityConflictDelegate onConflict)
            : base("im.getsocial.sdk.core.unity.proxy.AddUserIdentityObserverProxy")
        {
            this.onCompleteAction = onComplete;
            this.onFailureAction = onFailure;
            this.onConflictDelegate = onConflict;
        }

        void onComplete(int result)
        {
            MainThreadExecutor.Queue(() => onCompleteAction((CurrentUser.AddIdentityResult) result));
        }

        void onConflict(string localUser, string remoteUser, AndroidJavaObject userIdentityResolverProxy)
        {
            var localUserObject = new User(new JSONObject(localUser));
            var remoteUserObject = new User(new JSONObject(remoteUser));
            
            if(onConflictDelegate == null)
            {
                userIdentityResolverProxy.Call("resolve", (int) CurrentUser.AddIdentityConflictResolutionStrategy.Remote);
            }
            else
            {
                onConflictDelegate(localUserObject, remoteUserObject, strategy => userIdentityResolverProxy.Call("resolve", (int)strategy));
            }
        }

        void onError(AndroidJavaObject javaException)
        {
            if(onFailureAction != null)
            {
                var message = javaException.IsJavaNull() ? string.Empty : javaException.Call<String>("getMessage");
                var ex = new Exception(message);
                MainThreadExecutor.Queue(() => onFailureAction(ex.Message));
            }
        }
    }
}
#endif

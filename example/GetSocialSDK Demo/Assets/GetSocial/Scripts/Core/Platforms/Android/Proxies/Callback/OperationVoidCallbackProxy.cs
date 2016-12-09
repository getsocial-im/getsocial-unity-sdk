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
using System;
using UnityEngine;

namespace GetSocialSdk.Core
{
    class OperationVoidCallbackProxy : JavaInterfaceProxy
    {
        private readonly Action onSuccessAction;
        private readonly Action<string> onFailureAction;

        internal OperationVoidCallbackProxy(Action onSuccess, Action<string> onFailure)
            : base("im.getsocial.sdk.core.callback.OperationVoidCallback")
        {
            this.onSuccessAction = onSuccess;
            this.onFailureAction = onFailure;
        }

        public void onSuccess()
        {
#if DEVELOPMENT_BUILD
            Debug.Log("onSuccess");
#endif
            MainThreadExecutor.Queue(onSuccessAction);
        }

        private void onFailure(AndroidJavaObject errorAJO)
        {
#if DEVELOPMENT_BUILD
            Debug.Log("onFailure: " + errorAJO);
#endif
            string errorMessage = errorAJO.Call<string>("getMessage") ?? string.Empty;
            MainThreadExecutor.Queue(() => onFailureAction(errorMessage));
        }
    }
}
#endif

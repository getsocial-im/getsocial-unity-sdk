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
    class OperationCallbackProxy<TResult> : JavaInterfaceProxy where TResult : class
    {
        private readonly Action<TResult> onSuccessAction;
        private readonly Action<string> onFailureAction;
        private readonly Func<string, TResult> convertFunc;

        internal OperationCallbackProxy(Action<TResult> onSuccess, Action<string> onFailure, Func<string, TResult> convertFunc)
            : base("im.getsocial.sdk.core.unity.proxy.callback.OperationStringCallbackProxy")
        {
            this.onSuccessAction = onSuccess;
            this.onFailureAction = onFailure;
            this.convertFunc = convertFunc;
        }

        public void onSuccess(string result)
        {
#if DEVELOPMENT_BUILD
            Debug.Log("onSuccess: " + result);
#endif
            TResult deserializedResult = convertFunc(result);
            MainThreadExecutor.Queue(() => onSuccessAction(deserializedResult));
        }

        public void onFailure(string error)
        {
#if DEVELOPMENT_BUILD
            Debug.Log("onFailure: " + error);
#endif
            MainThreadExecutor.Queue(() => onFailureAction(error));
        }

        // additional callbacks for an Unity JNI issue
        protected void onSuccess(AndroidJavaObject nullRef)
        {
            MainThreadExecutor.Queue(() => onSuccessAction(default(TResult)));
        }
        
        protected void onFailure(AndroidJavaObject nullRef)
        {
            MainThreadExecutor.Queue(() => onFailureAction(string.Empty));
        }
    }
}
#endif

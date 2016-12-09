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
    class OperationGenericCallbackProxy<TResult> : JavaInterfaceProxy where TResult : class
    {
        private readonly Action<TResult> onSuccessAction;
        private readonly Action<string> onFailureAction;
        private readonly Func<AndroidJavaObject, TResult> convertFunc;
        
        internal OperationGenericCallbackProxy(Action<TResult> onSuccess, 
                                               Action<string> onFailure, 
                                               Func<AndroidJavaObject, TResult> convertFunc)
            : base("im.getsocial.sdk.core.callback.OperationCallback")
        {
            this.onSuccessAction = onSuccess;
            this.onFailureAction = onFailure;
            this.convertFunc = convertFunc;
        }
        
        public void onSuccess(AndroidJavaObject result)
        {
#if DEVELOPMENT_BUILD
            Debug.Log("onSuccess: " + result.ToJavaString());
#endif
            TResult deserializedResult = convertFunc(result);
            MainThreadExecutor.Queue(() => onSuccessAction(deserializedResult));
        }
        
        public void onFailure(AndroidJavaObject exception)
        {
            var errorMessage = exception.Call<String>("getMessage");
#if DEVELOPMENT_BUILD
            Debug.Log("onFailure: " + errorMessage);
#endif
            MainThreadExecutor.Queue(() => onFailureAction(errorMessage));
        }
    }
}
#endif

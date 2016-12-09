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
    class SubmitLeaderboardScoreListenerProxy : JavaInterfaceProxy
    {
        private readonly Action<int> onSuccessAction;
        private readonly Action onFailureAction;

        internal SubmitLeaderboardScoreListenerProxy(Action<int> onSuccess, Action onFailure)
            : base("im.getsocial.sdk.core.unity.proxy.SubmitLeaderboardScoreListener")
        {
            this.onSuccessAction = onSuccess;
            this.onFailureAction = onFailure;
        }

        void onSuccess(int newRank)
        {
            MainThreadExecutor.Queue(() => onSuccessAction(newRank));
        }

        void onFailure(AndroidJavaObject exception)
        {
            MainThreadExecutor.Queue(onFailureAction);
        }
    }
}
#endif

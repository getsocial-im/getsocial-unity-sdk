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

namespace GetSocialSdk.Core
{
    class OnReferralDataReceivedHandlerProxy : JavaInterfaceProxy
    {
        private readonly OnReferralDataReceived onReferralDataReceivedAction;

        internal OnReferralDataReceivedHandlerProxy(OnReferralDataReceived onReferralDataReceivedAction) : base("im.getsocial.sdk.core.unity.proxy.OnReferralDataReceivedListener")
        {
            this.onReferralDataReceivedAction = onReferralDataReceivedAction;
        }

        void onReferralDataReceived(string referralDataJSONString)
        {
            if(onReferralDataReceivedAction != null)
            {
                var referralDataList = new List<Dictionary<string,string>>();

                JSONObject referralDataJSON = new JSONObject(referralDataJSONString);

                foreach(JSONObject referralDataJSONDictionary in referralDataJSON.list)
                {
                    referralDataList.Add(referralDataJSONDictionary.ToDictionary());
                }

                onReferralDataReceivedAction(referralDataList);
            }
        }
    }
}
#endif

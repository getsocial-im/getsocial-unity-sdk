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
    internal static class OnViewActionProxy
    {
        public delegate void ExecuteViewActionDelegate(IntPtr actionPtr, string actionName, string serializedData);

        [MonoPInvokeCallback(typeof(ExecuteViewActionDelegate))]
        public static void OnViewBuilderActionCallback(IntPtr actionPtr, string actionName, string serializedData)
        {
            if(actionPtr != IntPtr.Zero)
            {
                var action = actionPtr.Cast<ViewBuilder.OnViewActionDelegate>();
                action(actionName, serializedData);
            }
        }
    }
}
#endif

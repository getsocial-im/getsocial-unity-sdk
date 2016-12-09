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
    public static class OnAddUserIdentityConflictProxy
    {
        public delegate void OnAddUserIdentityConflictDelegate(IntPtr actionPointer, 
                                                               string currentUserSerialized, 
                                                               string remoteUserSerialized, 
                                                               IntPtr resolveConflictCallbackPtr);

        [MonoPInvokeCallback(typeof(OnAddUserIdentityConflictDelegate))]
        public static void OnUserIdentityConflict(IntPtr actionPointer,
                                                  string currentUserSerialized,
                                                  string remoteUserSerialized,
                                                  IntPtr resolveConflictCallbackPtr)
        {
            var currentUser = new User(new JSONObject(currentUserSerialized));
            var remoteUser = new User(new JSONObject(remoteUserSerialized));

            var action = actionPointer.Cast<CurrentUser.OnAddIdentityConflictDelegate>();

            action(currentUser, remoteUser, resolutionStrategy =>
            GetSocialNativeBridgeIOS._executeAddUserIndentityConflictResolver(resolveConflictCallbackPtr, (int)resolutionStrategy));
        }
    }
}
#endif

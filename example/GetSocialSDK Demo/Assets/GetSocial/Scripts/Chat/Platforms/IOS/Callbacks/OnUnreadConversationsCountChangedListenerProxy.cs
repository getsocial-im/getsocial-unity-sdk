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

#if ENABLE_GETSOCIAL_CHAT
#if UNITY_IOS
using System;
using GetSocialSdk.Core;

namespace GetSocialSdk.Chat
{
    internal static class OnUnreadConversationsCountChangedListenerProxy
    {
        public delegate void OnUnreadConversationsCountChangedListenerDelegate(IntPtr actionPtr, int unreadConversationsCount);

        [MonoPInvokeCallback(typeof(OnUnreadConversationsCountChangedListenerDelegate))]
        public static void OnUnreadConversationsCountChange(IntPtr actionPtr, int unreadConversationsCount)
        {
            if(actionPtr != IntPtr.Zero)
            {
                var action = actionPtr.Cast<Action<int>>();
                action(unreadConversationsCount);
            }
        }
    }
}
#endif
#endif

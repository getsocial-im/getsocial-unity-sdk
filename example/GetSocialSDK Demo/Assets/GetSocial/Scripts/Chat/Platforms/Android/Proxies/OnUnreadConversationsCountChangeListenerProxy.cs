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
#if UNITY_ANDROID
using System;
using GetSocialSdk.Core;

namespace GetSocialSdk.Chat
{
    internal class OnUnreadConversationsCountChangeListenerProxy : JavaInterfaceProxy
    {
        private Action<int> onUnreadConversationsCountChangeAction;

        internal OnUnreadConversationsCountChangeListenerProxy(Action<int> onUnreadConversationsCountChange) : base("im.getsocial.sdk.chat.GetSocialChat$OnUnreadConversationsCountChangedListener")
        {
            this.onUnreadConversationsCountChangeAction = onUnreadConversationsCountChange;
        }

        void onUnreadConversationsCountChanged(int unreadConversationsCount)
        {
            if(onUnreadConversationsCountChangeAction != null)
            {
                MainThreadExecutor.Queue(
                    () => onUnreadConversationsCountChangeAction(unreadConversationsCount)
                );
            }
        }
    }
}
#endif
#endif

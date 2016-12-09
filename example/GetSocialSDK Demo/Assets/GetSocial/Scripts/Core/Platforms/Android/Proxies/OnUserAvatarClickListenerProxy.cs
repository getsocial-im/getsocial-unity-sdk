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

namespace GetSocialSdk.Core
{
    class OnUserAvatarClickListenerProxy : JavaInterfaceProxy
    {
        private readonly OnUserAvatarClick onUserAvatarClickAction;

        internal OnUserAvatarClickListenerProxy(OnUserAvatarClick onUserAvatarClick) : base("im.getsocial.sdk.core.unity.proxy.OnUserAvatarClickListener")
        {
            this.onUserAvatarClickAction = onUserAvatarClick;
        }

        bool onUserAvatarClick(string userIdentitySerialized, int source)
        {
            if(onUserAvatarClickAction != null)
            {
                var user = new User(new JSONObject(userIdentitySerialized));
                var sourceEnum = (GetSocial.SourceView)source;
                return onUserAvatarClickAction(user, sourceEnum);
            }
            return false;
        }
    }
}
#endif

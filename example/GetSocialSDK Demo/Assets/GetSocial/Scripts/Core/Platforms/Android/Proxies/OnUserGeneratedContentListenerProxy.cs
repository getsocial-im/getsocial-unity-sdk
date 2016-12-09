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
using UnityEngine;

namespace GetSocialSdk.Core
{
    internal class OnUserGeneratedContentListenerProxy : JavaInterfaceProxy
    {
        private readonly OnUserGeneratedContent onUserGeneratedContentDelegate;

        internal OnUserGeneratedContentListenerProxy(OnUserGeneratedContent onUserGeneratedContent) : base("im.getsocial.sdk.core.unity.proxy.OnUserGeneratedContentListenerProxy")
        {
            this.onUserGeneratedContentDelegate = onUserGeneratedContent;
        }

        // callback to handle null content
        string onUserGeneratedContent(int type, AndroidJavaObject content)
        {
            // when content is null - don't post it
            return null;
        }

        string onUserGeneratedContent(int type, string content)
        {
            return HandleOnUserGeneratedContent(type, content);
        }

        private string HandleOnUserGeneratedContent(int source, string content)
        {
            if(onUserGeneratedContentDelegate != null)
            {
                return onUserGeneratedContentDelegate((GetSocial.ContentSource)source, content);
            }
            return content;
        }
    }
}
#endif

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
namespace GetSocialSdk.Chat
{
    static class GetSocialChatFactory
    {
        internal static IGetSocialChatNativeBridge InstantiateGetSocialChat()
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            return GetSocialChatAndroid.GetInstance();
            #elif UNITY_IOS && !UNITY_EDITOR
            return GetSocialChatIOS.GetInstance();
            #else
            // if UNITY_EDITOR
            return GetSocialChatEditorMock.GetInstance();
            #endif
        }
    }
}
#endif

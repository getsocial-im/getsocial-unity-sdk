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
    /// <summary>
    /// Typing status of the user.
    /// </summary>
    public enum TypingStatus
    {
        /// <summary>
        /// The user is typing.
        /// </summary>
        Typing = 1,
        /// <summary>
        /// The user is not typing.
        /// </summary>
        NotTyping = 2,
        /// <summary>
        /// Typing status is unknown.
        /// </summary>
        Unknown = 1000
    }
}
#endif

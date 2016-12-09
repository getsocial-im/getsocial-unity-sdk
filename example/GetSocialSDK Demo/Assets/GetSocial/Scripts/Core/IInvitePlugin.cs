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

using System;
using System.Collections.Generic;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// This interface provides the invite friends functionality used by the GetSocial SDK.
    /// </summary>
    public interface IInvitePlugin : IPlugin
    {
        /// <summary>
        /// When this method is called, the plugin can assume:
        ///     - the GetSocial is initialized
        ///     - no other invite plugin is active.
        /// 
        /// The plugin MUST NOT assume:
        ///     - handlers listing for the state change from unidentified to verifying have completed.
        /// 
        /// InviteFriends MUST guarantee that exactly one of the callbacks is eventually called. (i.e. either onSuccess, onCancel or onFailure)
        /// </summary>
        void InviteFriends(string subject, string text, string referralDataUrl, byte[] image, Action<string, List<string>> onSuccess, Action onCancel, Action<Exception> onFailure);
    } 
}

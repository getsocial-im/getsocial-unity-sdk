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

using UnityEngine;

namespace GetSocialSdk.Core
{
    static class Log
    {
        public static void D(string message, params object[] arguments)
        {
            if(GetSocialSettings.IsDebugLogsEnabled)
            {
                if(arguments != null && arguments.Length > 0)
                {
                    Debug.Log(string.Format(message, arguments));
                }
                else
                {
                    Debug.Log(message);
                }
            }
        }

        public static void I(string message, params object[] arguments)
        {
            if(arguments != null && arguments.Length > 0)
            {
                Debug.Log(string.Format(message, arguments));
            }
            else
            {
                Debug.Log(message);
            }
        }

        public static void W(string message, params object[] arguments)
        {
            if(arguments != null && arguments.Length > 0)
            {
                Debug.LogWarning(string.Format(message, arguments));
            }
            else
            {
                Debug.LogWarning(message);
            }
        }

        public static void E(string message, params object[] arguments)
        {
            if(arguments != null && arguments.Length > 0)
            {
                Debug.LogError(string.Format(message, arguments));
            }
            else
            {
                Debug.LogError(message);
            }
        }
    }
}


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
    internal class JavaInterfaceProxy : AndroidJavaProxy
    {
        private static JavaInterfaceProxy comparingWho;

        internal JavaInterfaceProxy(string javaInterface) : base(javaInterface) {}

        /// <summary>
        /// Unity forwards all calls from Java to C# including call to equals(), so we have to override it.
        /// More about issue: http://forum.unity3d.com/threads/androidjavaproxy-equals.243438/
        /// </summary>
        protected bool equals(AndroidJavaObject other)
        {
            bool result = false;
            
            if(comparingWho != null)
            {
                result = comparingWho == this;
                comparingWho = null;
            }
            else
            {
                comparingWho = this;
                result = other.Call<bool>("equals", other);
            }
            
            return result;
        }

        protected string toString()
        {
            return GetType().Name;
        }
    }
}
#endif

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

using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace GetSocialSdk.Core
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct NativeReadOnlyDictionary
    {
        public readonly int Count;
        public readonly string[] Keys;
        public readonly string[] Values;

        public NativeReadOnlyDictionary(IDictionary<string,string> dict)
        {
            this.Count = dict != null ? dict.Count : 0;
            this.Keys = dict != null ? new string[this.Count] : null;
            this.Values = dict != null ? new string[this.Count] : null;

            if(dict != null)
            {
                int index = 0;
                foreach(KeyValuePair<string,string> pair in dict)
                {
                    this.Keys[index] = pair.Key;
                    this.Values[index] = pair.Value;
                    index++;
                }
            }
        }
    }
}

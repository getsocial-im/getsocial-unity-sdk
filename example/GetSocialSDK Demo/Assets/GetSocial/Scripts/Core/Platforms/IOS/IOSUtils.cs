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

#if UNITY_IOS
using System;
using System.Runtime.InteropServices;

namespace GetSocialSdk.Core
{
    static class IOSUtils
    {
        public static IntPtr GetPointer(this object obj)
        {
            return obj == null ? IntPtr.Zero : GCHandle.ToIntPtr(GCHandle.Alloc(obj));
        }

        public static T Cast<T>(this IntPtr instancePtr)
        {
            var instanceHandle = GCHandle.FromIntPtr(instancePtr);
            if (!(instanceHandle.Target is T)) throw new InvalidCastException("Failed to cast IntPtr");

            var castedTarget = (T)instanceHandle.Target;
            return castedTarget;
        }
    }
}
#endif

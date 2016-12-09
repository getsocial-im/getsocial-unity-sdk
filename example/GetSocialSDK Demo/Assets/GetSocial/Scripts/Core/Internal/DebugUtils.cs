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
using System.Reflection;
using System.Text;
using System;
using System.Collections.Generic;

namespace GetSocialSdk.Core
{
    public static class DebugUtils
    {
        public static void LogMethodCall(MethodBase method, params object[] values)
        {
            if(!GetSocialSettings.IsDebugLogsEnabled)
            {
                return;
            }
            
            ParameterInfo[] parameters = method.GetParameters();
            
            StringBuilder message = new StringBuilder().AppendFormat("Method call: {0}(", method.Name);
            for(int i = 0; i < parameters.Length; i++)
            {
                message.AppendFormat("{0}: {1}", parameters[i].Name, values[i] ?? "null");
                if(i < parameters.Length - 1)
                {
                    message.Append(", ");
                }
            }
            message.Append(")");
            
            Debug.Log(message);
        }

        public static void TraceMethodCall()
        {
            try
            {
                throw new Exception("THIS EXCEPTION IS HARMLESS. TRACING METHOD CALL.");
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }
        }

        public static string ToDebugString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            StringBuilder sb = new StringBuilder();
            foreach(TKey key in dictionary.Keys)
            {
                sb.Append("{");
                sb.Append(key);
                sb.Append("=");
                sb.Append(dictionary[key]);
                sb.Append("}\n");
            }
            return sb.ToString();
        }
    }
}

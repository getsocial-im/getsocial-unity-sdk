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

namespace GetSocialSdk.Core
{
    internal static class Check
    {
        public delegate bool Condition();

        public static void IfTrue(Condition condition, string message = "")
        {
            if(!condition())
            {
                throw new ArgumentException(message);
            }
        }

        public static class Argument
        {
            public static void IsNotNull(object argument, string argumentName, string message = "")
            {
                if(argument == null)
                {
                    throw new ArgumentNullException(argumentName, message);
                }
            }

            public static void IsStrNotNullOrEmpty(string argument, string argumentName, string message)
            {
                if(string.IsNullOrEmpty(argument))
                {
                    throw new ArgumentException(message, argumentName);
                }
            }

            public static void IsNotNegative(int argument, string argumentName)
            {
                if(argument < 0)
                {
                    throw new ArgumentOutOfRangeException(argumentName, argumentName + " must not be negative.");
                }
            }
        }
    }
}

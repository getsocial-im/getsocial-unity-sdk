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
using System.Globalization;
using GetSocialSdk.Core;
using System.Collections.Generic;

namespace GetSocialSdk.Core
{
    static class ParseUtils
    {
        public const string TimestampFormat = "dd/MM/yyyy HH:mm:ss";

        public static DateTime ParseTimestamp(string date)
        {
            return DateTime.ParseExact(date, TimestampFormat, CultureInfo.InvariantCulture);
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static List<User> ParseUserList(string jsonStr)
        {
            var json = new JSONObject(jsonStr);
            var ret = new List<User>(json.Count);
            
            foreach(var user in json.list)
            {
                ret.Add(new User(user));
            }
            return ret;
        }
    }
}

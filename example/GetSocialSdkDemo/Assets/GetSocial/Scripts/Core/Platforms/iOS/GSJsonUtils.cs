using System.Collections.Generic;
using System.Linq;
using GetSocialSdk.MiniJSON;

#if UNITY_IOS

namespace GetSocialSdk.Core
{
    public static class GSJsonUtils
    {
        public static Dictionary<string, string> ParseDictionary(string json)
        {
            var dictionary = GSJson.Deserialize(json) as Dictionary<string, object>;

            var result = new Dictionary<string, string>();

            foreach (var key in dictionary.Keys)
            {
                result[key] = (string) dictionary[key];
            }

            return result;
        }

        public static Dictionary<string, object> ToDict(this string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                GetSocialDebugLogger.W("Trying to deserialize empty or null string as dictionary");
                return new Dictionary<string, object>();
            }

            return GSJson.Deserialize(json) as Dictionary<string, object>;
        }

        public static Dictionary<string, string> ToStrStrDict(this Dictionary<string, object> json)
        {
            if (json == null || json.Count == 0)
            {
                return new Dictionary<string, string>();
            }

            return json.ToDictionary(entry => entry.Key, entry => entry.Value as string);
        }

    }
}

#endif
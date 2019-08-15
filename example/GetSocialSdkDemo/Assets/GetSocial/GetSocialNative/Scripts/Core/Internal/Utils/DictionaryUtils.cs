#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;

namespace GetSocialSdk.Core
{
    internal static class DictionaryUtils
    {
        public static T GetOrDefault<K, T>(this Dictionary<K, T> dictionary, K key, T defVal)
        {
            return dictionary.ContainsKey(key) ? dictionary[key] : defVal;
        }

        public static Dictionary<string, string> ToDictionaryOfStrings(this Dictionary<string, object> dictionary)
        {
            return dictionary == null ? null : dictionary.ToDictionary(k => k.Key, k => k.Value as string);
        }
    }
}
#endif

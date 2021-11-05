using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GetSocialSdk.Core
{
    public static class Collections
    {
        public static bool DictionaryEquals<TKey, TValue>(this Dictionary<TKey, TValue> self,
            Dictionary<TKey, TValue> other)
        {
            return self.Count == other.Count && !self.Except(other).Any();
        }

        public static bool ListEquals<T>(this List<T> self, List<T> other)
        {
            if (self.Count != other.Count)
            {
                return false;
            }

            return !self.Where((t, i) => !Equals(t, other[i])).Any();
        }

        public static bool Texture2DEquals(this Texture2D self, Texture2D other)
        {
            if (self == other)
            {
                return true;
            }

            if (self == null || other == null)
            {
                return false;
            }

            return ListEquals(self.GetPixels().ToList(), other.GetPixels().ToList());
        }

        public static void AddAll<TKey, TValue>(this IDictionary<TKey, TValue> container, IDictionary<TKey, TValue> items)
        {
            foreach (var property in items)
            {
                container[property.Key] = property.Value;
            }
        }

        public static void AddAll<T>(this ICollection<T> container, IEnumerable<T> items)
        {
            foreach (var property in items)
            {
                container.Add(property);
            }
        }

        public static string Print<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null || dictionary.Count == 0)
            {
                return "";
            }


            var sb = new System.Text.StringBuilder();
            foreach (var key in dictionary.Keys)
            {
                sb.Append(key);
                sb.Append("=");
                sb.Append(dictionary[key]);
                sb.Append(",");
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }


        public static string Print<T>(this IList<T> list)
        {
            if (list == null || list.Count == 0)
            {
                return "";
            }

            var sb = new System.Text.StringBuilder();
            foreach (var item in list)
            {
                sb.Append(item);
                sb.Append(",");
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

        public static List<string> BuildList(string rawString)
        {
            if (rawString == null || rawString.Length == 0)
            {
                return new List<string>();
            }
            string[] elements = rawString.Split(',');
            return elements.ToList<string>();
        }

        public static Dictionary<string, string> BuildDictionary(string rawString)
        {
            if (rawString == null || rawString.Length == 0)
            {
                return new Dictionary<string, string>();
            }
            var result = new Dictionary<string, string>();
            string[] elements = rawString.Split(',');
            foreach (string element in elements)
            {
                var entry = element.Split('=');
                if (entry.Length == 2)
                {
                    result.Add(entry[0], entry[1]);
                }
            }
            return result;
        }

    }
}

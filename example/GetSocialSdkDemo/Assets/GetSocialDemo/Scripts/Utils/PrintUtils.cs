using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.GetSocialDemo.Scripts.Utils
{
    public static class PrintUtils
    {
        public static string ToPrettyString<T>(this List<T> list)
        {
            return "[" + string.Join("\n", list.ConvertAll(item => item.ToString()).ToArray()) + "]";
        }
    }
}
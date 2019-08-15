#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
using System;
namespace GetSocialSdk.Core
{
    public static class ActionUtils
    {
        public static void SafeCall(this Action action)
        {
            if (action != null) action();
        }

        public static void SafeCall<T>(this Action<T> action, T param)
        {
            if (action != null) action(param);
        }
    }
}
#endif

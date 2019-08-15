#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
namespace GetSocialSdk.Core
{
    internal static class LocalStorageExtension
    {
        public static void Delete(this ILocalStorage localStorage, params string[] keys)
        {
            foreach (var key in keys)
            {
                localStorage.Delete(key);
            }
        }
    }
}
#endif

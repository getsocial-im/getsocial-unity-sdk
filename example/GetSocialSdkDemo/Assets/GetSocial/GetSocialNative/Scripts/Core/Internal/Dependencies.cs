#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR

namespace GetSocialSdk.Core
{
    internal static class Dependencies
    {
        public static ILocalStorage GetLocalStorage()
        {
            return new LocalStorage();
        }

        public static IMetaDataReader GetMetaDataReader()
        {
            return new MetaDataReader();
        }
    }
}
#endif

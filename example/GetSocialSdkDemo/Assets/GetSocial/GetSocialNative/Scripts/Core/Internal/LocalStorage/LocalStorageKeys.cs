#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
namespace GetSocialSdk.Core
{
    internal static class LocalStorageKeys
    {
        public const string UserId = "user_id";
        public const string UserPassword = "user_password";
        public const string AppId = "app_id";
    }
}
#endif

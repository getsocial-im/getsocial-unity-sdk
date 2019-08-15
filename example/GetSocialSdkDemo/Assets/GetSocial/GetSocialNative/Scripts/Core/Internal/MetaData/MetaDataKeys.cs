#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
namespace GetSocialSdk.Core
{
    internal static class MetaDataKeys
    {
        public const string AppId = "im.getsocial.sdk.AppId";
        public const string AutoInit = "im.getsocial.sdk.AutoInitSdk";
    }
}
#endif

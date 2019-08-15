#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
namespace GetSocialSdk.Core
{
    internal interface ILocalStorage
    {
        string GetString(string key);
        void Set(string key, string value);
        void Delete(string key);
    }
}
#endif

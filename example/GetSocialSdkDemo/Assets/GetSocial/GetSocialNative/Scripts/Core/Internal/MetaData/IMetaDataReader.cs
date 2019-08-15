#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
namespace GetSocialSdk.Core
{
    internal interface IMetaDataReader
    {
        string GetString(string key, string defaultVal = null);
        bool GetBool(string key, bool defaultValue = false);
    }
}
#endif

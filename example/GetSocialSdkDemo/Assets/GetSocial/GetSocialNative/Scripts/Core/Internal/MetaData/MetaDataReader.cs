#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR

using System.Collections.Generic;

namespace GetSocialSdk.Core
{
    internal class MetaDataReader : IMetaDataReader
    {
        private static Dictionary<string, object> GetSocialMetaData
        {
            get
            {
                return new Dictionary<string, object>
                {
                    { MetaDataKeys.AppId, GetSocialSettings.AppId },
                    { MetaDataKeys.AutoInit, GetSocialSettings.IsAutoInitEnabled },
                };
            }
        }
        
        public string GetString(string key, string defaultVal = null)
        {
            return GetSocialMetaData.ContainsKey(key) ? GetSocialMetaData[key] as string : null;
        }

        public bool GetBool(string key, bool defaultValue = false)
        {
            return GetSocialMetaData.ContainsKey(key) ? (bool) GetSocialMetaData[key] : defaultValue;
        }
    }
}
#endif

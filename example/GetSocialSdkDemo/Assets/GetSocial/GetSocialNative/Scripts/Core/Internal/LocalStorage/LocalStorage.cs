#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
using UnityEngine;

namespace GetSocialSdk.Core
{
    internal class LocalStorage : ILocalStorage
    {
        private static string GetSocialKey(string key)
        {
            return "getsocial_" + key;
        }
        
        public string GetString(string key)
        {
            return PlayerPrefs.GetString(GetSocialKey(key), null);
        }

        public void Set(string key, string value)
        {
            PlayerPrefs.SetString(GetSocialKey(key), value);
            PlayerPrefs.Save();
        }

        public void Delete(string key)
        {
            PlayerPrefs.DeleteKey(GetSocialKey(key));
            PlayerPrefs.Save();
        }
    }
}
#endif

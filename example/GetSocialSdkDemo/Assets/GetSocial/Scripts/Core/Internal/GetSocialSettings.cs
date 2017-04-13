using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GetSocialSdk.Core
{
    public class GetSocialSettings : ScriptableObject
    {
        const string SettingsAssetName = "GetSocialSettings";
        const string SettingsAssetPath = "Assets/GetSocial/Resources/";

        static GetSocialSettings _instance;

        [SerializeField]
        string _appId = string.Empty;

        [SerializeField]
        bool _useGetSocialUi = true;

        [SerializeField]
        bool _isAutoRegisrationForPushesEnabled = true;

        [SerializeField]
        bool _IosProductionAps = false;

        [SerializeField]
        bool _useCustomDomainForDeeplinking = false;

        [SerializeField]
        string _customDomainForDeeplinking = string.Empty;

        [SerializeField]
        string _getSocialDomainPrefixForDeeplinking = string.Empty;


        #region initialization

        public static GetSocialSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load(SettingsAssetName) as GetSocialSettings;
                    if (_instance == null)
                    {
                        _instance = CreateInstance<GetSocialSettings>();
                        SaveAsset(SettingsAssetPath, SettingsAssetName);
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region public methods

        public static string AppId
        {
            get { return Instance._appId; }
            set
            {
                Instance._appId = value;
                MarkAssetDirty();
            }
        }

        public static bool UseGetSocialUi
        {
            get { return Instance._useGetSocialUi; }
            set
            {
                Instance._useGetSocialUi = value;
                MarkAssetDirty();
            }
        }

        public static bool IsAutoRegisrationForPushesEnabled
        {
            get { return Instance._isAutoRegisrationForPushesEnabled; }
            set
            {
                Instance._isAutoRegisrationForPushesEnabled = value;
                MarkAssetDirty();
            }
        }

        public static bool IosProductionAps
        {
            get { return Instance._IosProductionAps; }
            set
            {
                Instance._IosProductionAps = value;
                MarkAssetDirty();
            }
        }

        public static bool UseCustomDomainForDeeplinking
        {
            get { return Instance._useCustomDomainForDeeplinking; }
            set
            {
                if (value != Instance._useCustomDomainForDeeplinking)
                {
                    Instance._useCustomDomainForDeeplinking = value;
                    MarkAssetDirty();
                }

            }
        }

        public static string CustomDomainForDeeplinking
        {
            get { return Instance._customDomainForDeeplinking; }
            set
            {
                if (value != Instance._customDomainForDeeplinking)
                {
                    Instance._customDomainForDeeplinking = value;
                    MarkAssetDirty();
                }
            }
        }

        public static string GetSocialDomainPrefixForDeeplinking
        {
            get { return Instance._getSocialDomainPrefixForDeeplinking; }
            set
            {
                if (value != Instance._getSocialDomainPrefixForDeeplinking)
                {
                    Instance._getSocialDomainPrefixForDeeplinking = value;
                    MarkAssetDirty();
                }
            }
        }

        #endregion

        #region private methods

        static void SaveAsset(string directory, string name)
        {
#if UNITY_EDITOR
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            AssetDatabase.CreateAsset(Instance, directory + name + ".asset");
            AssetDatabase.Refresh();
#endif
        }

        static void MarkAssetDirty()
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(Instance);
#endif
        }

        #endregion
    }
}
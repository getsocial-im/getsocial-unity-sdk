using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GetSocialSdk.Core
{

    public class GetSocialSettings : ScriptableObject
    {
        public const string UnityDemoAppAppId = "LuDPp7W0J4";
        
        const string SettingsAssetName = "GetSocialSettings";
        const string SettingsAssetPath = "Resources/";
        
        static GetSocialSettings _instance;
        
        [SerializeField]
        string _appId = string.Empty;

        [SerializeField]
        bool _isAutoRegisrationForPushesEnabled = true;

        [SerializeField]
        bool _isForegroundNotificationsEnabled = false;
        
        [SerializeField]
        bool _autoInitEnabled = true;

        [SerializeField]
        bool _disableFacebookReferralCheck = false;
        
        [SerializeField]
        bool _useGetSocialUi = true;

        [SerializeField]
        string _getSocialCustomConfigurationFilePath = string.Empty;

        [SerializeField]
        string _getSocialDefaultConfigurationFilePath = string.Empty;

        [SerializeField] 
        string _iosPushEnvironment = string.Empty;

        [SerializeField] 
        List<string> _deeplinkingDomains = new List<string>();

        [SerializeField] 
        bool _isAndroidEnabled = false;
        
        [SerializeField] 
        bool _isIosEnabled = false;

        [SerializeField] 
        bool _isIosPushEnabled = false;
        
        [SerializeField] 
        bool _isAndroidPushEnabled = false;
        
        [SerializeField] 
        bool _isAppIdValid = true;

        [SerializeField] 
        bool _isRichNotificationsEnabled = true;

        [SerializeField] 
        bool _shouldWaitForListener = false;

        [SerializeField] 
        string _extensionBundleId = string.Empty;

        [SerializeField] 
        string _extensionProvisioningProfile = string.Empty;

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
                        AppId = UnityDemoAppAppId;

                        SaveAsset(Path.Combine(GetPluginPath(), SettingsAssetPath), SettingsAssetName);
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

        public static bool IsForegroundNotificationsEnabled
        {
            get { return Instance._isForegroundNotificationsEnabled; }
            set
            {
                Instance._isForegroundNotificationsEnabled = value;
                MarkAssetDirty();
            }
        }

        public static bool ShouldWaitForPushListener
        {
            get { return Instance._shouldWaitForListener; }
            set
            {
                Instance._shouldWaitForListener = value;
                MarkAssetDirty();
            }
        }


        public static bool IsAutoInitEnabled
        {
            get { return Instance._autoInitEnabled; }
            set
            {
                Instance._autoInitEnabled = value;
                MarkAssetDirty();
            }
        }

        public static bool IsFacebookReferralCheckDisabled
        {
            get { return Instance._disableFacebookReferralCheck; }
            set
            {
                Instance._disableFacebookReferralCheck = value;
                MarkAssetDirty();
            }
        }

        public static string IosPushEnvironment
        {
            get { return Instance._iosPushEnvironment; }
            set
            {
                Instance._iosPushEnvironment= value;
                MarkAssetDirty();
            }
        }
        
        public static List<string> DeeplinkingDomains
        {
            get { return Instance._deeplinkingDomains; }
            set
            {
                Instance._deeplinkingDomains = value;
                MarkAssetDirty();
            }
        }

        public static bool IsAndroidEnabled
        {
            get { return Instance._isAndroidEnabled; }
            set
            {
                Instance._isAndroidEnabled = value;
                MarkAssetDirty();
            }
        }
        
        public static bool IsIosEnabled
        {
            get { return Instance._isIosEnabled; }
            set
            {
                Instance._isIosEnabled = value;
                MarkAssetDirty();
            }
        }
        
        public static string UiConfigurationCustomFilePath
        {
            get 
            {
                if (string.IsNullOrEmpty(Instance._getSocialCustomConfigurationFilePath) 
                    && !string.IsNullOrEmpty(Instance._getSocialDefaultConfigurationFilePath)) 
                {
                    Instance._getSocialCustomConfigurationFilePath = Instance._getSocialDefaultConfigurationFilePath;
                    Instance._getSocialDefaultConfigurationFilePath = string.Empty;
                    MarkAssetDirty();
                }
                return Instance._getSocialCustomConfigurationFilePath; 
            } 
            set
            {
                Instance._getSocialCustomConfigurationFilePath = value;
                MarkAssetDirty();
            }
        }
        
        public static bool IsIosPushEnabled
        {
            get { return Instance._isIosPushEnabled; }
            set
            {
                Instance._isIosPushEnabled = value;
                MarkAssetDirty();
            }
        }
        
        public static bool IsAndroidPushEnabled
        {
            get { return Instance._isAndroidPushEnabled; }
            set
            {
                Instance._isAndroidPushEnabled = value;
                MarkAssetDirty();
            }
        }
        
        public static bool IsAppIdValidated
        {
            get { return Instance._isAppIdValid; }
            set
            {
                Instance._isAppIdValid = value;
                MarkAssetDirty();
            }
        }
        
        public static bool IsRichPushNotificationsEnabled
        {
            get { return Instance._isRichNotificationsEnabled; }
            set
            {
                Instance._isRichNotificationsEnabled = value;
                MarkAssetDirty();
            }
        }
        
        public static string ExtensionBundleId
        {
            get { return Instance._extensionBundleId; }
            set
            {
                Instance._extensionBundleId = value;
                MarkAssetDirty();
            }
        }

        public static string ExtensionProvisioningProfile
        {
            get { return Instance._extensionProvisioningProfile; }
            set
            {
                Instance._extensionProvisioningProfile = value;
                MarkAssetDirty();
            }
        }

        public static string GetPluginPath()
        {
            // get GetSocial plugin path relative to Assets folder
            return GetAbsolutePluginPath().Replace("\\", "/").Replace(Application.dataPath, "Assets");
        }

        public static string GetAbsolutePluginPath()
        {
            // get absolute path to GetSocial folder
            return Path.GetDirectoryName(Path.GetDirectoryName( FindEditor(Application.dataPath)));
        }
        
        private static string FindEditor(string path)
        {
            foreach (var d in Directory.GetDirectories(path))
            {
                foreach (var f in Directory.GetFiles(d))
                {
                    if (f.Contains("GetSocialSettingsEditor.cs"))
                    {
                        return f;
                    }
                }

                var rec = FindEditor(d);
                if (rec != null)
                {
                    return rec;
                }
            }

            return null;
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
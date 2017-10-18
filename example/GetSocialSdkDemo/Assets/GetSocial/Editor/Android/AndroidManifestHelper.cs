using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GetSocialSdk.Core;
using GetSocialSdk.Editor.Android.Manifest;
using UnityEngine;

namespace GetSocialSdk.Editor
{
    public class AndroidManifestHelper
    {
        #region constants

        /// <summary>
        /// Relative path to your AndroidManifest.xml file.
        ///
        /// Change it if your manifest is not in the root of Plugins directory.
        /// </summary>
        const string MainManifestPath = "Plugins/Android/AndroidManifest.xml";
        
        /// <summary>
        /// Complete list of modifications needed for GetSocial Android SDK.
        /// </summary>
        readonly List<AndroidManifestNode> GetSocialModifications = new List<AndroidManifestNode>
        {
            new UsesPermission(UsesPermission.InternetPermission),
            new UsesPermission(UsesPermission.AccessNetoworkStatePermission),
            
            new MetaData("im.getsocial.sdk.AppId", GetSocialSettings.AppId),
            new MetaData("im.getsocial.sdk.Runtime", "UNITY"),
            new MetaData("im.getsocial.sdk.RuntimeVersion", Application.unityVersion),
            new MetaData("im.getsocial.sdk.WrapperVersion", BuildConfig.UnitySdkVersion),
            new MetaData("im.getsocial.sdk.AutoRegisterForPush", GetSocialSettings.IsAutoRegisrationForPushesEnabled.ToString().ToLower()),
            new MetaData("im.getsocial.sdk.AutoInitSdk", GetSocialSettings.IsAutoInitEnabled.ToString().ToLower()),
            new MetaData("im.getsocial.sdk.UiConfigurationFile", GetSocialSettings.UiConfigurationDefaultFilePath),
            
            new Provider("im.getsocial.sdk.AutoInitSdkContentProvider", string.Format("{0}.AutoInitSdkContentProvider", PlayerSettingsCompat.bundleIdentifier), false),
            new Provider("im.getsocial.sdk.invites.ImageContentProvider", string.Format("{0}.smartinvite.images.provider", PlayerSettingsCompat.bundleIdentifier), true),
        
            new Activity("im.getsocial.sdk.unity.GetSocialDeepLinkingActivity", CreateDeepLinkingIntentFilter()),
            new Receiver("im.getsocial.sdk.invites.MultipleInstallReferrerReceiver", CreateInstallReferrerIntentFilter())
        };
        
        /// <summary>
        /// List of AndroidManifest elements that are deprecated and should be removed.
        /// </summary>
        readonly List<AndroidManifestNode> DeprecatedModifications = new List<AndroidManifestNode>
        {
            new Receiver("im.getsocial.sdk.invites.InstallReferrerReceiver", CreateInstallReferrerIntentFilter())
        };
        #endregion

        
        #region fields
        
        private readonly AndroidManifest _manifest;
        
        #endregion
        
        
        #region public api

        public AndroidManifestHelper()
        {
            var manifestPath = Path.Combine(Application.dataPath, MainManifestPath);
            EnsureManifestExists(manifestPath);
            _manifest = new AndroidManifest(manifestPath);
        }

        public bool IsConfigurationCorrect()
        {
            return GetSocialModifications.All(node => _manifest.Contains(node))
                && DeprecatedModifications.All(node => !_manifest.Contains(node));
        }

        public string ConfigurationSummary()
        {
            StringBuilder sb = new StringBuilder();

            var modificationsResult = GetSocialModifications
                .GroupBy(node => _manifest.Contains(node))
                .ToDictionary(group => group.Key, group => group.ToList());

            var deprecationsResult = DeprecatedModifications
                .GroupBy(node => _manifest.Contains(node))
                .ToDictionary(group => group.Key, group => group.ToList());

            if (modificationsResult.ContainsKey(false) || deprecationsResult.ContainsKey(true))
            {
                sb.AppendLine("Please regenerate AndroidManifest.xml to fix the issues.\n");
            }

            if (deprecationsResult.ContainsKey(true))
            {
                sb.AppendLine("DEPRECATED CONFIGURATION:");
                deprecationsResult[true].ForEach(node => sb.AppendFormat(" ✘ {0}\n", node.ToString()));
                sb.AppendLine();
            }
            
            if (modificationsResult.ContainsKey(false))
            {
                sb.AppendLine("WRONG CONFIGURATION:");
                modificationsResult[false].ForEach(node => sb.AppendFormat(" ✘ {0}\n", node.ToString()));
                sb.AppendLine();
            }
            
            if (modificationsResult.ContainsKey(true))
            {
                sb.AppendLine("CORRECT CONFIGURATION:");
                modificationsResult[true].ForEach(node => sb.AppendFormat(" ✔ {0}\n", node.ToString()));
            }
            
            return sb.ToString();
        }

        public void Regenerate()
        {
            GetSocialModifications.ForEach(node =>
            {
                _manifest.RemoveSimilar(node);
                _manifest.Add(node);
            });
            
            DeprecatedModifications.ForEach(node =>
            {
                _manifest.RemoveSimilar(node);
            });
            
            _manifest.Save();
            
            Debug.Log("GetSocial: successfully regenerated AndroidManifest.xml with GetSocial modifications");
        }
        
        #endregion


        #region private methods

        private static IntentFilter CreateInstallReferrerIntentFilter()
        {
            var intentFilter = new IntentFilter();
            intentFilter.AddChild(new Action("com.android.vending.INSTALL_REFERRER"));
            
            return intentFilter;
        }
        
        private static IntentFilter CreateDeepLinkingIntentFilter()
        {
            var intentFilter = new IntentFilter();
            
            intentFilter.AddChild(new Data("getsocial", GetSocialSettings.AppId));
            GetSocialSettings.DeeplinkingDomains.ForEach(domain => intentFilter.AddChild(new Data("https", domain)));
            
            intentFilter.AddChild(new Category("android.intent.category.BROWSABLE"));
            intentFilter.AddChild(new Category("android.intent.category.DEFAULT"));
            
            intentFilter.AddChild(new Action("android.intent.action.VIEW"));
            
            return intentFilter;
        }

        private void EnsureManifestExists(string manifestPath)
        {
            if(!File.Exists(manifestPath))
            {
                var backupManifestPath = Path.Combine(Application.dataPath, "GetSocial/Editor/Android/BackupManifest/AndroidManifest.xml");

                var manifestDirectoryPath = Path.GetDirectoryName(manifestPath);
                if (!Directory.Exists(manifestDirectoryPath))
                {
                    Directory.CreateDirectory(manifestDirectoryPath);
                }
                
                File.Copy(backupManifestPath, manifestPath);
            }
        }
        
        #endregion
    }
}
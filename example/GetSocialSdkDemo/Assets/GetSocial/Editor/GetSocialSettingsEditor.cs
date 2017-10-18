/**
 *     Copyright 2015-2016 GetSocial B.V.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using GetSocialSdk.Core;

namespace GetSocialSdk.Editor
{
    [InitializeOnLoad]
    [CustomEditor(typeof(GetSocialSettings))]
    public class GetSocialSettingsEditor : UnityEditor.Editor
    {
        public const string DemoAppPackage = "im.getsocial.demo.unity";

        // Android
        const string ShowAndroidSettingsEditorPref = "ShowAndroidSettings";

        // iOS
        const string ShowIosSettingsEditorPref = "ShowIosSettings";
        
        static bool ShowAndroidSettings
        {
            set { EditorPrefs.SetBool(ShowAndroidSettingsEditorPref, value); }
            get { return EditorPrefs.GetBool(ShowAndroidSettingsEditorPref); }
        }

        static bool ShowIosSettings
        {
            set { EditorPrefs.SetBool(ShowIosSettingsEditorPref, value); }
            get { return EditorPrefs.GetBool(ShowIosSettingsEditorPref); }
        }

        private static RemoteConfigRequest _remoteConfigRequest;
        private static string _remoteRequestStatusLabel;
        private static bool _isAndroidManifestConfigurationCorrect = false;
        private static bool _isUiConfigurationFileCorrect = true;
        private static string _androidManifestConfigurationSummary = string.Empty;

        #region lifecycle

        void OnEnable()
        {
            UpdateDefineSymbols();
            UpdateRemoteConfig();
            UpdateAndroidManifestCheck();
            UpdateUiConfigFileCheck();
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Space(15);
            DrawGeneralSettings();
            
            GUILayout.Space(15);
            DrawAndroidSettings();

            GUILayout.Space(15);
            DrawIosSettings();

            GUILayout.Space(15);
            DrawAdditionalInfo();
        }
        #endregion

        
        #region menus definition
        [MenuItem("GetSocial/Edit Settings")]
        public static void Edit()
        {
            Selection.activeObject = GetSocialSettings.Instance;
        }

        [MenuItem("GetSocial/GetSocial Dashboard")]
        public static void OpenGetSocialDashboard()
        {
            Application.OpenURL("http://dashboard.getsocial.im/");
        }

        [MenuItem("GetSocial/Documentation")]
        public static void OpenGetSocialDocumentation()
        {
            Application.OpenURL("http://docs.getsocial.im");
        }
        #endregion

        
        #region methods
        void DrawGeneralSettings()
        {
            GetSocialEditorUtils.BeginSetSmallIconSize();
            EditorGUILayout.LabelField(new GUIContent(" General Settings", GetSocialEditorUtils.GetSocialIcon), EditorStyles.boldLabel);
            GetSocialEditorUtils.EndSetSmallIconSize();
            
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                DrawAppIdSettings();
                DrawCommonSettings();

                GUILayout.Space(15);
                DrawPushNotificationSettings();

                GUILayout.Space(15);
                DrawUiSettings();
            }
            EditorGUILayout.EndVertical();
        }

        void DrawAppIdSettings()
        {
            var getSocialAppKeyLabel = new GUIContent("GetSocial App Id [?]", "Get unique App Id on GetSocial Dashboard");
            EditorGUILayout.LabelField(getSocialAppKeyLabel, EditorStyles.boldLabel);
            
            DrawAppIdValidation(GetSocialSettings.AppId);
            var newAppKeyValue = EditorGUILayout.TextField(GetSocialSettings.AppId);

            if (GUILayout.Button(new GUIContent(_remoteRequestStatusLabel, "Click label to check again"), EditorStyles.miniLabel))
            {
                UpdateRemoteConfig();
            }
            
            SetAppId(newAppKeyValue);
        }

        void DrawCommonSettings()
        {
            var autoInitSdkEnabled = new GUIContent("Initialize Automatically [?]", "If this setting is checked, GetSocial will be initialized automatically, if not, you need to call GetSocial.Init() method.");
            var shouldAutoInitSdk = EditorGUILayout.ToggleLeft(autoInitSdkEnabled, GetSocialSettings.IsAutoInitEnabled);
            
            SetAutoInitEnabled(shouldAutoInitSdk);
        }
        
        static bool IsDemoAppPackage()
        {
            return PlayerSettingsCompat.bundleIdentifier == DemoAppPackage;
        }

        static bool IsDemoAppId(string appId)
        {
            return appId == GetSocialSettings.UnityDemoAppAppId;
        }

        private void DrawPushNotificationSettings()
        {
            EditorGUILayout.LabelField("Push Notifications", EditorStyles.boldLabel);

            var enablePushNotificationAutoRegistering = new GUIContent("Register Automatically [?]", "If this setting is checked, GetSocial push notifications will be registered automatically, if not, you need to call GetSocial.RegisterForPushNotification() method.");
            var isAutoRegisrationForPushesEnabled = EditorGUILayout.ToggleLeft(enablePushNotificationAutoRegistering, GetSocialSettings.IsAutoRegisrationForPushesEnabled);
            
            SetAutoRegisterPushEnabled(isAutoRegisrationForPushesEnabled);
        }

        void DrawAndroidSettings()
        {
            var androidSettingsText = " Android Settings";
            if (!ShowAndroidSettings && !GetSocialSettings.IsAdroidEnabled)
            {
                androidSettingsText += " (platform disabled)";
            } 
            else if (!ShowAndroidSettings && !_isAndroidManifestConfigurationCorrect)
            {
                androidSettingsText += " (need to regenerate AndroidManifest.xml)";
            }
            
            var androidSettingsLabel = EditorGuiUtils.GetBoldLabel(androidSettingsText, "", GetSocialEditorUtils.AndroidIcon);
            
            GetSocialEditorUtils.BeginSetSmallIconSize();
            ShowAndroidSettings = EditorGUILayout.Foldout(ShowAndroidSettings, androidSettingsLabel);
            GetSocialEditorUtils.EndSetSmallIconSize();

            if (ShowAndroidSettings)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    DrawDashboardSettingToogle("Platform status", 
                        GetSocialSettings.IsAdroidEnabled ? "✔️ Enabled [?]" : "✘ Disabled [?]");
                    
                    DrawDashboardSettingToogle("Push notifications status", 
                        GetSocialSettings.IsAndroidPushEnabled ? "✔️ Enabled [?]" : "✘ Disabled [?]");

                    GUILayout.Space(15f);
                    DrawAndroidManifestSettings();
                }
                EditorGUILayout.EndVertical();
            }
        }

        private void DrawAndroidManifestSettings()
        {
            EditorGUI.BeginDisabledGroup(!GetSocialSettings.IsAdroidEnabled || !GetSocialSettings.IsAppIdValidated);
            {
                EditorGUILayout.LabelField("Android Manifest", EditorStyles.boldLabel);

                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Configuration status");
                    
                    var lableContent = new GUIContent(
                        _isAndroidManifestConfigurationCorrect ? "✔ Correct [?]" : "✘ Not complete [?]",  
                        _androidManifestConfigurationSummary
                    );
                    GUILayout.Button(lableContent, EditorStyles.label, EditorGuiUtils.OneThirdWidth);
                }
                
                EditorGuiUtils.ColoredBackground(
                    _isAndroidManifestConfigurationCorrect ? GUI.backgroundColor : Color.green,
                    () => {
                        if (GUILayout.Button("Regenerate Manifest"))
                        {
                            new AndroidManifestHelper().Regenerate();
                            UpdateAndroidManifestCheck();
                        }
                });
                
                if (!_isAndroidManifestConfigurationCorrect)
                {
                    EditorGUILayout.HelpBox("AndroidManifest.xml configuration is not complete. Regenerate manifest to avoid issues with running GetSocial SDK.", MessageType.Warning);                            
                }
            }
            EditorGUI.EndDisabledGroup();
        }
        
        private static void DrawDashboardSettingToogle(string settingText, string status)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField(settingText);
                
                var buttonLabel = new GUIContent(status, "Click on the label to update the setting on the GetSocial Dashboard. GetSocial Editor will fetch the latest setting automatically.");
                if (GUILayout.Button(buttonLabel, EditorStyles.label, EditorGuiUtils.OneThirdWidth))
                {
                    OpenGetSocialDashboard();
                }
            }
        }

        void DrawIosSettings()
        {
            var iosSettingsText = !ShowIosSettings && !GetSocialSettings.IsIosEnabled ? " iOS Settings (platform disabled)" : " iOS Settings";
            var iosSettingsLabel = EditorGuiUtils.GetBoldLabel(iosSettingsText, "", GetSocialEditorUtils.IOSIcon);
            
            GetSocialEditorUtils.BeginSetSmallIconSize();
            ShowIosSettings = EditorGUILayout.Foldout(ShowIosSettings, iosSettingsLabel);
            GetSocialEditorUtils.EndSetSmallIconSize();

            if (ShowIosSettings)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    DrawDashboardSettingToogle("Platform status", 
                        GetSocialSettings.IsIosEnabled ? "✔️ Enabled [?]" : "✘ Disabled [?]");
                    
                    DrawDashboardSettingToogle("Push notifications status", 
                        GetSocialSettings.IsIosPushEnabled ? "✔️ Enabled [?]" : "✘ Disabled [?]");

                    if (GetSocialSettings.IsIosPushEnabled)
                    {
                        DrawDashboardSettingToogle("Push notifications environment",
                            "    " + GetSocialSettings.IosPushEnvironment.Capitalize());
                    }
                }
                EditorGUILayout.EndVertical();
            }
        }

        void DrawUiSettings()
        {
            EditorGUILayout.LabelField("GetSocial UI", EditorStyles.boldLabel);
            
            var newIsGetSocialUiEnabledValue =
                EditorGUILayout.ToggleLeft(" Use GetSocial UI", GetSocialSettings.UseGetSocialUi);
            SetGetSocialUiEnabled(newIsGetSocialUiEnabledValue);
            
            EditorGUI.BeginDisabledGroup(!GetSocialSettings.UseGetSocialUi);
            {
                var uiConfigurationLabel = new GUIContent("UI Configuration File Path [?]", "Path to the UI configuration json relative to StreamingAssets/ folder. \nLeave empty to use default UI configuration.");
                using (new FixedWidthLabel(uiConfigurationLabel))
                {
                    var filePath = EditorGUILayout.TextField(GetSocialSettings.UiConfigurationDefaultFilePath);
                    SetUiConfigDefaultFilePath(filePath);
                }
                
                if (!_isUiConfigurationFileCorrect)
                {
                    EditorGUILayout.HelpBox("UI configuration file not found in the StreamingAssets folder. Note that file name should contain .json extension.", MessageType.Error);
                }
            }
            EditorGUI.EndDisabledGroup();
        }

        static void DrawAdditionalInfo()
        {
            GetSocialEditorUtils.BeginSetSmallIconSize();
            EditorGUILayout.LabelField(new GUIContent(" SDK Info", GetSocialEditorUtils.InfoIcon),
                EditorStyles.boldLabel);
            GetSocialEditorUtils.EndSetSmallIconSize();

            using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
            {
                EditorGuiUtils.SelectableLabelField(
                    new GUIContent("SDK Version [?]", "GetSocial SDK Version"),
                    BuildConfig.UnitySdkVersion);    
            }
        }

        void DrawAppIdValidation(string appIdValue)
        {
            if (!IsDemoAppPackage() && IsDemoAppId(appIdValue))
            {
                EditorGUILayout.HelpBox(
                    "You are using GetSocial test app key with your bundle identifier. Are you sure this is what you want?",
                    MessageType.Warning);
            }
            
            var isEmpty = string.IsNullOrEmpty(appIdValue);
            if (isEmpty)
            {
                EditorGUILayout.HelpBox("Missing app id? Don't worry, get one at the GetSocial Dashboard",
                    MessageType.Info);
            }
            else if (!GetSocialSettings.IsAppIdValidated)
            {
                EditorGUILayout.HelpBox("App Id is not valid. Please visit GetSocial Dashboard to get the correct one.",
                    MessageType.Error);
            }
            else
            {
                const int validAppKeyLength = 40;
                var hasAppKeyLength = appIdValue.Length == validAppKeyLength;
                if (hasAppKeyLength)
                {
                    EditorGUILayout.HelpBox("You are using deprecated format of the app id. Please visit GetSocial Dashboard to get the updated app id.",
                        MessageType.Warning);
                }
            }
        }

        void SetAppId(string value)
        {
            if (!string.IsNullOrEmpty(value) && !value.Equals(GetSocialSettings.AppId))
            {
                GetSocialSettings.AppId = value;
                UpdateRemoteConfig();
                UpdateAndroidManifestCheck();
            }
        }


        void SetUiConfigDefaultFilePath(string value)
        {
            if (!value.Equals(GetSocialSettings.UiConfigurationDefaultFilePath))
            {
                GetSocialSettings.UiConfigurationDefaultFilePath = value;
                UpdateAndroidManifestCheck();
                UpdateUiConfigFileCheck();
            }
        }

        private void SetAutoRegisterPushEnabled(bool value)
        {
            if (GetSocialSettings.IsAutoRegisrationForPushesEnabled != value)
            {
                GetSocialSettings.IsAutoRegisrationForPushesEnabled = value;
                UpdateAndroidManifestCheck();
            }
        }

        private void SetAutoInitEnabled(bool value)
        {
            if (GetSocialSettings.IsAutoInitEnabled != value)
            {
                GetSocialSettings.IsAutoInitEnabled = value;
                UpdateAndroidManifestCheck();
            }
        }

        void SetGetSocialUiEnabled(bool value)
        {
            if (GetSocialSettings.UseGetSocialUi != value)
            {
                GetSocialSettings.UseGetSocialUi = value;
                UpdateDefineSymbols();
            }
        }
        
        static void UpdateDefineSymbols()
        {
            DefinesToggler.ToggleUseGetSocialUiDefine(GetSocialSettings.UseGetSocialUi);
        }

        void UpdateRemoteConfig()
        {
            _remoteRequestStatusLabel = "Validating App Id...";
            
            if (_remoteConfigRequest != null && _remoteConfigRequest.IsInProgress)
            {
                _remoteConfigRequest.Cancel();
            }
            
            _remoteConfigRequest = RemoteConfigRequest.ForAppId(GetSocialSettings.AppId);
            _remoteConfigRequest.Start(
                onSuccess: remoteConfig =>
                {
                    if (remoteConfig.IsSuccessful)
                    {
                        GetSocialSettings.IsAdroidEnabled = remoteConfig.Android.IsEnabled;
                        GetSocialSettings.IsAndroidPushEnabled = remoteConfig.Android.IsPushNotificationEnabled;
                        GetSocialSettings.IsIosEnabled = remoteConfig.Ios.IsEnabled;
                        GetSocialSettings.IsIosPushEnabled = remoteConfig.Ios.IsPushNotificationEnabled;
                        GetSocialSettings.IosPushEnvironment = remoteConfig.Ios.PushEnvironment;
                        GetSocialSettings.DeeplinkingDomains = ExtractDeeplinksFromRemoteConfig(remoteConfig);
                        GetSocialSettings.IsAppIdValidated = true;

                        _remoteRequestStatusLabel =
                            string.Format("App Id is valid. Settings updated today at {0:H:mm:ss} [?]", DateTime.Now);
                    }
                    else
                    {
                        var errorMessage = string.Format("Failed to validate App Id. Error: {0}", remoteConfig.ErrorMessage);
                        
                        Debug.LogError(errorMessage);

                        GetSocialSettings.IsAppIdValidated = false;
                        _remoteRequestStatusLabel = string.Format("{0} [?]", errorMessage);
                    }
                },
                onFailure: error =>
                {
                    var errorMessage = string.Format("Failed to validate App Id. Error: {0}", error);
                        
                    Debug.LogError(errorMessage);
                    GetSocialSettings.IsAppIdValidated = false;
                    _remoteRequestStatusLabel = string.Format("{0} [?]", errorMessage);
                }
            );
        }

        private List<string> ExtractDeeplinksFromRemoteConfig(RemoteConfig remoteConfig)
        {
            var productionDeepLinkDomains = remoteConfig.Android.DeepLinkDomains;
            var deeplinks = new List<string>(productionDeepLinkDomains);

            // add testing environment domains for the test app
            if (GetSocialSettings.AppId == GetSocialSettings.UnityDemoAppAppId)
            {
                productionDeepLinkDomains.ForEach(productionDomain =>
                {
                    var parts = productionDomain.Split('.').ToList();
                    parts.Insert(1, "testing");
                    string testingDomain = string.Join(".", parts.ToArray());
                    
                    deeplinks.Add(testingDomain);
                });
            }
            
            return deeplinks;
        }

        private static void UpdateAndroidManifestCheck()
        {
            var androidManifestHelper = new AndroidManifestHelper();
            
            _isAndroidManifestConfigurationCorrect = androidManifestHelper.IsConfigurationCorrect();
            _androidManifestConfigurationSummary = androidManifestHelper.ConfigurationSummary();
        }
        
        private void UpdateUiConfigFileCheck()
        {
            _isUiConfigurationFileCorrect = string.IsNullOrEmpty(GetSocialSettings.UiConfigurationDefaultFilePath)
                                            || File.Exists(Path.Combine(Application.streamingAssetsPath,
                                                GetSocialSettings.UiConfigurationDefaultFilePath));
        }

        
        #endregion
    }
}
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
    [CustomEditor(typeof(GetSocialSettings))]
    public class GetSocialSettingsEditor : UnityEditor.Editor
    {
        public const string DemoAppPackage = "im.getsocial.demo.unity";
        private const string PlatformDisabledLabel = " (platform disabled)";

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
        private static UiConfigValidationResult _uiConfigurationValidationResult = new UiConfigValidationResult(true, "");

        #region lifecycle
        
        void OnEnable()
        {
            UpdateRemoteConfig();
            UpdateUiConfigFileCheck();
            AndroidManifestHelper.RemoveSdk6Configs();
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
        [MenuItem("GetSocial/Edit Settings", false, 1000)]
        public static void Edit()
        {
            Selection.activeObject = GetSocialSettings.Instance;
        }

        [MenuItem("GetSocial/GetSocial Dashboard", false, 1500)]
        public static void OpenGetSocialDashboard()
        {
            Application.OpenURL(string.Format("http://dashboard.getsocial.im/?utm_source={0}&utm_medium=unity-editor", BuildConfig.PublishTarget));
        }

        [MenuItem("GetSocial/Documentation", false, 1500)]
        public static void OpenGetSocialDocumentation()
        {
            Application.OpenURL(string.Format("http://docs.getsocial.im/?utm_source={0}&utm_medium=unity-editor", BuildConfig.PublishTarget));
        }

        [MenuItem("GetSocial/Helper Methods/Trigger Referral Data Listener", false, 2000)]
        public static void TriggerReferralDataListener()
        {
            var token = "token";
            var referrerUserId = "userId";
            var referrerChannelId = "channelId";
            var IsFirstMatch = true;
            var IsGuaranteedMatch = true;
            var IsReinstall = false;
            var IsFirstMatchLink = true;
            var linkParams = new Dictionary<string, string>();
            var originalLinkParams = new Dictionary<string, string>();

            UnityEditorHelperFunctions.TriggerOnReferralDataReceivedListener(token,
                referrerUserId,
                referrerChannelId,
                IsFirstMatch,
                IsGuaranteedMatch,
                IsReinstall,
                IsFirstMatchLink,
                linkParams,
                originalLinkParams); ;
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
            
            var facebookReferralCheckDisabled = new GUIContent("Disable Facebook Referral Check [?]", "If this setting is checked, GetSocial won't use Facebook SDK to check referral data.");
            var shouldDisableFaceReferralCheck = EditorGUILayout.ToggleLeft(facebookReferralCheckDisabled, GetSocialSettings.IsFacebookReferralCheckDisabled);

            SetDisableFacebookReferralCheck(shouldDisableFaceReferralCheck);

        }
        
        static bool IsDemoAppPackage()
        {
            return PlayerSettings.applicationIdentifier == DemoAppPackage;
        }

        static bool IsDemoAppId(string appId)
        {
            return appId == GetSocialSettings.UnityDemoAppAppId;
        }

        private void DrawPushNotificationSettings()
        {
            EditorGUILayout.LabelField("Push Notifications", EditorStyles.boldLabel, EditorGuiUtils.OneThirdWidth);

            var enablePushNotificationAutoRegistering = new GUIContent("Register Automatically [?]", "If this setting is checked, GetSocial push notifications will be registered automatically, if not, you need to call GetSocial.RegisterForPushNotification() method.");
            var isAutoRegisrationForPushesEnabled = EditorGUILayout.ToggleLeft(enablePushNotificationAutoRegistering, GetSocialSettings.IsAutoRegisrationForPushesEnabled);
            
            var enableForegroundNotifications = new GUIContent("Show Notification In Foreground [?]", "If this setting is checked, all GetSocial push notifications will be shown when app is in foreground. Otherwise, the notification will be delegated to NotificationListener.");
            var isForegroundNotificationEnabled = EditorGUILayout.ToggleLeft(enableForegroundNotifications, GetSocialSettings.IsForegroundNotificationsEnabled);

            var waitForPushListener = new GUIContent("Has Custom Notification On Click Listener [?]", "If you want to handle notification clicks with `Notifications.SetOnNotificationClickedListener`, set this checked. If it is checked - the default behaviour is not invoked.");
            var shouldWaitForListener = EditorGUILayout.ToggleLeft(waitForPushListener, GetSocialSettings.ShouldWaitForPushListener);
            
            SetAutoRegisterPushEnabled(isAutoRegisrationForPushesEnabled);
            SetForegroundNotificationsEnabled(isForegroundNotificationEnabled);
            SetShouldWaitForPushListener(shouldWaitForListener);
        }

        void DrawAndroidSettings()
        {
            var androidSettingsText = " Android Settings";
            if (!ShowAndroidSettings && !GetSocialSettings.IsAndroidEnabled)
            {
                androidSettingsText += PlatformDisabledLabel;
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
                        GetSocialSettings.IsAndroidEnabled ? "✔️ Enabled [?]" : "✘ Disabled [?]");
                    
                    DrawDashboardSettingToogle("Push notifications status", 
                        GetSocialSettings.IsAndroidPushEnabled ? "✔️ Enabled [?]" : "✘ Disabled [?]");

                    if (Application.platform == RuntimePlatform.WindowsEditor ||
                        Application.platform == RuntimePlatform.OSXEditor)
                    {
                        DrawAndroidSigningSignatureHash();
                    }
                }
                EditorGUILayout.EndVertical();
            }
        }

        private static void DrawDashboardSettingToogle(string settingText, string status)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField(settingText, EditorGuiUtils.OneThirdWidth);
                
                var buttonLabel = new GUIContent(status, "Click on the label to update the setting on the GetSocial Dashboard. GetSocial Editor will fetch the latest setting automatically.");
                if (GUILayout.Button(buttonLabel, EditorStyles.label, EditorGuiUtils.TwoThirdsWidth))
                {
                    OpenGetSocialDashboard();
                }
            }
        }

        void DrawIosSettings()
        {
            var iosSettingsText = " iOS Settings";
            if (!ShowIosSettings && !GetSocialSettings.IsIosEnabled)
            {
                iosSettingsText += PlatformDisabledLabel;
            }
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
#if UNITY_2018_1_OR_NEWER                        
                        var richNotificationsEnabled = new GUIContent("Enable Rich Notifications [?]", "If it is enabled, notifications with images/videos can be displayed.");
                        var enableRichNotifications = EditorGUILayout.ToggleLeft(richNotificationsEnabled, GetSocialSettings.IsRichPushNotificationsEnabled);
                        SetRichNotificationsEnabled(enableRichNotifications);
                        
                        // extension bundle id, only if UNITY_2018_1_OR_NEWER
                        EditorGUILayout.BeginHorizontal();
                        var extensionBundleIdLabel = new GUIContent("Notification Extension Bundle Id [?]", "Bundle id of the extension.");
                        EditorGUILayout.LabelField(extensionBundleIdLabel, EditorGuiUtils.OneThirdWidth);
                        var extensionBundleId = EditorGUILayout.TextField(GetSocialSettings.ExtensionBundleId, EditorGuiUtils.OneThirdWidth);
                        if (extensionBundleId.Length == 0)
                        {
                            extensionBundleId = PlayerSettings.applicationIdentifier + ".getsocialextension";
                        }
                        SetExtensionBundleId(extensionBundleId);
                        if (GUILayout.Button("More info", EditorStyles.miniButton, EditorGuiUtils.OneThirdWidth))
                        {
                            Application.OpenURL(string.Format("https://docs.getsocial.im/guides/notifications/setup-push-notifications/unity/#receiving-rich-push-notifications-and-badges-ios-only", BuildConfig.PublishTarget));
                        }
                        EditorGUILayout.EndHorizontal();

                        // extension provisioning profile
                        EditorGUILayout.BeginHorizontal();
                        var extensionProvisioningProfileLabel = new GUIContent("Notification Extension Provisioning Profile [?]", "Name of the provision profile used for signing the extension, or leave it empty if you use automatic signing");
                        EditorGUILayout.LabelField(extensionProvisioningProfileLabel, EditorGuiUtils.OneThirdWidth);
                        var extensionProvisioningProfile = EditorGUILayout.TextField(GetSocialSettings.ExtensionProvisioningProfile, EditorGuiUtils.OneThirdWidth);
                        SetExtensionProvisioningProfile(extensionProvisioningProfile);
                        if (GUILayout.Button("More info", EditorStyles.miniButton, EditorGuiUtils.OneThirdWidth))
                        {
                            Application.OpenURL(string.Format("https://docs.getsocial.im/guides/notifications/setup-push-notifications/unity/#receiving-rich-push-notifications-and-badges-ios-only", BuildConfig.PublishTarget));
                        }
                        EditorGUILayout.EndHorizontal();
#endif                        
                    }

                }
                EditorGUILayout.EndVertical();
            }
        }

        void DrawUiSettings()
        {
            EditorGUILayout.LabelField("GetSocial UI", EditorStyles.boldLabel);
            
            var uiConfigurationLabel = new GUIContent("UI Configuration File Path [?]", "Path to the UI configuration json relative to StreamingAssets/ folder. \nLeave empty to use default UI configuration.");
            using (new FixedWidthLabel(uiConfigurationLabel))
            {
                var filePath = EditorGUILayout.TextField(GetSocialSettings.UiConfigurationCustomFilePath);
                SetUiConfigCustomFilePath(filePath);
            }

            var configCheckResult = _uiConfigurationValidationResult;
            if (!configCheckResult.Result)
            {
                EditorGUILayout.HelpBox(configCheckResult.Message, MessageType.Error);
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

        static void DrawAndroidSigningSignatureHash()
        {
            string label = "Signing-certificate fingerprint [?]";
            GUIContent content = new GUIContent(label,
                "SHA-256 hash of the keystore you use to sign your application.");

            var hasError = GetSocialEditorUtils.KeyStoreUtilError != null;
            if (hasError) 
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(label, GUILayout.Height(16), EditorGuiUtils.OneThirdWidth);
                if (GUILayout.Button("More info", EditorStyles.miniButton, EditorGuiUtils.OneThirdWidth))
                {
                    Application.OpenURL(string.Format("https://docs.getsocial.im/knowledge-base/android-signing-key-sha256/?utm_source={0}&utm_medium=unity-editor", BuildConfig.PublishTarget));
                }
                if (GUILayout.Button("Refresh", EditorStyles.miniButton, EditorGuiUtils.OneThirdWidth))
                {
                    AssetDatabase.ImportAsset(GetSocialSettings.GetPluginPath() + Path.DirectorySeparatorChar + 
                                              "Editor" + Path.DirectorySeparatorChar + 
                                              "GetSocialSettingsEditor.cs");
                }
                EditorGUILayout.EndHorizontal();
            }
            else 
            {
                EditorGuiUtils.SelectableLabelField(content, GetSocialEditorUtils.SigningKeyHash);
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                if (!GetSocialEditorUtils.UserDefinedKeystore())
                {
                    EditorGUILayout.HelpBox("You are using default Android keystore to sign your application. Are you sure this is what you want?",
                        MessageType.Warning);
                } else if (GetSocialEditorUtils.KeyStoreUtilError != null)
                {
                    EditorGUILayout.HelpBox(GetSocialEditorUtils.KeyStoreUtilError,
                        MessageType.Warning);
                }
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
            else if (_remoteRequestStatusLabel.Contains("resolve"))
            {
                EditorGUILayout.HelpBox(_remoteRequestStatusLabel,
                    MessageType.Error);
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
            }
        }


        void SetUiConfigCustomFilePath(string value)
        {
            if (!value.Equals(GetSocialSettings.UiConfigurationCustomFilePath))
            {
                GetSocialSettings.UiConfigurationCustomFilePath = value;
                UpdateUiConfigFileCheck();
            }
        }

        private void SetAutoRegisterPushEnabled(bool value)
        {
            if (GetSocialSettings.IsAutoRegisrationForPushesEnabled != value)
            {
                GetSocialSettings.IsAutoRegisrationForPushesEnabled = value;
            }
        }

        private void SetForegroundNotificationsEnabled(bool value)
        {
            if (GetSocialSettings.IsForegroundNotificationsEnabled != value)
            {
                GetSocialSettings.IsForegroundNotificationsEnabled = value;
            }
        }

        private void SetShouldWaitForPushListener(bool value)
        {
            if (GetSocialSettings.ShouldWaitForPushListener != value)
            {
                GetSocialSettings.ShouldWaitForPushListener = value;
            }
        }

        private void SetAutoInitEnabled(bool value)
        {
            if (GetSocialSettings.IsAutoInitEnabled != value)
            {
                GetSocialSettings.IsAutoInitEnabled = value;
            }
        }

        private void SetDisableFacebookReferralCheck(bool value)
        {
            if (GetSocialSettings.IsFacebookReferralCheckDisabled != value)
            {
                GetSocialSettings.IsFacebookReferralCheckDisabled = value;
            }
        }

        private void SetRichNotificationsEnabled(bool value)
        {
            if (GetSocialSettings.IsRichPushNotificationsEnabled != value)
            {
                GetSocialSettings.IsRichPushNotificationsEnabled = value;
            }
        }

        private void SetExtensionBundleId(string value)
        {
            if (!GetSocialSettings.ExtensionBundleId.Equals(value))
            {
                GetSocialSettings.ExtensionBundleId = value;
            }
        }

        private void SetExtensionProvisioningProfile(string value)
        {
            if (!GetSocialSettings.ExtensionProvisioningProfile.Equals(value))
            {
                GetSocialSettings.ExtensionProvisioningProfile = value;
            }
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
                        GetSocialSettings.IsAndroidEnabled = remoteConfig.Android.IsEnabled;
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
        
        private static void UpdateUiConfigFileCheck()
        {
            _uiConfigurationValidationResult = GetSocialEditorUtils.CheckCustomUiConfig();
        }
        
        #endregion
    }
}
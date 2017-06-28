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

using UnityEngine;
using UnityEditor;
using GetSocialSdk.Core;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;

namespace GetSocialSdk.Editor
{
    [InitializeOnLoad]
    [CustomEditor(typeof(GetSocialSettings))]
    public class GetSocialSettingsEditor : UnityEditor.Editor
    {
        public const string GetSocialSmartInvitesLinkDomain = "gsc.im";
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

        [UsedImplicitly]
        void OnEnable()
        {
            GetSocialAndroidManifestHelper.Refresh();
            UpdateDefineSymbols();
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

        [MenuItem("GetSocial/Edit Settings")]
        public static void Edit()
        {
            Selection.activeObject = GetSocialSettings.Instance;
        }

        [MenuItem("GetSocial/GetSocial Dashboard")]
        public static void OpenGetSocialMemberCenter()
        {
            Application.OpenURL("http://dashboard.getsocial.im/");
        }

        [MenuItem("GetSocial/Documentation")]
        public static void OpenGetSocialDocumentation()
        {
            Application.OpenURL("http://docs.getsocial.im");
        }

        #region  methods

        void DrawGeneralSettings()
        {
            GetSocialEditorUtils.BeginSetSmallIconSize();
            EditorGUILayout.LabelField(new GUIContent(" General Settings", GetSocialEditorUtils.GetSocialIcon),
                EditorStyles.boldLabel);
            GetSocialEditorUtils.EndSetSmallIconSize();
            EditorGUILayout.BeginVertical(GUI.skin.box);

            DrawAppIdSettings();

            GUILayout.Space(15);
            DrawModuleSettings();

            GUILayout.Space(15);
            DrawPushNotificationSettings();
            
            GUILayout.Space(15);
            DrawDefaultThemeSettings();

            EditorGUILayout.EndVertical();
        }

        void DrawDefaultThemeSettings()
        {
            if (!GetSocialSettings.UseGetSocialUi)
            {
                return;
            }
            var getSocialUiThemeLabel = new GUIContent("UI configuration file path [?]",
                "Add a path to your default UI configuration json file (e.g. getsocial/default.json). Leave empty to use default configuration.");
            EditorGUILayout.LabelField(getSocialUiThemeLabel, EditorStyles.boldLabel);
            var filePath = EditorGUILayout.TextField(GetSocialSettings.UiConfigurationDefaultFilePath);
            
            SetUiConfigDefaultFilePath(filePath);
        }

        void DrawAppIdSettings()
        {
            var getSocialAppKeyLabel = new GUIContent("GetSocial App Id [?]",
                "Get unique App Id on GetSocial Dashboard");
            EditorGUILayout.LabelField(getSocialAppKeyLabel, EditorStyles.boldLabel);
            var newAppKeyValue = EditorGUILayout.TextField(GetSocialSettings.AppId);
            if (!IsDemoAppPackage() && IsDemoAppId())
            {
                EditorGUILayout.HelpBox(
                    "You are using GetSocial test app key with your bundle identifier. Are you sure this is what you want?",
                    MessageType.Warning);
            }
            DrawAppIdValidation(newAppKeyValue);
            SetAppId(newAppKeyValue);
        }

        static bool IsDemoAppPackage()
        {
            return PlayerSettingsCompat.bundleIdentifier == DemoAppPackage;
        }

        static bool IsDemoAppId()
        {
            return GetSocialSettings.AppId == GetSocialSettings.UnityDemoAppAppId;
        }

        void DrawModuleSettings()
        {
            var enabledModulesLabel = new GUIContent("Enabled Modules [?]",
                "If you are not using GetSocial UI, feel free to disable UI module to reduce app size.");
            EditorGUILayout.LabelField(enabledModulesLabel, EditorStyles.boldLabel);

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ToggleLeft(" GetSocial Core", true);
            EditorGUI.EndDisabledGroup();

            var newIsGetSocialUiEnabledValue =
                EditorGUILayout.ToggleLeft(" GetSocial UI", GetSocialSettings.UseGetSocialUi);
            SetGetSocialUiEnabled(newIsGetSocialUiEnabledValue);

        }

        private void DrawPushNotificationSettings()
        {
            var enablePushNotificationAutoRegistering = new GUIContent("Push Notifications [?]",
                "If this setting is checked, GetSocial push notifications will be registered automatically, if not, you need to call GetSocial.RegisterForPushNotification() method.");
            EditorGUILayout.LabelField(enablePushNotificationAutoRegistering, EditorStyles.boldLabel);

            var isAutoRegisrationForPushesEnabled =
                EditorGUILayout.ToggleLeft("Register Automatically", GetSocialSettings.IsAutoRegisrationForPushesEnabled);
            SetAutoRegisterPushEnabled(isAutoRegisrationForPushesEnabled);
        }

        static void DrawAndroidSettings()
        {
            var androidSettingsText = EditorGuiUtils.GetBoldLabel(" Android Settings [?]",
                "These settings will modify your AndroidManifest.xml", GetSocialEditorUtils.AndroidIcon);
            GetSocialEditorUtils.BeginSetSmallIconSize();
            ShowAndroidSettings = EditorGUILayout.Foldout(ShowAndroidSettings, androidSettingsText);
            GetSocialEditorUtils.EndSetSmallIconSize();
            if (ShowAndroidSettings)
            {
                GetSocialAndroidManifestHelper.DrawManifestCheckerGUI();
            }
        }

        void DrawIosSettings()
        {
            var iosSettingsText = EditorGuiUtils.GetBoldLabel(" iOS Settings [?]", "These settings will modify your app.entitlements", GetSocialEditorUtils.IOSIcon);
            GetSocialEditorUtils.BeginSetSmallIconSize();
            ShowIosSettings = EditorGUILayout.Foldout(ShowIosSettings, iosSettingsText);
            GetSocialEditorUtils.EndSetSmallIconSize();
            if (ShowIosSettings)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);

                EditorGUILayout.HelpBox("Please note. Settings below should match settings on the GetSocial Dashboard to make push notifications and deeplinking work", MessageType.Info);
                GUILayout.Space(15);
                DrawIosPushNotificationSettings();
                DrawIosDeeplinkingSettings();

                EditorGUILayout.EndVertical();
            }
        }

        private void DrawIosPushNotificationSettings()
        {
            var isProdAps = new GUIContent("Apple Push Services environment [?]", "Configuration for GetSocial Push Notifications");
            EditorGUILayout.LabelField(isProdAps, EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            GetSocialSettings.IosProductionAps = !EditorGUILayout.ToggleLeft("Sandbox", !GetSocialSettings.IosProductionAps);
            GetSocialSettings.IosProductionAps = EditorGUILayout.ToggleLeft("Production", GetSocialSettings.IosProductionAps);
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(15);
        }

        private static void DrawIosDeeplinkingSettings()
        {
            EditorGUILayout.LabelField("Smart Invites link format", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            GetSocialSettings.UseCustomDomainForDeeplinking = !EditorGUILayout.ToggleLeft("GetSocial domain (default)", !GetSocialSettings.UseCustomDomainForDeeplinking);
            GetSocialSettings.UseCustomDomainForDeeplinking = EditorGUILayout.ToggleLeft("Custom domain", GetSocialSettings.UseCustomDomainForDeeplinking);
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(15);

            if (GetSocialSettings.UseCustomDomainForDeeplinking)
            {
                EditorGUILayout.LabelField("Custom domain", EditorStyles.boldLabel);

                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                GetSocialSettings.CustomDomainForDeeplinking = EditorGUILayout.TextField(GetSocialSettings.CustomDomainForDeeplinking);
                EditorGUILayout.LabelField("/XXXXXX");
                EditorGUILayout.EndHorizontal();
            }

            var getSocialDomainPrefixLabel = GetSocialSettings.UseCustomDomainForDeeplinking
                ? new GUIContent("Fallback url (GetSocial domain prefix) [?]", "If you use custom domain users will never see this url, but internally we use it in some cases for redirects")
                : new GUIContent("GetSocial domain prefix");
            EditorGUILayout.LabelField(getSocialDomainPrefixLabel, EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            GetSocialSettings.GetSocialDomainPrefixForDeeplinking = EditorGUILayout.TextField(GetSocialSettings.GetSocialDomainPrefixForDeeplinking);
            EditorGUILayout.LabelField(string.Format(".{0}/XXXXXX", GetSocialSmartInvitesLinkDomain));
            EditorGUILayout.EndHorizontal();
        }

        static void DrawAdditionalInfo()
        {
            GetSocialEditorUtils.BeginSetSmallIconSize();
            EditorGUILayout.LabelField(new GUIContent(" SDK Info", GetSocialEditorUtils.InfoIcon),
                EditorStyles.boldLabel);
            GetSocialEditorUtils.EndSetSmallIconSize();

            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            EditorGUILayout.BeginVertical();
            EditorGuiUtils.SelectableLabelField(new GUIContent("SDK Version [?]", "GetSocial SDK Version"),
                BuildConfig.UnitySdkVersion);
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        void DrawAppIdValidation(string newAppIdValue)
        {
            var isEmpty = string.IsNullOrEmpty(newAppIdValue);
            if (isEmpty)
            {
                EditorGUILayout.HelpBox("Missing app id? Don't worry, get one at the GetSocial Dashboard",
                    MessageType.Info);
            }
            else
            {
                const int validAppKeyLength = 40;
                var hasAppKeyLength = newAppIdValue.Length == validAppKeyLength;
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
                GetSocialAndroidManifestHelper.Refresh();
            }
        }


        void SetUiConfigDefaultFilePath(string value)
        {
            if (!value.Equals(GetSocialSettings.UiConfigurationDefaultFilePath))
            {
                GetSocialSettings.UiConfigurationDefaultFilePath = value;
                GetSocialAndroidManifestHelper.UpdateDefaultUiConfigurationFilePath();
            }
        }

        private void SetAutoRegisterPushEnabled(bool value)
        {
            if (GetSocialSettings.IsAutoRegisrationForPushesEnabled != value)
            {
                GetSocialSettings.IsAutoRegisrationForPushesEnabled = value;
                UpdateAndroidAutoRegisterPushEnabled();
            }
        }

        private void UpdateAndroidAutoRegisterPushEnabled()
        {
            GetSocialAndroidManifestHelper.UpdateAutoRegisterForPush();
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

        #endregion
    }
}
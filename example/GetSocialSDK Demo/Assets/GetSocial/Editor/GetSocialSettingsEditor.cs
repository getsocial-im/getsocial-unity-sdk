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
using System.Net;
using System;

namespace GetSocialSdk.Editors
{
    [InitializeOnLoad]
    [CustomEditor(typeof(GetSocialSettings))]
    public class GetSocialSettingsEditor : Editor
    {
        private const string UnityTestAppApplicationKey = "KEiqY3xIWix8CnAPPW9m0000000ti70h8r9x8Q74";
        private const string TestAppPackageTesting = "im.getsocial.testapp.unity.testing";
        private const string TestAppPackageMaster = "im.getsocial.testapp.unity.master";

        // Check for updates
        private const string LatestReleaseApiURL = "https://api.github.com/repos/getsocial-im/getsocial-unity-sdk/releases/latest";
        private const string LatestReleaseURL = "https://github.com/getsocial-im/getsocial-unity-sdk/releases/latest";
        private const string NewVersionAvailableFormat = "GetSocial plugin v{0} is now available!";

        private enum NewVersionCheckSource
        {
            OnLoad,
            OnUserCheckedForUpdate
        }

        // If you got to many top level menus in Unity, update this to move GetSocial menu around
        // e.g. set it to "Window/" to move GetSocial menu to "Window -> GetSocial"
        private const string GetSocialMenuParent = "";

        private const char DefineSymbolsSeparator = ';';
        private readonly BuildTargetGroup[] BuildTargetGroups = {BuildTargetGroup.Android, BuildTargetGroup.iPhone};
        private const string EnableGetSocialChatDefineSymbol = "ENABLE_GETSOCIAL_CHAT";

        private const string GetSocialJarSearchPattern = "getsocial-android-sdk*.jar";
        private const string CoreJarNamePrefix = "getsocial-android-sdk-core";
        private const string ChatJarNamePrefix = "getsocial-android-sdk-chat";

        private const string JarsSourcePath = "Assets/GetSocial/Editor/Android/Libraries/";
        private const string JarsSourceDependenciesPath = JarsSourcePath + "Dependencies";
        
        private const string JarsDestinationPath = "Assets/Plugins/Android/";

        private const string ShowAdvancedSettingsEditorPref = "ShowAdvancedSettings";
        private const string ShowIOSSettingsEditorPref = "ShowIOSSettings";
        private const string ShowAndroidSettingsEditorPref = "ShowAndroidSettings";

        private static bool ShowAdvancedSettings
        {
            set { EditorPrefs.SetBool(ShowAdvancedSettingsEditorPref, value); }
            get { return EditorPrefs.GetBool(ShowAdvancedSettingsEditorPref); }
        }
        private static bool ShowIOSSettings
        {
            set { EditorPrefs.SetBool(ShowIOSSettingsEditorPref, value); }
            get { return EditorPrefs.GetBool(ShowIOSSettingsEditorPref); }
        }
        private static bool ShowAndroidSettings
        {
            set { EditorPrefs.SetBool(ShowAndroidSettingsEditorPref, value); }
            get { return EditorPrefs.GetBool(ShowAndroidSettingsEditorPref); }
        }

        static GetSocialSettingsEditor()
        {
            CheckForUpdatesOnReleaseRepo(NewVersionCheckSource.OnLoad);
        }

        void OnEnable()
        {
            AndroidManifestHelper.Refresh();
        }

        public override void OnInspectorGUI()
        {
            if(!IsProjectConfigurationValid())
            {
                DrawFixProjectConfigurationMessage();
            }
            
            DrawGeneralSettings();
            
            GUILayout.Space(15);
            DrawAdvancedSettings();

            GUILayout.Space(15);
            DrawIOSSettings();
            
            GUILayout.Space(15);
            DrawAndroidSettings();
            
            GUILayout.Space(15);
            DrawAdditionalInfo();
        }

        [MenuItem(GetSocialMenuParent + "GetSocial/Edit Settings")]
        public static void Edit()
        {
            Selection.activeObject = GetSocialSettings.Instance;
        }

        [MenuItem(GetSocialMenuParent + "GetSocial/GetSocial Dashboard")]
        public static void OpenGetSocialMemberCenter()
        {
            Application.OpenURL("http://developers.getsocial.im/");
        }

        [MenuItem(GetSocialMenuParent + "GetSocial/Documentation")]
        public static void OpenGetSocialDocumentation()
        {
            Application.OpenURL("http://docs.getsocial.im");
        }

        #region check_for_updates
        [MenuItem(GetSocialMenuParent + "GetSocial/Check for Updates...", false, priority: 2000)]
        public static void CheckForUpdates()
        {
            CheckForUpdatesOnReleaseRepo(NewVersionCheckSource.OnUserCheckedForUpdate);
        }

        private static string GetLastReleaseVersion()
        {
            HttpWebRequest request = WebRequest.Create(LatestReleaseApiURL) as HttpWebRequest;
            
            request.Method = "GET";
            request.UserAgent = "Unity Editor";
            request.Proxy = null;
            request.KeepAlive = false;
            request.Accept = "application/vnd.github.v3+json";

            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string jsonResponse = reader.ReadToEnd().ToString();
                        var lastReleaseTag = new JSONObject(jsonResponse).GetField("tag_name").str;
                        return lastReleaseTag.Replace("v", string.Empty);
                    }
                }
            }
            catch (Exception)
            {
                // Return the current vesion if failed
                return BuildConfig.UnitySdkVersion;
            }
        }

        private static void CheckForUpdatesOnReleaseRepo(NewVersionCheckSource source)
        {
            var latestReleaseTag = GetLastReleaseVersion();
            if(source == NewVersionCheckSource.OnLoad)
            {
                LogNewVersionAvailable(latestReleaseTag);
            }
            else
            {
                ShowUpdateDialog(latestReleaseTag);
            }
        }

        private static void LogNewVersionAvailable(string latestReleaseTag)
        {
            if(!IsInstalledVersionLatest(latestReleaseTag))
            {
                Debug.Log(string.Format(NewVersionAvailableFormat, latestReleaseTag) + ". Go to GetSocial -> Check for Updates to download the latest version");
            }
        }

        private static void ShowUpdateDialog(string latestReleaseVersion)
        {
            var dialogTitle = "Update GetSocial SDK";
            if(IsInstalledVersionLatest(latestReleaseVersion))
            {
                EditorUtility.DisplayDialog(dialogTitle, "You already have the latest version installed (" + latestReleaseVersion + ")", "OK");
            }
            else
            {
                var downloadUpdate = EditorUtility.DisplayDialog(dialogTitle, 
                                        string.Format(NewVersionAvailableFormat, latestReleaseVersion), "Download Update", "Cancel");
                if(downloadUpdate)
                {
                    Application.OpenURL(LatestReleaseURL);
                }
            }
        }

        private static bool IsInstalledVersionLatest(string latestVersion)
        {
            return latestVersion == BuildConfig.UnitySdkVersion;
        }
        #endregion

        #region private methods
        private bool IsProjectConfigurationValid()
        {
            return IsDefineSymbolsConfigurationValid() && IsJarLibrariesConfigurationValid();
        }

        private bool IsDefineSymbolsConfigurationValid()
        {
            bool isValid = true;
            
            foreach(var targetGroup in BuildTargetGroups)
            {
                var defineSymbolsString = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
                var defineSymbols = new List<string>(defineSymbolsString.Split(DefineSymbolsSeparator));
                
                var containsEnableChatSymbol = defineSymbols.Contains(EnableGetSocialChatDefineSymbol);
                isValid &= containsEnableChatSymbol == GetSocialSettings.IsChatEnabled;
            }
            
            return isValid;
        }

        private bool IsJarLibrariesConfigurationValid()
        {
            bool isValid = true;
            
            var getSocialJarsInPlugins = FindGetSocialDestinationJars();

            var isCoreLibraryExists = getSocialJarsInPlugins.Any(filePath => filePath.Contains(CoreJarNamePrefix));
            isValid &= isCoreLibraryExists;
            
            var isChatLibraryExists = getSocialJarsInPlugins.Any(filePath => filePath.Contains(ChatJarNamePrefix));
            isValid &= GetSocialSettings.IsChatEnabled == isChatLibraryExists;
            
            return isValid;
        }

        void DrawFixProjectConfigurationMessage()
        {
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            
            EditorGUILayout.LabelField("Looks like project is misconfigured");
            
            var backupColor = GUI.color;
            GUI.color = Color.green;
            if(GUILayout.Button("Fix it"))
            {
                UpdateDefineSymbols();
                UpdateAndroidLibraries();
            }
            GUI.color = backupColor;
            
            EditorGUILayout.EndHorizontal();
        }

        void DrawGeneralSettings()
        {
            GetSocialEditorUtils.BeginSetSmallIconSize();
            EditorGUILayout.LabelField(new GUIContent(" General Settings", GetSocialEditorUtils.GetSocialIcon), EditorStyles.boldLabel);
            GetSocialEditorUtils.EndSetSmallIconSize();
            EditorGUILayout.BeginVertical (GUI.skin.box);

            DrawAppKeySettings ();

            GUILayout.Space (15);
            DrawModuleSettings ();

            EditorGUILayout.EndVertical ();
        }

        private void DrawAppKeySettings()
        {
            var getSocialAppKeyLabel = new GUIContent("GetSocial App Key [?]", "Get unique App Key on GetSocial Dashboard");
            EditorGUILayout.LabelField(getSocialAppKeyLabel, EditorStyles.boldLabel);
            var newAppKeyValue = EditorGUILayout.TextField(GetSocialSettings.AppKey);
            if(!IsTestAppPackage() && IsAppKeyGetSocialTestAppKey())
            {
                EditorGUILayout.HelpBox("You are using GetSocial test app key with your bundle identifier. Are you sure this is what you want?", MessageType.Warning);
            }
            IsAppKeyValid(newAppKeyValue);
            SetAppKey(newAppKeyValue);
        }

        private static bool IsTestAppPackage()
        {
            return PlayerSettings.bundleIdentifier == TestAppPackageTesting || PlayerSettings.bundleIdentifier == TestAppPackageMaster;
        }

        private static bool IsAppKeyGetSocialTestAppKey()
        {
            return GetSocialSettings.AppKey == UnityTestAppApplicationKey;
        }

        private void DrawModuleSettings()
        {
            var enabledModulesLabel = new GUIContent("Enabled Modules [?]", "Disable unused modules to reduce app size");
            EditorGUILayout.LabelField(enabledModulesLabel, EditorStyles.boldLabel);
            
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ToggleLeft(" Core (Activity Feed, Notifications, Smart Invites)", true);
            EditorGUI.EndDisabledGroup();
            
            var newIsChatEnabledValue = EditorGUILayout.ToggleLeft(" Chat", GetSocialSettings.IsChatEnabled);
            SetIsChatEnabled(newIsChatEnabledValue);
        }

        void DrawIOSSettings()
        {
            var iosSettingsText = GetBoldLabel(" IOS Specific Settings [?]", "These are settings specific to IOS", GetSocialEditorUtils.IOSIcon);
            GetSocialEditorUtils.BeginSetSmallIconSize();
            ShowIOSSettings = EditorGUILayout.Foldout(ShowIOSSettings, iosSettingsText);
            GetSocialEditorUtils.EndSetSmallIconSize();
            if(ShowIOSSettings)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                var isAutoRegisrationForPushesEnabledIOS = EditorGUILayout.ToggleLeft(" Enable automatic registration for push notifications on GetSocial initialization", GetSocialSettings.IsAutoRegisrationForPushesEnabledIOS);
                SetIsAutomaticRegistrationForPushesOnIOSEnabled(isAutoRegisrationForPushesEnabledIOS);
                EditorGUILayout.EndVertical();
            }
        }

        private void DrawAndroidSettings()
        {
            var androidSettingsText = GetBoldLabel(" AndroidManifest.xml and other Android Settings [?]", "These settings will modify your AndroidManifest.xml", GetSocialEditorUtils.AndroidIcon);
            GetSocialEditorUtils.BeginSetSmallIconSize();
            ShowAndroidSettings = EditorGUILayout.Foldout(ShowAndroidSettings, androidSettingsText);
            GetSocialEditorUtils.EndSetSmallIconSize();
            if(ShowAndroidSettings)
            {
                AndroidManifestHelper.DrawManifestCheckerGUI();
            }
        }

        private void DrawAdvancedSettings()
        {
            GetSocialEditorUtils.BeginSetSmallIconSize();
            ShowAdvancedSettings = EditorGUILayout.Foldout(ShowAdvancedSettings, GetBoldLabel(" Advanced Settings", string.Empty, GetSocialEditorUtils.SettingsIcon));
            GetSocialEditorUtils.EndSetSmallIconSize();
            if(ShowAdvancedSettings)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                var newLogMethodCallsValue = EditorGUILayout.ToggleLeft(" Enable Debug Logs", GetSocialSettings.IsDebugLogsEnabled);
                SetLogMethodCallsInEditor(newLogMethodCallsValue);
                EditorGUILayout.EndVertical();
            }
        }

        void DrawAdditionalInfo()
        {
            GetSocialEditorUtils.BeginSetSmallIconSize();
            EditorGUILayout.LabelField(new GUIContent(" SDK Info", GetSocialEditorUtils.InfoIcon), EditorStyles.boldLabel);
            GetSocialEditorUtils.EndSetSmallIconSize();
            
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            EditorGUILayout.BeginVertical();
            SelectableLabelField(new GUIContent("SDK Version [?]", "GetSocial SDK Version"), BuildConfig.UnitySdkVersion);
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        private bool IsAppKeyValid(string newAppKeyValue)
        {
            var isEmpty = string.IsNullOrEmpty(newAppKeyValue);
            if(isEmpty)
            {
                EditorGUILayout.HelpBox("Missing app key? Don't worry, get one at the GetSocial Dashboard", MessageType.Info);
            } 
            else
            {
                const int validAppKeyLength = 40;
                var hasValidLength = newAppKeyValue.Length == validAppKeyLength;
                if(!hasValidLength)
                {
                    EditorGUILayout.HelpBox("Wrong App Key. Check if you copied it properly on the GetSocial Dashboard", MessageType.Error);
                }
                return !isEmpty && hasValidLength;
            }
            
            return false;
        }

        private void SetAppKey(string value)
        {
            if(!string.IsNullOrEmpty(value) && !value.Equals(GetSocialSettings.AppKey))
            {
                GetSocialSettings.AppKey = value;
            }
        }

        private void SetIsChatEnabled(bool value)
        {
            if(GetSocialSettings.IsChatEnabled != value)
            {
                GetSocialSettings.IsChatEnabled = value;
                UpdateDefineSymbols();
                UpdateAndroidLibraries();
            }
        }

        private void SetIsAutomaticRegistrationForPushesOnIOSEnabled(bool value)
        {
            if(GetSocialSettings.IsAutoRegisrationForPushesEnabledIOS != value)
            {
                GetSocialSettings.IsAutoRegisrationForPushesEnabledIOS = value;
            }
        }

        private void SetLogMethodCallsInEditor(bool value)
        {
            if(GetSocialSettings.IsDebugLogsEnabled != value)
            {
                GetSocialSettings.IsDebugLogsEnabled = value;
            }
        }

        private void UpdateDefineSymbols()
        {
            foreach(var targetGroup in BuildTargetGroups)
            {
                var defineSymbolsString = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
                
                var defineSymbols = new List<string>(defineSymbolsString.Split(DefineSymbolsSeparator));
                defineSymbols.RemoveAll(defineSymbol => EnableGetSocialChatDefineSymbol.Equals(defineSymbol));
                
                if(GetSocialSettings.IsChatEnabled)
                {
                    defineSymbols.Add(EnableGetSocialChatDefineSymbol);
                }
                
                var modifiedDefineSymbols = string.Join(DefineSymbolsSeparator.ToString(), defineSymbols.ToArray());
                
                PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, modifiedDefineSymbols);
            }
        }

        private void UpdateAndroidLibraries()
        {
            // delete current jars and corresponding meta files in Plugins/Android folder
            FindGetSocialDestinationJars().ForEach(file => 
            {
                File.Delete(file);
                File.Delete(file + ".meta");
            });

            var getSocialJarsInPlugins = FindGetSocialSourceJars();
            if(getSocialJarsInPlugins.Count == 0)
            {
                Debug.LogError("No GetSocial Android libraries found in " + JarsSourcePath);
                return;
            }
            var coreSourceJar = getSocialJarsInPlugins.Single(x => x.Contains(CoreJarNamePrefix));
            var chatSourceJar = getSocialJarsInPlugins.Single(x => x.Contains(ChatJarNamePrefix));

            Directory.CreateDirectory(JarsDestinationPath);

            var coreDestJar = Path.Combine(JarsDestinationPath, Path.GetFileName(coreSourceJar));
            File.Copy(coreSourceJar, coreDestJar);
            if(GetSocialSettings.IsChatEnabled)
            {
                var chatDestJar = Path.Combine(JarsDestinationPath, Path.GetFileName(chatSourceJar));
                File.Copy(chatSourceJar, chatDestJar);
            }
            
            CopyAndroidDependenciesToPlugins();
            
            AssetDatabase.Refresh();
        }
        
        public static void CopyAndroidDependenciesToPlugins()
        {
            var dependencies = Directory.GetFiles(JarsSourceDependenciesPath, "*.jar", SearchOption.TopDirectoryOnly).ToList();
            dependencies.ForEach(jar => File.Copy(jar, Path.Combine(JarsDestinationPath, Path.GetFileName(jar)), true));
            AssetDatabase.Refresh();
        }
        #endregion

        #region helpers
        private static List<string> FindGetSocialSourceJars()
        {
            return FindGetSocialJars(JarsSourcePath);
        }

        private static List<string> FindGetSocialDestinationJars()
        {
            return Directory.Exists(JarsDestinationPath) ? FindGetSocialJars(JarsDestinationPath) : new List<string>();
        }

        private static List<string> FindGetSocialJars(string path)
        {
            return Directory.GetFiles(path, GetSocialJarSearchPattern, SearchOption.TopDirectoryOnly).ToList();
        }
        #endregion

        #region gui_helpers
        private static void SelectableLabelField(GUIContent label, string value)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, GUILayout.Width(180), GUILayout.Height(16));
            EditorGUILayout.SelectableLabel(value, GUILayout.Height(16));
            EditorGUILayout.EndHorizontal();
        }

        private static GUIContent GetBoldLabel(string text, string tooltip = "", Texture2D icon = null)
        {
            GUIContent label;
            if (icon != null)
            {
                label = new GUIContent(text, icon, tooltip);
            }
            else
            {
                label = new GUIContent(text, tooltip);
            }
            var style = EditorStyles.foldout;
            style.fontStyle = FontStyle.Bold;
            return label;
        }
        #endregion
    }
}

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

using System.IO;
using UnityEditor;
using UnityEngine;
using System;
using GetSocialSdk.Core;

namespace GetSocialSdk.Editor
{
    public static class GetSocialAndroidManifestHelper
    {
        #region constants

        /// <summary>
        /// Relative path to your AndroidManifest.xml file.
        ///
        /// Change it if your manifest is not in the root of Plugins directory.
        /// </summary>
        const string MainManifestPath = "Plugins/Android/AndroidManifest.xml";

        /// <summary>
        /// Relative path to default AndroidManifest.xml file that will be copied to Plugins folder if one does not exist there.
        /// </summary>
        const string DefaultBackupManifestPath = "GetSocial/Editor/Android/BackupManifest/AndroidManifest.xml";

        public static readonly string ManifestPathInProject = Path.Combine(Application.dataPath, MainManifestPath);
        public static readonly string PluginsAndroidPathInProject = Path.Combine(Application.dataPath, "Plugins/Android");

        const char Bullet = '\u2022';

        const string GetSocialDeepLinkingActivityName = "im.getsocial.sdk.unity.GetSocialDeepLinkingActivity";
        const string GetSocialDeepLinkingActivityScheme = "getsocial";

        const string AutoInitContentProviderName = "im.getsocial.sdk.AutoInitSdkContentProvider";
        const string AutoInitContentProviderAuthorityFormat = "{0}.AutoInitSdkContentProvider";

        const string InstallReferrerReceiverName = "im.getsocial.sdk.invites.InstallReferrerReceiver";

        const string ImageContentProviderName = "im.getsocial.sdk.invites.ImageContentProvider";
        const string ImageContentProviderFormat = "{0}.smartinvite.images.provider";

        const string AppIdMetaTagName = "im.getsocial.sdk.AppId";
        const string SdkRuntimeMetaTagName = "im.getsocial.sdk.Runtime";
        const string SdkRuntimeVersionMetaTagName = "im.getsocial.sdk.RuntimeVersion";
        const string SdkWrapperVesrionMetaTagName = "im.getsocial.sdk.WrapperVersion";
        const string AutoRegisterForPushMetaTagName = "im.getsocial.sdk.AutoRegisterForPush";

        #endregion


        #region static fields

        static bool _areBasicPermissionsPresent;
        static bool _areMetaTagsPresent;
        static bool _isAutoInitcontentProviderPresent;
        static bool _isGetSocialDeepLinkingActivityPresent;
        static bool _isImageContentProviderPresent;
        static bool _isInstallReferrerReceiverPresent;
        static bool _areDataApiDependencyLibsPresent;
        static bool _areUiDependencyLibsPresent;

        #endregion

        public static void Refresh()
        {
            if (DoesManifestExist())
            {
                RefreshAllAndroidChecks();
            }
        }

        static void RefreshAllAndroidChecks()
        {
            Debug.Log(string.Format("Rechecking your manifest at \n\t<color=green>{0}</color>", MainManifestPath));

            var androidManifest = new AndroidManifest(ManifestPathInProject);

            // basics
            _areBasicPermissionsPresent = androidManifest.ContainsPermissions(AndroidManifest.InternetPermission, AndroidManifest.AccessNetoworkStatePermission);

            _areMetaTagsPresent = androidManifest.ContainsMetaTag(AppIdMetaTagName, GetSocialSettings.AppId);
            _areMetaTagsPresent &= androidManifest.ContainsMetaTag(SdkRuntimeMetaTagName, "UNITY");
            _areMetaTagsPresent &= androidManifest.ContainsMetaTag(SdkRuntimeVersionMetaTagName, Application.unityVersion);
            _areMetaTagsPresent &= androidManifest.ContainsMetaTag(SdkWrapperVesrionMetaTagName, BuildConfig.UnitySdkVersion);

            _isAutoInitcontentProviderPresent = androidManifest.ContainsContentProvider(AutoInitContentProviderName, string.Format(AutoInitContentProviderAuthorityFormat, PlayerSettingsCompat.bundleIdentifier));

            // deeplinking and referral data
            _isGetSocialDeepLinkingActivityPresent = androidManifest.ContainsDeepLinkingActivity(GetSocialDeepLinkingActivityName, GetSocialDeepLinkingActivityScheme, GetSocialSettings.AppId);

            // smart invites image sharing
            _isImageContentProviderPresent = androidManifest.ContainsContentProvider(ImageContentProviderName, string.Format(ImageContentProviderFormat, PlayerSettingsCompat.bundleIdentifier));

            // install tracking
            _isInstallReferrerReceiverPresent = androidManifest.ContainsReceiver(InstallReferrerReceiverName);
        }


        #region gui

        public static void DrawManifestCheckerGUI()
        {
            DrawCheckIfNotDefaultPackageGUI();

            if (!DoesManifestExist())
            {
                var message = string.Format(
                    "Failed to locate file '{0}'.\n\n" +
                    "Press a button to create new Android manifest or update constant MainManifestPath in GetSocialAndroidManifestHelper.cs with correct path.",
                    MainManifestPath);
                DrawFixProjectConfigurationMessage(message, "Create AndroidManifest.xml", GenerateManifestIfNotPresent);
            }
            else
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.HelpBox("Click the button below to check your AndroidManifest.xml for possible issues.\n\n" +
                                        "Apply proposed updates ONLY if you are actually planning to use feature that requires them", MessageType.Info);
                if (GUILayout.Button("Check Dependencies and AndroidManifest.xml"))
                {
                    RefreshAllAndroidChecks();
                }

                DrawManifestFixProposals();
                EditorGUILayout.EndVertical();
            }
        }

        static void DrawManifestFixProposals()
        {
            FixMessageGroupTitle("GetSocial Dependencies");
            DrawDependencyLibsFixes();

            FixMessageGroupTitle("AndroidManifest.xml checks");
            DrawBasicFixes();
            DrawSmartInvitesFixes();
            DrawDeepLinkingFixes();
        }

        static void DrawBasicFixes()
        {
            Action drawFixPermissionsAction = () => DrawFixProjectConfigurationMessage("You don't have all the required permissions for GetSocial to work", "Add missing permissions", AddRequiredPermissions);
            DrawFixBox(_areBasicPermissionsPresent, "All required permissions added", drawFixPermissionsAction);

            Action drawFixMissingMetaTagsAction = () => DrawFixProjectConfigurationMessage("You don't have required meta tags added for GetSocial to work", "Ensure correct meta tags", AddMetaTags);
            DrawFixBox(_areMetaTagsPresent, "GetSocial meta tags added", drawFixMissingMetaTagsAction);

            Action drawFixAutoInitContentProviderAction = () => DrawFixProjectConfigurationMessage(string.Format("{0} must be present to enable GetSocial SDK auto-initialization", AutoInitContentProviderName), "Ensure correct content provider", AddAutoInitContentProvider);
            DrawFixBox(_isAutoInitcontentProviderPresent, "AutoInitSdkContentProvider added", drawFixAutoInitContentProviderAction);
        }

        static void DrawDeepLinkingFixes()
        {
            Action drawFixAction = () => DrawFixProjectConfigurationMessage(string.Format("{0} must be present and correctly configured for Deeplinking and Referral Data to work", GetSocialDeepLinkingActivityName), "Ensure correct GetSocialDeepLinkingActivity", AddGetSocialDeepLinkingActivity);
            DrawFixBox(_isGetSocialDeepLinkingActivityPresent, "GetSocialDeeplinkingActivity added", drawFixAction);
        }

        static void DrawSmartInvitesFixes()
        {
            Action drawFixImageContentProvider =
                () => DrawFixProjectConfigurationMessage(string.Format("{0} must be present to attach images to Smart Invites", ImageContentProviderName), "Ensure correct ImageContentProvider", AddImageContentProvider);
            DrawFixBox(_isImageContentProviderPresent, "ImageContentProvider added", drawFixImageContentProvider);

            Action drawFixReferrerReceiver =
                () => DrawFixProjectConfigurationMessage(string.Format("{0} must be present to be able to Track Installs and retrieve Referral Data", InstallReferrerReceiverName), "Add InstallReferrerReceiver", AddInstallReferrerReceiver);
            DrawFixBox(_isInstallReferrerReceiverPresent, "InstallReferrerReceiver added", drawFixReferrerReceiver);
        }

        static void DrawDependencyLibsFixes()
        {
            Action drawFixDataApiDependencies =
                () => DrawFixProjectConfigurationMessage("You don't have all GetSocial Data API dependencies libs in your Plugins/Android folder", "Add Libs", () => FixDependencies("gson*"));
            DrawFixBox(_areDataApiDependencyLibsPresent, "All GetSocial Data API dependencies are present", drawFixDataApiDependencies);

            if (GetSocialSettings.UseGetSocialUi)
            {
                Action drawFixUiDependencies =
                    () => DrawFixProjectConfigurationMessage("You don't have all GetSocial UI dependencies libs in your Plugins/Android folder", "Add Libs", () => FixDependencies("picasso*"));
                DrawFixBox(_areUiDependencyLibsPresent, "All GetSocial UI dependencies are present", drawFixUiDependencies);
            }
        }

        #endregion


        #region fix manifest helpers

        /*
         * <uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
         * <uses-permission android:name="android.permission.INTERNET" />
         */
        static void AddRequiredPermissions()
        {
            UseAndroidManifest(androidManifest =>
                androidManifest.AddPermissions(AndroidManifest.InternetPermission, AndroidManifest.AccessNetoworkStatePermission)
            );
        }

        /*
         * <meta-data
         *		android:name="im.getsocial.sdk.AppId"
         *		android:value="s8i7N997ga98n" />
         * <meta-data
         *		android:name="im.getsocial.sdk.Runtime"
         *		android:value="UNITY" />
         * <meta-data
         *		android:name="im.getsocial.sdk.RuntimeVersion"
         *		android:value="5.2.1f1" />
         * <meta-data
         *		android:name="im.getsocial.sdk.WrapperVersion"
         *		android:value="5.0.0" />
         */
        static void AddMetaTags()
        {
            UseAndroidManifest(androidManifest =>
            {
                androidManifest.AddMetaTag(AppIdMetaTagName, GetSocialSettings.AppId);
                androidManifest.AddMetaTag(SdkRuntimeMetaTagName, "UNITY");
                androidManifest.AddMetaTag(SdkRuntimeVersionMetaTagName, Application.unityVersion);
                androidManifest.AddMetaTag(SdkWrapperVesrionMetaTagName, BuildConfig.UnitySdkVersion);
            });
        }

        /*
         * <provider
         *		android:authorities="${applicationId}.AutoInitSdkContentProvider"
         *		android:exported="false"
         *		android:enabled="true"
         *		android:name="im.getsocial.sdk.AutoInitSdkContentProvider"/>
         */
        static void AddAutoInitContentProvider()
        {
            UseAndroidManifest(androidManifest =>
                androidManifest.AddContentProvider(AutoInitContentProviderName, string.Format(AutoInitContentProviderAuthorityFormat, PlayerSettingsCompat.bundleIdentifier))
            );
        }

        /*
         * <activity android:name="im.getsocial.sdk.core.unity.GetSocialDeepLinkingActivity" android:exported="true">
         *     <intent-filter>
         *         <action android:name="android.intent.action.VIEW" />
         *         <category android:name="android.intent.category.DEFAULT" />
         *         <category android:name="android.intent.category.BROWSABLE" />
         *         <data android:scheme="getsocial" android:host="ti70h8r9x8Q74" />
         *     </intent-filter>
         * </activity>
         */
        static void AddGetSocialDeepLinkingActivity()
        {
            UseAndroidManifest(androidManifest =>
                androidManifest.AddDeepLinkingActivity(GetSocialDeepLinkingActivityName, GetSocialDeepLinkingActivityScheme, GetSocialSettings.AppId)
            );
        }

        /*
         * <provider
         *     android:name="im.getsocial.sdk.invites.ImageContentProvider"
         *     android:authorities="${applicationId}.smartinvite.images.provider"
         *     android:exported="true"/>
         */
        static void AddImageContentProvider()
        {
            UseAndroidManifest(androidManifest =>
                androidManifest.AddContentProvider(ImageContentProviderName, string.Format(ImageContentProviderFormat, PlayerSettingsCompat.bundleIdentifier), true)
            );
        }

        /*
         *   <receiver
         *       android:name="im.getsocial.sdk.invites.InstallReferrerReceiver">
         *       <intent-filter>
         *           <action android:name="com.android.vending.INSTALL_REFERRER" />
         *       </intent-filter>
         *   </receiver>
         */
        static void AddInstallReferrerReceiver()
        {
            UseAndroidManifest(androidManifest =>
                androidManifest.AddInstallReferrerReceiver(InstallReferrerReceiverName)
            );
        }

        #endregion


        #region libs dependencies

        static bool CheckIfDependencyLibsPresent(params string[] dependencies)
        {
            var libJars = GetSocialEditorUtils.GetFiles(PluginsAndroidPathInProject, string.Join(";", dependencies), SearchOption.AllDirectories);
            return libJars.Length >= dependencies.Length;
        }

        static void FixDependencies(params string[] dependencies)
        {
            GetSocialSettingsEditor.CopyAndroidDependenciesToPlugins();
            RefreshAllAndroidChecks();
        }

        #endregion


        #region draw ui helpers

        static void DrawFixBox(bool isFixed, string correctMsg, Action drawFixAction)
        {
            if (isFixed)
            {
                DrawAllCorrectBox(correctMsg);
            }
            else
            {
                drawFixAction();
            }
        }

        public static void DrawCheckIfNotDefaultPackageGUI()
        {
            var package = PlayerSettingsCompat.bundleIdentifier;
            if (string.IsNullOrEmpty(package))
            {
                EditorGUILayout.HelpBox("Package name is empty, please specify it in Android Player settings", MessageType.Warning);
            }

            if (package == "com.Company.ProductName")
            {
                EditorGUILayout.HelpBox("Please change the default Unity Bundle Identifier (com.Company.ProductName) to your package", MessageType.Warning);
            }
        }

        static void FixMessageGroupTitle(string title)
        {
            GUILayout.Space(7f);
            EditorGUILayout.LabelField(Bullet + " " + title + ":", EditorStyles.boldLabel);
        }

        static void DrawFixProjectConfigurationMessage(string message, string fixButtonText, Action fixAction)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Space(5f);
            var labelStyle = EditorStyles.label;
            labelStyle.wordWrap = true;
            EditorGUILayout.LabelField(message, labelStyle);

            var backupColor = GUI.color;
            GUI.color = Color.green;
            EditorGUILayout.Space();
            if (GUILayout.Button(fixButtonText))
            {
                fixAction();
            }
            GUI.color = backupColor;
            EditorGUILayout.EndVertical();
        }

        static void DrawAllCorrectBox(string text)
        {
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            GUILayout.Space(5f);
            var backupColor = GUI.color;
            GUI.color = Color.green;
            EditorGUILayout.LabelField(text);
            GUI.color = backupColor;

            EditorGUILayout.EndHorizontal();
        }

        #endregion


        #region android manifest helpers

        static void UseAndroidManifest(Action<AndroidManifest> action)
        {
            var androidManifest = new AndroidManifest(ManifestPathInProject);

            action(androidManifest);

            androidManifest.Save();

            RefreshAllAndroidChecks();
            AssetDatabase.Refresh();
        }

        static void GenerateManifestIfNotPresent()
        {
            // only copy over a fresh copy of the AndroidManifest if one does not exist
            if (!DoesManifestExist())
            {
                Debug.Log("AndroidManifest.xml does not exist in Plugins folder, creating one for you...");
                var inputFile = Path.Combine(Application.dataPath, DefaultBackupManifestPath);
                File.Copy(inputFile, ManifestPathInProject);
            }
            AssetDatabase.Refresh();
            RefreshAllAndroidChecks();
        }

        static bool DoesManifestExist()
        {
            return File.Exists(ManifestPathInProject);
        }

        #endregion

        #region push_notifications

        public static void UpdateAutoRegisterForPush()
        {
            UseAndroidManifest(androidManifest =>
            {
                androidManifest.AddMetaTag(AutoRegisterForPushMetaTagName, GetSocialSettings.IsAutoRegisrationForPushesEnabled.ToString().ToLower());
            });
        }

        #endregion
    }
}
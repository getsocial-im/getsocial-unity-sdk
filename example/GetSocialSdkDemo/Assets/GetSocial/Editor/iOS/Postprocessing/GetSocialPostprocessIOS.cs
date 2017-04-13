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
using System.IO;
using UnityEditor.iOS.Xcode.GetSocial;
using GetSocialSdk.Core;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace GetSocialSdk.Editor
{
    public static class GetSocialPostprocessIOS
    {
        // modify this constant if GetSocial is not subdirectory of the Assets/
        public const string RootGetSocialPath = "";

        public static void UpdateXcodeProject(string projectPath)
        {
            Debug.Log(string.Format("GetSocial Xcode postprocessing started for project '{0}'", projectPath));

            PBXProjectUtils.ModifyPbxProject(projectPath, (project, target) =>
            {
                AddOtherLinkerFlags(project, target);
                SetupDeepLinking(project, projectPath, target);
                EmbedFrameworks(project, target);
                AddStripFrameworksScriptBuildPhase(project, target);
                RemoveUiPluginFiles(project, target);
            });

            PBXProjectUtils.ModifyPlist(projectPath, (plistDocument) =>
            {
                AddGetSocialAppId(plistDocument);
                AddAnalyticsSuperPropertiesMetaData(plistDocument);
                WhitelistApps(plistDocument);
                SetAutoRegisterForPushTag(plistDocument);
            });
        }

        static void RemoveUiPluginFiles(PBXProject project, string target)
        {
            if (GetSocialSettings.UseGetSocialUi)
            {
                return;
            }

            Debug.Log("Removing GetSocial UI files from build as Native UI is disabled");
            var uiBridgeGuid =
                project.FindFileGuidByProjectPath(
                    "Libraries/GetSocial/Editor/iOS/GetSocialUI/GetSocialUIUnityBridge.mm");
            if (uiBridgeGuid == null)
            {
                Debug.Log("GetSocialUIUnityBridge.mm not found in the project.");
                return;
            }

            project.RemoveFileFromBuild(target, uiBridgeGuid);
        }

        static void AddOtherLinkerFlags(PBXProject project, string target)
        {
            project.UpdateBuildProperty(target, "OTHER_LDFLAGS", new[]
            {
                "-ObjC",
                "-licucore"
            }, new string[] { });
        }

        static void AddStripFrameworksScriptBuildPhase(PBXProject project, string target)
        {
            project.AddShellScript(target,
                @"bash ""$BUILT_PRODUCTS_DIR/$FRAMEWORKS_FOLDER_PATH/GetSocial.framework/strip_frameworks.sh""");
        }

        static void EmbedFrameworks(PBXProject project, string target)
        {
            const string defaultLocationInProj = "Frameworks/Plugins/iOS/GetSocial";
            const string coreFrameworkName = "GetSocial.framework";
            const string uiFrameworkName = "GetSocialUI.framework";
            var relativeCoreFrameworkPath = Path.Combine(defaultLocationInProj, coreFrameworkName);
            var relativeUiFrameworkPath = Path.Combine(defaultLocationInProj, uiFrameworkName);
            project.AddDynamicFrameworkToProject(target, relativeCoreFrameworkPath);
            project.AddDynamicFrameworkToProject(target, relativeUiFrameworkPath);
            Debug.Log("GetSocial Dynamic Frameworks added to Embedded binaries.");
        }

        #region deep_linking

        static void SetupDeepLinking(PBXProject project, string projectPath, string target)
        {
            Debug.LogWarning(
                "Setting up deep linking...\n\tFor universal links setup please refer to http://docs.getsocial.im/?platform=unity#using-universal-links");

            // URL Schemes (iOS <= 8)
            AddGetSocialUrlScheme(projectPath);

            // App links (iOS >=9 )
            AddAppEntitlements(projectPath, project, target);
        }

        static void AddGetSocialUrlScheme(string projectPath)
        {
            Debug.Log(string.Format("Setting up GetSocial deep linking for iOS <= 8 for '{0}'", projectPath));
            PBXProjectUtils.ModifyPlist(projectPath, AddGetSocialUrlSchemeToPlist,
                "Failed to set up GetSocial deep linking for iOS <= 8.");
        }

        static void AddGetSocialUrlSchemeToPlist(PlistDocument plistInfoFile)
        {
            const string CFBundleURLTypes = "CFBundleURLTypes";
            const string CFBundleURLSchemes = "CFBundleURLSchemes";

            if (!plistInfoFile.ContainsKey(CFBundleURLTypes))
            {
                plistInfoFile.root.CreateArray(CFBundleURLTypes);
            }

            var cFBundleURLTypesElem = plistInfoFile.root.values[CFBundleURLTypes] as PlistElementArray;

            var getSocialUrlSchemesArray = new PlistElementArray();
            getSocialUrlSchemesArray.AddString(string.Format("getsocial-{0}", GetSocialSettings.AppId));

            if (cFBundleURLTypesElem != null)
            {
                var getSocialSchemeElem = cFBundleURLTypesElem.AddDict();
                getSocialSchemeElem.values[CFBundleURLSchemes] = getSocialUrlSchemesArray;
            }
        }

        static void AddAppEntitlements(string projectPath, PBXProject project, string target)
        {
            const string appEntitlementsFilePath = "GetSocial/Editor/iOS/Files/app.entitlements";

            var appEntitlementsFilePathCombined = Path.Combine(RootGetSocialPath, appEntitlementsFilePath);
            var unityProjectPath = Path.Combine(Application.dataPath, appEntitlementsFilePathCombined);
            var xcodeProjectPath = Path.Combine(projectPath, "app.entitlements");

            GenerateAppEntitlements(unityProjectPath);

            // to avoid errors when building with "Append" option
            if (!File.Exists(xcodeProjectPath))
            {
                File.Copy(unityProjectPath, xcodeProjectPath);
            }

            project.AddFileToBuild(target,
                project.AddFile("app.entitlements", "app.entitlements", PBXSourceTree.Source));
            project.AddBuildProperty(target, "CODE_SIGN_ENTITLEMENTS", "app.entitlements");
        }

        public static void GenerateAppEntitlements(string path)
        {
            var entitlements = new PlistDocument();
            var assosiatedDomainsNode = entitlements.root.CreateArray("com.apple.developer.associated-domains");

            var aps = GetSocialSettings.IosProductionAps ? "production" : "development";
            entitlements.root.SetString("aps-environment", aps);

            GetRequiredAssosiatedDomains().ForEach(domain => assosiatedDomainsNode.AddString(domain));

            entitlements.WriteToFile(path);
        }

        private static List<string> GetRequiredAssosiatedDomains()
        {
            var assosiatedDomains = new List<string>();

            assosiatedDomains.Add(string.Format("applinks:{0}.{1}", GetSocialSettings.GetSocialDomainPrefixForDeeplinking,
                GetSocialSettingsEditor.GetSocialSmartInvitesLinkDomain));

            var domain0 = GetSocialSettings.UseCustomDomainForDeeplinking
                ? string.Format("applinks:{0}", GetSocialSettings.CustomDomainForDeeplinking)
                : string.Format("applinks:{0}-gsalt.{1}", GetSocialSettings.GetSocialDomainPrefixForDeeplinking, GetSocialSettingsEditor.GetSocialSmartInvitesLinkDomain);
            assosiatedDomains.Add(domain0);

            // add assosiated domains for testing environement for demo app
            if (PlayerSettings.iPhoneBundleIdentifier.Equals(GetSocialSettingsEditor.DemoAppPackage))
            {
                assosiatedDomains.Add(string.Format("applinks:{0}.testing.{1}", GetSocialSettings.GetSocialDomainPrefixForDeeplinking,
                    GetSocialSettingsEditor.GetSocialSmartInvitesLinkDomain));
                assosiatedDomains.Add(string.Format("applinks:{0}-gsalt.testing.{1}", GetSocialSettings.GetSocialDomainPrefixForDeeplinking,
                    GetSocialSettingsEditor.GetSocialSmartInvitesLinkDomain));
            }

            return assosiatedDomains;
        }

        #endregion

        private static void AddGetSocialAppId(PlistDocument plistDocument)
        {
            plistDocument.root.SetString("im.getsocial.sdk.AppId", GetSocialSettings.AppId);
        }

        private static void AddAnalyticsSuperPropertiesMetaData(PlistDocument plistDocument)
        {
            plistDocument.root.SetString("im.getsocial.sdk.Runtime", "UNITY");
            plistDocument.root.SetString("im.getsocial.sdk.RuntimeVersion", Application.unityVersion);
            plistDocument.root.SetString("im.getsocial.sdk.WrapperVersion", BuildConfig.UnitySdkVersion);
        }

        static void WhitelistApps(PlistDocument plistInfoFile)
        {
            const string LSApplicationQueriesSchemes = "LSApplicationQueriesSchemes";
            string[] fbApps =
            {
                "fbapi",
                "fbapi20130214",
                "fbapi20130410",
                "fbapi20130702",
                "fbapi20131010",
                "fbapi20131219",
                "fbapi20140410",
                "fbapi20140116",
                "fbapi20150313",
                "fbapi20150629",
                "fbauth",
                "fbauth2",
                "fb-messenger-api20140430",
                "fb-messenger-api",
                "fbshareextension"
            };

            string[] otherApps =
            {
                "kik-share",
                "kakaolink",
                "line",
                "whatsapp"
            };

            var appsArray = plistInfoFile.root.CreateArray(LSApplicationQueriesSchemes);
            fbApps.ToList().ForEach(appsArray.AddString);
            otherApps.ToList().ForEach(appsArray.AddString);
        }

        private static void SetAutoRegisterForPushTag(PlistDocument plistDocument)
        {
            plistDocument.root.SetBoolean("im.getsocial.sdk.AutoRegisterForPush", GetSocialSettings.IsAutoRegisrationForPushesEnabled);
        }
    }
}
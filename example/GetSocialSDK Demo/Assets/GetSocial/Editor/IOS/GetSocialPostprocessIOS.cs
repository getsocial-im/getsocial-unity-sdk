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
using UnityEditor.iOS.Xcode;
using GetSocialSdk.Core;
using System;

namespace GetSocialSdk.Editors
{
    public static class GetSocialPostprocessIOS
    {
        // modify this constant if GetSocial is not subdirectory of the Assets/
        public const string RootGetSocialPath = "";

        private const string BaseFrameworkSourcePath = RootGetSocialPath + "GetSocial/Editor/IOS/Frameworks/";
        private const string CoreFrameworkPath = BaseFrameworkSourcePath + "GetSocial.embeddedframework.zip";
        private const string ChatFrameworkPath = BaseFrameworkSourcePath + "GetSocialChat.embeddedframework.zip";

        public static void UpdateXcodeProject(string projectPath)
        {
            Debug.Log(string.Format("Addding GetSocial dependencies to Xcode project for '{0}'", projectPath));
            
            try
            {
                string pbxprojPath = Path.Combine(projectPath, "Unity-iPhone.xcodeproj/project.pbxproj");
                
                PBXProject project = new PBXProject();
                project.ReadFromString(File.ReadAllText(pbxprojPath));
                
                string frameworksPath = Path.Combine(projectPath, "Frameworks");
                EnsureDirectoryExists(frameworksPath);
                
                string target = project.TargetGuidByName("Unity-iPhone");
                
                UnzipFramework(Path.Combine(Application.dataPath, CoreFrameworkPath), frameworksPath);
                AddGetSocialCoreFramework(ref project, target);
                
                if(GetSocialSettings.IsChatEnabled)
                {
                    UnzipFramework(Path.Combine(Application.dataPath, ChatFrameworkPath), frameworksPath);
                    AddGetSocialChatFramework(ref project, target);
                }

                // Required for plugin to work
                project.UpdateBuildProperty(target, "OTHER_LDFLAGS", new [] {
                    "-ObjC",
                    "-licucore"
                }, new string[] { });

                SetupDeepLinking(ref project, projectPath, target);
                
                File.WriteAllText(pbxprojPath, project.WriteToString());
                
                Debug.Log(string.Format("GetSocial dependencies successfully added to Xcode project."));
            }
            catch(Exception e)
            {
                Debug.LogException(e);
                Debug.LogError("Failed to add GetSocial dependencies to Xcode project.");
            }
        }

        #region deep_linking
        private static void SetupDeepLinking(ref PBXProject project, string projectPath, string target)
        {
            Debug.LogWarning("Setting up deep linking...\n\tFor universal links setup please refer to http://docs.getsocial.im/?platform=unity#using-universal-links");

            // URL Schemes (iOS <= 8)
            AddGetSocialUrlScheme(projectPath);

            // App links (iOS >=9 )
            AddAppEntitlements(projectPath, project, target);
        }

        static void AddGetSocialUrlScheme(string projectPath)
        {
            Debug.Log(string.Format("Setting up GetSocial deep linking for iOS <= 8 for '{0}'", projectPath));

            try
            {
                var plistInfoFile = new PlistDocument();
                
                string infoPlistPath = Path.Combine(projectPath, "Info.plist");
                plistInfoFile.ReadFromString(File.ReadAllText(infoPlistPath));

                AddGetSocialUrlSchemeToPlist(plistInfoFile);
                
                File.WriteAllText(infoPlistPath, plistInfoFile.WriteToString());
                Debug.Log(string.Format("Setting up GetSocial deep linking for iOS <= 8 was successful."));
            }
            catch(Exception e)
            {
                Debug.LogException(e);
                Debug.LogError("Failed to set up GetSocial deep linking for iOS <= 8.");
            }
        }

        static void AddGetSocialUrlSchemeToPlist(PlistDocument plistInfoFile)
        {
            const string CFBundleURLTypes = "CFBundleURLTypes";
            const string CFBundleURLSchemes = "CFBundleURLSchemes";

            if (!plistInfoFile.root.values.ContainsKey(CFBundleURLTypes))
            {
                plistInfoFile.root.CreateArray(CFBundleURLTypes);
            }

            var cFBundleURLTypesElem = plistInfoFile.root.values[CFBundleURLTypes] as PlistElementArray;

            PlistElementArray getSocialUrlSchemesArray = new PlistElementArray();
            getSocialUrlSchemesArray.AddString(string.Format("getsocial-{0}", GetSocialSettings.AppId));

            PlistElementDict getSocialSchemeElem = cFBundleURLTypesElem.AddDict();
            getSocialSchemeElem.values[CFBundleURLSchemes] = getSocialUrlSchemesArray;
        }
        
        private static void AddAppEntitlements(string projectPath, PBXProject project, string target)
        {
            const string appEntitlementsFilePath = "GetSocial/Editor/IOS/Files/app.entitlements";

            string appEntitlementsFilePathCombined = Path.Combine(RootGetSocialPath, appEntitlementsFilePath);
            string unityProjectPath = Path.Combine(Application.dataPath, appEntitlementsFilePathCombined);
            string xcodeProjectPath = Path.Combine(projectPath, "app.entitlements");
            
            File.Copy(unityProjectPath, xcodeProjectPath);
            
            project.AddFileToBuild(target, project.AddFile("app.entitlements", "app.entitlements", PBXSourceTree.Source));
            project.AddBuildProperty(target, "CODE_SIGN_ENTITLEMENTS", "app.entitlements");
        }
        #endregion

        #region private methods
        private static void AddGetSocialCoreFramework(ref PBXProject project, string target)
        {
            project.AddFileToBuild(target, project.AddFile("Frameworks/GetSocial.embeddedframework/GetSocial.framework", "Frameworks/GetSocial.embeddedframework/GetSocial.framework", PBXSourceTree.Source));
            project.AddFileToBuild(target, project.AddFile("Frameworks/GetSocial.embeddedframework/Resources/GetSocial.bundle", "Frameworks/GetSocial.embeddedframework/Resources/GetSocial.bundle", PBXSourceTree.Source));
            
            // add framework directory to the framework include path
            // project.SetBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(inherited)");
            project.AddBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(PROJECT_DIR)/Frameworks/GetSocial.embeddedframework");
            
            // add dependencies
            project.AddDynamicLibraryToProject(target, "libsqlite3.dylib");
            project.AddFrameworkToProject(target, "Accelerate.framework", weak: false);
            project.AddFrameworkToProject(target, "AdSupport.framework", weak: false);
            project.AddFrameworkToProject(target, "AVFoundation.framework", weak: false);
            project.AddFrameworkToProject(target, "CFNetwork.framework", weak: false);
            project.AddFrameworkToProject(target, "CoreFoundation.framework", weak: false);
            project.AddFrameworkToProject(target, "CoreGraphics.framework", weak: false);
            project.AddFrameworkToProject(target, "CoreImage.framework", weak: false);
            project.AddFrameworkToProject(target, "CoreTelephony.framework", weak: false);
            project.AddFrameworkToProject(target, "CoreText.framework", weak: false);
            project.AddFrameworkToProject(target, "Foundation.framework", weak: false);
            project.AddFrameworkToProject(target, "MessageUI.framework", weak: false);
            project.AddFrameworkToProject(target, "MobileCoreServices.framework", weak: false);
            project.AddFrameworkToProject(target, "QuartzCore.framework", weak: false);
            project.AddFrameworkToProject(target, "Security.framework", weak: false);
            project.AddFrameworkToProject(target, "Social.framework", weak: false);
            project.AddFrameworkToProject(target, "SystemConfiguration.framework", weak: false);
            project.AddFrameworkToProject(target, "UIKit.framework", weak: false);
        }

        private static void AddGetSocialChatFramework(ref PBXProject project, string target)
        {
            project.AddFileToBuild(target, project.AddFile("Frameworks/GetSocialChat.embeddedframework/GetSocialChat.framework", "Frameworks/GetSocialChat.embeddedframework/GetSocialChat.framework", PBXSourceTree.Source));
            project.AddFileToBuild(target, project.AddFile("Frameworks/GetSocialChat.embeddedframework/Resources/GetSocialChat.bundle", "Frameworks/GetSocialChat.embeddedframework/Resources/GetSocialChat.bundle", PBXSourceTree.Source));

            // add framework directory to the framework include path
            project.AddBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(PROJECT_DIR)/Frameworks/GetSocialChat.embeddedframework");
        }

        private static void UnzipFramework(string sourcePath, string destinationPath)
        {
            if(File.Exists(sourcePath))
            {
                string args = string.Format("-o '{0}' -d '{1}'", sourcePath, destinationPath);
                System.Diagnostics.Process.Start("unzip", args).WaitForExit();
            }
            else
            {
                throw new Exception(string.Format("Failed to find framework to unzip at path '{0}'.\n" +
                    "Terminating Xcode project postrocessing.", sourcePath));
            }
        }

        private static void EnsureDirectoryExists(string path)
        {
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        #endregion
    }
}

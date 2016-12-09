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
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;

namespace GetSocialSdk.Editors
{
    /// <summary>
    /// Demo app IOS postprocess for iOS.
    ///
    /// Modifies Info.plist of the exporeted project to include app transport security changes. 
    /// 
    /// https://developers.facebook.com/docs/ios/ios9#whitelistapp
    /// </summary>
    public static class DemoAppIOSPostprocess
    {        
        [PostProcessBuild(512)]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string projectPath)
        {
            if(BuildTarget.iPhone == buildTarget)
            {
                string pbxprojPath = Path.Combine(projectPath, "Unity-iPhone.xcodeproj/project.pbxproj");
                PBXProject project = new PBXProject();
                project.ReadFromString(File.ReadAllText(pbxprojPath));
                string target = project.TargetGuidByName("Unity-iPhone");

                DisableBitcode(project, target);

                File.WriteAllText(pbxprojPath, project.WriteToString());
                
                UpdatePlist(projectPath);

                #if UNITY_5
                MakeUnity5Adjustments(projectPath);
                #endif
            }
        }

        private static void DisableBitcode(PBXProject project, string target)
        {
            project.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
        }
        
        #if UNITY_5
        private static void MakeUnity5Adjustments(string path)
        {
            try
            {
                string pbxprojPath = Path.Combine(path, "Unity-iPhone.xcodeproj/project.pbxproj");

                PBXProject project = new PBXProject();
                project.ReadFromString(File.ReadAllText(pbxprojPath));

                string target = project.TargetGuidByName(PBXProject.GetUnityTargetName());

                // Disable arc for FbUnityInterface.mm
                string fileGuid =
                    project.FindFileGuidByProjectPath("Facebook/FbUnityInterface.mm");
                if (fileGuid == null)
                {
                    Debug.LogError("FbUnityInterface.mm not found!");
                }
                project.SetCompileFlagsForFile(target, fileGuid, new List<string> {"-fno-objc-arc"});

                File.WriteAllText(pbxprojPath, project.WriteToString());
            }
            catch(Exception e)
            {
                Debug.LogException(e);
                Debug.LogError("Failed to make Unity 5 Adjustments.");
            }
        }
        #endif

        #region plist_for_ios9
        private static void UpdatePlist(string projectPath)
        {
            Debug.Log(string.Format("Updating Info.plist in Xcode project for '{0}'", projectPath));

            try
            {
                var plistInfoFile = new PlistDocument();

                string infoPlistPath = Path.Combine(projectPath, "Info.plist");
                plistInfoFile.ReadFromString(File.ReadAllText(infoPlistPath));

                AllowArbitraryLoads(plistInfoFile);
                WhitelistApps(plistInfoFile);

                File.WriteAllText(infoPlistPath, plistInfoFile.WriteToString());
                Debug.Log(string.Format("Info.plist successfully updated in Xcode project."));
            }
            catch(Exception e)
            {
                Debug.LogException(e);
                Debug.LogError("Failed to update Info.plist in Xcode project.");
            }
        }
        
        // Allow all HTTP communication for the test app
        // Disable ATS altogether, use this with care in your game as it imposes security issue
        private static void AllowArbitraryLoads(PlistDocument plistInfoFile)
        {
            const string NSAppTransportSecurity = "NSAppTransportSecurity";
            
            var root = plistInfoFile.root;
            var appTransportSecurity = root.CreateDict(NSAppTransportSecurity);
            appTransportSecurity.SetBoolean("NSAllowsArbitraryLoads", true);
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
        #endregion
    }
}


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
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode.GetSocial;
using GetSocialSdk.Editor;
using UnityEngine;

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
            if (BuildTarget.iOS != buildTarget) return;
    
            Copy(Path.Combine(Application.dataPath, "../IosStreamingAssets/Raw"), Path.Combine(projectPath, "Data/Raw"));
            PBXProjectUtils.ModifyPbxProject(projectPath, DisableBitcode);
            PBXProjectUtils.ModifyPbxProject(projectPath, AddTestingFramework);
            PBXProjectUtils.ModifyPlist(projectPath, AllowArbitraryLoads);
        }

        static void DisableBitcode(PBXProject project, string target)
        {
            project.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
        }

        static void AddTestingFramework(PBXProject project, string target)
        {
            project.AddDynamicFrameworkToProject(target, "Frameworks/Plugins/iOS/GetSocial/GetSocialTestsUtility.framework");
        }
        
        static void Copy(string sourceDir, string targetDir)
        {
            if (!Directory.Exists(targetDir)) Directory.CreateDirectory(targetDir);

            foreach(var file in Directory.GetFiles(sourceDir))
                File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)));

            foreach(var directory in Directory.GetDirectories(sourceDir))
                Copy(directory, Path.Combine(targetDir, Path.GetFileName(directory)));
        }

        #region plist_for_ios9

        // Allow all HTTP communication for the test app
        // Disable ATS altogether, use this with care in your game as it imposes security issue
        static void AllowArbitraryLoads(PlistDocument plistInfoFile)
        {
            const string NSAppTransportSecurity = "NSAppTransportSecurity";

            var root = plistInfoFile.root;
            var appTransportSecurity = root.CreateDict(NSAppTransportSecurity);
            appTransportSecurity.SetBoolean("NSAllowsArbitraryLoads", true);
        }

        #endregion
    }
}
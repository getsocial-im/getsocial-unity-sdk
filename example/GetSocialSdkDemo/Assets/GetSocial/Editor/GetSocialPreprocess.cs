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
using System.IO;
using GetSocialSdk.Core;
using GetSocialSdk.MiniJSON;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.Collections.Generic;

namespace GetSocialSdk.Editor
{
    public class GetSocialPreprocess : IPreprocessBuildWithReport
    {        
        public int callbackOrder { get { return 0; } }
        public void OnPreprocessBuild(BuildReport report)
        {
            Debug.Log("GetSocialPreprocess.OnPreprocessBuild for target " + report.summary.platform + " at path " + report.summary.outputPath);
            if (report.summary.platform == BuildTarget.iOS) 
            {
                FileHelper.MarkIOSFrameworks();
            }
            if (report.summary.platform == BuildTarget.Android)
            {
                AndroidManifestHelper.RemoveSdk6Configs();
            } 
            GetSocialSettings.UpdateConfigsFile();
        }
        
    }
}


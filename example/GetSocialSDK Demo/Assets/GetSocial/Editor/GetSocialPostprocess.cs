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
using UnityEngine;

namespace GetSocialSdk.Editors
{
    public static class GetSocialPostprocess
    {
        [PostProcessBuild(256)]
        public static void OnPostProcessBuild(BuildTarget target, string path)
        {
            if(BuildTarget.iPhone == target)
            {
                GetSocialPostprocessIOS.UpdateXcodeProject(path);
            }

            if(BuildTarget.Android == target)
            {
                if(PlayerSettings.bundleIdentifier == "com.Company.ProductName")
                {
                    Debug.LogError("Please change the default Unity Bundle Identifier (com.Company.ProductName) to your package.");
                }
            }
        }
    }
}


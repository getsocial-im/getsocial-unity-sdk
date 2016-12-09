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

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GetSocialSdk.Core
{
    public class GetSocialSettings : ScriptableObject
    {
        private const int AppIdLength = 13;

        private const string SettingsAssetName = "GetSocialSettings";
        private const string SettingsAssetPath = "Assets/GetSocial/Resources/";

        private static GetSocialSettings instance;

        [SerializeField]
        private string appKey = string.Empty;

        [SerializeField]
        private bool isChatEnabled = true;

        [SerializeField]
        private bool isDebugLogsEnabled = true;

        [SerializeField]
        private bool isAutoRegisrationForPushesEnabledIOS = true;

        #region initialization
        public static GetSocialSettings Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = Resources.Load(SettingsAssetName) as GetSocialSettings;
                    if(instance == null)
                    {
                        instance = CreateInstance<GetSocialSettings>();
                        instance.SaveAsset(SettingsAssetPath, SettingsAssetName + ".asset");
                    }
                }
                return instance;
            }
        }
        #endregion

        #region public methods
        public static string AppKey
        {
            get
            {
                return Instance.appKey;
            }
            set
            {
                Instance.appKey = value;
                Instance.MarkAssetDirty();
            }
        }

        // Last 13 chars if AppKey
        public static string AppId
        {
            get
            {
                return Instance.appKey == null ? null : Instance.appKey.Substring(Instance.appKey.Length - AppIdLength);
            }
        }
        
        public static bool IsChatEnabled
        {
            get
            {
                return Instance.isChatEnabled;
            }
            set
            {
                Instance.isChatEnabled = value;
                Instance.MarkAssetDirty();
            }
        }

        public static bool IsDebugLogsEnabled
        {
            get
            {
                return Instance.isDebugLogsEnabled;
            }
            set
            {
                Instance.isDebugLogsEnabled = value;
                Instance.MarkAssetDirty();
            }
        }

        public static bool IsAutoRegisrationForPushesEnabledIOS
        {
            get
            {
                return Instance.isAutoRegisrationForPushesEnabledIOS;
            }
            set
            {
                Instance.isAutoRegisrationForPushesEnabledIOS = value;
                Instance.MarkAssetDirty();
            }
        }
        #endregion

        #region private methods
        private void SaveAsset(string directory, string name)
        {
            #if UNITY_EDITOR
            if(!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            AssetDatabase.CreateAsset(this, directory + name);
            AssetDatabase.Refresh();
            #endif
        }

        private void MarkAssetDirty()
        {
            #if UNITY_EDITOR
            EditorUtility.SetDirty(Instance);
            EditorApplication.SaveAssets();
            #endif
        }
        #endregion
    }

}


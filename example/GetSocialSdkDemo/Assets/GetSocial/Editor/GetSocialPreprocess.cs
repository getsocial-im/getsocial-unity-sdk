
using UnityEditor;

#if UNITY_5_6_OR_NEWER
using UnityEditor.Build;
#endif

using UnityEngine;

namespace GetSocialSdk.Editor
{

#if UNITY_5_6_OR_NEWER
    
    public class GetSocialPreprocess : IPreprocessBuild
    {
        public void OnPreprocessBuild(BuildTarget target, string path)
        {
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
            {
                if (!FileHelper.CheckiOSFramework())
                {
                    Debug.LogError("GetSocial: Native libraries for GetSocial SDK are missing. Download it before building the project");
                }
            }
            
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                if (!FileHelper.CheckAndroidFramework())
                {
                    Debug.LogError("GetSocial: Native libraries for GetSocial SDK are missing. Download it before building the project");
                }
            }
        }

        public int callbackOrder {
            get { return 1; }
        }
    }
    
#endif
    
}
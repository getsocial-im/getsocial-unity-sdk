using UnityEditor;

namespace GetSocialSdk.Editor
{
    public class PlayerSettingsCompat
    {
        public static string bundleIdentifier
        {
            get
            {
                #if UNITY_5_6_OR_NEWER
                var appId = PlayerSettings.applicationIdentifier;
                #else
                var appId = PlayerSettings.bundleIdentifier;
                #endif
                return appId;
            }

            set
            {
                #if UNITY_5_6_OR_NEWER
                PlayerSettings.applicationIdentifier = value;
                #else
                PlayerSettings.bundleIdentifier = value;
                #endif
            }
        }

        public static string iPhoneBundleIdentifier
        {
            get
            {
                #if UNITY_5_6_OR_NEWER
                var iOsBundleIdentifier = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.iOS);
                #else
                var iOsBundleIdentifier = PlayerSettings.iPhoneBundleIdentifier;
                #endif
                return iOsBundleIdentifier;
            }
        }
    }
}
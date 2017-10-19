using System;
using System.Linq;
using UnityEditor;

namespace GetSocialSdk.Editor
{
    public class FileHelper
    {
        public static void SetGetSocialUiEnabled(bool enabled)
        {
            var androidCoreLib = AssetDatabase.FindAssets("getsocial-library-*", new [] {"Assets/GetSocial/Plugins/Android"});
            var androidUiLib = AssetDatabase.FindAssets("getsocial-ui-*", new [] {"Assets/GetSocial/Plugins/Android"});
            var iosCoreLib = AssetDatabase.FindAssets("GetSocial.framework", new[] {"Assets/GetSocial/Plugins/iOS"});
            var iosUiLib = AssetDatabase.FindAssets("GetSocialUI.framework", new[] {"Assets/GetSocial/Plugins/iOS"});
            
            UpdatePlatformState(iosCoreLib, BuildTarget.iOS, true);
            UpdatePlatformState(iosUiLib, BuildTarget.iOS, enabled);
            UpdatePlatformState(androidCoreLib, BuildTarget.Android, true);
            UpdatePlatformState(androidUiLib, BuildTarget.Android, enabled);
        }

        private static void UpdatePlatformState(string[] paths, BuildTarget platform, bool enabled)
        {
            if (paths.Length == 0)
            {
                return;
            }
            
            var plugin = AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(paths.First())) as PluginImporter;
            ClearAllPlatforms(plugin);
            plugin.SetCompatibleWithPlatform(platform, enabled);
        }

        private static void ClearAllPlatforms(PluginImporter plugin)
        {
            plugin.SetCompatibleWithEditor(false);
            plugin.SetCompatibleWithAnyPlatform(false);
            Enum.GetValues(typeof(BuildTarget))
                .Cast<BuildTarget>()
                .Where(target => !IsObsolete(target))
                .Where(target => target != BuildTarget.NoTarget)
                .ToList()
                .ForEach(target => plugin.SetCompatibleWithPlatform(target, false));
        }
        
        private static bool IsObsolete(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (ObsoleteAttribute[])
                fi.GetCustomAttributes(typeof(ObsoleteAttribute), false);
            return attributes != null && attributes.Length > 0;
        }
    }
}
using System;
using System.IO;
using System.Linq;
using GetSocialSdk.Core;
using UnityEditor;
using UnityEngine;

namespace GetSocialSdk.Editor
{
    public static class FileHelper
    {
        public static bool AndroidDownloadInProgress { get; private set; }
        public static bool IOSDownloadInProgress { get; private set; }
        public static float AndroidDownloadProgress { get; private set; }
        public static float IOSDownloadProgress { get; private set; }

        private const string DownloadUrlAndroid = "http://s3.amazonaws.com/downloads.getsocial.im/unity/releases/{0}/getsocial-unity-android-sdk.zip";
        private const string DownloadUrliOS = "http://s3.amazonaws.com/downloads.getsocial.im/unity/releases/{0}/getsocial-unity-ios-sdk.zip";

        private const string DestinationFolderPathiOS = "Plugins/iOS";
        private const string AndroidFrameworkName_Core = "getsocial-library-release.aar";
        private const string AndroidFrameworkName_UI = "getsocial-ui-release.aar";
        private const string IOSFrameworkName_Core = "GetSocialSDK.framework";
        private const string IOSFrameworkName_UI = "GetSocialUI.framework";
        private const string IOSFrameworkName_Extension = "GetSocialNotificationExtension.framework";
        private static readonly string[] IOSFrameworks = new[] { IOSFrameworkName_Core, IOSFrameworkName_Extension, IOSFrameworkName_UI };
        private static readonly string[] AndroidFrameworks = new[] { AndroidFrameworkName_Core, AndroidFrameworkName_UI };

        private const string DevelopmentVersion = "development";

        private static void MarkIosFiles()
        {
            var coreDir = Path.Combine(GetSocialSettings.GetPluginPath(), "Editor/iOS/GetSocial");
            var uiDir = Path.Combine(GetSocialSettings.GetPluginPath(), "Editor/iOS/GetSocialUI");
            ForEachIn(coreDir, path => UpdatePlatformState(path, BuildTarget.iOS, true));
            ForEachIn(uiDir, path => UpdatePlatformState(path, BuildTarget.iOS, true));
            MarkIOSFrameworks();
        }

        public static void MarkIOSFrameworks()
        {
            var iOSFrameworksDir = Path.Combine(GetSocialSettings.GetPluginPath(), DestinationFolderPathiOS);

            foreach (var framework in IOSFrameworks)
            {
                UpdatePlatformState(Path.Combine(iOSFrameworksDir, framework), BuildTarget.iOS, true);
            }
        }

        private static void ForEachIn(string path, Action<string> each)
        {
            Directory.GetFiles(path)
                .Where(it => !it.EndsWith(".meta"))
                .ToList()
                .ForEach(each);
            Directory.GetDirectories(path)
                .ToList()
                .ForEach(dir => ForEachIn(dir, each));
        }

        private static bool IsInBatchMode()
        {
            string commandLineOptions = Environment.CommandLine;

            if (commandLineOptions.Contains("-batchmode"))
            {
                return true;
            }
            return false;
        }

        internal static void UpdatePlatformState(string filePath, BuildTarget platform, bool enabled)
        {
            var plugin = AssetImporter.GetAtPath(filePath) as PluginImporter;
            if (plugin == null) return;
            ClearAllPlatforms(plugin);
            plugin.SetCompatibleWithPlatform(platform, enabled);
#if UNITY_2018_3_OR_NEWER
            if (platform == BuildTarget.iOS && filePath.EndsWith("framework"))
            {
                plugin.SetPlatformData(platform, "AddToEmbeddedBinaries", "true");
            }
#endif
            plugin.SaveAndReimport();
        }

        private static void ClearAllPlatforms(PluginImporter plugin)
        {
            plugin.SetCompatibleWithEditor(false);
            plugin.SetCompatibleWithAnyPlatform(false);
            Enum.GetValues(typeof(BuildTarget))
                .Cast<BuildTarget>()
                .Where(target => !IsObsolete(target))
                .Where(target => (int)target != -2)
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
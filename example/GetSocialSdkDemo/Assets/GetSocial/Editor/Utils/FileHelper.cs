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

        private const string DestinationFolderPathAndroid = "Plugins/Android";
        private const string DestinationFolderPathiOS = "Plugins/iOS";
        private const string IOSArchiveName = "getsocial-unity-ios-sdk.zip";
        private const string AndroidArchiveName = "getsocial-unity-android-sdk.zip";

        private const string AndroidFrameworkName_Core = "getsocial-library-release.aar";
        private const string AndroidFrameworkName_UI = "getsocial-ui-release.aar";
        private const string IOSFrameworkName_Core = "GetSocial.framework";
        private const string IOSFrameworkName_UI = "GetSocialUI.framework";
        private const string IOSFrameworkName_Extension = "GetSocialExtension.framework";
        private static readonly string[] IOSFrameworks = new[] { IOSFrameworkName_Core, IOSFrameworkName_Extension, IOSFrameworkName_UI };
        private static readonly string[] AndroidFrameworks = new[] { AndroidFrameworkName_Core, AndroidFrameworkName_UI };

        private const string DevelopmentVersion = "development";

        [UnityEditor.Callbacks.DidReloadScripts]
        internal static void MarkIosFiles()
        {
            var coreDir = Path.Combine(GetSocialSettings.GetPluginPath(), "Editor/iOS/GetSocial");
            var uiDir = Path.Combine(GetSocialSettings.GetPluginPath(), "Editor/iOS/GetSocialUI");
            ForEachIn(coreDir, path => UpdatePlatformState(path, BuildTarget.iOS, true));
            ForEachIn(uiDir, path => UpdatePlatformState(path, BuildTarget.iOS, true));

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

        public static void SetGetSocialUiEnabled(bool enabled)
        {
            if (Directory.Exists(Path.Combine(GetSocialSettings.GetPluginPath(), DestinationFolderPathAndroid)) && !AndroidDownloadInProgress)
            {
                var path = Path.Combine(GetSocialSettings.GetPluginPath(), DestinationFolderPathAndroid);

                UpdatePlatformState(Path.Combine(path, AndroidFrameworkName_UI), BuildTarget.Android, enabled);
            }

            if (Directory.Exists(Path.Combine(GetSocialSettings.GetPluginPath(), DestinationFolderPathiOS)) && !IOSDownloadInProgress)
            {
                var path = Path.Combine(GetSocialSettings.GetPluginPath(), DestinationFolderPathiOS);

                UpdatePlatformState(Path.Combine(path, IOSFrameworkName_UI), BuildTarget.iOS, enabled);
            }
        }

        public static void DownloadiOSFramework(Action onSuccess = null, Action<string> onFailure = null)
        {
            IOSDownloadInProgress = true;

            var pluginFolderPath = Path.Combine(GetSocialSettings.GetPluginPath(), DestinationFolderPathiOS);
            RemoveOldVersions(pluginFolderPath);

            var downloadFrameworkRequest = DownloadFrameworkRequest.Create(string.Format(DownloadUrliOS, BuildConfig.UnitySdkVersion),
                pluginFolderPath,
                Path.Combine(pluginFolderPath, IOSArchiveName));

            downloadFrameworkRequest.Start(() =>
            {
                IOSDownloadInProgress = false;
                IOSDownloadProgress = 0;
                if (UnzipFramework(Path.Combine(pluginFolderPath, IOSArchiveName), pluginFolderPath))
                {
                    AddFrameworksToAssets(new[]
                    {
                        Path.Combine(DestinationFolderPathiOS, IOSFrameworkName_Core),
                        Path.Combine(DestinationFolderPathiOS, IOSFrameworkName_Extension),
                        Path.Combine(DestinationFolderPathiOS, IOSFrameworkName_UI)
                    });

                    foreach (var framework in IOSFrameworks)
                    {
                        UpdatePlatformState(Path.Combine(pluginFolderPath, framework), BuildTarget.iOS, true);
                    }
                    if (onSuccess != null)
                    {
                        onSuccess();
                    }
                }
                else
                {
                    var error = "unzip command failed";
                    Debug.LogError(string.Format("GetSocial: Failed to download native iOS SDK, error: {0}", error));
                    if (onFailure != null)
                    {
                        onFailure(error);
                    }
                }
            }, progress =>
            {
                IOSDownloadProgress = progress;
            }, error =>
            {
                IOSDownloadInProgress = false;
                Debug.LogError(string.Format("GetSocial: Failed to download native iOS SDK, error: {0}", error));
                if (onFailure != null)
                {
                    onFailure(error);
                }
            }, IsInBatchMode());
        }

        public static bool CheckiOSFramework()
        {
            var pluginFolderPath = Path.Combine(GetSocialSettings.GetPluginPath(), DestinationFolderPathiOS);
            var sdkVersion = ReadSDKVersion(pluginFolderPath);
            if (DevelopmentVersion.Equals(sdkVersion))
            {
                return true;
            }
            if (Directory.Exists(Path.Combine(pluginFolderPath, IOSFrameworkName_Core)))
            {
                if (BuildConfig.UnitySdkVersion.Equals(sdkVersion))
                {
                    return true;
                }
            }
            return false;
        }

        public static void DownloadAndroidFramework(Action onSuccess = null, Action<string> onFailure = null)
        {
            AndroidDownloadInProgress = true;

            var pluginFolderPath = Path.Combine(GetSocialSettings.GetPluginPath(), DestinationFolderPathAndroid);

            // remove old versions if any
            RemoveOldVersions(pluginFolderPath);

            var downloadFrameworkRequest = DownloadFrameworkRequest.Create(string.Format(DownloadUrlAndroid, BuildConfig.UnitySdkVersion),
                pluginFolderPath,
                Path.Combine(pluginFolderPath, AndroidArchiveName));

            downloadFrameworkRequest.Start(() =>
            {
                AndroidDownloadProgress = 0;
                AndroidDownloadInProgress = false;
                if (UnzipFramework(Path.Combine(pluginFolderPath, AndroidArchiveName), pluginFolderPath))
                {
                    AddFrameworksToAssets(new[]
                    {
                        Path.Combine(DestinationFolderPathAndroid, AndroidFrameworkName_Core),
                        Path.Combine(DestinationFolderPathAndroid, AndroidFrameworkName_UI)
                    });

                    foreach (var framework in AndroidFrameworks)
                    {
                        UpdatePlatformState(Path.Combine(pluginFolderPath, framework), BuildTarget.Android, true);
                    }
                    if (onSuccess != null)
                    {
                        onSuccess();
                    }
                }
                else
                {
                    var error = "unzip command failed";
                    Debug.LogError(String.Format("GetSocial: Failed to download native Android SDK, error: {0}", error));
                    if (onFailure != null)
                    {
                        onFailure(error);
                    }
                }
            }, progress =>
            {
                AndroidDownloadProgress = progress;
            }, error =>
            {
                AndroidDownloadInProgress = false;
                Debug.LogError(String.Format("GetSocial: Failed to download native Android SDK, error: {0}", error));
                if (onFailure != null)
                {
                    onFailure(error);
                }
            }, IsInBatchMode());
        }

        public static bool CheckAndroidFramework()
        {
            var pluginFolderPath = Path.Combine(GetSocialSettings.GetPluginPath(), DestinationFolderPathAndroid);
            var sdkVersion = ReadSDKVersion(pluginFolderPath);
            if (DevelopmentVersion.Equals(sdkVersion))
            {
                return true;
            }
            if (File.Exists(Path.Combine(pluginFolderPath, AndroidFrameworkName_Core)))
            {
                if (BuildConfig.UnitySdkVersion.Equals(sdkVersion))
                {
                    return true;
                }
            }
            return false;
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

        private static string ReadSDKVersion(string pluginFolderPath)
        {
            if (Directory.Exists(pluginFolderPath))
            {
                return ReadFileContent(Path.Combine(pluginFolderPath, "version"));
            }
            return null;
        }

        private static string ReadFileContent(string filepath)
        {
            string retValue = null;
            if (File.Exists(filepath))
            {
                StreamReader file = new StreamReader(filepath);
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    retValue += line;
                }
                file.Close();
            }
            return retValue;
        }

        private static void RemoveOldVersions(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                return;
            }
            // this function requires / as path separator, so if we're on Windows we have to replace it
            FileUtil.DeleteFileOrDirectory(directoryPath.Replace(Path.DirectorySeparatorChar, '/'));
            Directory.CreateDirectory(directoryPath);
        }

        private static void AddFrameworksToAssets(string[] filePath)
        {
            foreach (var path in filePath)
            {
                AssetDatabase.ImportAsset(Path.Combine(GetSocialSettings.GetPluginPath(), path), ImportAssetOptions.ForceUpdate);
            }
        }

        private static bool UnzipFramework(string zipFilePath, string destinationPath)
        {
            var success = ZipUtils.ExtractZipFile(zipFilePath, destinationPath);
            if (success)
            {
                File.Delete(zipFilePath);
            }

            return success;
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
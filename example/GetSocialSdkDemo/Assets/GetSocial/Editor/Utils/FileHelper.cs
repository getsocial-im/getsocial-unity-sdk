﻿using System;
using System.IO;
using System.Linq;
using GetSocialSdk.Core;
using UnityEditor;
using UnityEngine;

namespace GetSocialSdk.Editor
{
    public class FileHelper
    {
        public static bool AndroidDownloadInProgress { get; private set; }
        public static bool IOSDownloadInProgress { get; private set; }

        private const string DownloadUrlAndroid = "http://s3.amazonaws.com/downloads.getsocial.im/unity/releases/{0}/getsocial-unity-android-sdk.zip"; 
        private const string DownloadUrliOS = "http://s3.amazonaws.com/downloads.getsocial.im/unity/releases/{0}/getsocial-unity-ios-sdk.zip";

        private const string DestinationFolderPathAndroid = "GetSocial/Plugins/Android";
        private const string DestinationFolderPathiOS = "GetSocial/Plugins/iOS";
        private const string IOSArchiveName = "getsocial-unity-ios-sdk.zip";
        private const string AndroidArchiveName = "getsocial-unity-android-sdk.zip";
        
        private const string AndroidFrameworkName_Core = "getsocial-library-release.aar";
        private const string AndroidFrameworkName_UI = "getsocial-ui-release.aar";
        private const string IOSFrameworkName_Core = "GetSocial.framework";
        private const string IOSFrameworkName_UI = "GetSocialUI.framework";
        
        private const string DevelopmentVersion = "development";

        public static void SetGetSocialUiEnabled(bool enabled)
        {
            var androidCoreLib = new string[0];
            var androidUiLib = new string[0];

            var iosCoreLib = new string[0];
            var iosUiLib = new string[0];

            if (Directory.Exists(Path.Combine(Application.dataPath, DestinationFolderPathAndroid)) && !AndroidDownloadInProgress)
            {
                androidCoreLib = AssetDatabase.FindAssets(AndroidFrameworkName_Core, new [] { Path.Combine( "Assets", DestinationFolderPathAndroid) });
                androidUiLib = AssetDatabase.FindAssets(AndroidFrameworkName_UI, new [] { Path.Combine( "Assets", DestinationFolderPathAndroid) });
            }

            if (Directory.Exists(Path.Combine(Application.dataPath, DestinationFolderPathiOS)) && !IOSDownloadInProgress)
            {
                iosCoreLib = AssetDatabase.FindAssets(IOSFrameworkName_Core, new[] { Path.Combine( "Assets", DestinationFolderPathiOS) });
                iosUiLib = AssetDatabase.FindAssets(IOSFrameworkName_UI, new[] { Path.Combine("Assets", DestinationFolderPathiOS) });
            }
            
            UpdatePlatformState(iosCoreLib, BuildTarget.iOS, true);
            UpdatePlatformState(iosUiLib, BuildTarget.iOS, enabled);
            UpdatePlatformState(androidCoreLib, BuildTarget.Android, true);
            UpdatePlatformState(androidUiLib, BuildTarget.Android, enabled);
            
        }

        public static void DownloadiOSFramework(Action onSuccess = null, Action<string> onFailure = null)
        {
            IOSDownloadInProgress = true;
            
            var pluginFolderPath = Path.Combine(Application.dataPath, DestinationFolderPathiOS);
            RemoveOldVersions(pluginFolderPath);

            var downloadFrameworkRequest = DownloadFrameworkRequest.Create(string.Format(DownloadUrliOS, BuildConfig.UnitySdkVersion),
                pluginFolderPath,
                Path.Combine(pluginFolderPath, IOSArchiveName));

            downloadFrameworkRequest.Start(() =>
            {
                IOSDownloadInProgress = false;
                UnzipFramework(Path.Combine(pluginFolderPath, IOSArchiveName), pluginFolderPath);
                AddFrameworksToAssets(new[]
                {
                    Path.Combine(DestinationFolderPathiOS, IOSFrameworkName_Core),
                    Path.Combine(DestinationFolderPathiOS, IOSFrameworkName_UI)
                });
                if (onSuccess != null)
                {
                    onSuccess();
                }
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
            var pluginFolderPath = Path.Combine(Application.dataPath, DestinationFolderPathiOS);
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

            var pluginFolderPath = Path.Combine(Application.dataPath, DestinationFolderPathAndroid);

            // remove old versions if any
            RemoveOldVersions(pluginFolderPath);
            
            var downloadFrameworkRequest = DownloadFrameworkRequest.Create(string.Format(DownloadUrlAndroid, BuildConfig.UnitySdkVersion),
                pluginFolderPath,
                Path.Combine(pluginFolderPath, AndroidArchiveName));
                
            downloadFrameworkRequest.Start(() =>
            {
                UnzipFramework(Path.Combine(pluginFolderPath, AndroidArchiveName), pluginFolderPath);
                AddFrameworksToAssets(new[]
                {
                    Path.Combine(DestinationFolderPathAndroid, AndroidFrameworkName_Core),
                    Path.Combine(DestinationFolderPathAndroid, AndroidFrameworkName_UI)
                });
                if (onSuccess != null)
                {
                    onSuccess();
                }
                AndroidDownloadInProgress = false;
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
            var pluginFolderPath = Path.Combine(Application.dataPath, DestinationFolderPathAndroid);
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
 
            if (commandLineOptions.Contains("-batchmode") )
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

        private static void RemoveOldVersions(String directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                RemoveNotNeededFiles(directoryPath, Directory.GetFiles(directoryPath));
            }
        }

        private static void AddFrameworksToAssets(String[] filePath)
        {
            foreach (var path in filePath)
            {
                AssetDatabase.ImportAsset(Path.Combine("Assets", path), ImportAssetOptions.ForceUpdate);
            }
        }

        private static void RemoveNotNeededFiles(string folderPath, string[] fileNames)
        {
            foreach (var fileName in fileNames)
            {
                if (File.Exists(Path.Combine(folderPath, fileName)))
                {
                    File.Delete(Path.Combine(folderPath, fileName));
                }
            }
        }
        
        private static void UnzipFramework(string zipFilePath, string destinationPath)
        {
            ZipUtils.ExtractZipFile(zipFilePath, destinationPath);
            File.Delete(zipFilePath);
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
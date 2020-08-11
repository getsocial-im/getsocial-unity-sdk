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
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using GetSocialSdk.Core;
using Debug = UnityEngine.Debug;
using Object = System.Object;

namespace GetSocialSdk.Editor
{

    public class UiConfigValidationResult
    {
        public bool Result { get; private set; }
        public string Message { get; private set; }
        internal UiConfigValidationResult(bool result, string message)
        {
            Result = result;
            Message = message;
        }
    }
    
    [InitializeOnLoad]
    public static class GetSocialEditorUtils
    {
        public static Texture2D AndroidIcon;
        public static Texture2D IOSIcon;
        public static Texture2D SettingsIcon;
        public static Texture2D InfoIcon;
        public static Texture2D GetSocialIcon;

        static string _editorPath;
        static string _editorGuiPath;
        
        private static string signingKeyHash;
        private static string keystoreUtilError;
        private static string previousKeystorePath;
        private static string previousKeystorePass;
        private static string previousKeyAlias;

        private const int PLATFORM_UNIX_NEW_VALUE = 4;
        private const int PLATFORM_MAC_OS = 6;
        private const int PLATFORM_UNIX_OLD_VALUE = 128;
        
        static GetSocialEditorUtils()
        {
            Initialize();
            AndroidIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(_editorGuiPath + "/android.png", typeof(Texture2D));
            IOSIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(_editorGuiPath + "/ios.png", typeof(Texture2D));
            SettingsIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(_editorGuiPath + "/settings.png", typeof(Texture2D));
            InfoIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(_editorGuiPath + "/icon_info.png", typeof(Texture2D));
            GetSocialIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(_editorGuiPath + "/getsocial.png", typeof(Texture2D));
        }

        static void Initialize()
        {
            var rootDir = new DirectoryInfo(Application.dataPath);
            var files = rootDir.GetFiles("GetSocialSettingsEditor.cs", SearchOption.AllDirectories);
            _editorPath = Path.GetDirectoryName(files[0].FullName.Replace("\\", "/").Replace(Application.dataPath, "Assets"));
            _editorGuiPath = Path.Combine(_editorPath, "GUI");
        }

        public static void BeginSetSmallIconSize()
        {
            EditorGUIUtility.SetIconSize(new Vector2(14, 14));
        }

        public static void EndSetSmallIconSize()
        {
            EditorGUIUtility.SetIconSize(Vector2.zero);
        }

        public static string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            var searchPatterns = searchPattern.Split(';');
            var files = new List<string>();
            foreach (string sp in searchPatterns)
            {
                files.AddRange(Directory.GetFiles(path, sp, searchOption));
            }
            files.Sort();
            return files.ToArray();
        }
        
        public static string SigningKeyHash
        {
            get
            {
                keystoreUtilError = null;
                string keystorePath = DebugKeyStorePath;
                string keystorePass = "android";
                string keyAlias = "androiddebugkey";

                if (UserDefinedKeystore())
                {
                    keystorePath = PlayerSettings.Android.keystoreName;
                    keystorePass = PlayerSettings.Android.keystorePass;
                    keyAlias = PlayerSettings.Android.keyaliasName;
                    if (!KeystorePassDefined())
                    {
                        keystoreUtilError =
                            "Keystore password is not set. Make sure Keystore is properly configured in Player Settings -> Android -> Publishing settings.\n" + 
                            "If you entered the password press Refresh to try to read signature again.\n" + 
                            "You can ignore this warning and get SHA key signature manually. Click on the 'More Info' button to get the detailed guide.";
                        return "";
                    }
                }

                bool settingsAreSame = keystorePath.Equals(previousKeystorePath) &&
                                         keystorePass.Equals(previousKeystorePass) &&
                                         keyAlias.Equals(previousKeyAlias);
                
                if ((signingKeyHash == null && keystoreUtilError == null) || !settingsAreSame)
                {
                    keystoreUtilError = null;
                    previousKeystorePath = keystorePath;
                    previousKeystorePass = keystorePass;
                    previousKeyAlias = keyAlias;
                    
                    if (!HasAndroidKeystoreFile(keystorePath))
                    {
                        keystoreUtilError = "Error: Can't find Android keystore " + keystorePath + ". Make sure Keystore is properly configured in Player Settings -> Android -> Publishing settings.";
                        return "";
                    }
                    if (!DoesKeytoolExist())
                    {
                        keystoreUtilError = "Error: keytool command line utility does not exist. Make sure you have Java Sdk installed and path is added to environment path.";
                        return "";
                    }
                    if (!HasAndroidSdk())
                    {
                        keystoreUtilError = "Error: Can't find Android Sdk. Make sure it is installed and path is added to environment variabled.";
                        return "";
                    }
                    signingKeyHash = GetKeyHash(keystorePath, keystorePass, keyAlias); 
                }
                return signingKeyHash;
            }
        }

        public static bool UserDefinedKeystore()
        {
            return !string.IsNullOrEmpty(PlayerSettings.Android.keystoreName);
        }

        public static bool KeystorePassDefined()
        {
            return !string.IsNullOrEmpty(PlayerSettings.Android.keystorePass);
        }

        public static string KeyStoreUtilError
        {
            get
            {
                return keystoreUtilError;
            }
        }

        private static string DebugKeyStorePath
        {
            get
            {
                return (Application.platform == RuntimePlatform.WindowsEditor) ?
                    Environment.GetEnvironmentVariable("HOMEDRIVE") + Environment.GetEnvironmentVariable("HOMEPATH") + @"\.android\debug.keystore" :
                    Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"/.android/debug.keystore";
            }
        }

        private static bool HasAndroidSdk()
        {
            return EditorPrefs.HasKey("AndroidSdkRoot") && Directory.Exists(EditorPrefs.GetString("AndroidSdkRoot"));
        }
        
        private static bool HasAndroidKeystoreFile(string keystorePath)
        {
            return File.Exists(keystorePath);
        }
        
        private static string GetKeyHash(string keystoreName, string keystorePassword, string aliasName)
        {
            var proc = new Process();
            var hasAlias = !string.IsNullOrEmpty(aliasName);
            var arguments = hasAlias
                ? @"{1} ""keytool -list -v -keystore {0}{2}{0} -storepass {0}{3}{0} -alias {0}{4}{0}""" 
                : @"{1} ""keytool -list -v -keystore {0}{2}{0} -storepass {0}{3}{0}""";
            
            proc.StartInfo.FileName = IsUnix() ? "bash" : "cmd";
            var prefix = IsUnix() ? "-c" : "/C";
            var quotes = IsUnix() ? "'" : @"""";
             
            proc.StartInfo.Arguments = hasAlias
                ? string.Format(arguments, quotes, prefix, keystoreName, keystorePassword, aliasName) 
                : string.Format(arguments, quotes, prefix, keystoreName, keystorePassword);

            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            var keyHash = new StringBuilder();
            while (!proc.HasExited)
            {
                keyHash.Append(proc.StandardOutput.ReadToEnd());
            }

            if (proc.ExitCode == 255)
            {
                return "";
            }

            var response = keyHash.ToString();
            const string errorLiteral = "keytool error:";
            if (response.Contains(errorLiteral))
            {
                int errorBeginIndex = response.IndexOf("Exception:") + "Exception:".Length + 1;
                int errorEndIndex = response.IndexOf('\n', errorBeginIndex);
                keystoreUtilError = "Warning: " + response.Substring(errorBeginIndex, (errorEndIndex - errorBeginIndex)).Trim() + ". Make sure Keystore is properly configured in Player Settings -> Android -> Publishing settings.\nYou can ignore this warning and get SHA key signature manually. Click on the 'More Info' button to get the detailed guide.";
                return "";
            }
            const string sha256Literal = "SHA256:";
            if (response.Contains(sha256Literal))
            {
                int shaBeginIndex = response.IndexOf(sha256Literal) + sha256Literal.Length;
                int shaEndIndex = response.IndexOf('\n', shaBeginIndex);
                return response.Substring(shaBeginIndex, (shaEndIndex - shaBeginIndex)).Trim();
            }

            keystoreUtilError =
                "Warning: Can't read signature. Make sure Keystore is properly configured in Player Settings -> Android -> Publishing settings." +
                "\nYou can ignore this warning and get SHA key signature manually. Click on the 'More Info' button to get the detailed guide.";
            return "";
        }

        private static bool IsUnix()
        {
            var platform = (int)Environment.OSVersion.Platform;
            return (platform == PLATFORM_MAC_OS) || (platform == PLATFORM_UNIX_NEW_VALUE) || (platform == PLATFORM_UNIX_OLD_VALUE);
        }
        
        private static bool DoesKeytoolExist()
        {
            var proc = new Process();
            if (IsUnix())
            {
                proc.StartInfo.FileName = "bash";
                proc.StartInfo.Arguments = @"-c keytool";
            }
            else
            {
                proc.StartInfo.FileName = "cmd";
                proc.StartInfo.Arguments = @"/C keytool";
            }

            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            proc.Start();
            proc.WaitForExit();
            if (IsUnix())
            {
                return proc.ExitCode != 127;
            }
            else
            {
                return proc.ExitCode == 0;
            }
        }

        public static UiConfigValidationResult CheckCustomUiConfig()
        {
            if (string.IsNullOrEmpty(GetSocialSettings.UiConfigurationCustomFilePath))
            {
                return new UiConfigValidationResult(true, "");
            }

            var completePath = Path.Combine(Application.streamingAssetsPath,
                GetSocialSettings.UiConfigurationCustomFilePath);
            if (File.Exists(completePath))
            {
                return ValidateCustomUiConfigContent(completePath);
            }
            if (!Directory.Exists(completePath)) return new UiConfigValidationResult(false, "GetSocial: Custom UI Configuration file not found at " + completePath + ". Make sure the file exists and it has .json extension");
            var jsonFileCounter = Directory.GetFiles(completePath).Count(file => file.EndsWith(".json"));
            if (jsonFileCounter != 1)
            {
                return new UiConfigValidationResult(false, "GetSocial: Custom UI Configuration directory at " + completePath + " contains multiple JSON files.");
            }

            var filePath = Directory.GetFiles(completePath).Single(file => file.EndsWith(".json"));
            return ValidateCustomUiConfigContent(filePath);
        }

        private static UiConfigValidationResult ValidateCustomUiConfigContent(string path)
        {
            try
            {
                JsonUtility.FromJson<object>(File.ReadAllText(path));
            }
            catch (Exception exception)
            {
                Debug.LogError("GetSocial: Could not parse custom UI configuration file, error: " + exception.Message);
                return new UiConfigValidationResult(false, "GetSocial: Custom UI Configuration file (" + path + ") is not a valid JSON file.");
            }

            return new UiConfigValidationResult(true, "");
        }

    }
}

using System;
using System.IO;
using GetSocialSdk.Core;
using UnityEditor;
using UnityEditor.Android;
using UnityEngine;

namespace GetSocialSdk.Editor
{
    class GetSocialGradlePostProcess : IPostGenerateGradleAndroidProject
    {
        public int callbackOrder
        {
            get { return 0; }
        }

        public void OnPostGenerateGradleAndroidProject(string path)
        {
            Debug.Log("GetSocialGradlePostProcess.OnPostGenerateGradleAndroidProject at path " + path);
#if UNITY_2019_3_OR_NEWER
            File.AppendAllText(path + "/../build.gradle", mainGradleText());
            File.AppendAllText(path + "/../launcher/build.gradle", gradleText());
            FileUtil.CopyFileOrDirectory(Path.Combine(Application.streamingAssetsPath, "getsocial.json"), Path.Combine(path, "../launcher/getsocial.json"));
#else
            File.AppendAllText(path + "/build.gradle", mainGradleText());
            File.AppendAllText(path + "/build.gradle", gradleText());
            FileUtil.CopyFileOrDirectory(Path.Combine(Application.streamingAssetsPath, "getsocial.json"), Path.Combine(path, "getsocial.json"));
#endif
            if (PlayerSettings.applicationIdentifier == "com.Company.ProductName")
            {
                Debug.LogError(
                    "GetSocial: Please change the default Unity Bundle Identifier (com.Company.ProductName) to your package.");
            }
        }
        private static string mainGradleText() {
            return @"
allprojects {
    buildscript {
        repositories {
            maven {
                url 'https://plugins.gradle.org/m2/'
            }
        }
        dependencies {
            classpath 'im.getsocial:plugin-v7:[1,2)'
        }
    }
}
            ";
        }
        private static string gradleText() {
            return string.Format(@"
apply plugin: 'im.getsocial'
getsocial {{
    appId '{0}'
}}
", GetSocialSettings.AppId);
        }
    }
}
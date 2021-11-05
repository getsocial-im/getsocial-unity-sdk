using System;
using System.Collections.Generic;
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
            string jsonSourceFilePath = Path.Combine(Application.streamingAssetsPath, "getsocial.json");
            string jsonTargetFilePath;
            string mainGradleFilePath;
            string gradleFilePath;
#if UNITY_2019_3_OR_NEWER
            mainGradleFilePath = path + "/../build.gradle";
            gradleFilePath = path + "/../launcher/build.gradle";
            jsonTargetFilePath = Path.Combine(path, "../launcher/getsocial.json");
#else
            mainGradleFilePath = path + "/build.gradle";
            gradleFilePath = path + "/build.gradle";
            jsonTargetFilePath = Path.Combine(path, "getsocial.json");
#endif
            if (File.Exists(mainGradleFilePath))
            {
                var modifiedGradleProperties = new List<string>();
                var lines = File.ReadAllLines(mainGradleFilePath);
                bool mavenRepoAdded = false;
                bool dependencyAdded = false;

                foreach (string line in lines)
                {

                    modifiedGradleProperties.Add(line);
                    if (line.Contains(gradle_repositories) && !mavenRepoAdded)
                    {
                        modifiedGradleProperties.Add("\t\tmaven {");
                        modifiedGradleProperties.Add("\t\t\turl 'https://plugins.gradle.org/m2/'");
                        modifiedGradleProperties.Add("\t\t}");
                        mavenRepoAdded = true;
                    }
                    if (line.Contains(gradle_dependencies) && !dependencyAdded)
                    {
                        modifiedGradleProperties.Add("\t\tclasspath 'im.getsocial:plugin-v7:[1,2)'");
                        dependencyAdded = true;
                        
                    }
                }
                File.WriteAllText(mainGradleFilePath, string.Join("\n", modifiedGradleProperties.ToArray()) + "\n");
            }
            else {
                File.AppendAllText(mainGradleFilePath, mainGradleText());
            }
            File.AppendAllText(gradleFilePath, gradleText());
            FileUtil.CopyFileOrDirectory(jsonSourceFilePath, jsonTargetFilePath);

            if (PlayerSettings.applicationIdentifier == "com.Company.ProductName")
            {
                Debug.LogError(
                    "GetSocial: Please change the default Unity Bundle Identifier (com.Company.ProductName) to your package.");
            }
        }

        private static string gradle_repositories = "repositories";
        private static string gradle_dependencies = "dependencies";

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
    version '7.6.8'
}}
", GetSocialSettings.AppId);
        }
    }
}

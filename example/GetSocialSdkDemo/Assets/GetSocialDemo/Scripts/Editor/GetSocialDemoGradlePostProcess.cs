using System;
using System.IO;
using GetSocialSdk.Core;
using UnityEditor;
using UnityEditor.Android;
using UnityEngine;

namespace GetSocialSdk.Editor
{
    class GetSocialDemoGradlePostProcess : IPostGenerateGradleAndroidProject
    {
        public int callbackOrder
        {
            get { return 1; }
        }

        public void OnPostGenerateGradleAndroidProject(string path)
        {
            Debug.Log("GetSocialDemoGradlePostProcess.OnPostGenerateGradleAndroidProject at path " + path);
            File.AppendAllText(path + "/../gradle.properties", 
                        Environment.NewLine + "systemProp.im.getsocial.plugin.developerMode=true" + Environment.NewLine);
        }
    }
}
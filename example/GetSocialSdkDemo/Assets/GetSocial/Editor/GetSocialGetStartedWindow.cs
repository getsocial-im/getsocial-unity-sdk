using UnityEditor;
using UnityEngine;

namespace GetSocialSdk.Editor
{
    public class GetSocialGetStartedWindow : WebViewEditorWindow
    {
        private const string DoNotShowGetSocialWelcomeScreenPref = "DoNotShowGetSocialGetStartedWindow";

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            if (!PlayerPrefs.HasKey(DoNotShowGetSocialWelcomeScreenPref))
            {
                PlayerPrefs.SetInt(DoNotShowGetSocialWelcomeScreenPref, 1);
                PlayerPrefs.Save();

                Open();
            }
        }

        [MenuItem("GetSocial/Getting Started", false, 1000)]
        static void Open()
        {
            // FIXME: a terrible hack to go around broken native to js calls.
            // Ideally after page was loaded in the webview we have call JS method from Unity to set which skin to use
            // but calling JS form native is broken, so have to go around it.
            // Details: https://github.com/kimsama/Unity-WebViewEditorWindow/issues/1
            var pageToLoad = EditorGUIUtility.isProSkin ? "index_pro.html" : "index.html";
            var pagePath = Application.dataPath + "/GetSocial/Editor/HTML/" + pageToLoad;

            CreateWebViewEditorWindow<GetSocialGetStartedWindow>("GetSocial", pagePath, 200, 400, 800, 600);
        }

        public void OpenGetSocialSettings()
        {
            GetSocialSettingsEditor.Edit();
        }
    }
}
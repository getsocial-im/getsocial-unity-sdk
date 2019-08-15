using System.IO;
using System.Linq;
using GetSocialSdk.Core;
using UnityEditor;
using UnityEngine;

namespace GetSocialSdk.Editor {
    public class GetSocialGetStartedWindow : EditorWindow {
        private const string DoNotShowGetSocialWelcomeScreenPref = "DoNotShowGetSocialGetStartedWindow";

        private Texture2D _image;

        [MenuItem ("GetSocial/Getting Started", false, 1000)]
        static void Open () {
            GetSocialGetStartedWindow window = GetWindow<GetSocialGetStartedWindow> ("GetSocial");
            window.maxSize = new Vector2 (470, 600);
            window.minSize = new Vector2 (470, 400);
            window.Show (true);
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded () {
            var arguments = System.Environment.GetCommandLineArgs ();
            if (arguments.Contains ("-nographics")) {
                Debug.Log ("Don't open GetSocial Window if `nographics` param is passed");
                return;
            }
            // Open window automatically on the first launch
            // Keep PlayerPrefs for the compatibility with previous version if it was already saved there
            if (EditorPrefs.HasKey (DoNotShowGetSocialWelcomeScreenPref) || PlayerPrefs.HasKey (DoNotShowGetSocialWelcomeScreenPref)) return;

            EditorPrefs.SetInt (DoNotShowGetSocialWelcomeScreenPref, 1);
#if !UNITY_CLOUD_BUILD
            Open ();
#endif           
        }

        void OnEnable () {
            _image = new Texture2D (1, 1);
            var imagePath = Path.Combine (GetSocialSettings.GetPluginPath (), "Editor/GUI/" + Styles.CurrentSkin.ImageName);
            _image.LoadImage (System.IO.File.ReadAllBytes (imagePath));
            _image.Apply ();
        }

        void OnGUI () {
            EditorGUILayout.BeginVertical ();
            GUILayout.Label (_image, Styles.LogoStyle);
            GUILayout.Label ("Thanks for installing GetSocial Unity SDK", Styles.HeaderStyle);
            GUILayout.Label ("To start with GetSocial:", Styles.H2Style);

            EditorGUILayout.BeginHorizontal ();
            GUILayout.Label ("1. Create your account at ", Styles.TextStyle);
            if (GUILayout.Button ("dashboard.getsocial.im", Styles.ButtonTextStyle)) {
                OpenURL ("https://dashboard.getsocial.im");
            }
            GUILayout.FlexibleSpace ();
            EditorGUILayout.EndHorizontal ();

            EditorGUILayout.BeginHorizontal ();
            GUILayout.Label ("2. Copy GetSocial app ID from the Dashboard into ", Styles.TextStyle);
            if (GUILayout.Button ("GetSocial Unity settings", Styles.ButtonTextStyle)) {
                GetSocialSettingsEditor.Edit ();
            }
            GUILayout.FlexibleSpace ();
            EditorGUILayout.EndHorizontal ();

            EditorGUILayout.BeginHorizontal ();
            GUILayout.Label ("3. Visit ", Styles.TextStyle);
            if (GUILayout.Button ("documentation", Styles.ButtonTextStyle)) {
                OpenURL ("https://docs.getsocial.im");
            }
            GUILayout.Label (" to learn how to integrate GetSocial features", Styles.TextStyleWithoutMargin);
            GUILayout.FlexibleSpace ();
            EditorGUILayout.EndHorizontal ();

            GUILayout.Label ("", Styles.LineStyle);

            EditorGUILayout.BeginHorizontal ();
            GUILayout.Space (Styles.Padding);
            if (GUILayout.Button ("Product overview", Styles.ButtonTextStyle)) {
                OpenURL ("https://www.getsocial.im/products/");
            }
            GUILayout.Space (Styles.Padding + 5);
            if (GUILayout.Button ("Docs", Styles.ButtonTextStyle)) {
                OpenURL ("https://docs.getsocial.im");
            }
            GUILayout.Space (Styles.Padding + 5);
            if (GUILayout.Button ("Support", Styles.ButtonTextStyle)) {
                OpenURL ("mailto:support@getsocial.im");
            }
            GUILayout.FlexibleSpace ();
            EditorGUILayout.EndHorizontal ();
            EditorGUILayout.EndVertical ();
        }

        private static void OpenURL (string url) {
            if (url.StartsWith ("http")) {
                url += "?utm_source=" + BuildConfig.PublishTarget + "&utm_medium=unity-editor-onboarding-page";
            }
            Application.OpenURL (url);
        }

        private static class Styles {
            public const int Padding = 15;
            public static Skin CurrentSkin;

            public static GUIStyle LogoStyle, HeaderStyle, H2Style, TextStyle, TextStyleWithoutMargin, ButtonTextStyle, LineStyle;
            static Styles () {
                CurrentSkin = EditorGUIUtility.isProSkin ? (Skin) new ProSkin () : new NormalSkin();

                LogoStyle = new GUIStyle ();
                LogoStyle.fixedHeight = 50;
                LogoStyle.margin = new RectOffset (Padding, 0, Padding, 0);

                HeaderStyle = new GUIStyle ();
                HeaderStyle.fontSize = 20;
                HeaderStyle.fontStyle = FontStyle.Bold;
                HeaderStyle.margin = new RectOffset (Padding, 0, Padding, 0);
                HeaderStyle.normal.textColor = CurrentSkin.TextColor;

                H2Style = new GUIStyle ();
                H2Style.fontSize = 17;
                H2Style.fontStyle = FontStyle.Bold;
                H2Style.margin = new RectOffset (Padding, 0, Padding * 2, 0);
                H2Style.normal.textColor = CurrentSkin.TextColor;

                TextStyle = new GUIStyle ();
                TextStyle.margin = new RectOffset (Padding, 0, Padding, 0);
                TextStyle.fontSize = 12;
                TextStyle.normal.textColor = CurrentSkin.TextColor;

                TextStyleWithoutMargin = new GUIStyle ();
                TextStyleWithoutMargin.margin = new RectOffset (0, 0, Padding, 0);
                TextStyleWithoutMargin.fontSize = 12;
                TextStyleWithoutMargin.normal.textColor = CurrentSkin.TextColor;

                LineStyle = new GUIStyle ();
                LineStyle.fixedHeight = 2;
                LineStyle.stretchWidth = true;
                LineStyle.margin = new RectOffset (Padding, 0, Padding * 2, 0);
                LineStyle.normal.background = new Texture2D (1, 1);
                LineStyle.normal.background.SetPixel (0, 0, Color.white);

                ButtonTextStyle = new GUIStyle ();
                ButtonTextStyle.margin = new RectOffset (0, 0, Padding, 0);
                ButtonTextStyle.fontSize = 12;
                ButtonTextStyle.alignment = TextAnchor.MiddleLeft;
                ButtonTextStyle.normal.textColor = CurrentSkin.LinkColor;
            }
        }

        private interface Skin {
            Color TextColor { get; }
            Color LinkColor { get; }
            string ImageName { get; }
        }

        private class NormalSkin : Skin {
            private readonly Color _textColor = new Color32 (40, 40, 40, 255);
            private readonly Color _linkColor = new Color32 (21, 98, 228, 255);

            public Color TextColor { get { return _textColor; } }
            public Color LinkColor { get { return _linkColor; } }
            public string ImageName { get { return "logo.png"; } }
        }

        private class ProSkin : Skin {
            private readonly Color _textColor = new Color32 (219, 219, 219, 255);
            private readonly Color _linkColor = new Color32 (87, 141, 232, 255);

            public Color TextColor { get { return _textColor; } }
            public Color LinkColor { get { return _linkColor; } }
            public string ImageName { get { return "logo_pro.png"; } }
        }
    }
}
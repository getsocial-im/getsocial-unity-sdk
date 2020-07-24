#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;

namespace GetSocialSdk.Core
{

    internal class LocalizableText
    {
        // FIXME: get current language on main thread
        //internal static string CurrentLanguage {
        //    get {
        //        MainThreadExecutor.Queue(() => {
        //            return Application.systemLanguage.ToLanguageCode();
        //        });
        //    }
        //}

        // private static string NonLocalizedOverride = "non_localized_override";

        public Dictionary<string, string> LocalizedTexts { get; private set; }

        internal LocalizableText(string unlocalizableText)
        {
            LocalizedTexts = new Dictionary<string, string>();
        }

        internal LocalizableText(Dictionary<string, string> localizedTexts)
        {
            LocalizedTexts = localizedTexts;
        }

        internal string LocalizedValue()
        {
            //if (LocalizedTexts.ContainsKey(NonLocalizedOverride))
            //{
            //    return LocalizedTexts[NonLocalizedOverride];
            //}
            //if (LocalizedTexts.ContainsKey(CurrentLanguage))
            //{
            //    return LocalizedTexts[CurrentLanguage];
            //}
            //if (LocalizedTexts.ContainsKey("en"))
            //{
            //    return LocalizedTexts["en"];
            //}
            if (LocalizedTexts.Count == 1)
            {
                return LocalizedTexts.Values.First();
            }
            return "";
        }

    }
}
#endif
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace GetSocialSdk.Core
{
    internal static class Localization
    {
        private static readonly IDictionary<SystemLanguage, string> SystemLanguageToLanguageCodeSupportsGetSocial =
            new Dictionary<SystemLanguage, string>
            {
                {SystemLanguage.ChineseSimplified, LanguageCodes.ChineseSimplified},
                {SystemLanguage.Chinese, LanguageCodes.ChineseTraditional},
                {SystemLanguage.ChineseTraditional, LanguageCodes.ChineseTraditional},
                {SystemLanguage.Danish, LanguageCodes.Danish},
                {SystemLanguage.Dutch, LanguageCodes.Dutch},
                {SystemLanguage.English, LanguageCodes.English},
                {SystemLanguage.French, LanguageCodes.French},
                {SystemLanguage.German, LanguageCodes.German},
                {SystemLanguage.Icelandic, LanguageCodes.Icelandic},
                {SystemLanguage.Indonesian, LanguageCodes.Indonesian},
                {SystemLanguage.Italian, LanguageCodes.Italian},
                {SystemLanguage.Japanese, LanguageCodes.Japanese},
                {SystemLanguage.Korean, LanguageCodes.Korean},
                {SystemLanguage.Norwegian, LanguageCodes.Norwegian},
                {SystemLanguage.Polish, LanguageCodes.Polish},
                {SystemLanguage.Portuguese, LanguageCodes.Portuguese},
                {SystemLanguage.Russian, LanguageCodes.Russian},
                {SystemLanguage.Spanish, LanguageCodes.Spanish},
                {SystemLanguage.Swedish, LanguageCodes.Swedish},
                {SystemLanguage.Turkish, LanguageCodes.Turkish},
                {SystemLanguage.Ukrainian, LanguageCodes.Ukrainian},
                {SystemLanguage.Vietnamese, LanguageCodes.Vietnamese}
            };
        
        private static readonly IDictionary<SystemLanguage, string> SystemLanguageToLanguageCode = new Dictionary<SystemLanguage, string>
        {
            {SystemLanguage.Afrikaans, "af"},
            {SystemLanguage.Arabic, "ar"},
            {SystemLanguage.Basque, "eu"},
            {SystemLanguage.Belarusian, "be"},
            {SystemLanguage.Bulgarian, "bg"},
            {SystemLanguage.Catalan, "ca"},
            {SystemLanguage.Czech, "cs"},
            {SystemLanguage.Estonian, "et"},
            {SystemLanguage.Faroese, "fo"},
            {SystemLanguage.Finnish, "fi"},
            {SystemLanguage.Greek, "el"},
            {SystemLanguage.Hebrew, "he"},
            {SystemLanguage.Hungarian, "hu"},
            {SystemLanguage.Latvian, "lv"},
            {SystemLanguage.Lithuanian, "lt"},
            {SystemLanguage.Romanian, "ro"},
            {SystemLanguage.SerboCroatian, "sh"},
            {SystemLanguage.Slovak, "sk"},
            {SystemLanguage.Slovenian, "sl"},
            {SystemLanguage.Thai, "th"},
            {SystemLanguage.Unknown, "-"}
        };
        
        public static string ToLanguageCode(this SystemLanguage language)
        {
            return SystemLanguageToLanguageCodeSupportsGetSocial.ContainsKey(language)
                ? SystemLanguageToLanguageCodeSupportsGetSocial[language]
                : SystemLanguageToLanguageCode.ContainsKey(language) 
                    ? SystemLanguageToLanguageCode[language] : "-";
        }

        public static string ValidateLanguageCode(string languageCode)
        {
            return AllLanguages.Contains(languageCode)
                ? languageCode
                : FallbackLanguages.ContainsKey(languageCode)
                    ? FallbackLanguages[languageCode]
                    : LanguageCodes.DefaultLanguage;
        }
        
        private static IList<string> AllLanguages
        {
            get
            {
                return typeof(LanguageCodes).GetFields(BindingFlags.Public | BindingFlags.Static)
                    .Where(f => f.FieldType == typeof(string))
                    .Where(f => !string.Equals(f.Name, "DefaultLanguage", StringComparison.Ordinal))
                    .ToList()
                    .ConvertAll(f => (string) f.GetValue(null));
            }
        }

        private static IDictionary<string, string> FallbackLanguages
        {
            get
            {
                return new Dictionary<string, string>
                {
                    {"zh", LanguageCodes.ChineseSimplified},
                    {"zh-CN", LanguageCodes.ChineseSimplified},
                    {"zh-SG", LanguageCodes.ChineseSimplified},
                    {"zh-TW", LanguageCodes.ChineseTraditional},
                    {"zh-HK", LanguageCodes.ChineseTraditional},
                };
            }
        }
    }
}
#endif

#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
using UnityEngine;

namespace GetSocialSdk.Core
{
    internal static class NativeBuildConfig
    {
        public const string SdkVersion = BuildConfig.UnitySdkVersion;
        public static string HadesUrl = HadesProductionUrl;

#if GETSOCIAL_DEBUG
        public const bool Debug = true;
#else
        public const bool Debug = false;
#endif

        internal const string HadesProductionUrl = "https://hades.getsocial.im/sdk";
        internal const string HadesTestingUrl = "https://hades.testing.getsocial.im/sdk";
    }
}
#endif

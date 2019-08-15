#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
namespace GetSocialSdk.Core
{
    internal static class AnalyticsEventDetails
    {
        public const string AppSessionStart = "app_session_start";
        public const string AppSessionEnd = "app_session_end";
    }
}
#endif

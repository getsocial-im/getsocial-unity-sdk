#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
namespace GetSocialSdk.Core
{
    public static class EngineUtils
    {
        public static bool IsApplicationRunning()
        {
            return ApplicationStateListener.Instance != null;
        }
    }
}
#endif

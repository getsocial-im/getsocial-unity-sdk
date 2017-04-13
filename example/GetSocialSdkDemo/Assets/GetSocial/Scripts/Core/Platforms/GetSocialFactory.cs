namespace GetSocialSdk.Core
{
    static class GetSocialFactory
    {
        internal static IGetSocialNativeBridge Instance
        {
            get
            {
#if UNITY_ANDROID && !UNITY_EDITOR
            return GetSocialNativeBridgeAndroid.Instance;
#elif UNITY_IOS && !UNITY_EDITOR
            return GetSocialNativeBridgeIOS.Instance;
#else
                // if UNITY_EDITOR
                return GetSocialNativeBridgeMock.Instance;
#endif
            }
        }
    }
}
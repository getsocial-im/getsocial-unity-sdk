namespace GetSocialSdk.Core
{
    static class GetSocialFactory
    {
        private static IGetSocialBridge _bridge;

        internal static IGetSocialBridge Bridge
        {
            get { return _bridge ?? (_bridge = FindBridge()); }
        }

        private static IGetSocialBridge FindBridge()
        {
#if UNITY_EDITOR
            return new GetSocialNativeBridge();
#elif UNITY_ANDROID
            return new GetSocialJsonBridge(new MethodCallerAndroid());
#elif UNITY_IOS
            return new GetSocialJsonBridge(new MethodCalleriOS());
#else 
            return new GetSocialNativeBridge();
#endif
        }
    }
}
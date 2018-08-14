#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

namespace GetSocialSdk.Core
{
    public static class GetSocialFBMessengerPluginHelper
    {
        public static void RegisterFBMessengerPlugin()
        {
#if UNITY_IOS && !UNITY_EDITOR
            _gs_registerFBMessengerPlugin();
#endif
        }
    
#if UNITY_IOS && !UNITY_EDITOR
        [DllImport("__Internal")]
        static extern void _gs_registerFBMessengerPlugin();
#endif
    }
}
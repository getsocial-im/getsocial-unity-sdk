#if UNITY_IOS
using System;
using AOT;

namespace GetSocialSdk.Core
{
    public static class InviteChannelPluginCallbacks
    {
        public delegate bool IsAvailableForDeviceDelegate(IntPtr instancePtr, string inviteChannelJson);

        public delegate void PresentChannelInterfaceDelegate(
            IntPtr instancePtr, string inviteChannelJson, string invitePackageJson,
            IntPtr onCompletePtr, IntPtr onCancelPtr, IntPtr onFailurePtr);

        [MonoPInvokeCallback(typeof(IsAvailableForDeviceDelegate))]
        public static bool IsAvailableForDevice(IntPtr instancePtr, string inviteChannelJson)
        {
            var channel = new InviteChannel().ParseFromJson(inviteChannelJson);
            return instancePtr.Cast<InviteChannelPlugin>().IsAvailableForDevice(channel);
        }

        [MonoPInvokeCallback(typeof(PresentChannelInterfaceDelegate))]
        public static void PresentChannelInterface(IntPtr instancePtr, string inviteChannelJson,
            string invitePackageJson,
            IntPtr onCompletePtr, IntPtr onCancelPtr, IntPtr onFailurePtr)
        {
            var channel = new InviteChannel().ParseFromJson(inviteChannelJson);
            var package = new InvitePackage().ParseFromJson(invitePackageJson);

            instancePtr.Cast<InviteChannelPlugin>()
                .PresentChannelInterface(channel, package,
                    () => { GetSocialNativeBridgeIOS._gs_executeInviteSuccessCallback(onCompletePtr); },
                    () => { GetSocialNativeBridgeIOS._gs_executeInviteSuccessCallback(onCancelPtr); },
                    exception => { GetSocialNativeBridgeIOS._gs_executeInviteSuccessCallback(onFailurePtr); });
        }
    }
}

#endif
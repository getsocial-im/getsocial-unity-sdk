#import <GetSocialSDK/GetSocialSDK.h>
#import "GetSocialFBMessengerInvitePlugin.h"

#pragma clang diagnostic push
#pragma ide diagnostic ignored "OCUnusedGlobalDeclarationInspection"
extern "C" {
    void _gs_registerFBMessengerPlugin()
    {
        [GetSocialInvites registerPlugin:[GetSocialFBMessengerInvitePlugin new] forChannel:GetSocialInviteChannelIds.facebookMessenger];
    }
}
#pragma clang diagnostic pop

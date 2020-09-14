#import <GetSocialSDK/GetSocialSDK.h>
#import "GetSocialFacebookSharePlugin.h"

#pragma clang diagnostic push
#pragma ide diagnostic ignored "OCUnusedGlobalDeclarationInspection"
extern "C" {
    void _gs_registerFacebookSharePlugin()
    {
        [GetSocialInvites registerPlugin:[GetSocialFacebookSharePlugin new] forChannel:GetSocialInviteChannelIds.facebook];
    }
}
#pragma clang diagnostic pop

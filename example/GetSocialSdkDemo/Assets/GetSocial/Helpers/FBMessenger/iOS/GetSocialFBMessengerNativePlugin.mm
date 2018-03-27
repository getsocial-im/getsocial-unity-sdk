#import <GetSocial/GetSocial.h>
#import "GetSocialFBMessengerInvitePlugin.h"

#pragma clang diagnostic push
#pragma ide diagnostic ignored "OCUnusedGlobalDeclarationInspection"
extern "C" {
    void _gs_registerFBMessengerPlugin()
    {
        [GetSocial registerInviteChannelPlugin:[GetSocialFBMessengerInvitePlugin new] forChannelId:GetSocial_InviteChannelPluginId_Facebook_Messenger];
    }
}
#pragma clang diagnostic pop

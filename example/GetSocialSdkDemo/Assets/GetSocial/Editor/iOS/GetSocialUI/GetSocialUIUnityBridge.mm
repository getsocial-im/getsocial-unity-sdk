//
// Created by Taras Leskiv on 05/01/2017.
//

#include <GetSocialUI/GetSocialUI.h>
#include "GetSocialBridgeUtils.h"
#include "GetSocialJsonUtils.h"
#import "GetSocialFunctionDefs.h"

typedef void(ActivityActionButtonClickedDelegate)(void *actionPtr, const char *buttonId, const char *serializedActivityPost);

#pragma clang diagnostic push
#pragma ide diagnostic ignored "OCUnusedGlobalDeclarationInspection"
extern "C" {
NS_ASSUME_NONNULL_BEGIN

bool _showSmartInvitesView(
        const char *title,
        const char *serializedInviteContent,
        const char *serializedCustomReferralData,
        StringCallbackDelegate stringCallback /* with providerId */, void *onInviteCompletePtr, void *onInviteCancelPtr,
        FailureWithDataCallbackDelegate failureCallback, void *onFailurePtr) {

    GetSocialUIInvitesView *invitesView = [GetSocialUI createInvitesView];

    if (title) {
        NSString *titleStr = [GetSocialBridgeUtils createNSStringFrom:title];
        invitesView.windowTitle = titleStr;
    }

    if (serializedInviteContent) {
        NSString *serializedInviteContentStr = [GetSocialBridgeUtils createNSStringFrom:serializedInviteContent];
        GetSocialMutableInviteContent *customInviteContent = [GetSocialJsonUtils deserializeCustomInviteContent:serializedInviteContentStr];
        [invitesView setCustomInviteContent:customInviteContent];
    }

    if (serializedCustomReferralData) {
        NSString *serializedCustomReferralDataStr = [GetSocialBridgeUtils createNSStringFrom:serializedCustomReferralData];
        NSDictionary *referralData = [GetSocialJsonUtils deserializeCustomReferralData:serializedCustomReferralDataStr];
        [invitesView setCustomReferralData:referralData];
    }

    if (onInviteCompletePtr) {
        [invitesView setHandlerForInvitesSent:^(NSString *_Nonnull providerId) {
            stringCallback(onInviteCompletePtr, providerId.UTF8String);

        }                              cancel:^(NSString *_Nonnull providerId) {
            stringCallback(onInviteCancelPtr, providerId.UTF8String);
        }                             failure:^(NSString *_Nonnull providerId, NSError *_Nonnull error) {
            const char *serializedErr = [GetSocialJsonUtils serializeError:error].UTF8String;
            failureCallback(onFailurePtr, providerId.UTF8String, serializedErr);
        }];
    }

    [invitesView show];
    return true;
}

#pragma mark UI Configuration

bool _loadConfiguration(const char *filePath) {
    NSString *filePathStr = [GetSocialBridgeUtils createNSStringFrom:filePath];
    return [GetSocialUI loadConfiguration:filePathStr];
}

bool _loadDefaultConfiguration(const char *filePath) {
    return [GetSocialUI loadDefaultConfiguration];
}

#pragma mark Close-Open

bool _closeView(bool saveViewState) {
    return [GetSocialUI closeView:saveViewState];
}

bool _restoreView() {
    return [GetSocialUI restoreView];
}

bool _showActivityFeedView(
        const char *feed,
        ActivityActionButtonClickedDelegate callback, void *onButtonClickPtr) {
    NSString *feedStr = [GetSocialBridgeUtils createNSStringFrom:feed];

    GetSocialUIActivityFeedView *view = [GetSocialUI createActivityFeedView:feedStr];

    if (onButtonClickPtr) {
        [view setActionListener:^(NSString *action, GetSocialActivityPost *post) {
            NSString *serializedPost = [GetSocialJsonUtils serializeActivityPost: post];
            callback(onButtonClickPtr, action.UTF8String, serializedPost.UTF8String);
        }];
    }
    
    [view show];
    return true;
}

#pragma mark - Activity Feed

NS_ASSUME_NONNULL_END
}

#pragma clang diagnostic pop

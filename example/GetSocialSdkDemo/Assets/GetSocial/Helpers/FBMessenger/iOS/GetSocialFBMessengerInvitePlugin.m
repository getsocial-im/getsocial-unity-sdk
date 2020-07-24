/*
 *    	Copyright 2015-2020 GetSocial B.V.
 *
 *	Licensed under the Apache License, Version 2.0 (the "License");
 *	you may not use this file except in compliance with the License.
 *	You may obtain a copy of the License at
 *
 *    	http://www.apache.org/licenses/LICENSE-2.0
 *
 *	Unless required by applicable law or agreed to in writing, software
 *	distributed under the License is distributed on an "AS IS" BASIS,
 *	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *	See the License for the specific language governing permissions and
 *	limitations under the License.
 */

#import "GetSocialFBMessengerInvitePlugin.h"
#import <FBSDKShareKit/FBSDKShareKit.h>

@interface GetSocialFBMessengerInvitePlugin ()<FBSDKSharingDelegate>

@property(nonatomic, copy) void (^successCallback)(NSDictionary<NSString *,NSString *> *);
@property(nonatomic, copy) void (^cancelCallback)(NSDictionary<NSString *,NSString *> *);
@property(nonatomic, copy) void (^failureCallback)(NSError*, NSDictionary<NSString *,NSString *> *);

@end

@implementation GetSocialFBMessengerInvitePlugin

- (BOOL)isAvailableForDevice:(GetSocialInviteChannel *)inviteChannel
{
    return [[UIApplication sharedApplication] canOpenURL:[NSURL URLWithString:@"fb-messenger-api://"]]            // old version scheme
           || [[UIApplication sharedApplication] canOpenURL:[NSURL URLWithString:@"fb-messenger-share-api://"]];  // new version scheme
}

- (void)presentPluginWithInviteChannel:(GetSocialInviteChannel *)inviteChannel
                         invite:(GetSocialInvite *)invite
                      onViewController:(UIViewController *)viewController
                               success:(void (^)(NSDictionary<NSString *,NSString *> *))successCallback
                                cancel:(void (^)(NSDictionary<NSString *,NSString *> *))cancelCallback
                               failure:(void (^)(NSError* error, NSDictionary<NSString *,NSString *> *))failureCallback
{
    self.successCallback = successCallback;
    self.failureCallback = failureCallback;
    self.cancelCallback = cancelCallback;

    FBSDKShareLinkContent *content = [[FBSDKShareLinkContent alloc] init];
    content.contentURL = [NSURL URLWithString:invite.referralUrl];

    [FBSDKMessageDialog showWithContent:content delegate:self];
}

- (void)sharer:(id<FBSDKSharing>)sharer didCompleteWithResults:(NSDictionary *)results
{
    BOOL didComplete = [results[@"didComplete"] boolValue];
    NSString *completionGesture = results[@"completionGesture"];
    if (didComplete)
    {
        if (completionGesture && ![completionGesture isEqualToString:@"cancel"])
        {
            if (self.successCallback)
            {
                self.successCallback(@{});
            }
        }
        else
        {
            if (self.cancelCallback)
            {
                self.cancelCallback(@{});
            }
        }
    }
    else
    {
        if (self.failureCallback)
        {
            self.failureCallback(
                                 [NSError errorWithDomain:@"GetSocialFBMessengerInvitePlugin" code:1000 userInfo:@{NSLocalizedDescriptionKey : @"Failed to invite."}], @{});
        }
    }
}

- (void)sharer:(id<FBSDKSharing>)sharer didFailWithError:(NSError *)error
{
    if (self.failureCallback)
    {
        self.failureCallback(error, @{});
    }
}

- (void)sharerDidCancel:(id<FBSDKSharing>)sharer
{
    if (self.cancelCallback)
    {
        self.cancelCallback(@{});
    }
}

@end

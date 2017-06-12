//
//  GetSocial
//
//  Copyright © 2017 GetSocial BV. All rights reserved.
//

#import <GetSocial/GetSocialConstants.h>
#import <GetSocial/GetSocialInviteContent.h>
#import <GetSocial/GetSocialInvitePackage.h>
#import <GetSocial/GetSocialInviteProperties.h>
#import <GetSocial/GetSocialInviteChannel.h>

/*!
 * @abstract Base abstract class for all GetSocial invite channel plugins.
 * @discussion Use this class as a base for your own invite channel plugins implementations.
 * List of supported invite channels is defined in GetSocialConstants file.
 */
@interface GetSocialInviteChannelPlugin : NSObject

NS_ASSUME_NONNULL_BEGIN

/** @name Properties */

/*!
 * @abstract Called if invite sending was successful.
 */
@property(nonatomic) GetSocialInviteSuccessCallback successCallback;

/*!
 * @abstract Called if invite sending was cancelled.
 */
@property(nonatomic) GetSocialInviteCancelCallback cancelCallback;

/*!
 * @abstract Called if invite sending failed.
 */
@property(nonatomic) GetSocialFailureCallback failureCallback;

/** @name Methods */

/*!
 * @abstract Checks if invite channel plugin is available for the device.
 * @discussion You can check if plugin is enabled on GetSocial Dashboard.
 * @see GetSocialInviteChannel class isEnabled method.
 * @param inviteChannel channel to check
 * @result YES, if available, otherwise NO.
 */
- (BOOL)isAvailableForDevice:(GetSocialInviteChannel *)inviteChannel;

/*!
 * @abstract Implementation of the method should present invite channel plugin UI interface.
 * @discussion <b>IMPORTANT:</b> At least one of the provided callbacks should be invoked as a result
 *      of method execution. Ignoring this requirement will result in misbehaviour of the GetSocial Framework
 *      and incorrect analytics data.
 *
 * @param inviteChannel     Invite channel to use.
 * @param invitePackage     invite content to send.
 * @param viewController    viewController to be used for presentation.
 * @param successCallback   Callback to call if invite sending was successfull.
 * @param cancelCallback    Callback to call if invite sending was cancelled.
 * @param failureCallback   Callback to call if invite sending failed.
 */
- (void)presentPluginWithInviteChannel:(GetSocialInviteChannel *)inviteChannel
                         invitePackage:(GetSocialInvitePackage *)invitePackage
                      onViewController:(UIViewController *)viewController
                               success:(GetSocialInviteSuccessCallback)successCallback
                                cancel:(GetSocialInviteCancelCallback)cancelCallback
                               failure:(GetSocialFailureCallback)failureCallback;

NS_ASSUME_NONNULL_END

@end

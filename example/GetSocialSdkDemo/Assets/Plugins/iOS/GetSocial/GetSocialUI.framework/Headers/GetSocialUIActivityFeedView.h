//
//  GetSocialUI
//
//  Copyright © 2017 GetSocial BV. All rights reserved.
//

#import <GetSocialUI/GetSocialUIView.h>
#import <GetSocialUI/GetSocialUIPublicConstants.h>

NS_ASSUME_NONNULL_BEGIN

/*!
 *  @abstract Defines GetSocialUIActivityFeedView class.
 */
@interface GetSocialUIActivityFeedView : GetSocialUIView

/*!
 *  @abstract Creates GetSocialUIActivityFeedView using the provided feed id.
 *
 *  @param feed id of activity feed.
 */
+ (instancetype)viewForFeed:(NSString *)feed;

/*!
 *  @abstract Sets an action handler, that will be called if button on Activity Feed is pressed.
 *
 *  @param buttonActionHandler block that will be called.
 */
- (void)setActionButtonHandler:(ActivityButtonActionHandler)buttonActionHandler;

/*!
 *  @abstract Sets a handler, that will be called if user clicks on any user avatar.
 *
 *  @param avatarClickHandler block that will be called.
 */
- (void)setAvatarClickHandler:(AvatarClickHandler)avatarClickHandler;

NS_ASSUME_NONNULL_END

@end

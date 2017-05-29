//
// Created by Orest Savchak on 5/16/17.
// Copyright (c) 2017 GetSocial BV. All rights reserved.
//

#import <Foundation/Foundation.h>

#import <GetSocialUI/GetSocialUIView.h>
#import <GetSocialUI/GetSocialUIPublicConstants.h>

@interface GetSocialUIActivityDetailsView : GetSocialUIView

NS_ASSUME_NONNULL_BEGIN

/*!
 *  @abstract Create a new instance.
 *
 *  @param activityId Activity ID.
 */
+ (instancetype)viewForActivityId:(NSString *)activityId;

/*!
 *  @abstract Sets an action handler, that will be called if button on Activity Feed is pressed.
 *
 *  @param actionButtonHandler block that will be called.
 */
- (void)setActionButtonHandler:(ActivityButtonActionHandler)actionButtonHandler;

/*!
 *  @abstract By default is YES. If you want to open only comments, without feed view in history, set is to NO.
 *
 *  @param showActivityFeed Show feed or not.
 */
- (void)setShowActivityFeedView:(BOOL)showActivityFeed;

/*!
 *  @abstract Sets a handler, that will be called if user clicks on any user avatar.
 *
 *  @param avatarClickHandler block that will be called.
 */
- (void)setAvatarClickHandler:(AvatarClickHandler)avatarClickHandler;

NS_ASSUME_NONNULL_END;

@end

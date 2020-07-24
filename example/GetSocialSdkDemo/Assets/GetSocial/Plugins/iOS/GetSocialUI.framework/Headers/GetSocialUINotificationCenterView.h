//
//  GetSocialUINotificationCenterView.h
//  GetSocialUI
//
//  Copyright © 2020 GetSocial BV. All rights reserved.
//

#import <GetSocialSDK/GetSocialSDK.h>
#import "GetSocialUIView.h"

NS_ASSUME_NONNULL_BEGIN

/*!
 * Describes GetSocialUINotificationCenterView class.
 */
@interface GetSocialUINotificationCenterView : GetSocialUIView

/*!
 *  @abstract Creates GetSocialUINotificationCenterView using the notification query.
 *
 *  @param query feeds query.
 */
+ (instancetype)viewForQuery:(GetSocialNotificationsQuery *)query;

/*!
 *  @abstract   Sets notification click handler.
 */
@property(nonatomic, copy) GetSocialUINotificationClickHandler clickHandler;

@end

NS_ASSUME_NONNULL_END

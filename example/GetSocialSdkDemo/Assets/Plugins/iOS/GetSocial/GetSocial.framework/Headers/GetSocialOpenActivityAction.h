//
//  GetSocial
//
//  Copyright © 2017 GetSocial BV. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "GetSocialNotificationAction.h"

/*!
 * Action should open activity with provided identifier.
 */
@interface GetSocialOpenActivityAction : GetSocialNotificationAction

/*!
 * @abstract Identifier of activity that should be opened.
 */
@property (nonatomic, strong, readonly) NSString *activityId;

@end

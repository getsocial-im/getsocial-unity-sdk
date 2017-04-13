//
//  GetSocial
//
//  Copyright © 2017 GetSocial BV. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "GetSocialConstants.h"

/*!
 * Represents an action, that should be executed if application was started with clicking on GetSocial push notification.
 */
@interface GetSocialNotificationAction : NSObject

/*!
 * Returns an enum value of the action.
 */
@property (nonatomic, readonly) GetSocialNotificationActionType action;

@end

//
// Created by Orest Savchak on 5/17/17.
// Copyright (c) 2017 GetSocial BV. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "GetSocialNotificationAction.h"

/*!
 * Action should open user profile with provided identifier.
 */
@interface GetSocialOpenProfileAction : GetSocialNotificationAction

/*!
 * @abstract Identifier of user to open profile for.
 */
@property (nonatomic, strong, readonly) NSString *userId;

@end
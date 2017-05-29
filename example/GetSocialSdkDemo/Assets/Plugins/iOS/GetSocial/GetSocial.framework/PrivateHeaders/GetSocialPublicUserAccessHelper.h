//
// Created by Orest Savchak on 5/26/17.
// Copyright (c) 2017 GetSocial BV. All rights reserved.
//

#import <Foundation/Foundation.h>

@class GetSocialPublicUser;

@interface GetSocialPublicUserAccessHelper : NSObject

+ (BOOL)isApp:(GetSocialPublicUser *)publicUser;

@end
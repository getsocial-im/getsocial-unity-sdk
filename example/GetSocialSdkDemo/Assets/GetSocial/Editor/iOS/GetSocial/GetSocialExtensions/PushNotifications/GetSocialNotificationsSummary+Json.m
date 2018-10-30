//
//  GetSocialNotificationsSummary+Json.m
//  Unity-iPhone
//
//  Created by Orest Savchak on 10/22/18.
//

#import "GetSocialNotificationsSummary+Json.h"

@implementation GetSocialNotificationsSummary (Json)

- (NSMutableDictionary *)toJsonDictionary
{
    return [@{
             @"SuccessfullySentCount": @(self.successfullySentCount)
             } mutableCopy];
}

@end

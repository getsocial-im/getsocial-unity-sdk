//
//  GetSocialAction+Json.m
//  Unity-iPhone
//
//  Created by Orest Savchak on 12/7/18.
//

#import "GetSocialAction+Json.h"
#import "NSMutableDictionary+GetSocial.h"
#
@implementation GetSocialAction(Json)

- (NSMutableDictionary *)toJsonDictionary
{
    return @{
             @"Data": self.data ?: [NSNull null],
             @"Type": self.type ?: [NSNull null]
             }.mutableCopy;
}

@end

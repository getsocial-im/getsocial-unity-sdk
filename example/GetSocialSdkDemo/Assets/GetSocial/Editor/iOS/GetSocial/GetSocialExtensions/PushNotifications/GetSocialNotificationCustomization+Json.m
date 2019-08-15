//
//  GetSocialNotificationCustomization+Json.m
//  Unity-iPhone
//
//

#import "GetSocialNotificationCustomization+Json.h"

@implementation GetSocialNotificationCustomization(Json)

- (NSMutableDictionary *)toJsonDictionary
{
    return @{
             @"BackgroundImageConfiguration": self.backgroundImageConfiguration ?: [NSNull null],
             @"TitleColor": self.titleColor ?: [NSNull null],
             @"TextColor": self.textColor ?: [NSNull null]
             }.mutableCopy;
}

@end

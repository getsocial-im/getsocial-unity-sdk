//
//  GetSocialActionButton+Json.m
//  Unity-iPhone
//
//

#import "GetSocialActionButton+Json.h"
#import "NSMutableDictionary+GetSocial.h"

@implementation GetSocialActionButton(Json)

- (NSMutableDictionary *)toJsonDictionary
{
    return @{
             @"Id": self.actionId ?: [NSNull null],
             @"Title": self.title ?: [NSNull null]
             }.mutableCopy;
}
@end

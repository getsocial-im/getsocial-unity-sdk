//
//  GetSocialReferralUser+Json.m
//  Unity-iPhone
//
//  Created by Orest Savchak on 21.10.2019.
//

#import "GetSocialReferralUser+Json.h"
#import "GetSocialPublicUser+Json.h"
#import "NSMutableDictionary+GetSocial.h"

@implementation GetSocialReferralUser(Json)

- (NSMutableDictionary *)toJsonDictionary
{
    NSMutableDictionary *dictionary = [super toJsonDictionary];
    [dictionary gs_setValueOrNSNull:self.event forKey:@"Event"];
    [dictionary gs_setValueOrNSNull:@(self.eventDate) forKey:@"EventDate"];
    [dictionary gs_setValueOrNSNull:self.eventData forKey:@"EventData"];
    return dictionary;
}

@end

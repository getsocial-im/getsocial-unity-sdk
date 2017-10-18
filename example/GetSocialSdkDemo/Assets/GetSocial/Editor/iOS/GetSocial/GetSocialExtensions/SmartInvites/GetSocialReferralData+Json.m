//
// Created by Orest Savchak on 6/2/17.
//

#import "GetSocialReferralData+Json.h"
#import "NSMutableDictionary+GetSocial.h"


@implementation GetSocialReferralData (Json)

- (NSMutableDictionary *)toJsonDictionary
{
    NSMutableDictionary *dictionary = [NSMutableDictionary new];
    [dictionary gs_setValueOrNSNull:self.token forKey:@"Token"];
    [dictionary gs_setValueOrNSNull:self.referrerUserId forKey:@"ReferrerUserId"];
    [dictionary gs_setValueOrNSNull:self.referrerChannelId forKey:@"ReferrerChannelId"];
    [dictionary gs_setValueOrNSNull:@(self.isFirstMatch) forKey:@"IsFirstMatch"];
    [dictionary gs_setValueOrNSNull:@(self.isGuaranteedMatch) forKey:@"IsGuaranteedMatch"];
    [dictionary gs_setValueOrNSNull:self.customData forKey:@"CustomReferralData"];
    [dictionary gs_setValueOrNSNull:self.originalCustomData forKey:@"OriginalCustomReferralData"];
    return dictionary;
}

@end

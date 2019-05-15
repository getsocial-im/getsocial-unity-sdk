//
//  GetSocialPromoCode+Json.m
//  Unity-iPhone
//
//  Created by Orest Savchak on 5/9/19.
//

#import "GetSocialPromoCode+Json.h"
#import "NSMutableDictionary+GetSocial.h"
#import "GetSocialUserReference+Json.h"

@implementation GetSocialPromoCode(Json)

- (NSMutableDictionary *)toJsonDictionary {
    NSMutableDictionary *dictionary = [NSMutableDictionary new];
    [dictionary gs_setValueOrNSNull:self.code forKey:@"Code"];
    [dictionary gs_setValueOrNSNull:self.data forKey:@"Data"];
    [dictionary gs_setValueOrNSNull:@(self.maxClaimCount) forKey:@"MaxClaimCount"];
    [dictionary gs_setValueOrNSNull:@(self.claimCount) forKey:@"ClaimCount"];
    [dictionary gs_setValueOrNSNull:@(self.enabled) forKey:@"Enabled"];
    [dictionary gs_setValueOrNSNull:@(self.claimable) forKey:@"Claimable"];
    [dictionary gs_setValueOrNSNull:self.startDate == nil ? nil : @(self.startDate.timeIntervalSince1970) forKey:@"StartDate"];
    [dictionary gs_setValueOrNSNull:self.endDate == nil ? nil : @(self.endDate.timeIntervalSince1970) forKey:@"EndDate"];
    
    [dictionary gs_setValueOrNSNull:self.creator.toJsonDictionary forKey:@"Creator"];
    return dictionary;
}

@end

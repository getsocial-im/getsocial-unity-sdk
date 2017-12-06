//
//  GetSocialJsonUtils.h
//  Unity-iPhone
//
//  Created by Taras Leskiv on 19/12/2016.
//
//

#import <Foundation/Foundation.h>
#import <GetSocial/GetSocial.h>

@interface GetSocialJsonUtils : NSObject

#pragma mark - Deserialize - received as strings FROM Unity

+ (GetSocialMutableInviteContent *)deserializeCustomInviteContent:(NSString *)customInviteContentJson;

+ (NSDictionary *)deserializeCustomReferralData:(NSString *)customReferralDataJson;

+ (GetSocialActivitiesQuery *)deserializeActivitiesQuery:(NSString *)serializedQuery;

+ (GetSocialActivityPostContent *)deserializeActivityContent:(NSString *)content;

+ (GetSocialAuthIdentity *)deserializeIdentity:(NSString *)identity;

+ (GetSocialUsersQuery *)deserializeUsersQuery:(NSString *)query;

+ (NSArray<NSString *> *)deserializeStringList:(NSString *)jsonStringList;

@end

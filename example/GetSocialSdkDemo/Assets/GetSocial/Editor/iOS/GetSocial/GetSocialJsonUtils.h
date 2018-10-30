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

+ (GetSocialMutableInviteContent *)deserializeCustomInviteContent:(NSDictionary *)json;

+ (NSDictionary *)deserializeLinkParams:(NSDictionary *)json;

+ (GetSocialActivitiesQuery *)deserializeActivitiesQuery:(NSDictionary *)json;

+ (GetSocialActivityPostContent *)deserializeActivityContent:(NSDictionary *)json;

+ (GetSocialUserUpdate *) deserializeUserUpdate:(NSDictionary *)json;

+ (GetSocialAuthIdentity *)deserializeIdentity:(NSDictionary *)json;

+ (GetSocialUsersQuery *)deserializeUsersQuery:(NSDictionary *)json;

+ (GetSocialNotificationsCountQuery *)deserializeNotificationsCountQuery:(NSDictionary *)json;

+ (GetSocialNotificationsQuery *)deserializeNotificationsQuery:(NSDictionary *)json;

+ (GetSocialPurchaseData *)deserializePurchaseData:(NSDictionary *)json;

+ (GetSocialNotificationContent *)deserializeNotificationContent:(NSDictionary *)json;

@end

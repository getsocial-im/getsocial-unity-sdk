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

#pragma mark - Serialize - passed as strings TO Unity

+ (NSString *)serializeError:(NSError *)error;

// Smart Invites
+ (NSString *)serializeReferralData:(GetSocialReferralData *)referralData;

+ (NSString *)serializeInvitePackage:(GetSocialInvitePackage *)invitePackage;

+ (NSString *)serializeInviteProvider:(GetSocialInviteChannel *)inviteChannel;

+ (NSString *)serializeInviteChannelsList:(NSArray<GetSocialInviteChannel *> *)inviteChannels;

// Push Notifications

+ (NSString *)serializeNotificationAction:(GetSocialNotificationAction *)action;

// User Management

+ (NSString *)serializeUserIdentities:(NSDictionary *)dictionary;

+ (NSString *)serializeConflictUser:(GetSocialConflictUser *)conflictUser;

+ (NSString *)serializePublicUser:(GetSocialPublicUser *)publicUser;

// Activity Feed
+ (NSMutableDictionary *)createPostAuthorDictionary:(GetSocialPostAuthor *)postAuthor;

+ (NSString *)serializePublicUserArray:(NSArray<GetSocialPublicUser *> *)authors;

+ (NSString *)serializeActivityPost:(GetSocialActivityPost *)post;

+ (NSString *)serializeActivityPostList:(NSArray<GetSocialActivityPost *> *)posts;

#pragma mark - Deserialize - received as strings FROM Unity

+ (GetSocialMutableInviteContent *)deserializeCustomInviteContent:(NSString *)customInviteContentJson;

+ (NSDictionary *)deserializeCustomReferralData:(NSString *)customReferralDataJson;

+ (GetSocialActivitiesQuery *)deserializeActivitiesQuery:(NSString *)serializedQuery;

+ (GetSocialActivityPostContent *)deserializeActivityContent:(NSString *)content;

+ (GetSocialAuthIdentity *)deserializeIdentity:(NSString *)identity;

#pragma mark - Helpers

+ (NSString *)serializeDictionary:(NSDictionary *)dictionary;

+ (NSString *)serializeArray:(NSArray *)array;

@end

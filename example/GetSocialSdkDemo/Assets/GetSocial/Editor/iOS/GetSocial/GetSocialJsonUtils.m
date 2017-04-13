//
//  GetSocialJsonUtils.m
//  Unity-iPhone
//
//  Created by Taras Leskiv on 19/12/2016.
//
//

#import "GetSocialJsonUtils.h"
#import "GetSocialBridgeUtils.h"

@implementation GetSocialJsonUtils

#pragma mark - Serialize - passed as strings TO Unity

+ (NSString *)serializeInviteProvider:(GetSocialInviteChannel *)inviteChannel
{
    NSMutableDictionary *inviteProviderDic = [self createInviteProviderJsonDic:inviteChannel];
    return [self serializeDictionary:inviteProviderDic];
}

+ (NSMutableDictionary *)createInviteProviderJsonDic:(GetSocialInviteChannel *)inviteProvider
{
    NSMutableDictionary *inviteProviderDic = [[NSMutableDictionary alloc] init];
    [self setNullableValueForDictionary:inviteProviderDic key:@"Id" value:inviteProvider.channelId];
    [self setNullableValueForDictionary:inviteProviderDic key:@"Name" value:inviteProvider.name];
    [self setNullableValueForDictionary:inviteProviderDic key:@"IconImageUrl" value:inviteProvider.iconUrl];
    [self setNullableValueForDictionary:inviteProviderDic key:@"Description" value:inviteProvider.description];
    [inviteProviderDic setValue:@([inviteProvider displayOrder]) forKey:@"DisplayOrder"];
    [inviteProviderDic setValue:@(inviteProvider.enabled) forKey:@"IsEnabled"];

    NSMutableDictionary *inviteContentDic = [self createInviteContentJsonDictionary:inviteProvider.inviteContent];
    [inviteProviderDic setValue:inviteContentDic forKey:@"InviteContent"];
    return inviteProviderDic;
}

+ (NSString *)serializeInviteChannelsList:(NSArray<GetSocialInviteChannel *> *)inviteChannels
{
    NSMutableArray *inviteChannelArr = [[NSMutableArray alloc] initWithCapacity:inviteChannels.count];

    for (int i = 0; i < inviteChannels.count; ++i)
    {
        GetSocialInviteChannel *channel = inviteChannels[i];
        [inviteChannelArr addObject:[self createInviteProviderJsonDic:channel]];
    }

    return [self serializeArray:inviteChannelArr];
}


+ (NSMutableDictionary *)createInviteContentJsonDictionary:(GetSocialInviteContent *)inviteContent
{
    NSMutableDictionary *inviteContentDic = [[NSMutableDictionary alloc] init];
    [self setNullableValueForDictionary:inviteContentDic key:@"ImageUrl" value:inviteContent.imageUrl];
    [self setNullableValueForDictionary:inviteContentDic key:@"Subject" value:inviteContent.subject];
    [self setNullableValueForDictionary:inviteContentDic key:@"Text" value:inviteContent.text];
    return inviteContentDic;
}

+ (NSString *)serializeNotificationAction:(GetSocialNotificationAction *)action
{
    NSDictionary *dictionary = @{};
    switch (action.action) {
        case GetSocialNotificationActionOpenActivity:
        {
            GetSocialOpenActivityAction *openActivity = (GetSocialOpenActivityAction *)action;
            dictionary = @{
                           @"Type": @"OPEN_ACTIVITY",
                           @"ActivityId": openActivity.activityId
                               };
            break;
        }
            
        default:
            break;
    }
    return [self serializeDictionary:dictionary];
}

+ (NSString *)serializeUserIdentities:(NSDictionary *)dictionary
{
    return [self serializeDictionary:dictionary];
}

+ (NSString *)serializeConflictUser:(GetSocialConflictUser *)conflictUser
{
    //TODO it should be the same as Pivate, not Public user
    return [self serializePublicUser:conflictUser];
}

+ (NSMutableDictionary *)createPostAuthorDictionary:(GetSocialPostAuthor *)postAuthor
{
    NSMutableDictionary *postAuthorDic = [self serializePublicUserDictionary:postAuthor];
    [self setNullableValueForDictionary:postAuthorDic key:@"IsVerified" value:@(postAuthor.verified)];
    return postAuthorDic;
}

+ (NSString *)serializePublicUserArray:(NSArray<GetSocialPublicUser *> *)authors
{
    NSMutableArray *authorsArr = [[NSMutableArray alloc] initWithCapacity:authors.count];

    for (int i = 0; i < authors.count; ++i)
    {
        GetSocialPublicUser *author = authors[i];
        [authorsArr addObject:[self serializePublicUserDictionary:author]];
    }

    return [self serializeArray:authorsArr];
}

+ (NSString *)serializePublicUser:(GetSocialPublicUser *)publicUser
{
    NSMutableDictionary *publicUserDic = [self serializePublicUserDictionary:publicUser];
    return [self serializeDictionary:publicUserDic];
}

+ (NSMutableDictionary *)serializePublicUserDictionary:(GetSocialPublicUser *)publicUser
{
    NSMutableDictionary *publicUserDic = [[NSMutableDictionary alloc] init];
    [publicUserDic setValue:publicUser.userId forKey:@"Id"];
    [publicUserDic setValue:publicUser.displayName forKey:@"DisplayName"];
    [publicUserDic setValue:publicUser.avatarUrl forKey:@"AvatarUrl"];

    NSMutableDictionary *identitiesDic = [[NSMutableDictionary alloc] init];
    NSDictionary *identities = publicUser.authIdentities;
    for (NSString *key in identities)
    {
        identitiesDic[key] = identities[key];
    }
    [publicUserDic setValue:identitiesDic forKey:@"Identities"];
    return publicUserDic;
}

+ (NSString *)serializeError:(NSError *)error
{
    NSMutableDictionary *errorDic = [[NSMutableDictionary alloc] init];

    [self setNullableValueForDictionary:errorDic key:@"Message" value:error.localizedDescription];
    [errorDic setValue:@(error.code) forKey:@"ErrorCode"];

    NSMutableDictionary *userInfo = [[NSMutableDictionary alloc] init];

    for (NSString *key in error.userInfo)
    {
        userInfo[key] = [error.userInfo[key] description];
    }

    if (error.userInfo)
    {
        [self setNullableValueForDictionary:errorDic key:@"UserInfo" value:userInfo];
    }

    return [self serializeDictionary:errorDic];
}

+ (NSString *)serializeReferralData:(GetSocialReferralData *)referralData
{
    if (!referralData)
    {
        return nil;
    }
    NSMutableDictionary *referralDataDic = [[NSMutableDictionary alloc] init];
    [self setNullableValueForDictionary:referralDataDic key:@"Token" value:referralData.token];
    [self setNullableValueForDictionary:referralDataDic key:@"ReferrerUserId" value:referralData.referrerUserId];
    [self setNullableValueForDictionary:referralDataDic key:@"ReferrerChannelId" value:referralData.referrerChannelId];
    [self setNullableValueForDictionary:referralDataDic key:@"IsFirstMatch" value:@(referralData.isFirstMatch)];
    [self setNullableValueForDictionary:referralDataDic key:@"CustomReferralData" value:referralData.customData];
    return [self serializeDictionary:referralDataDic];
}

+ (NSString *)serializeInvitePackage:(GetSocialInvitePackage *)invitePackage
{
    NSMutableDictionary *invitePackageDic = [[NSMutableDictionary alloc] init];
    [self setNullableValueForDictionary:invitePackageDic key:@"Subject" value:invitePackage.subject];
    [self setNullableValueForDictionary:invitePackageDic key:@"Text" value:invitePackage.text];
    [self setNullableValueForDictionary:invitePackageDic key:@"UserName" value:invitePackage.userName];
    [self setNullableValueForDictionary:invitePackageDic key:@"ReferralDataUrl" value:invitePackage.referralUrl];

    NSString *imageBase64 = [GetSocialBridgeUtils encodeToBase64String:[invitePackage image]];
    [self setNullableValueForDictionary:invitePackageDic key:@"Image" value:imageBase64];

    return [self serializeDictionary:invitePackageDic];
}

+ (NSString *)serializeActivityPost:(GetSocialActivityPost *)post
{
    NSMutableDictionary *activityPostDic = [self serializeActivityPostDic:post];

    return [self serializeDictionary:activityPostDic];
}

+ (NSMutableDictionary *)serializeActivityPostDic:(GetSocialActivityPost *)post
{
    NSMutableDictionary *activityPostDic = [[NSMutableDictionary alloc] init];
    [self setNullableValueForDictionary:activityPostDic key:@"Id" value:post.activityId];
    [self setNullableValueForDictionary:activityPostDic key:@"Text" value:post.text];
    [self setNullableValueForDictionary:activityPostDic key:@"ImageUrl" value:post.imageUrl];
    [self setNullableValueForDictionary:activityPostDic key:@"CreatedAt" value:@(post.createdAt)];
    [self setNullableValueForDictionary:activityPostDic key:@"StickyStart" value:@(post.stickyStart)];
    [self setNullableValueForDictionary:activityPostDic key:@"StickyEnd" value:@(post.stickyEnd)];
    [self setNullableValueForDictionary:activityPostDic key:@"ButtonTitle" value:post.buttonTitle];
    [self setNullableValueForDictionary:activityPostDic key:@"ButtonAction" value:post.buttonAction];
    [self setNullableValueForDictionary:activityPostDic key:@"CommentsCount" value:@(post.commentsCount)];
    [self setNullableValueForDictionary:activityPostDic key:@"LikesCount" value:@(post.likesCount)];
    [self setNullableValueForDictionary:activityPostDic key:@"IsLikedByMe" value:@(post.isLikedByMe)];

    NSMutableDictionary *authorDic = [self createPostAuthorDictionary:post.author];
    [self setNullableValueForDictionary:activityPostDic key:@"Author" value:authorDic];
    return activityPostDic;
}

+ (NSString *)serializeActivityPostList:(NSArray<GetSocialActivityPost *> *)posts
{
    NSMutableArray *postsArray = [[NSMutableArray alloc] initWithCapacity:posts.count];

    for (int i = 0; i < posts.count; ++i)
    {
        GetSocialActivityPost *post = posts[i];
        [postsArray addObject:[self serializeActivityPostDic:post]];
    }

    return [self serializeArray:postsArray];
}

#pragma mark - Deserialize - received as strings FROM Unity

+ (GetSocialMutableInviteContent *)deserializeCustomInviteContent:(NSString *)customInviteContentJson
{
    NSDictionary *json = [self deserializeDictionary:customInviteContentJson];

    GetSocialMutableInviteContent *content = [[GetSocialMutableInviteContent alloc] init];
    content.subject = json[@"Subject"];
    content.imageUrl = json[@"ImageUrl"];
    content.text = json[@"Text"];

    return content;
}

+ (NSDictionary *)deserializeCustomReferralData:(NSString *)customReferralDataJson
{

#if DEBUG
    NSLog(@"JSON Input: %@", customReferralDataJson);
#endif

    NSDictionary *json = [self deserializeDictionary:customReferralDataJson];
    return json;
}

const int FILTER_NONE = 0;
const int FILTER_BEFORE = 1;
const int FILTER_AFTER = 2;

+ (GetSocialActivitiesQuery *)deserializeActivitiesQuery:(NSString *)serializedQuery
{
    NSDictionary *json = [self deserializeDictionary:serializedQuery];

    GetSocialActivitiesQuery *query;

    NSString *feed = json[@"Feed"];
    if ([feed class] == [NSNull class])
    {
        // comments
        query = [GetSocialActivitiesQuery commentsToPost:json[@"ParentActivityId"]];
    } else
    {
        // posts
        query = [GetSocialActivitiesQuery postsForFeed:feed];
    }

    // Limit
    int limit = [json[@"Limit"] intValue];
    [query setLimit:limit];

    // Filtering
    int filter = [json[@"Filter"] intValue];
    if (filter != FILTER_NONE)
    {
        [query setFilter:[self parseFilter:filter] activityId:json[@"FilteringActivityId"]];
    }

    return query;
}

+ (GetSocialActivitiesFilter)parseFilter:(int)filter
{
    if (filter == FILTER_BEFORE) return ActivitiesBefore;
    if (filter == FILTER_AFTER) return ActivitiesAfter;
    return NoFilter;
}

+ (GetSocialActivityPostContent *)deserializeActivityContent:(NSString *)content
{
    NSDictionary *dictionary = [self deserializeDictionary:content];

    GetSocialActivityPostContent *postContent = [GetSocialActivityPostContent new];
    postContent.text = dictionary[@"Text"];
    postContent.buttonTitle = dictionary[@"ButtonTitle"];
    postContent.buttonAction = dictionary[@"ButtonAction"];
    postContent.image = [GetSocialBridgeUtils decodeUIImageFrom:dictionary[@"Image"]];

    return postContent;
}

+ (GetSocialAuthIdentity *)deserializeIdentity:(NSString *)identity
{
    NSDictionary *dictionary = [self deserializeDictionary:identity];
    
    return [GetSocialAuthIdentity customIdentityForProvider:dictionary[@"ProviderId"]
                                                     userId:dictionary[@"ProviderUserId"]
                                                accessToken:dictionary[@"AccessToken"]];
}

#pragma mark - Helpers

+ (NSString *)serializeDictionary:(NSDictionary *)dictionary
{
    NSError *writeError = nil;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dictionary options:nil error:&writeError];

    NSString *jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];

    NSLog(@"JSON Output: %@", jsonString);

    return jsonString;
}

+ (void)setNullableValueForDictionary:(NSMutableDictionary *)dictionary
                                  key:(NSString *)key
                                value:(NSObject *)value
{
    if (value == nil)
    {
        [dictionary setValue:[NSNull null] forKey:key];
    } else
    {
        [dictionary setValue:value forKey:key];
    }
}

+ (NSString *)serializeArray:(NSArray *)array
{
    NSError *writeError = nil;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:array options:nil error:&writeError];

    NSString *jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];

#if DEBUG
    NSLog(@"JSON Output: %@", jsonString);
#endif

    return jsonString;
}

+ (NSDictionary *)deserializeDictionary:(NSString *)jsonDic
{
    NSError *e = nil;
    NSDictionary *dictionary = [NSJSONSerialization JSONObjectWithData:[jsonDic dataUsingEncoding:NSUTF8StringEncoding] options:NSJSONReadingMutableContainers error:&e];
    if (dictionary != nil)
    {
        NSMutableDictionary *prunedDict = [NSMutableDictionary dictionary];
        [dictionary enumerateKeysAndObjectsUsingBlock:^(NSString *key, id obj, BOOL *stop) {
            if (![obj isKindOfClass:[NSNull class]]) {
                prunedDict[key] = obj;
            }
        }];
        return prunedDict;
    }
    return dictionary;
}


@end

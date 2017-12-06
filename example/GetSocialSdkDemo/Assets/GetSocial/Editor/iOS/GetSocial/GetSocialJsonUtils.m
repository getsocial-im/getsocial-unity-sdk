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

#pragma mark - Deserialize - received as strings FROM Unity

+ (GetSocialMutableInviteContent *)deserializeCustomInviteContent:(NSString *)customInviteContentJson
{
    NSDictionary *json = [self deserializeDictionary:customInviteContentJson];

    GetSocialMutableInviteContent *content = [[GetSocialMutableInviteContent alloc] init];
    content.subject = json[@"Subject"];
    content.imageUrl = json[@"ImageUrl"];
    content.text = json[@"Text"];
    content.image = [GetSocialBridgeUtils decodeUIImageFrom:json[@"Image"]];

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
    if (feed == nil)
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
    [query setFilterByUser:json[@"FilterUserId"]];

    // Filtering
    int filter = [json[@"Filter"] intValue];
    if (filter != FILTER_NONE)
    {
        [query setFilter:[self parseFilter:filter] activityId:json[@"FilteringActivityId"]];
    }
    BOOL isFriendsFeed = [json[@"FriendsFeed"] boolValue];
    [query setIsFriendsFeed:isFriendsFeed];

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

+ (GetSocialUsersQuery *)deserializeUsersQuery:(NSString *)query
{
    NSDictionary *dictionary = [self deserializeDictionary:query];
    
    GetSocialUsersQuery *usersQuery = [GetSocialUsersQuery usersByDisplayName:dictionary[@"Query"]];
    [usersQuery setLimit:[dictionary[@"Limit"] intValue]];
    return usersQuery;
}

#pragma mark - Helpers

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

+ (NSArray<NSString *> *)deserializeStringList:(NSString *)jsonStringList
{
    NSError* localError = nil;
    NSArray<NSString*> *array = [NSJSONSerialization JSONObjectWithData:[jsonStringList dataUsingEncoding:NSUTF8StringEncoding] options:NSJSONReadingMutableContainers error:&localError];

    return array;
}

@end

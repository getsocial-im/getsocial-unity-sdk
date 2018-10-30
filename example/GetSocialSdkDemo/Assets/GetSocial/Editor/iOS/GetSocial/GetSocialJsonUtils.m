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

+ (GetSocialMutableInviteContent *)deserializeCustomInviteContent:(NSDictionary *)json
{
    GetSocialMutableInviteContent *content = [[GetSocialMutableInviteContent alloc] init];
    content.subject = json[@"Subject"];
    content.imageUrl = json[@"ImageUrl"];
    content.text = json[@"Text"];
    content.image = [GetSocialBridgeUtils decodeUIImageFrom:json[@"Image"]];
    content.video = [GetSocialBridgeUtils decodeNSDataFrom:json[@"Video"]];

    return content;
}

+ (NSDictionary *)deserializeLinkParams:(NSDictionary *)json
{

#if DEBUG
    NSLog(@"JSON Input: %@", json);
#endif
    if (json == nil)
    {
        return nil;
    }

    id rawImage = json[@"$image"];
    if (rawImage != nil)
    {
        UIImage* image = [GetSocialBridgeUtils decodeUIImageFrom:(NSString*)rawImage];
        if (image)
        {
            [json setValue:image forKey:@"$image"];
        }
    }
    return json;
}

+ (GetSocialActivitiesQuery *)deserializeActivitiesQuery:(NSDictionary *)json
{
    NSString *feed = json[@"Feed"];
    GetSocialActivitiesQuery *query = feed == nil
    ? [GetSocialActivitiesQuery commentsToPost:json[@"ParentActivityId"]]
    : [GetSocialActivitiesQuery postsForFeed:feed];

    // Limit
    int limit = [json[@"Limit"] intValue];
    [query setLimit:limit];
    [query setFilterByUser:json[@"FilterUserId"]];

    // Filtering
    int filter = [json[@"Filter"] intValue];
    if (filter != 0)
    {
        [query setFilter:(GetSocialActivitiesFilter)filter activityId:json[@"FilteringActivityId"]];
    }

    NSArray *tags = json[@"Tags"];
    if (tags) 
    {
        [query setTags:tags];
    }
    BOOL isFriendsFeed = [json[@"FriendsFeed"] boolValue];
    [query setIsFriendsFeed:isFriendsFeed];

    return query;
}

+ (GetSocialUserUpdate *) deserializeUserUpdate:(NSDictionary *)json
{
    GetSocialUserUpdate *userUpdate = [GetSocialUserUpdate new];
    [userUpdate setDisplayName:json[@"DisplayName"]];

    if(json[@"AvatarUrl"] != nil)
    {
        [userUpdate setAvatarUrl:json[@"AvatarUrl"] ];
    }
    else if(json[@"Avatar"] != nil)
    {
        [userUpdate setAvatar:[GetSocialBridgeUtils decodeUIImageFrom:json[@"Avatar"]]];
    }
    
    //TODO: make use and deserialize internal properties in some future or at least in parallel universe
    //These are already implemented on the SDK side. Also in Unity at UserUpdate.cs
    NSDictionary *privateProperies = json[@"PrivateProperties"];
    [privateProperies enumerateKeysAndObjectsUsingBlock:^(id  _Nonnull key, id  _Nonnull obj, BOOL * _Nonnull stop) {
        [userUpdate setPrivatePropertyValue:obj forKey:key];
    }];
    NSDictionary *publicProperties = json[@"PublicProperties"];
    [publicProperties enumerateKeysAndObjectsUsingBlock:^(id  _Nonnull key, id  _Nonnull obj, BOOL * _Nonnull stop) {
        [userUpdate setPublicPropertyValue:obj forKey:key];
    }];
    
#if DEBUG
    NSLog(@"JSON Input: %@", serializedUserUpdate);
 #endif
    
    return userUpdate;
    
}

+ (GetSocialActivityPostContent *)deserializeActivityContent:(NSDictionary *)dictionary
{
    GetSocialActivityPostContent *postContent = [GetSocialActivityPostContent new];
    postContent.text = dictionary[@"Text"];
    postContent.buttonTitle = dictionary[@"ButtonTitle"];
    postContent.buttonAction = dictionary[@"ButtonAction"];
    postContent.image = [GetSocialBridgeUtils decodeUIImageFrom:dictionary[@"Image"]];
    postContent.video = [GetSocialBridgeUtils decodeNSDataFrom:dictionary[@"Video"]];

    return postContent;
}

+ (GetSocialAuthIdentity *)deserializeIdentity:(NSDictionary *)dictionary
{
    return [GetSocialAuthIdentity customIdentityForProvider:dictionary[@"ProviderId"]
                                                     userId:dictionary[@"ProviderUserId"]
                                                accessToken:dictionary[@"AccessToken"]];
}

+ (GetSocialUsersQuery *)deserializeUsersQuery:(NSDictionary *)dictionary
{
    GetSocialUsersQuery *usersQuery = [GetSocialUsersQuery usersByDisplayName:dictionary[@"Query"]];
    [usersQuery setLimit:[dictionary[@"Limit"] intValue]];
    return usersQuery;
}

+ (GetSocialNotificationsQuery *)deserializeNotificationsQuery:(NSDictionary *)json
{
    NSNumber *isRead = json[@"IsRead"];
    
    GetSocialNotificationsQuery *query = isRead == nil
    ? [GetSocialNotificationsQuery readAndUnread]
    : [isRead boolValue] ? [GetSocialNotificationsQuery read] : [GetSocialNotificationsQuery unread];
    
    // Limit
    int limit = [json[@"Limit"] intValue];
    [query setLimit:limit];
    
    // Filtering
    int filter = [json[@"Filter"] intValue];
    if (filter != 0)
    {
        [query setFilter:(GetSocialNotificationsFilter)filter notificationId:json[@"FilteringNotificationId"]];
    }
    id types = json[@"Types"];
    [query setTypes:types];
    
    return query;
}

+ (GetSocialNotificationsCountQuery *)deserializeNotificationsCountQuery:(NSDictionary *)json
{
    NSNumber *isRead = json[@"IsRead"];
    GetSocialNotificationsCountQuery *query = isRead == nil
    ? [GetSocialNotificationsCountQuery readAndUnread]
    : [isRead boolValue] ? [GetSocialNotificationsCountQuery read] : [GetSocialNotificationsCountQuery unread];
    
    
    id types = json[@"Types"];
    [query setTypes:types];
    
    return query;
}

+ (GetSocialPurchaseData *)deserializePurchaseData:(NSDictionary *)json
{
    GetSocialPurchaseData* purchaseData = [GetSocialPurchaseData new];
    purchaseData.productId = json[@"ProductId"];
    NSString* priceStr = json[@"Price"];
    purchaseData.price = priceStr.floatValue;
    purchaseData.priceCurrency = json[@"PriceCurrency"];
    purchaseData.productTitle = json[@"ProductTitle"];
    purchaseData.transactionIdentifier = json[@"PurchaseId"];

    NSString* purchaseDateStr = json[@"PurchaseDate"];
    NSDateFormatter* formatter = [NSDateFormatter new];
    [formatter setDateFormat:@"MM/dd/yyyy HH:mm:ss"];
    purchaseData.purchaseDate = [formatter dateFromString:purchaseDateStr];
    
    NSString* productTypeStr = json[@"ProductType"];
    if ([productTypeStr isEqualToString:@"0"]) {
        purchaseData.productType = Item;
    } else {
        purchaseData.productType = Subscription;
    }

    return purchaseData;
}

+ (GetSocialNotificationContent *)deserializeNotificationContent:(NSDictionary *)json
{
    GetSocialNotificationContent *notificationContent = [GetSocialNotificationContent withText:json[@"Text"]];
    [notificationContent setTitle:json[@"Title"]];
    
    if (json[@"Action"] != nil)
    { 
        [notificationContent setActionType:(GetSocialNotificationActionType) [json[@"Action"] intValue]];
    }
    [notificationContent addActionData:json[@"ActionData"]];
    
    [notificationContent setTemplateName:json[@"Template"]];
    [notificationContent addTemplatePlaceholders:json[@"TemplatePlaceholders"]];
    
    return notificationContent;
}
@end

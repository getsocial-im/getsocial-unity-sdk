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
    GetSocialMutableInviteContent *content = [GetSocialMutableInviteContent new];
    
    content.subject         = json[@"Subject"];
    content.text            = json[@"Text"];
    content.mediaAttachment = [self deserializeMediaAttachment:json[@"MediaAttachment"]];

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
    NSString *parentActivityId = json[@"ParentActivityId"];
    GetSocialActivitiesQuery *query = feed == nil
        ? [GetSocialActivitiesQuery commentsToPost:parentActivityId]
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

    if (json[@"AvatarUrl"] != nil)
    {
        [userUpdate setAvatarUrl:json[@"AvatarUrl"] ];
    }
    else if (json[@"Avatar"] != nil)
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
    
    postContent.text            = dictionary[@"Text"];
    postContent.buttonTitle     = dictionary[@"ButtonTitle"];
    postContent.buttonAction    = dictionary[@"ButtonAction"];
    postContent.action          = [self deserializeAction:dictionary[@"Action"]];
    postContent.mediaAttachment = [self deserializeMediaAttachment:dictionary[@"MediaAttachment"]];

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
    GetSocialNotificationsQuery *query = [GetSocialNotificationsQuery withStatuses:json[@"Statuses"]];
    [query setActions:json[@"Actions"]];
    
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
    GetSocialNotificationsCountQuery *query = [GetSocialNotificationsCountQuery withStatuses:json[@"Statuses"]];
    [query setActions:json[@"Actions"]];
    [query setTypes:json[@"Types"]];
    
    return query;
}

+ (GetSocialPurchaseData *)deserializePurchaseData:(NSDictionary *)json
{
    GetSocialPurchaseData *purchaseData = [GetSocialPurchaseData new];
    
    purchaseData.productId              = json[@"ProductId"];
    purchaseData.price                  = [json[@"Price"] floatValue];
    purchaseData.priceCurrency          = json[@"PriceCurrency"];
    purchaseData.productTitle           = json[@"ProductTitle"];
    purchaseData.transactionIdentifier  = json[@"PurchaseId"];
    purchaseData.productType            = [@"0" isEqualToString:json[@"ProductType"]] ? Item : Subscription;

    NSString *purchaseDateStr = json[@"PurchaseDate"];
    NSDateFormatter *formatter = [NSDateFormatter new];
    [formatter setDateFormat:@"MM/dd/yyyy HH:mm:ss"];
    purchaseData.purchaseDate = [formatter dateFromString:purchaseDateStr];
    
    return purchaseData;
}

+ (GetSocialNotificationContent *)deserializeNotificationContent:(NSDictionary *)json
{
    GetSocialNotificationContent *notificationContent = [GetSocialNotificationContent withText:json[@"Text"]];
    [notificationContent setTitle:json[@"Title"]];
    [notificationContent setAction:[self deserializeAction:json[@"Action"]]];
    [notificationContent setMediaAttachment:[self deserializeMediaAttachment:json[@"MediaAttachment"]]];
    [notificationContent setTemplateName:json[@"Template"]];
    [notificationContent addTemplatePlaceholders:json[@"TemplatePlaceholders"]];
    
    NSArray *actionButtons = json[@"ActionButtons"];
    for (NSString *actionButton in actionButtons)
    {
        [notificationContent addActionButton:[self deserializeActionButton:actionButton] ];
    }
    [notificationContent setCustomization:[self deserializeCustomization:json[@"Customization"]]];
    
    return notificationContent;
}

+ (GetSocialNotificationCustomization*)deserializeCustomization:(NSString*)customizationJson {
    if (customizationJson.length == 0) {
        return nil;
    }
    NSDictionary *json = [GetSocialBridgeUtils createDictionaryFromNSString:customizationJson];
    GetSocialNotificationCustomization* customization = [GetSocialNotificationCustomization new];
    customization.backgroundImageConfiguration = json[@"BackgroundImageConfiguration"];
    customization.titleColor = json[@"TitleColor"];
    customization.textColor = json[@"TextColor"];
    return customization;
}

+ (GetSocialActionButton *)deserializeActionButton:(NSString *)actionButtonJson
{
    NSDictionary *json = [GetSocialBridgeUtils createDictionaryFromNSString:actionButtonJson];
    
    return [GetSocialActionButton createWithTitle:json[@"Title"]
                                      andActionId:json[@"Id"]];
}

+ (GetSocialMediaAttachment *)deserializeMediaAttachment:(NSString *)mediaAttachmentJson
{
    if (mediaAttachmentJson.length == 0)
    {
        return nil;
    }
    NSDictionary *json = [GetSocialBridgeUtils createDictionaryFromNSString:mediaAttachmentJson];
    
    NSString *method = json.allKeys.firstObject;
    NSString *object = json[method];
    
    if ([@"imageUrl" isEqualToString:method])
    {
        return [GetSocialMediaAttachment imageUrl:object];
    } else if ([@"image" isEqualToString:method])
    {
        return [GetSocialMediaAttachment image:[GetSocialBridgeUtils decodeUIImageFrom:object]];
    } else if ([@"videoUrl" isEqualToString:method])
    {
        return [GetSocialMediaAttachment videoUrl:object];
    } else if ([@"video" isEqualToString:method])
    {
        return [GetSocialMediaAttachment video:[GetSocialBridgeUtils decodeNSDataFrom:object]];
    }
    
    return nil;
}

+ (GetSocialAction *)deserializeAction:(NSString *)actionJson
{
    if (actionJson.length == 0)
    {
        return nil;
    }
    NSDictionary *json = [GetSocialBridgeUtils createDictionaryFromNSString:actionJson];
    
    if (json[@"Type"] == nil)
    {
        return nil;
    }
    
    GetSocialActionBuilder *actionBuilder = [[GetSocialActionBuilder alloc] initWithType:json[@"Type"] ];
    [actionBuilder addActionData:json[@"Data"]];

    return [actionBuilder build];
}

+ (GetSocialPromoCodeBuilder *)deserializePromoCodeBuilder:(NSDictionary *)json
{
    GetSocialPromoCodeBuilder *builder = json[@"Code"] == nil
        ? [GetSocialPromoCodeBuilder withRandomCode]
        : [GetSocialPromoCodeBuilder withCode:json[@"Code"]];
    
    [builder setMaxClaimCount:[json[@"MaxClaimCount"] unsignedIntValue]];
    [builder addData:json[@"Data"]];
    [builder setTimeLimitWithStartDate:[self dateFromString:json[@"StartDate"]] endDate:[self dateFromString:json[@"EndDate"]]];
    return builder;
}

+ (NSDate *)dateFromString:(NSString *)timestamp
{
    if (timestamp == nil) {
        return nil;
    }
    
    NSDateFormatter *dateformat = [NSDateFormatter new];
    [dateformat setDateFormat:@"dd/MM/yyyy hh:mm:ss"];
    return [dateformat dateFromString: timestamp];
}

@end

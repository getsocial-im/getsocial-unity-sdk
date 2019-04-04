//
// Created by Orest Savchak on 6/2/17.
//

#import "GetSocialNotification+Json.h"
#import "NSMutableDictionary+GetSocial.h"
#import "GetSocialAction+Json.h"
#import "NSArray+GetSocial.h"
#import "GetSocialUserReference+Json.h"

@implementation GetSocialNotification (Json)

- (NSMutableDictionary *)toJsonDictionary
{
    NSMutableDictionary *dictionary = @{}.mutableCopy;
    [dictionary gs_setValueOrNSNull:self.title forKey:@"Title"];
    [dictionary gs_setValueOrNSNull:self.text forKey:@"Text"];
    [dictionary gs_setValueOrNSNull:self.type forKey:@"Type"];
    [dictionary gs_setValueOrNSNull:[self.notificationAction toJsonDictionary] forKey:@"Action"];
    [dictionary gs_setValueOrNSNull:@(self.action) forKey:@"OldAction"];
    [dictionary gs_setValueOrNSNull:self.notificationId forKey:@"Id"];
    [dictionary gs_setValueOrNSNull:self.imageUrl forKey:@"ImageUrl"];
    [dictionary gs_setValueOrNSNull:self.videoUrl forKey:@"VideoUrl"];
    [dictionary gs_setValueOrNSNull:self.status forKey:@"Status"];
    NSArray *actionButtons = [self.actionButtons gs_map:^id(GetSocialActionButton *it) {
        return @{@"Title": it.title ?: [NSNull null], @"Id": it.actionId ?: [NSNull null]};
    }];
    [dictionary gs_setValueOrNSNull:actionButtons forKey:@"ActionButtons"];
    [dictionary gs_setValueOrNSNull:@(self.createdAt) forKey:@"CreatedAt"];
    [dictionary gs_setValueOrNSNull:[self.sender toJsonDictionary] forKey:@"Sender"];
    return dictionary;
}

@end

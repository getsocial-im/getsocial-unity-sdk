//
// Created by Orest Savchak on 6/2/17.
//

#import "GetSocialNotificationAction+Json.h"


@implementation GetSocialNotificationAction (Json)

- (NSMutableDictionary *)toJsonDictionary
{
    NSDictionary *dictionary = @{};
    switch (self.action) {
        case GetSocialNotificationActionOpenActivity:
        {
            GetSocialOpenActivityAction *openActivity = (GetSocialOpenActivityAction *)self;
            dictionary = @{
                    @"Type": @"OPEN_ACTIVITY",
                    @"ActivityId": openActivity.activityId
            };
            break;
        }
        case GetSocialNotificationActionOpenProfile:
        {
            GetSocialOpenProfileAction *openProfile = (GetSocialOpenProfileAction *)self;
            dictionary = @{
                    @"Type": @"OPEN_PROFILE",
                    @"UserId": openProfile.userId
            };
        }

        default:
            break;
    }
    return [dictionary mutableCopy];
}

@end

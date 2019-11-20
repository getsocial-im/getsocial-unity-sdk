#import <GetSocialExtension/GetSocialNotificationExtensionHandler.h>
#import "GetSocialNotificationService.h"

@interface GetSocialNotificationService()

@property (nonatomic, strong) GetSocialNotificationExtensionHandler *handler;

@end

@implementation GetSocialNotificationService

- (void)didReceiveNotificationRequest:(UNNotificationRequest *)request withContentHandler:(void (^)(UNNotificationContent *_Nonnull))contentHandler
{
    self.handler = [GetSocialNotificationExtensionHandler new];
    [self.handler handleNotificationRequest:request withContentHandler:contentHandler];
}

- (void)serviceExtensionTimeWillExpire
{
    [self.handler serviceExtensionTimeWillExpire];
}

@end

#import <GetSocialNotificationExtension/GetSocialNotificationExtension.h>
#import "GetSocialNotificationService.h"

@interface GetSocialNotificationService()

@property (nonatomic, strong) GetSocialNotificationRequestHandler *handler;

@end

@implementation GetSocialNotificationService

- (void)didReceiveNotificationRequest:(UNNotificationRequest *)request withContentHandler:(void (^)(UNNotificationContent *_Nonnull))contentHandler
{
    self.handler = [GetSocialNotificationRequestHandler new];
    [self.handler handleNotificationRequest:request withContentHandler:contentHandler];
}

- (void)serviceExtensionTimeWillExpire
{
    [self.handler serviceExtensionTimeWillExpire];
}

@end

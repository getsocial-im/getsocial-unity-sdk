//
//  NotificationService.m
//  GetSocialNotificationServiceExtension
//
//  Copyright © 2018 GetSocial. All rights reserved.
//

#import "GetSocialNotificationService.h"

#define GETSOCIAL_NOTIFICATION_CATEGORY @"getsocial-extension"
#define GETSOCIAL_NOTIFICATION_DATA @"gs_data"
#define GETSOCIAL_NOTIFICATION_IMAGE @"i_url"
#define GETSOCIAL_NOTIFICATION_VIDEO @"v_url"

@implementation GetSocialNotificationService

- (void)didReceiveNotificationRequest:(UNNotificationRequest *)request withContentHandler:(void (^)(UNNotificationContent *_Nonnull))contentHandler
{
    UNMutableNotificationContent *modifiedNotificationContent = [request.content mutableCopy];
    NSString *category = request.content.categoryIdentifier;
    // modify notification only if category is getsocial
    if ([category isEqualToString:GETSOCIAL_NOTIFICATION_CATEGORY])
    {
        NSDictionary *userInfo = request.content.userInfo;
        NSDictionary *gsData = userInfo[GETSOCIAL_NOTIFICATION_DATA];
        NSString *imageUrl = gsData[GETSOCIAL_NOTIFICATION_IMAGE];
        NSString *videoUrl = gsData[GETSOCIAL_NOTIFICATION_VIDEO];
        if (imageUrl != nil || videoUrl != nil)
        {
            NSString *mediaUrlString = videoUrl != nil ? videoUrl : imageUrl;
            NSURL *mediaUrl = [NSURL URLWithString:mediaUrlString];
            NSString *fileName = mediaUrl.lastPathComponent;
            
            NSArray *paths = NSSearchPathForDirectoriesInDomains(NSCachesDirectory, NSUserDomainMask, YES);
            NSString *cachesDirectory = paths[0];
            NSString *fullPath = [cachesDirectory stringByAppendingPathComponent:fileName];
            
            NSData *data = [NSData dataWithContentsOfURL:mediaUrl];
            [data writeToFile:fullPath atomically:YES];
            
            NSURL *localURL = [NSURL fileURLWithPath:fullPath];
            NSError *error = nil;
            UNNotificationAttachment *attachment = [UNNotificationAttachment attachmentWithIdentifier:fileName URL:localURL options:nil error:&error];
            // if attachment was created, we can add it to notification
            if (error == nil)
            {
                modifiedNotificationContent.attachments = @[ attachment ];
            }
            else
            {
                NSLog(@"Could not create attachment from url %@, error: %@", mediaUrl, error);
            }
        }
    }
    // call completion handler with modified content
    if (contentHandler)
    {
        contentHandler(modifiedNotificationContent);
    }
}

@end

//
//  GetSocial
//
//  Copyright © 2017 GetSocial BV. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GetSocial/GetSocial.h>

@interface GetSocialAccessHelper : NSObject

/*!
 * @abstract Initializes GetSocial Framework, it is automatically called during application start or change in the connection
 *
 * @param success Called if GetSocial SDK initialized successfully.
 * @param failure Called if GetSocial SDK initialization failed.
 */
+ (void)initWithSuccess:(GetSocialSuccessCallback)success failure:(GetSocialFailureCallback)failure;

/*!
 *  Resets GetSocial Framework.
 */
+ (void)reset;

/*!
 * @abstract Sets Hades configuration
 */
+ (void)setHadesConfiguration:(id)hadesConfiguration;

/*!
 * @abstract Sets Hades configuration by int value. Exposed for Unity.
 */
+ (void)setHadesConfigurationInt:(int)hadesConfigurationInt;

/*!
 * @abstract Returns available Hades configurations.
 */
+ (NSDictionary*)hadesConfigurations;

/*!
 * @abstract Returns currently used Hades configuration.
 */
+ (id)currentHadesConfiguration;

/*!
 * @abstract Returns integer value for currently used Hades configuration type. Exposed for Unity.
 */
+ (int)currentHadesConfigurationInt;

+ (void)setDefaultPushNotificationHandler:(GetSocialNotificationActionHandler)handler;

#pragma mark Analytics

+(void)trackAnalyticsEvent:(NSString *_Nonnull)eventName
           eventProperties:(NSDictionary *_Nullable)eventProperties;

+ (long)analyticsEventTimestamp;

@end

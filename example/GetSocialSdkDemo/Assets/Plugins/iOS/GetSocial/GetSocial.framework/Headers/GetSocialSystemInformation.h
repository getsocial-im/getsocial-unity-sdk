//
//  GetSocial
//
//  Copyright © 2017 GetSocial BV. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@interface GetSocialSystemInformation : NSObject

/**
 *  Returns name of host application
 *
 *  @return application name
 */
+ (NSString *)appName;

/**
 *  Returns bundle identifier of host application
 *
 *  @return bundle identifier
 */
+ (NSString *)packageName;

/**
 *  Returns advertising identifier.
 *
 *  @return advertising identifier
 */
+ (NSString *)deviceIdfa;

/**
 *  Returns identifier for vendor.
 *  @return identifier for vendor.
 */
+ (NSString *)deviceIdfv;

/**
 *  Device screen width
 *
 *  @return the width of the screen
 */
+ (NSNumber *)screenWidth;

/**
 *  Device screen height
 *
 *  @return the height of the screen
 */
+ (NSNumber *)screenHeight;

/**
 *  The version of the iOS
 *
 *  @return version number
 */
+ (NSString *)OSVersion;

/**
 *  The name of the operation system
 *
 *  @return os name
 */
+ (NSString *)OSName;

/**
 *  Get the density of the screen
 *
 *  @return density of the screen for the current device
 */
+ (CGFloat)density;

/**
 *  Get the internal version (Build version)
 *
 *  @return build version of the app
 */
+ (NSString *)appVersionName;

/**
 *  Get the version of the application
 *
 *  @return current version of the app
 */
+ (NSString *)appVersionCode;

/**
 *  Get device timezone
 *
 *  @return timezone, for example Europe/Amsterdam
 */
+ (NSString *)currentTimezone;

/**
 *  Get device timestamp
 *
 *  @return device timestamp in seconds
 */
+ (NSTimeInterval)currentTimestamp;

/**
 *  Get system up time
 *
 *  @return timestamp in seconds
 */
+ (NSTimeInterval)systemUpTime;

/**
 *  Get device language
 *
 *  @return device current language code
 */
+ (NSString *)deviceLanguageCode;

/**
 *  Get device carrier
 *
 *  @return device carrier
 */
+ (NSString *)carrier;

/**
 *  Get device manufacteurer
 *
 *  @return device manufacteurer
 */
+ (NSString *)manufacturer;

/**
 *  Get device model
 *
 *  @return the model of the device
 */
+ (NSString *)model;

/**
 *  Gets SDK runtime environment
 *
 *  @return the SDK runtime environment or NATIVE if it is not set in the metadata
 */
+ (NSString*)sdkRuntime;

/**
 *  Gets SDK runtime version
 *
 *  @return the SDK runtime version or UNKNOWN if it is not set in the metadata
 */
+ (NSString*)sdkRuntimeVersion;

/**
 *  Gets SDK runtime environment
 *
 *  @return the SDK wrapper version or UNKNOWN if it is not set in the metadata
 */
+ (NSString*)sdkWrapperVersion;

@end

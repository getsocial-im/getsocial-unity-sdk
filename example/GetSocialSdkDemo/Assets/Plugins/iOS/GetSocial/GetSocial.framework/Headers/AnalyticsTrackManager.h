//
//  GetSocial
//
//  Copyright © 2017 GetSocial BV. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "GetSocialConstants.h"

@interface AnalyticsTrackManager : NSObject

/*!
 * Track analytic events.
 *
 * @param eventName the name of the event to be tracked.
 * @param eventProperties the properties as key value to be tracked along with the event.
 */
+(void)trackAnalyticsEvent:(NSString* _Nonnull)eventName
           eventProperties:(NSDictionary *_Nullable)eventProperties;
@end

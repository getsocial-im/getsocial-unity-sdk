//
//  GetSocial
//
//  Copyright © 2017 GetSocial BV. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

/*!
 * @abstract Contains invite message content like attached image url, localized subject and text.
 */
@interface GetSocialInviteContent : NSObject
/** @name Properties */

/*!
 * @abstract Image url to use.
 */
@property(nonatomic, readonly) NSString *imageUrl;

/*!
 * @abstract Subject to use.
 */
@property(nonatomic, readonly) NSString *subject;

/*!
 * @abstract Text to use.
 */
@property(nonatomic, readonly) NSString *text;

@end

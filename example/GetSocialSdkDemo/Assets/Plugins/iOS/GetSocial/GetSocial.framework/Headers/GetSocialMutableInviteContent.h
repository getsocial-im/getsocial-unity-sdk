//
//  GetSocial
//
//  Copyright © 2017 GetSocial BV. All rights reserved.
//

#import <GetSocial/GetSocialInviteContent.h>
#import <GetSocial/GetSocialMutableInviteContent.h>

/*!
 * @abstract Contains mutable invite message content like attached image url, localized subject and text.
 */
@interface GetSocialMutableInviteContent : GetSocialInviteContent
/** @name Properties */

/*!
 * @abstract Image url to use.
 */
@property(nonatomic, readwrite) NSString *imageUrl;

/*!
 * @abstract Subject to use.
 */
@property(nonatomic, readwrite) NSString *subject;

/*!
 * @abstract Text to use.
 */
@property(nonatomic, readwrite) NSString *text;

@end

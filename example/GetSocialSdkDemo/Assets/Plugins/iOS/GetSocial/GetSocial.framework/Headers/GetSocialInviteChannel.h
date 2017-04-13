//
//  GetSocial
//
//  Copyright © 2017 GetSocial BV. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GetSocial/GetSocialInviteProperties.h>
#import <GetSocial/GetSocialInviteContent.h>

/*!
 * @abstract Stores information about a way to send an invite and how it should be presented.
 */
@interface GetSocialInviteChannel : NSObject
/** @name Properties */

/*!
 * @abstract Id of provider.
 */
@property(nonatomic, readonly) NSString *channelId;

/*!
 * @abstract Name of provider.
 */
@property(nonatomic, readonly) NSString *name;

/*!
 * @abstract Url of provider's icon.
 */
@property(nonatomic, readonly) NSString *iconUrl;

/*!
 * @abstract Status of provider.
 */
@property(nonatomic, readonly) BOOL enabled;

/*!
 * @abstract Display order of provider.
 */
@property(nonatomic, readonly) int displayOrder;

/*!
 * @abstract Default invite content of provider.
 */
@property(nonatomic, readonly) GetSocialInviteContent *inviteContent;

@end

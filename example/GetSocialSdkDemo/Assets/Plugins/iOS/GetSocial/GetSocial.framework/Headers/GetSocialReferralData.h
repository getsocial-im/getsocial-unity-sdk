//
//  GetSocial
//
//  Copyright © 2017 GetSocial BV. All rights reserved.
//

#import <Foundation/Foundation.h>

/*!
 * @abstract Defines ReferralData class.
 */
@interface GetSocialReferralData : NSObject

NS_ASSUME_NONNULL_BEGIN

/** @name Properties */

/*!
 * @abstract Unique Smart Invite link token. There is unique association between
 * token and attached referral data.
 */
@property(nonatomic,strong,readonly) NSString* token;

/*!
 * @abstract The GetSocial user id of the user that created the referral data.
 */
@property(nonatomic,strong,readonly) NSString* referrerUserId;

/*!
 * @abstract The id of the channel that was used for the invite.
 */
@property(nonatomic,strong,readonly) NSString* referrerChannelId;

/*!
 * @abstract Returns true if {@link ReferralData} for current token is retrieved for a
 * first time on this device.
 */
@property(nonatomic,readonly) bool isFirstMatch;

/*!
 @abstract Custom data that is assgined to this referral data.
 */
@property(nonatomic,strong,readonly) NSDictionary* customData;

NS_ASSUME_NONNULL_END
@end

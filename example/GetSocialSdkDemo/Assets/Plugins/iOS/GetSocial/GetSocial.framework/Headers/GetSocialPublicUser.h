//
//  GetSocial
//
//  Copyright © 2017 GetSocial BV. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GetSocial/GetSocialConstants.h>

/*!
 * @abstract Immutable properties for a public user.
 */
@interface GetSocialPublicUser : NSObject

NS_ASSUME_NONNULL_BEGIN

/*!
 * @abstract GetSocial User id.
 */
@property(nonatomic, readonly, assign) GetSocialId userId;

/*!
 * @abstract Display name.
 */
@property(nonatomic, readonly, copy) NSString* displayName;

/*!
 * @abstract Avatar url.
 */
@property(nonatomic, readonly, copy, nullable) NSString* avatarUrl;

/*!
 * @abstract Authentication identities added to user.
 */
@property(nonatomic, readonly) NSDictionary<NSString*, NSString*>* authIdentities;

/*!
 * @abstract Returns public property value for the specified key.
 *
 * @param propertyKey key of property.
 * @result property value or nil if property does not exist.
 */
-(nullable NSString *)publicPropertyForKey:(NSString*)propertyKey;

/*!
 * @abstract Checks if public property exists for the specified property key.
 *
 * @param  propertyKey key of property.
 * @return YES if property exists, NO if it does not exist.
 */
-(BOOL)hasPublicPropertyForKey:(NSString*)propertyKey;

NS_ASSUME_NONNULL_END

@end

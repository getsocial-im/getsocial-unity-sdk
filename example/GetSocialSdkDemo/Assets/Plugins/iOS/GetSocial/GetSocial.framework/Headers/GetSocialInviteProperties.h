//
//  GetSocial
//
//  Copyright © 2017 GetSocial BV. All rights reserved.
//

#import <Foundation/Foundation.h>

/*!
 * @abstract Contains properties of an invite.
 */
@interface GetSocialInviteProperties : NSObject
/** @name Properties */

/*!
 * @abstract Action to use.
 */
@property(nonatomic, readonly) NSString *action;

/*!
 * @abstract List of available fields.
 */
@property(nonatomic, readonly) NSArray *availableFields;

/*!
 * @abstract Type of content.
 */
@property(nonatomic, readonly) NSString *contentType;

/*!
 * @abstract Name of package.
 */
@property(nonatomic, readonly) NSString *packageName;

/*!
 * @abstract Url scheme.
 */
@property(nonatomic, readonly) NSString *urlScheme;

/*!
 * @abstract Uniform Type Identifiers.
 */
@property(nonatomic, readonly) NSString *UTI;

@end

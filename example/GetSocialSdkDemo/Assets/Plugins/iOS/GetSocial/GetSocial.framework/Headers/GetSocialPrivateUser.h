//
//  GetSocial
//
//  Copyright © 2017 GetSocial BV. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GetSocial/GetSocialPublicUser.h>

@interface GetSocialPrivateUser : GetSocialPublicUser

NS_ASSUME_NONNULL_BEGIN

/**
 *  Returns private property value for the specified key.
 *  @param  propertyKey key of property
 *  @return property value or nil if property does not exist
 */
-(nullable NSString *)privatePropertyForKey:(NSString*)propertyKey;

/**
 *  Checks if private property exists for the specified property key
 *  @param  propertyKey key of property
 *  @return YES if property exists, NO if it does not
 */
-(BOOL)hasPrivatePropertyForKey:(NSString*)propertyKey;

NS_ASSUME_NONNULL_END

@end

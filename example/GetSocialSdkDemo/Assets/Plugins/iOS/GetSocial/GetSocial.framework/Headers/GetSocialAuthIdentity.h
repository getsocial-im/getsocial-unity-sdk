//
//  GetSocial
//
//  Copyright © 2017 GetSocial BV. All rights reserved.
//

#import <Foundation/Foundation.h>

/*!
 * @abstract This class is representation of User Auth Identity, that is used by GetSocial framework to identify user
 * and to manage his accounts.
 */
@interface GetSocialAuthIdentity : NSObject

/*!
 * @abstract Creates a Facebook identity with specified access token.
 *
 * @param accessToken Token of Facebook user returned from FB SDK.
 * @result instance of GetSocialIdentity for Facebook user with specified access token.
 */
+ (instancetype)facebookIdentityWithAccessToken:(NSString *)accessToken;

/**
 * Create custom identity.
 *
 * @param providerName 	        Your custom provider name.
 * @param userId				Unique user identifier for your custom provider.
 * @param accessToken			Password of the user for your custom provider.
 *                       		It's a string, provided by the developer and it will be
 *                       		required by the GetSocial SDK to validate any future
 *                       		intent to add this same identity to another user.
 * @result An instance of GetSocialIdentity for your custom provider.
 */
+ (instancetype)customIdentityForProvider:(NSString *)providerName
                                   userId:(NSString *)userId
                              accessToken:(NSString *)accessToken;

@end

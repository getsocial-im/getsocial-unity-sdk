//
//  GetSocial
//
//  Copyright © 2017 GetSocial BV. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GetSocial/GetSocialConstants.h>

@class GetSocialUserUpdate;
@class GetSocialAuthIdentity;

/*!
 * @abstract Defines interface for managing user.
 */
@interface GetSocialUser : NSObject

NS_ASSUME_NONNULL_BEGIN

#pragma mark - User Details

/** @name Update User Details */

/*!
 * @abstract Set block to be called when user has been changed.
 * The action is executed on the main thread, so be careful with operations,
 * that you put inside block.
 * Handler will be called when:
 * - SDK initialization is finished;
 * - GetSocialUser#switchUserWithProviderId method was called and user was successfully changed.
 *
 * @param handler Block to be called if user has been changed
 *
 * @result YES if the operation was successful, otherwise NO.
 */
+ (BOOL)setOnUserChangedHandler:(GetSocialUserChangedHandler)handler;

/*!
 * @abstract Remove current handler.
 *
 * @result YES if the operation was successful, otherwise NO.
 */
+ (BOOL)removeOnUserChangedHandler;

/*!
 * @abstract Update user details.
 *
 * @param updateDetails New user update
 * @param success Block called if user is updated.
 * @param failure Block called if operation fails.
 */
+ (void)updateDetails:(GetSocialUserUpdate *)updateDetails
             success:(GetSocialSuccessCallback)success
             failure:(GetSocialFailureCallback)failure;

/** @name Update User Display Name */

/*!
 * @abstract Update user display name.
 *
 * @param newDisplayName Display name to set
 * @param success Block called if display name was updated
 * @param failure Block called if operation fails.
 */
+ (void)setDisplayName:(NSString *)newDisplayName
               success:(GetSocialSuccessCallback)success
               failure:(GetSocialFailureCallback)failure;

/** @name User Display Name */

/*!
 * @abstract Returns the display name of current user.
 *
 * @result Display Name of current user.
 */
+ (nullable NSString *)displayName;

/** @name Update User Avatar URL */

/*!
 * @abstract Update user avatar URL.
 *
 * @param newAvatarUrl Avatar URL to set
 * @param success Block called if avatar URL was updated
 * @param failure Block called if operation fails.
 */
+ (void)setAvatarUrl:(NSString *)newAvatarUrl
             success:(GetSocialSuccessCallback)success
             failure:(GetSocialFailureCallback)failure;

/** @name User Avatar URL */

/*!
 * @abstract Returns the avatar URL of current user.
 *
 * @result Avatar URL of current user.
 */
+ (nullable NSString *)avatarUrl;

/** @name User id */

/*!
 * @abstract Returns the id of current user.
 *
 * @result Id of current user.
 */
+(GetSocialId)userId;

#pragma mark - Identities
/** @name Identities */

/*!
 * @abstract Indicates if the user has at least one auth identity available.
 *
 * @result YES, if user is anonymous or framework is not initalized, NO, if user has at least one auth identities added.
 */
+(BOOL)isAnonymous;

/*!
 * @abstract Adds AuthIdentity for the specified provider.
 *
 * @param identity Identity to be added.
 * @param success Block called if identity is added.
 * @param conflict Block called if identity cannot be added due to a conflict with other user.
 * @param failure Block called if operation fails.
 */
+(void)addAuthIdentity:(GetSocialAuthIdentity *)identity success:(GetSocialSuccessCallback)success conflict:(GetSocialAddIdentityConflictCallback)conflict failure:(GetSocialFailureCallback)failure;

/*!
 * @abstract Removes auth identity from current user for the specific provider.
 *
 * @param providerId The provider connected to an auth identity on the current user to remove.
 * @discussion Valid providerIds are "facebook","googleplus","googleplay" and custom providers.
 * @param success Block called if identity is removed.
 * @param failure Block called if operation fails.
 */
+(void)removeAuthIdentityWithProviderId:(NSString*)providerId success:(GetSocialSuccessCallback)success failure:(GetSocialFailureCallback)failure;

/*!
 * @abstract Switches the current user with the PublicUser corresponding to the details provided.
 *
 * @param identity Identity that current user should be switched to.
 * @param success Block called if user is switched.
 * @param failure Block called if operation fails.
 */
+(void)switchUserToIdentity:(GetSocialAuthIdentity *)identity success:(GetSocialSuccessCallback)success failure:(GetSocialFailureCallback)failure;

/*!
 * @abstract Returns all identities added to the user. The key is the provider Id and the value is the user Id
    used internally by that provider for this user.
 *
 * @result All auth identities added to the user.
 */
+(NSDictionary<NSString*, NSString*>*)authIdentities;

#pragma mark - Friends

/*!
 * @abstract Add a friend for current user, if operation succeed - they both became friends.
 * If you're trying to add a user, that already is your friend, success callback will be called,
 * but user will be added to your friends list only once and your friends count won't be increased.
 *
 * @param userId 	Unique user identifier you want to become friend with.
 * @param success   Block called if friend was added successfully.
 * @param failure   Block called if operation fails.
 */
+ (void)addFriend:(GetSocialId)userId success:(GetSocialIntegerCallback)success failure:(GetSocialFailureCallback)failure;

/*!
 * @abstract Remove a user from friends list.
 * If you're trying to remove a user, that is not your friend, success callback will be called,
 * and your friends count won't be decreased.
 *
 * @param userId 	Unique user identifier you don't want to be friends anymore.
 * @param success   Block called if friend was removed successfully.
 * @param failure   Block called if operation fails.
 */
+ (void)removeFriend:(GetSocialId)userId success:(GetSocialIntegerCallback)success failure:(GetSocialFailureCallback)failure;

/*!
 * @abstract Check if user is your friend.
 *
 * @param userId 	Unique user identifier.
 * @param success   Block called with result if user is your friend or not.
 * @param failure   Block called if operation fails.
 */
+ (void)isFriend:(GetSocialId)userId success:(GetSocialResultCallback)success failure:(GetSocialFailureCallback)failure;

/*!
 * @abstract Remove a user from friends list.
 *
 * @param success   Block called with count of friends.
 * @param failure   Block called if operation fails.
 */
+ (void)friendsCountWithSuccess:(GetSocialIntegerCallback)success failure:(GetSocialFailureCallback)failure;

/*!
 * @abstract Get a list of friends for current user.
 *
 * @param offset     Limit of users.
 * @param limit    Offset position.
 * @param success   Block called with list of users that are friends of current user.
 * @param failure   Block called if operation fails.
 */
+ (void)friendsWithOffset:(int)offset limit:(int)limit success:(GetSocialUsersResultCallback)success failure:(GetSocialFailureCallback)failure;

NS_ASSUME_NONNULL_END

@end

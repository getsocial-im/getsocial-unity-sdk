//
//  GetSocial
//
//  Copyright © 2017 GetSocial BV. All rights reserved.
//

#import <GetSocial/GetSocialConstants.h>
#import <GetSocial/GetSocialInvitePackage.h>
#import <GetSocial/GetSocialInviteChannelPlugin.h>
#import <GetSocial/GetSocialMutableInviteContent.h>
#import <GetSocial/GetSocialConflictUser.h>
#import <GetSocial/GetSocialUser.h>
#import <GetSocial/GetSocialAuthIdentity.h>
#import <GetSocial/GetSocialCheckArgument.h>
#import <GetSocial/GetSocialActivityPostContent.h>
#import <GetSocial/GetSocialActivityPost.h>
#import <GetSocial/GetSocialActivityPostBuilder.h>
#import <GetSocial/GetSocialPublicUser.h>
#import <GetSocial/GetSocialPostAuthor.h>
#import <GetSocial/GetSocialActivitiesQuery.h>
#import <GetSocial/GetSocialSystemInformation.h>
#import <GetSocial/GetSocialUserUpdate.h>
#import <GetSocial/GetSocialReferralData.h>
#import <GetSocial/GetSocialNotificationAction.h>
#import <GetSocial/GetSocialOpenActivityAction.h>
#import <GetSocial/GetSocialOpenProfileAction.h>

/*!
 * @abstract Main interface of GetSocial.framework
 */
@interface GetSocial : NSObject

NS_ASSUME_NONNULL_BEGIN

/*!
 * @abstract Returns version of GetSocial Framework.
 *
 * @result String value of the framework version.
 */
+ (NSString *)sdkVersion;

#pragma mark - Initialization
/** @name Initialization */

/*!
 * @abstract Set an action, which should be executed after SDK initialized.
 * Executed immediately, if SDK is already initialized.
 * @param action Action to execute.
 */
+ (void)executeWhenInitialized:(void(^)())action;

/*!
 * @abstract Provides the status of GetSocial Framework initialization.
 *
 * @result YES if initialization completed successfully, otherwise NO.
 */
+ (BOOL)isInitialized;

/*!
 * @abstract Sets the global error handler block, that will we called after each internal crash in GetSocial Framework.
 *
 * @param errorHandler block to be called.
 * @result YES if the operation was successful, otherwise NO.
 */
+ (BOOL)setGlobalErrorHandler:(GetSocialGlobalErrorHandler)errorHandler;

/*!
 * @abstract Removes the global error handler.
 *
 * @result YES if operation was successful, otherwise NO.
 */
+ (BOOL)removeGlobalErrorHandler;

#pragma mark - Languages
/** @name Languages */

/*!
 * @abstract Sets the language of GetSocial Framework.
 * @discussion If provided value is incorrect, sets the default language.
 *
 * @param languageCode as defined in GetSocialConstants
 * @result YES if operation was successful, otherwise NO.
 */
+ (BOOL)setLanguage:(NSString *)languageCode;

/*!
 * @abstract Returns current language of GetSocial Framework.
 *
 * @result language code as defined in @see GetSocialConstants.h, or default language in case of failure.
 */
+ (NSString *)language;

#pragma mark - Invites
/** @name Invites */

/*!
 * @abstract Returns list of available invite channels.
 *
 * @result list of GetSocialInviteChannel classes.
 */
+ (NSArray<GetSocialInviteChannel *> *)inviteChannels;

/*!
 * @abstract Sends an invitation using the specified channel.
 *
 * @param channelId  Id of channel to use.
 * @param success    Called if invitation was sent successfully.
 * @param cancel     Called if invitation sending was canceled.
 * @param failure    Called if invitation could not be sent due to an error.
 */
+ (void)sendInviteWithChannelId:(NSString *)channelId
                        success:(GetSocialInviteSuccessCallback)success
                         cancel:(GetSocialInviteCancelCallback)cancel
                        failure:(GetSocialFailureCallback)failure;

/*!
 * @abstract Sends an invitation using the specified channel and invite content.
 *
 * @param channelId             Id of channel to use.
 * @param customInviteContent   Invite Content to send.
 * @param success               Called if invitation was sent successfully.
 * @param cancel                Called if invitation sending was canceled.
 * @param failure               Called if invitation could not be sent due to an error.
 */
+ (void)sendInviteWithChannelId:(NSString *)channelId
                  inviteContent:(GetSocialMutableInviteContent *_Nullable)customInviteContent
                        success:(GetSocialInviteSuccessCallback)success
                         cancel:(GetSocialInviteCancelCallback)cancel
                        failure:(GetSocialFailureCallback)failure;


/*!
 * @abstract Sends an invitation using the specified channel and invite content.
 *
 * @param channelId             Id of channel to use.
 * @param customInviteContent   Invite Content to send.
 * @param customReferralData    Custom referral data to send.
 * @param success               Called if invitation was sent successfully.
 * @param cancel                Called if invitation sending was canceled.
 * @param failure               Called if invitation could not be sent due to an error.
 */
+ (void)sendInviteWithChannelId:(NSString *)channelId
                  inviteContent:(GetSocialMutableInviteContent *_Nullable)customInviteContent
             customReferralData:(NSDictionary *_Nullable)customReferralData
                        success:(GetSocialInviteSuccessCallback)success
                         cancel:(GetSocialInviteCancelCallback)cancel
                        failure:(GetSocialFailureCallback)failure;

/*!
 * @abstract Registers an invite channel plugin for the specified channel id.
 *
 * @param inviteChannelPlugin  plugin to register.
 * @param channelId            channle id.
 * @result YES if the operation was successful, otherwise NO.
 */
+ (BOOL)registerInviteChannelPlugin:(__kindof GetSocialInviteChannelPlugin *)inviteChannelPlugin forChannelId:(NSString *)channelId;

/*!
 * @abstract Returns referral data received.
 *
 * @param success               Called if getting referral data finished. Referral data can be nil.
 * @param failure               Called if getting referral data failed.
 */
+ (void)referralDataWithSuccess:(GetSocialReferralDataCallback)success failure:(GetSocialFailureCallback)failure;

#pragma mark - Push notifications
/*!
 * @abstract If im.getsocial.sdk.AutoRegisterForPush meta property is set to false in the Info.plist,
 * call this method to register for push notifications.
 * Register your application for a push notifications.
 */
+ (void)registerForPushNotifications;

/*!
 * @abstract Set a handler to be notified if application is started with clicking on GetSocial notification.
 * @param handler Called with action.
 */
+ (void)setNotificationActionHandler:(GetSocialNotificationActionHandler)handler;

#pragma mark - Activities
/** @name Activities */

/*!
 * @abstract Retrieve list of announcements for global feed.
 *
 * @param success  Called with resulting list of activities if call was successful.
 * @param failure  Called if operation can not be called due to an error.
 */
+ (void)announcementsForGlobalFeedWithSuccess:(GetSocialActivitiesResultCallback)success
                                      failure:(GetSocialFailureCallback)failure;

/**
 *  Retrieve list of announcements for feed.
 *
 *  @param feed     Feed name
 *  @param success  Called with resulting list of activities if call was successful
 *  @param failure  Called if operation can not be called due to an error
 */
+ (void)announcementsForFeed:(NSString *)feed
                     success:(GetSocialActivitiesResultCallback)success
                     failure:(GetSocialFailureCallback)failure;

/*!
 * @abstract Retrieve list of activities.
 *
 * @param query    Filtering options.
 * @param success  Called with resulting list of activities if call was successful.
 * @param failure  Called if operation can not be called due to an error.
 */
+ (void)activitiesWithQuery:(GetSocialActivitiesQuery *)query
                    success:(GetSocialActivitiesResultCallback)success
                    failure:(GetSocialFailureCallback)failure;

/*!
 * @abstract Retrieve activity by id.
 *
 * @param activityId   Identifier of activity.
 * @param success      Called with resulting activity if call was successful.
 * @param failure      Called if operation can not be called due to an error.
 */
+ (void)activityWithId:(GetSocialId)activityId success:(GetSocialActivityResultCallback)success failure:(GetSocialFailureCallback)failure;

/*!
 * @abstract Post activity to global activity feed.
 *
 * @param activity content of activity, that should be posted.
 * @param success  Called with resulting activity if activity was posted successfully.
 * @param failure  Called if operation can not be called due to an error.
 */
+ (void)postActivityToGlobalFeed:(GetSocialActivityPostContent *)activity
                         success:(GetSocialActivityResultCallback)success
                         failure:(GetSocialFailureCallback)failure;

/*!
 * @abstract Post activity to activity feed.
 *
 * @param activity content of activity, that should be posted.
 * @param feed     Name of feed.
 * @param success  Called with resulting activity if activity was posted successfully.
 * @param failure  Called if operation can not be called due to an error.
 */
+ (void)postActivity:(GetSocialActivityPostContent *)activity
              toFeed:(NSString *)feed
             success:(GetSocialActivityResultCallback)success
             failure:(GetSocialFailureCallback)failure;

/*!
 * @abstract Post comment to activity with specified id.
 *
 * @param activityId 	identifier of activity, that we want to comment.
 * @param comment       content of comment, that should be posted.
 * @param success       Called with resulting activity if activity was posted successfully.
 * @param failure       Called if operation can not be called due to an error.
 */
+ (void)postComment:(GetSocialActivityPostContent *)comment
   toActivityWithId:(GetSocialId)activityId
            success:(GetSocialActivityResultCallback)success
            failure:(GetSocialFailureCallback)failure;

/*!
 * @abstract Like or unlike activity.
 *
 * @param activityId 	identifier of activity, that we want to like or unlike.
 * @param isLiked       should activity be liked or not.
 * @param success       Called with resulting activity if activity was liked/unliked successfully.
 * @param failure       Called if operation can not be called due to an error.
 */
+ (void)likeActivityWithId:(GetSocialId)activityId
                   isLiked:(BOOL)isLiked
                   success:(GetSocialActivityResultCallback)success
                   failure:(GetSocialFailureCallback)failure;

/*!
 * @abstract Get a list of users, that liked activity.
 *
 * @param activityId    identifier of activity, that we want to get who liked it.
 * @param offset        Offset position.
 * @param limit         Maximum count of users.
 * @param success       Called with list of users, that liked activity.
 * @param failure       Called if operation can not be called due to an error.
 */
+ (void)activityLikers:(GetSocialId)activityId
                offset:(int)offset
                 limit:(int)limit
               success:(GetSocialUsersResultCallback)success
               failure:(GetSocialFailureCallback)failure;

NS_ASSUME_NONNULL_END

@end

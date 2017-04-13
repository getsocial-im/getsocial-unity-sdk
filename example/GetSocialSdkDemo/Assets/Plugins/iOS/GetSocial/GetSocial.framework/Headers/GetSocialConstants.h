//
//  GetSocial
//
//  Copyright © 2017 GetSocial BV. All rights reserved.
//

#import <Foundation/NSObject.h>

@class GetSocialConflictUser;
@class GetSocialPublicUser;
@class GetSocialActivityPost;
@class GetSocialReferralData;
@class GetSocialNotificationAction;

#ifndef GetSocialLibrary_GetSocialConstants_h
#define GetSocialLibrary_GetSocialConstants_h

NS_ASSUME_NONNULL_BEGIN

/*!
 * @typedef GetSocialNotificationActionType
 * @abstract Enumeration represents possible actions to be executed if application was started with clicking on GetSocial push notification.
 * @constant GetSocialNotificationActionOpenActivity Open activity action.
 */
typedef NS_ENUM(NSInteger, GetSocialNotificationActionType) {
    /// open activity action
    GetSocialNotificationActionOpenActivity
};

 /*!
 * @typedef void (^GetSocialNotificationActionHandler)(GetSocialNotificationAction *action)
 * @abstract The typedef defines the block that is called if application was started with clicking on a GetSocial notification.
 */
typedef BOOL (^GetSocialNotificationActionHandler)(GetSocialNotificationAction *action);

/*!
 * @typedef void (^GetSocialGlobalErrorHandler)(NSException *exception)
 * @abstract The typedef defines the signature of a block that is called when any
 exception happens in the framework.
 */
typedef void (^GetSocialGlobalErrorHandler)(NSError *error);

/*!
 * @typedef void (^GetSocialUserChangedHandler)()
 * @abstract The typedef defines the signature of a block that is called when an GetSocial user
 * was changed.
 */
typedef void (^GetSocialUserChangedHandler)();

/*!
 * @typedef void (^GetSocialSuccessCallback)()
 * @abstract The typedef defines the signature of a block that is called when an operation
 completes.
 */
typedef void (^GetSocialSuccessCallback)();

/*!
 * @typedef void (^GetSocialFailureCallback)(NSError *error)
 * @abstract The typedef defines the signature of a block that is called when an operation
 fails.
 *
 * @param error is the error that caused the failure.
 */
typedef void (^GetSocialFailureCallback)(NSError *error);

/*!
 * @typedef void (^GetSocialResultCallback)(BOOL result)
 * @abstract The typedef defines the signature of a block that is called an operation completes with some boolean result
 *
 * @param result is the boolean result of operation.
 */
typedef void (^GetSocialResultCallback)(BOOL result);

/*!
 * @typedef void (^GetSocialIntegerCallback)(int result)
 * @abstract The typedef defines the signature of a block that is called an operation completes with some integer result
 *
 * @param result is the boolean result of operation.
 */
typedef void (^GetSocialIntegerCallback)(int result);

/*!
 * @typedef void (^GetSocialInviteSuccessCallback)()
 * @abstract The typedef defines the signature of a block that is call when invite operation succeeds.
 */
typedef void (^GetSocialInviteSuccessCallback)();

/*!
 * @typedef void (^GetSocialInviteCancelCallback)()
 * @abstract The typedef defines the signature of a block that is call when invite operation is cancelled.
 */
typedef void (^GetSocialInviteCancelCallback)();

/*!
 * @typedef void (^GetSocialReferralDataCallback)()
 * @abstract The typedef defines the signature of a block that is call when getting referral data.
 */
typedef void (^GetSocialReferralDataCallback)(GetSocialReferralData* _Nullable referralData);

/*!
 * @typedef void (^GetSocialPublicUserSuccessCallback)(id publicUser)
 * @abstract The typedef defines the signature of a block that is called user is retrieved.
 *
 * @param publicUser GetSocialPublicUser instance.
 */
typedef void (^GetSocialPublicUserSuccessCallback)(GetSocialPublicUser *publicUser);

/*!
 * @typedef void (^GetSocialAddIdentityConflictCallback)(id conflictUser)
 * @abstract The typedef defines the signature of a block that is called if identity cannot be added due conflict.
 *
 * @param conflictUser GetSocialConflictUser instance.
 */
typedef void (^GetSocialAddIdentityConflictCallback)(GetSocialConflictUser *conflictUser);

/*!
 * @typedef void (^GetSocialActivityResultCallback)(id result)
 * @abstract The typedef defines the signature of a block that is called when an operation
 completes.
 */
typedef void (^GetSocialActivityResultCallback)(GetSocialActivityPost *result);

/*!
 * @typedef void (^GetSocialActivitiesResultCallback)(NSArray<id> *result)
 * @abstract The typedef defines the signature of a block that is called when an operation
 completes.
 */
typedef void (^GetSocialActivitiesResultCallback)(NSArray<GetSocialActivityPost *> *result);

/*!
 * @typedef void(^GetSocialUsersResultCallback)(NSArray<id> *result)
 * @abstract The typedef defines the signature of a block that is called when an operation
 completes.
 */
typedef void (^GetSocialUsersResultCallback)(NSArray<GetSocialPublicUser *> *result);

/*!
 * @typedef NSString GetSocialId
 * @abstract The typedef defines SDK identifiers type.
 */
typedef NSString* GetSocialId;

#endif

/*!
 * @abstract Defines constant for GetSocial Framework
 */
@interface GetSocialConstants : NSObject

#pragma mark - Error codes

/*!
 * @abstract Unknown error
 */
extern NSInteger GetSocial_ErrorCode_Unknown;

/*!
 * @abstract Defines code for illegal argument error.
 */
extern NSInteger GetSocial_ErrorCode_IllegalArgument;

/*!
 * @abstract Defines code for illegal state error.
 */
extern NSInteger GetSocial_ErrorCode_IllegalState;

#pragma mark - Error keys

/*!
 * @abstract The corresponding value is an array containing the underlying errors.
 */
extern NSString* GetSocial_ErrorKey_ErrorsList;

/*!
 * @abstract The corresponding value is a dictionary containing context of the error.
 */
extern NSString* GetSocial_ErrorKey_Context;

#pragma mark - Invite Channel Plugin ids

/*!
 * @abstract Generic Invite Channel.
 */
extern NSString *GetSocial_InviteChannelPluginId_Generic;

/*!
 * @abstract Sms Invite Channel.
 */
extern NSString *GetSocial_InviteChannelPluginId_Sms;

/*!
 * @abstract Email Invite Channel.
 */
extern NSString *GetSocial_InviteChannelPluginId_Email;

/*!
 * @abstract Facebook Invite Channel.
 */
extern NSString *GetSocial_InviteChannelPluginId_Facebook;

/*!
 * @abstract Facebook Messenger Invite Channel.
 */
extern NSString *GetSocial_InviteChannelPluginId_Facebook_Messenger;

/*!
 * @abstract Native share Invite Channel.
 */
extern NSString *GetSocial_InviteChannelPluginId_NativeShare;

/*!
 * @abstract Twitter Invite Channel.
 */
extern NSString *GetSocial_InviteChannelPluginId_Twitter;

/*!
 * @abstract Google Plus Invite Channel.
 */
extern NSString *GetSocial_InviteChannelPluginId_GooglePlus;

/*!
 * @abstract Kik Invite Channel.
 */
extern NSString *GetSocial_InviteChannelPluginId_Kik;

/*!
 * @abstract Kakao Invite Channel.
 */
extern NSString *GetSocial_InviteChannelPluginId_Kakao;

/*!
 * @abstract WhatsApp Invite Channel.
 */
extern NSString *GetSocial_InviteChannelPluginId_WhatsApp;

/*!
 * @abstract Line Invite Channel.
 */
extern NSString *GetSocial_InviteChannelPluginId_Line;

#pragma mark - Invite Content Placeholder constants

/*!
 * @abstract Invite content placeholder for application invite url.
 */
extern NSString *GetSocial_InviteContentPlaceholder_App_Invite_Url;

// TODO: add in the next release
//*!
// * @abstract Invite content placeholder for user display name.
// */
//extern NSString *GetSocial_InviteContentPlaceholder_User_Display_Name;

#pragma mark - Activity Feed Constants

/*!
 * @abstract Constant for Global Activity Feed.
 */
extern NSString *GetSocial_ActivityFeed_GlobalFeed;

/*!
 * @abstract Default limit for Activity feed.
 */
extern NSInteger GetSocial_ActivityFeed_DefaultLimit;

#pragma mark - Language Codes

/*!
 * @abstract Get the dictionary of all available languages.
 */
+ (NSDictionary<NSString *, NSString *> *)allLanguageCodes;

/*!
 * @abstract Default language code.
 */
extern NSString *GetSocial_Languages_Default;
extern NSString *GetSocial_Languages_ChineseSimplified;
extern NSString *GetSocial_Languages_ChineseTraditional;
extern NSString *GetSocial_Languages_Danish;
extern NSString *GetSocial_Languages_Dutch;
extern NSString *GetSocial_Languages_English;
extern NSString *GetSocial_Languages_French;
extern NSString *GetSocial_Languages_German;
extern NSString *GetSocial_Languages_Icelandic;
extern NSString *GetSocial_Languages_Indonesian;
extern NSString *GetSocial_Languages_Italian;
extern NSString *GetSocial_Languages_Japanese;
extern NSString *GetSocial_Languages_Korean;
extern NSString *GetSocial_Languages_Malay;
extern NSString *GetSocial_Languages_Norwegian;
extern NSString *GetSocial_Languages_Polish;
extern NSString *GetSocial_Languages_Portuguese;
extern NSString *GetSocial_Languages_PortugueseBrazillian;
extern NSString *GetSocial_Languages_Russian;
extern NSString *GetSocial_Languages_Spanish;
extern NSString *GetSocial_Languages_Swedish;
extern NSString *GetSocial_Languages_Tagalog;
extern NSString *GetSocial_Languages_Turkish;
extern NSString *GetSocial_Languages_Ukrainian;
extern NSString *GetSocial_Languages_Vietnamese;

#pragma mark - Analytics Codes

extern NSString *GetSocial_AnalyticsEventDetails_Name_App_Session_Start;
extern NSString *GetSocial_AnalyticsEventDetails_Name_App_Session_End;
extern NSString *GetSocial_AnalyticsEventDetails_Name_Ui_Invite_Clicked;
extern NSString *GetSocial_AnalyticsEventDetails_Name_Ui_User_Activity_Action_Click;
extern NSString *GetSocial_AnalyticsEventDetails_Name_Ui_Content_Session;
extern NSString *GetSocial_AnalyticsEventDetails_Name_Sdk_Error;
extern NSString *GetSocial_AnalyticsEventDetails_Name_PushNotificationReceived;
extern NSString *GetSocial_AnalyticsEventDetails_Name_PushNotificationClicked;

extern NSString *GetSocial_AnalyticsEventDetails_Properties_Activities_Source;
extern NSString *GetSocial_AnalyticsEventDetails_Properties_Activity_Likers_Source;
extern NSString *GetSocial_AnalyticsEventDetails_Properties_Comment_Likers_Source;
extern NSString *GetSocial_AnalyticsEventDetails_Properties_Comments_Source;
extern NSString *GetSocial_AnalyticsEventDetails_Properties_Invites_Source;

extern NSString *GetSocial_AnalyticsEventDetails_Properties_Provider;
extern NSString *GetSocial_AnalyticsEventDetails_Properties_Action;
extern NSString *GetSocial_AnalyticsEventDetails_Properties_Title;
extern NSString *GetSocial_AnalyticsEventDetails_Properties_Duration;
extern NSString *GetSocial_AnalyticsEventDetails_Properties_Source;

extern NSString *GetSocial_AnalyticsEventDetails_Properties_PushNotificationId;

extern NSString *GetSocial_AnalyticsEventDetails_Properties_Error_Message;
extern NSString *GetSocial_AnalyticsEventDetails_Properties_Error_Source;
extern NSString *GetSocial_AnalyticsEventDetails_Properties_Error_Key;

#pragma mark - User management

/*!
 * @abstract AuthIdentityProviderId for Facebook
 */
extern NSString *GetSocial_AuthIdentityProviderId_Facebook;

NS_ASSUME_NONNULL_END

@end

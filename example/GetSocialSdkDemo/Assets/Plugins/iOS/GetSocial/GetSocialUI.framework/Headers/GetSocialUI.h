//
//  GetSocialUI
//
//  Copyright © 2017 GetSocial BV. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <GetSocialUI/GetSocialUIInvitesView.h>
#import <GetSocialUI/GetSocialUIActivityFeedView.h>
#import <GetSocialUI/GetSocialUIActivityDetailsView.h>


/*!
 * @abstract Main interface of GetSocialUI.framework.
 */
@interface GetSocialUI : NSObject

NS_ASSUME_NONNULL_BEGIN

#pragma mark - UI Configuration
/** @name UI Configuration */


/*!
 *  @abstract Loads a custom configuration from a configuration file.
 *
 *  @param  filePath The the path to the configuration file.
 *  @result YES, if configuration loaded, otherwise NO.
 */
+ (BOOL)loadConfiguration:(NSString *)filePath;

/*!
 * @abstract Loads default configuration.
 *
 *  @result YES, if configuration loaded, otherwise NO.
 */
+ (BOOL)loadDefaultConfiguration;

#pragma mark Generic View methods
/** @name Generic View methods */

/*!
 *  @abstract Shows GetSocial view.
 *
 *  @param view GetSocial view to be shown.
 *  @result YES, if view was shown, otherwise NO.
 */
+ (BOOL)showView:(GetSocialUIView *)view;

/*!
 *  @abstract Closes GetSocial view.
 *
 *  @param saveViewState YES if the state of GetSocial view needs to be restored later by calling restoreView.
 *  @result YES, if view was closed, otherwise NO.
 */
+ (BOOL)closeView:(BOOL)saveViewState;

/*!
 *  @abstract Displays GetSocial view, which was hidden using [GetSocialUI closeView:] method.
 *  
 *  @result YES, if view was restored, otherwise NO.
 */
+ (BOOL)restoreView;

#pragma mark - Invites
/** @name Invites */

/*!
 *  @abstract Creates Invites View.
 *
 *  @result instance of GetSocialUIInvitesView
 */
+ (GetSocialUIInvitesView *)createInvitesView;

#pragma mark - Activities
/** @name Activities */

/*!
 * @abstract Create global Activity Feed view.
 *
 * @result instance of GetSocialUIActivityFeedView.
 */
+ (GetSocialUIActivityFeedView *)createGlobalActivityFeedView;

/*!
 *  @abstract Create Activity Feed view with custom feed.
 *
 *  @param  id of activity feed.
 *  @result instance of GetSocialUIActivityFeedView.
 */
+ (GetSocialUIActivityFeedView *)createActivityFeedView:(NSString *)feed;

/*!
 * @abstract Create Activity Details view.
 * @param activityId Activity ID for which open a view.
 * @return instance of GetSocialUIActivityDetailsView.
 */
+ (GetSocialUIActivityDetailsView *)createActivityDetailsView:(NSString *)activityId;

NS_ASSUME_NONNULL_END

@end

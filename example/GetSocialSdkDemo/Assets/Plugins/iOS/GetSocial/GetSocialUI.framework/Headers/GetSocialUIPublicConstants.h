//
//  GetSocialUI
//
//  Copyright © 2017 GetSocial BV. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GetSocial/GetSocialActivityPost.h>

/*!
 * @typedef GetSocialUIAction
 * @abstract Enumeration represents possible actions user is going to perform., while using GetSocial UI.
 * @constant GetSocialUIActionOpenComments Open activity comments list.
 * @constant GetSocialUIActionPostActivity Post an activity.
 * @constant GetSocialUIActionLikeComment Post a comment.
 * @constant GetSocialUIActionLikeActivity Like an activity.
 * @constant GetSocialUIActionLikeComment Like a comment.
 */
typedef NS_ENUM(NSInteger, GetSocialUIActionType) {
    GetSocialUIActionOpenComments = 0,
    GetSocialUIActionPostActivity,
    GetSocialUIActionPostComment,
    GetSocialUIActionLikeActivity,
    GetSocialUIActionLikeComment
};

/*!
 *  @typedef void(^ActivityButtonActionHandler)(NSString *action, GetSocialActivityPost *post)
 *  @abstract The typedef defines the signature of a block that is called when any
 *   action triggered on Activity Feeds view.
 *
 */
typedef void(^ActivityButtonActionHandler)(NSString *action, GetSocialActivityPost *post);

/*!
 * @typedef void (^GetSocialUIPendingAction)()
 * @abstract the typedef defines block to be called if you want to perform UI action.
 */
typedef void (^GetSocialUIPendingAction)();

/*!
* @typedef void (^GetSocialUIActionHandler)(GetSocialUIAction actionType, GetSocialUIPendingAction pendingAction)
* @abstract The typedef defines the block that is called before execution of action. If you want to allow action, call
* pendingAction block. Otherwise, do nothing.
*/
typedef void (^GetSocialUIActionHandler)(GetSocialUIActionType actionType, GetSocialUIPendingAction pendingAction);
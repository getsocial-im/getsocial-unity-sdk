//
//
// Copyright (c) 2020 GetSocial BV. All rights reserved.
//

#import <Foundation/Foundation.h>

#import "GetSocialUIView.h"

/*!
 * Describes GetSocialUIActivityDetailsView class.
 */
@interface GetSocialUIActivityDetailsView : GetSocialUIView

NS_ASSUME_NONNULL_BEGIN

/*!
 *  @abstract Create a new instance.
 *
 *  @param activityId Activity ID.
 */
+ (instancetype)viewForActivityId:(NSString *)activityId;

/*!
 *  @abstract set action handler to be invoked on button clicked.
 *
 *  @param actionHandler block to be called.
 */
- (void)setActionHandler:(GetSocialActionHandler)actionHandler;

/*!
 *  @abstract By default is YES. If you want to open only comments, without feed view in history, set is to NO.
 *
 *  @param showActivityFeed Show feed or not.
 */
- (void)setShowActivityFeedView:(BOOL)showActivityFeed;

/*!
 *  @abstract Sets a handler, that will be called if user clicks on any user avatar.
 *
 *  @param avatarClickHandler block that will be called.
 */
- (void)setAvatarClickHandler:(AvatarClickHandler)avatarClickHandler;
/*!
 *  @abstract Sets a handler, that will be called if user clicks on any user mention.
 *
 *  @param mentionClickHandler block that will be called
 */
- (void)setMentionClickHandler:(MentionClickHandler)mentionClickHandler;

/*!
 *  @abstract Sets a handler, that will be called if user clicks on any tag.
 *
 *  @param tagClickHandler block that will be called
 */
- (void)setTagClickHandler:(TagClickHandler)tagClickHandler;

/*!
 * @abstract Make the feed read-only. UI elements, that allows to post, comment or like are hidden.
 * @param canInteract should feed be read-only.
 */
- (void)setCanInteract:(BOOL)canInteract;

/*!
 * Set a comment ID that should be focused when activity details are opened.
 * @param commentId ID of focused comment.
 */
- (void)setCommentId:(NSString*)commentId;

NS_ASSUME_NONNULL_END

@end

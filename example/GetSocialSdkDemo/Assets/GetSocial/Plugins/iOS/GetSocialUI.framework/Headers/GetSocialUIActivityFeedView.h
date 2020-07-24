//
//  GetSocialUI
//
//  Copyright © 2020 GetSocial BV. All rights reserved.
//

#import "GetSocialUIView.h"

NS_ASSUME_NONNULL_BEGIN

/*!
 *  @abstract Defines GetSocialUIActivityFeedView class.
 */
@interface GetSocialUIActivityFeedView : GetSocialUIView

/*!
 *  @abstract Creates GetSocialUIActivityFeedView using the provided activities query.
 *
 *  @param query feeds query.
 */
+ (instancetype)viewForQuery:(GetSocialActivitiesQuery *)query;

/*!
 *  @abstract set action handler to be invoked on button clicked.
 *
 *  @param actionHandler block to be called.
 */
- (void)setActionHandler:(GetSocialActionHandler)actionHandler;

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

NS_ASSUME_NONNULL_END

@end

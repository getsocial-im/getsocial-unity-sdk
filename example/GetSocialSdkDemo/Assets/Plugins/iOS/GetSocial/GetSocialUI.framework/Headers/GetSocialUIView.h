//
//  GetSocialUI
//
//  Copyright © 2017 GetSocial BV. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "GetSocialUIPublicConstants.h"

/*!
 * @abstract Defines generic GetSocialUIView
 */
@interface GetSocialUIView : NSObject

/** @name Properties */

/*!
 *  @abstract Title of the window.
 */
@property (nonatomic, copy) NSString *windowTitle;

/** @name Methods */

/*!
 * @abstract Sets listeners to handle GetSocial view state events.
 * @param onOpen Block is called when GetSocial view is opened.
 * @param onClose Block is called when GetSocial view is closed.
 */
- (void)setHandlerForViewOpen:(void (^)())onOpen close:(void (^)())onClose;

/*!
 * @abtract Sets the UI action handler. It allows you to track actions done by users while using GetSocial UI.
 * Also, you can allow or disallow users to perform some action. To perform an action, in UI action handler call
 * GetSocialUIPendingAction block. Without calling it, action won't be invoked.
 * If you don't set a handler, all actions will be performed.
 * @param actionHandler Block is called before each action.
 */
- (void)setUiActionHandler:(GetSocialUIActionHandler)actionHandler;

/*!
 *  @abstract Shows GetSocialUIView.
 *
 *  @result YES, if view is presented without any error, otherwise NO.
 */
- (BOOL)show;

@end

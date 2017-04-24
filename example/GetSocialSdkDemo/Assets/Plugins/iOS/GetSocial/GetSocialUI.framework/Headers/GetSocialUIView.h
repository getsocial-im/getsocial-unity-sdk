//
//  GetSocialUI
//
//  Copyright © 2017 GetSocial BV. All rights reserved.
//

#import <Foundation/Foundation.h>

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
 *  @abstract Shows GetSocialUIView.
 *
 *  @result YES, if view is presented without any error, otherwise NO.
 */
- (BOOL)show;

@end

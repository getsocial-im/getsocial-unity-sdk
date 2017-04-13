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
 *  @abstract Shows GetSocialUIView.
 *
 *  @result YES, if view is presented without any error, otherwise NO.
 */
- (BOOL)show;

@end

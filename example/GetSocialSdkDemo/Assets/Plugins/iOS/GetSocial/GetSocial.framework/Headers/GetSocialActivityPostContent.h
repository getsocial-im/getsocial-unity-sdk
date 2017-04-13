//
//  GetSocial
//
//  Copyright © 2017 GetSocial BV. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIImage.h>

/*!
 * @abstract Describes a ActivityPost entity that should be posted to Activity Feeds.
 */
@interface GetSocialActivityPostContent : NSObject

NS_ASSUME_NONNULL_BEGIN

/*!
 * @abstract Post's text.
 */
@property (nonatomic, copy, nullable) NSString *text;

/*!
 * @abstract Title of button.
 */
@property (nonatomic, copy, nullable) NSString *buttonTitle;

/*!
 * @abstract Action assigned to button.
 */
@property (nonatomic, copy, nullable) NSString *buttonAction;

/*!
 * @abstract Post's image.
 */
@property (nonatomic, strong, nullable) UIImage *image;

NS_ASSUME_NONNULL_END

@end

//
//  GetSocialUI
//
//  Copyright © 2017 GetSocial BV. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GetSocial/GetSocialActivityPost.h>

/*!
 *  @typedef void(^ActivityButtonActionListener)(NSString *action, GetSocialActivityPost *post)
 *  @abstract The typedef defines the signature of a block that is called when any
 *   action triggered on Activity Feeds view.
 *
 */
typedef void(^ActivityButtonActionListener)(NSString *action, GetSocialActivityPost *post);

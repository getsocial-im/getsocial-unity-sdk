//
//  GetSocial
//
//  Copyright © 2017 GetSocial BV. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GetSocial/GetSocialConstants.h>
#import <GetSocial/GetSocialPrivateUser.h>

/*!
 * @abstract When trying to add an identity and conflict in identities happens GetSocialConflictUser instance is returned
    to check the details of the conflict user to see which user you want to proceed with.
 */
@interface GetSocialConflictUser : GetSocialPrivateUser

@end

//
//  TestProviderPlugin.h
//  GetSocial
//
//  Created by Vass Gábor on 06/10/16.
//  Copyright © 2016 GetSocial BV. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "GetSocial/GetSocialInviteChannelPlugin.h"

typedef NS_ENUM(NSInteger, ExpectedResult){
    Call_Success = 0,
    Call_Cancel = 1,
    Call_Error = 2,
    Throw_Exception = 3
};

@interface TestInviteChannelPlugin : GetSocialInviteChannelPlugin

-(instancetype)initWithExpectedResult:(ExpectedResult)expectedResult;

@property(nonatomic) GetSocialInvitePackage *invitePackage;

@end

//
// Created by Orest Savchak on 3/7/17.
// Copyright (c) 2017 GetSocial BV. All rights reserved.
//

#import <Foundation/Foundation.h>



#import "ComponentProvider.h"
#import "PushRegistrator.h"



@interface TestPushRegistrator : NSObject<SHARED_PushRegistrator>

@property (nonatomic, copy) void (^onRegister)(void);

@end

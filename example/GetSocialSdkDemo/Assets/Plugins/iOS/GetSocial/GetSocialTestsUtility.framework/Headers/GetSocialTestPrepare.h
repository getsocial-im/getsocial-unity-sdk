//
//  GetSocialTestPrepare.h
//  GetSocial
//
//  Created by Orest Savchak on 7/28/17.
//  Copyright © 2017 GetSocial BV. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface GetSocialTestPrepare : NSObject

+ (void)setUpComponentResolver;
+ (void)initWithScenario:(NSString *)scenario;

+ (Class)classForModule:(NSString *)module andType:(NSString *)type;
+ (NSString *)testCases;
+ (NSArray *)testCasesForModule:(NSString *)module andType:(NSString *)type;
+ (NSString *)compareTestsForModule:(NSString *)module andType:(NSString *)type withResults:(NSArray *)results;

@end

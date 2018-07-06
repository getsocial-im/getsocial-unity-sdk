//
//  ComponentResolverFactory.h
//  GetSocial
//
//  Created by Vass Gábor on 18/10/2016.
//  Copyright © 2016 GetSocial BV. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "ComponentResolver.h"

@interface ComponentResolverFactory : NSObject

/**
 * Creates a new ComponentResolver instance
 **/
+ (SHARED_ComponentResolver *)createTestComponentResolver;

/**
 *  Sets the scenario to be used in mock framework
 *  @param scenario Name of scenario to be used
 *  @param componentResolver Used component resolver instance
 */
+ (void)setMockingScenario:(NSString *)scenario componentResolver:(SHARED_ComponentResolver *)componentResolver;

@end

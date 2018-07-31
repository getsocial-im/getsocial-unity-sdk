//
//  MockJsonProvider.h
//  GetSocial
//
//  Created by Vass Gábor on 18/10/2016.
//  Copyright © 2016 GetSocial BV. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "ComponentProvider.h"
#import "LocalFileProvider.h"

@interface IntegrationTestFileProvider : NSObject<SHARED_LocalFileProvider>

@end

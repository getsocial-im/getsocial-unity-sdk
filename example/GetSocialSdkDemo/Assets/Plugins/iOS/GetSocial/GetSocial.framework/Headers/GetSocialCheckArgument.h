//
//  GetSocial
//
//  Copyright © 2017 GetSocial BV. All rights reserved.
//

#import <Foundation/Foundation.h>

#define CheckArgumentNotNil(Argument, Name) checkNotNil(Argument, Name);
#define CheckState(State, Name) checkState(State, Name);

void checkNotNil(NSObject *object, NSString *paramName);
void checkState(BOOL state, NSString *paramName);

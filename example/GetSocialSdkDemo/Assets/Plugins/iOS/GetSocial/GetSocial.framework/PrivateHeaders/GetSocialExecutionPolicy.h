//
//  GetSocial
//
//  Copyright © 2017 GetSocial BV. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GetSocial/GetSocialConstants.h>

NS_ASSUME_NONNULL_BEGIN

typedef void(^GetSocialRunnable)();
typedef _Nullable id(^GetSocialCallable)();

@protocol GetSocialExecutionPolicyDelegate <NSObject>

- (void)handleError:(NSError *)error withCallback:(nullable GetSocialFailureCallback)callback;

@end

@interface GetSocialExecutionPolicy : NSObject

- (void)setErrorHandler:(nullable GetSocialGlobalErrorHandler)errorHandler;

- (void)removeErrorHandler;

- (instancetype)initWithDelegate:(id<GetSocialExecutionPolicyDelegate>)delegate;

/**
 * Run the single synchronous operation.
 * @param block operation to be executed
 * @return YES if operation was successfully, NO otherwise
 */
- (BOOL)run:(GetSocialRunnable)block;

/**
 * Run the single asynchronous operation with callback.
 * @param block operation to be executed
 * @param callback object to be notified with error, if operation fails
 */
- (void)run:(GetSocialRunnable)block withCallback:(nullable GetSocialFailureCallback)callback;

/**
 * Call the single synchronous operation with return.
 * @param block operation to get result from
 * @param fallback default value
 * @return result of operation if it was successful, default value otherwise
 */
- (nullable id)call:(GetSocialCallable)block withReturn:(nullable id)fallback;

NS_ASSUME_NONNULL_END

@end

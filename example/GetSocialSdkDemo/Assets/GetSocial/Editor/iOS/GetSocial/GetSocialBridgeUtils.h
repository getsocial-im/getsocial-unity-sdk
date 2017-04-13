#import <Foundation/Foundation.h>
#import <UIKit/UIImage.h>

@interface GetSocialBridgeUtils : NSObject

+ (NSString *)createNSStringFrom:(const char *)cstring;

+ (char *)createCStringFrom:(NSString *)string;

+ (NSArray *)createNSArray:(int)count values:(const char **)values;

+ (char *)cStringCopy:(const char *)string;

+ (NSString *)encodeToBase64String:(UIImage *)image;

+ (UIImage *)decodeUIImageFrom:(NSString *)base64String;

@end

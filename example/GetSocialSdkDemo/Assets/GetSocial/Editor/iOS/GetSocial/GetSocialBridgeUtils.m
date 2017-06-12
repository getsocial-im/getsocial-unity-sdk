#import "GetSocialBridgeUtils.h"

@implementation GetSocialBridgeUtils

// Converts C style string to NSString
+ (NSString *)createNSStringFrom:(const char *)cstring
{
    return [NSString stringWithUTF8String:(cstring ?: "")];
}

+ (char *)cStringCopy:(const char *)string
{
    char *res = (char *) malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
}

+ (char *)createCStringFrom:(NSString *)string
{
    if (!string)
    {
        string = @"";
    }
    return [self cStringCopy:[string UTF8String]];
}

+ (UIImage *)decodeUIImageFrom:(NSString *)base64String
{
    NSData *data = [[NSData alloc] initWithBase64EncodedString:base64String options:NSDataBase64DecodingIgnoreUnknownCharacters];
    return [UIImage imageWithData:data];
}

@end

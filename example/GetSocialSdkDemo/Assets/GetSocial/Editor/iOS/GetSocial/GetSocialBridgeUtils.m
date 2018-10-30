#import "GetSocialBridgeUtils.h"

@implementation GetSocialBridgeUtils

+ (NSDictionary *)createDictionaryFromCString:(const char *)cstring
{
    return [self createDictionaryFromNSString:[self createNSStringFrom:cstring]];
}

+ (NSArray *)createArrayFromCString:(const char *)cstring
{
    return [self createArrayFromNSString:[self createNSStringFrom:cstring]];
}
// Converts C style string to NSString
+ (NSString *)createNSStringFrom:(const char *)cstring
{
    if (cstring == NULL) 
    {
        return nil;
    }
    return [NSString stringWithUTF8String:cstring];
}

+ (NSDictionary *)createDictionaryFromNSString:(NSString *)jsonString
{
    NSError *e = nil;
    NSDictionary *dictionary = [NSJSONSerialization JSONObjectWithData:[jsonString dataUsingEncoding:NSUTF8StringEncoding] options:NSJSONReadingMutableContainers error:&e];
    if (dictionary != nil)
    {
        NSMutableDictionary *prunedDict = [NSMutableDictionary dictionary];
        [dictionary enumerateKeysAndObjectsUsingBlock:^(NSString *key, id obj, BOOL *stop) {
            if (![obj isKindOfClass:[NSNull class]]) {
                prunedDict[key] = obj;
            }
        }];
        return prunedDict;
    }
    return dictionary;
}

+ (NSArray *)createArrayFromNSString:(NSString *)jsonList
{
    NSError* localError = nil;
    NSArray *array = [NSJSONSerialization JSONObjectWithData:[jsonList dataUsingEncoding:NSUTF8StringEncoding] options:NSJSONReadingMutableContainers error:&localError];
    
    return array;
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
        return NULL;
    }
    return [self cStringCopy:[string UTF8String]];
}

+ (UIImage *)decodeUIImageFrom:(NSString *)base64String
{
    NSData *data = [[NSData alloc] initWithBase64EncodedString:base64String options:NSDataBase64DecodingIgnoreUnknownCharacters];
    return [UIImage imageWithData:data];
}

+ (NSData *)decodeNSDataFrom:(NSString *)base64String
{
    if (base64String.length == 0) {
        return nil;
    }
    NSData *data = [[NSData alloc] initWithBase64EncodedString:base64String options:NSDataBase64DecodingIgnoreUnknownCharacters];
    return data;
}


@end

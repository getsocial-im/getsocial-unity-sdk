//
// Created by Orest Savchak on 6/2/17.
//

#import "NSDictionary+GetSocial.h"
#import "Json.h"

@implementation NSDictionary (GetSocial)

- (NSDictionary<id, id> *)gs_map:(id (^)(id))block
{
    NSMutableDictionary *dictionary = [NSMutableDictionary dictionaryWithCapacity:self.count];
    [self enumerateKeysAndObjectsUsingBlock:^(id  _Nonnull key, id  _Nonnull obj, BOOL * _Nonnull stop) {
        dictionary[key] = block(obj);
    }];
    return dictionary;
}

- (NSString *)toJson
{
    NSError *writeError = nil;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:self options:nil error:&writeError];

    NSString *jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];

#if DEBUG
    NSLog(@"JSON Output: %@", jsonString);
#endif

    return jsonString;
}

- (NSString *)toJsonDictionaryString
{
    return [[self gs_map:^id(id<Json> value) {
        if ([value respondsToSelector:@selector(toJsonDictionary)]) {
            return [value toJsonDictionary];
        } else if ([value isKindOfClass:[NSString class] ]) {
            return value;
        } else if ([value isKindOfClass:[NSDictionary class] ]) {
            return value;
        } else {
            return value;
        }
    }] toJson];
}
- (NSString *)toJsonString
{
    return [self toJsonDictionaryString];
}

@end

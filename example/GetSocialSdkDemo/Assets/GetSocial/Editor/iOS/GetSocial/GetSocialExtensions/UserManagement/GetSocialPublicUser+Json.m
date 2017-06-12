//
// Created by Orest Savchak on 6/2/17.
//

#import "GetSocialPublicUser+Json.h"


@implementation GetSocialPublicUser (Json)

- (NSMutableDictionary *)toJsonDictionary
{
    NSMutableDictionary *dictionary = [[NSMutableDictionary alloc] init];
    [dictionary setValue:self.userId forKey:@"Id"];
    [dictionary setValue:self.displayName forKey:@"DisplayName"];
    [dictionary setValue:self.avatarUrl forKey:@"AvatarUrl"];
    [dictionary setValue:[self.authIdentities mutableCopy] forKey:@"Identities"];
    return dictionary;
}

@end

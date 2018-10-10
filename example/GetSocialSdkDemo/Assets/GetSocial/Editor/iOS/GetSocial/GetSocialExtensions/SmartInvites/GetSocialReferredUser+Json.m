//
//  GetSocialReferredUser+Json.m
//  Unity-iPhone
//
//

#import "GetSocialReferredUser+Json.h"
#import "GetSocialPublicUser+Json.h"

@implementation GetSocialReferredUser(Json)

- (NSMutableDictionary *)toJsonDictionary
{
    NSMutableDictionary *dictionary = [super toJsonDictionary];
    dictionary[@"InstallationDate"] = @(self.installationDate);
    dictionary[@"InstallationChannel"] = self.installationChannel;
    dictionary[@"InstallPlatform"] = self.installationPlatform;
    dictionary[@"Reinstall"] = @(self.isReinstall);
    dictionary[@"InstallSuspicious"] = @(self.isInstallSuspicious);
    return dictionary;
}

@end

#include <GetSocialTestsUtility/GetSocialTestPrepare.h>
#include <GetSocial/GetSocial.h>
#include <GetSocial/GetSocialAccessHelper.h>
#include "GetSocialBridgeUtils.h"
#include "GetSocialJsonUtils.h"
#include "NSObject+Json.h"

#pragma clang diagnostic push
#pragma ide diagnostic ignored "OCUnusedGlobalDeclarationInspection"
extern "C" {
NS_ASSUME_NONNULL_BEGIN

    typedef id(^GetSocialEntityConverter)(NSString *json);

    NSDictionary *converters = @{
        @"activities": @{
            @"ActivityPostContent": ^GetSocialActivityPostContent *(NSString *json) {
                return [GetSocialJsonUtils deserializeActivityContent:json];
            },
            @"ActivitiesQuery": ^GetSocialActivitiesQuery *(NSString *json) {
                return [GetSocialJsonUtils deserializeActivitiesQuery:json];
            },
            @"GetSocialUsersQuery": ^GetSocialUsersQuery *(NSString *json) {
                return [GetSocialJsonUtils deserializeUsersQuery:json];
            },
            @"GetSocialNotificationsQuery": ^GetSocialNotificationsQuery *(NSString *json) {
                return [GetSocialJsonUtils deserializeNotificationsQuery:json];
            },
            @"GetSocialNotificationsCountQuery": ^GetSocialNotificationsCountQuery *(NSString *json) {
                return [GetSocialJsonUtils deserializeNotificationsCountQuery:json];
            },
            @"GetSocialMutableInviteContent": ^GetSocialMutableInviteContent *(NSString *json) {
                return [GetSocialJsonUtils deserializeCustomInviteContent:json];
            },
            @"GetSocialAuthIdentity": ^GetSocialAuthIdentity *(NSString *json) {
                return [GetSocialJsonUtils deserializeIdentity:json];
            }
        }
    };

    void _gs_setUpComponents()
    {
        [GetSocialTestPrepare setUpComponentResolver];
    }
        
    void _gs_initWithScenario(const char * scenario)
    {
        NSString *scenarionStr = [GetSocialBridgeUtils createNSStringFrom:scenario];

        [GetSocialTestPrepare initWithScenario:scenarionStr];
    }
        
    char * _gs_getTestCases()
    {
        return [GetSocialBridgeUtils createCStringFrom:[GetSocialTestPrepare testCases]];
    }

    char * _gs_getTestCasesFor(const char *module, const char *type)
    {
        NSString *typeStr = [GetSocialBridgeUtils createNSStringFrom:type];
        NSString *moduleStr = [GetSocialBridgeUtils createNSStringFrom:module];
        
        NSArray *testCases = [GetSocialTestPrepare testCasesForModule:moduleStr andType:typeStr];
        
        return [GetSocialBridgeUtils createCStringFrom:testCases.toJsonString];
    }

    char * _gs_compareTests(const char *module, const char *type, const char *results)
    {
        NSString *typeStr = [GetSocialBridgeUtils createNSStringFrom:type];
        NSString *moduleStr = [GetSocialBridgeUtils createNSStringFrom:module];
        NSString *resultsStr = [GetSocialBridgeUtils createNSStringFrom:results];
        NSArray *resultsArray = [GetSocialJsonUtils deserializeList:resultsStr];
        GetSocialEntityConverter converter = converters[moduleStr][typeStr];

        NSMutableArray *testCases = [@[] mutableCopy];
        for (NSString *result : resultsArray) {
            [testCases addObject:converter(result)];
        }
        
        return [GetSocialBridgeUtils createCStringFrom:[GetSocialTestPrepare compareTestsForModule:moduleStr andType:typeStr withResults:testCases]];
    }

#pragma mark 
NS_ASSUME_NONNULL_END
}

#pragma clang diagnostic pop

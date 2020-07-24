using System;
using System.Collections.Generic;
using System.Linq;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public class UserIdList
    {
        [JsonSerializationKey("ids")] internal List<string> UserIds { get; set; }

        [JsonSerializationKey("provider")] internal string ProviderId { get; set; }

        public static UserIdList Create(List<string> userIds)
        {
            return new UserIdList {UserIds = userIds, ProviderId = null};
        }
        
        public static UserIdList Create(params string[] userIds)
        {
            return new UserIdList {UserIds = userIds.ToList(), ProviderId = null};
        }
        
        public static UserIdList CreateWithProvider(string provider, List<string> userIds)
        {
            return new UserIdList {UserIds = userIds, ProviderId = provider};
        }
        
        public static UserIdList CreateWithProvider(string provider, params string[] userIds)
        {
            return new UserIdList {UserIds = userIds.ToList(), ProviderId = provider};
        }

        internal List<string> AsString()
        {
            if (ProviderId == null)
            {
                return UserIds;
            }
            return UserIds.ConvertAll(x => ProviderId + ":" + x);
        }

    }
}
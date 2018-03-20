using System;

#if UNITY_ANDROID
using UnityEngine;
#endif

#if UNITY_IOS
using System.Collections.Generic;
using GetSocialSdk.MiniJSON;
#endif

namespace GetSocialSdk.Core
{
    public class UsersQuery : IGetSocialBridgeObject<UsersQuery>
    {
        private const int DefaultLimit = 20;
        
#pragma warning disable 414  
        private readonly string _query;
        private int _limit;
#pragma warning restore 414

        private UsersQuery(string query)
        {
            _query = query;
            _limit = DefaultLimit;
        }

        public static UsersQuery UsersByDisplayName(string query)
        {
            return new UsersQuery(query);
        }

        public UsersQuery WithLimit(int limit)
        {
            _limit = limit;
            return this;
        }
        
#if UNITY_IOS
        public string ToJson()
        {
            var json = new Dictionary<string, object>
            {
                {"Query", _query},
                {"Limit", _limit}
            };
            return GSJson.Serialize(json);
        }

        public UsersQuery ParseFromJson(Dictionary<string, object> json)
        {
            throw new NotImplementedException("Users Query is never sent from native to unity");
        }
#elif UNITY_ANDROID
        public AndroidJavaObject ToAJO()
        {
            return new AndroidJavaClass("im.getsocial.sdk.usermanagement.UsersQuery")
                .CallStaticAJO("usersByDisplayName", _query)
                .CallStaticAJO("withLimit", _limit);
        }
        public UsersQuery ParseFromAJO(AndroidJavaObject ajo)
        {
            throw new NotImplementedException();
        }
#endif
    }
}
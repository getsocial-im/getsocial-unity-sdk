using System.Collections.Generic;
using GetSocialSdk.MiniJSON;
using UnityEngine;

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
            var jsonDic = new Dictionary<string, object>
            {
                {"Query", _query},
                {"Limit", _limit}
            };
            return GSJson.Serialize(jsonDic);
        }

        public UsersQuery ParseFromJson(Dictionary<string, object> json)
        {
            throw new System.NotImplementedException("Users Query is never sent from native to unity");
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
            throw new System.NotImplementedException();
        }
#endif
    }
}
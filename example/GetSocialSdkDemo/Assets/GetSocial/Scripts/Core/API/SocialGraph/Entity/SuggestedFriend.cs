using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GetSocialSdk.Core
{

    /// <summary>
    /// Suggested friend entity.
    /// </summary>
    public class SuggestedFriend : PublicUser, IGetSocialBridgeObject<SuggestedFriend>
    {

        public int MutualFriendsCount { get; private set; }

        public override string ToString()
        {
            return string.Format("[SuggestedFriend: Id={0}, DisplayName={1}, Identities={2}, MutualFriendsCount={3}]", Id, DisplayName, Identities.ToDebugString(), MutualFriendsCount);
        }

#if UNITY_ANDROID
        public new UnityEngine.AndroidJavaObject ToAJO()
        {
            throw new System.NotImplementedException("SuggestedFriend is never passed to Android");
        }

        public new SuggestedFriend ParseFromAJO(UnityEngine.AndroidJavaObject ajo)
        {
            using (ajo)
            {
                base.ParseFromAJO(ajo);                 
                MutualFriendsCount = ajo.CallInt("getMutualFriendsCount");
            }
            return this;
        }
#elif UNITY_IOS
        public new string ToJson()
        {
            throw new System.NotImplementedException("SuggestedFriend is never passed to iOS");
        }

        public new SuggestedFriend ParseFromJson(Dictionary<string, object> jsonDic)
        {
            base.ParseFromJson(jsonDic);

            var friendsCount = jsonDic["MutualFriendsCount"];
            
            if (friendsCount != null)
            {
                MutualFriendsCount = (int)(long) friendsCount;
            }

            return this;
        }
#endif
    }
}
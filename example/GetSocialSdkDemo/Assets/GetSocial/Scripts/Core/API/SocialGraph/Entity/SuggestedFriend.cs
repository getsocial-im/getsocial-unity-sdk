using System;

#if UNITY_ANDROID
using UnityEngine;
#endif

#if UNITY_IOS
using System.Collections.Generic;
#endif

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
        public new AndroidJavaObject ToAJO()
        {
            throw new NotImplementedException("SuggestedFriend is never passed to Android");
        }

        public new SuggestedFriend ParseFromAJO(AndroidJavaObject ajo)
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
            throw new NotImplementedException("SuggestedFriend is never passed to iOS");
        }

        public new SuggestedFriend ParseFromJson(Dictionary<string, object> json)
        {
            base.ParseFromJson(json);

            var friendsCount = json["MutualFriendsCount"];
            
            if (friendsCount != null)
            {
                MutualFriendsCount = (int)(long) friendsCount;
            }

            return this;
        }
#endif
    }
}
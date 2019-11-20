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
    public sealed class ReferralUsersQuery : IConvertableToNative
    {
        private string _eventName = "";
        private int _offset;
        private int _limit = 50;

        ReferralUsersQuery(string eventName)
        {
            _eventName = eventName;
        }

        /// <summary>
        /// Creates new query to filter all the users.
        /// </summary>
        /// <returns>New query.</returns>
        public static ReferralUsersQuery AllUsers()
        {
            return new ReferralUsersQuery("");
        }

        /// <summary>
        /// Creates new query to filter users for the specified event.
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns>New query.</returns>
        public static ReferralUsersQuery UsersForEvent(string eventName)
        {
            return new ReferralUsersQuery(eventName);
        }

        public string GetEventName()
        {
            return _eventName;
        }

        public int GetOffset()
        {
            return _offset;
        }

        public ReferralUsersQuery SetOffset(int offset)
        {
            _offset = offset;
            return this;
        }

        public int GetLimit()
        {
            return _limit;
        }

        public ReferralUsersQuery SetLimit(int limit)
        {
            _limit = limit;
            return this;
        }

        override public string ToString()
        {
            return "ReferralUsersQuery(event: " + _eventName + ", offset: " + _offset + ", limit: " + _limit + ")";
        }

#if UNITY_ANDROID
        public AndroidJavaObject ToAjo()
        {
            var usersQueryClass = new AndroidJavaClass("im.getsocial.sdk.invites.ReferralUsersQuery");
            var usersQueryInstance = usersQueryClass.CallStaticAJO("usersForEvent", _eventName);
       
            usersQueryInstance.CallAJO("setLimit", _limit);
            usersQueryInstance.CallAJO("setOffset", _offset);

            return usersQueryInstance;
        }

#elif UNITY_IOS

        public string ToJson()
        {
            var json = new Dictionary<string, object>
            {
                {"EventName", _eventName},
                {"Offset", _offset},
                {"Limit", _limit},
            };
            return GSJson.Serialize(json);
        }
#endif

    }
}
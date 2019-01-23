using System;
using UnityEngine;

using System.Collections.Generic;
using System.Linq;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public sealed class NotificationsCountQuery : IConvertableToNative
    {
        private static readonly string[] AllTypes = new string[0];

#pragma warning disable 414        
        private readonly string[] _statuses;
        private string[] _types = AllTypes;
        private string[] _actions = new string[0];
#pragma warning restore 414
        private NotificationsCountQuery(params string[] statuses)
        {
            _statuses = statuses;
        }

        [Obsolete("Use WithStatuses()")]
        public static NotificationsCountQuery ReadAndUnread() 
        {
            return new NotificationsCountQuery(NotificationStatus.Read, NotificationStatus.Unread);
        }

        [Obsolete("Use WithStatuses()")]
        public static NotificationsCountQuery Read() 
        {
            return new NotificationsCountQuery(NotificationStatus.Read);
        }

        [Obsolete("Use WithStatuses()")]
        public static NotificationsCountQuery Unread() 
        {
            return new NotificationsCountQuery(NotificationStatus.Unread);
        }

        /// <summary>
        /// All possible statuses are listed in <see cref="NotificationStatus"/>.
        /// </summary>
        /// <param name="statuses">One or few statuses to filter by.</param>
        /// <returns>new query</returns>
        public static NotificationsCountQuery WithStatuses(params string[] statuses)
        {
            return new NotificationsCountQuery(statuses);
        }

        /// <summary>
        /// Notifications with any status.
        /// </summary>
        /// <returns>new query</returns>
        public static NotificationsCountQuery WithAllStatuses()
        {
            return new NotificationsCountQuery();
        }

        public NotificationsCountQuery OfAllTypes()
        {
            _types = AllTypes;
            return this;
        }
        public NotificationsCountQuery OfTypes(params string[] types)
        {
            _types = types;
            return this;
        }

        public NotificationsCountQuery WithActions(params string[] actions)
        {
            _actions = actions;
            return this;
        }

#if UNITY_ANDROID
        public AndroidJavaObject ToAjo()
        {
            var query = new AndroidJavaClass("im.getsocial.sdk.pushnotifications.NotificationsCountQuery")
                .CallStaticAJO("withStatuses", _statuses.ToList().ToJavaStringArray());
                
            if (_types.Length > 0)
            {
                query.CallAJO("ofTypes", _types.ToList().ToJavaStringArray() );    
            }
            
            if (_actions.Length > 0)
            {
                query.CallAJO("withActions", _actions.ToList().ToJavaStringArray() );    
            }
            
            return query;
        }


#elif UNITY_IOS
        public string ToJson()
        {
            var json = new Dictionary<string, object>
            {
                {"Statuses", _statuses},
                {"Types", _types},
                {"Actions", _actions}
            };
            return GSJson.Serialize(json);
        }
#endif
    }
}
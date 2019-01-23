using System;
using System.Collections.Generic;
using System.Linq;
using GetSocialSdk.MiniJSON;
using UnityEngine;

namespace GetSocialSdk.Core
{
    public sealed class NotificationsQuery : IConvertableToNative
    {
        /// <summary>
        /// Set of filtering options for <see cref="WithFilter(Filter, string)"/> method
        /// </summary>
        public enum Filter
        {
            /// <summary>
            /// No filter will be applied to the query.
            /// </summary>
            NoFilter,

            /// <summary>
            /// Query will provide all older notifications.
            /// </summary>
            Older,

            /// <summary>
            /// Query will provide all newer notifications.
            /// </summary>
            Newer
        }
        
        private static readonly string[] AllTypes = new string[0];

#pragma warning disable 414        
        private readonly string[] _statuses;
        private string[] _types = AllTypes;  
        private Filter _filter = Filter.NoFilter;
        private string _notificationId;
        private int _limit;
        private string[] _actions = new string[0];
#pragma warning restore 414
        
        private NotificationsQuery(params string[] statuses)
        {
            _statuses = statuses;
        }

        [Obsolete("Use WithStatuses()")]
        public static NotificationsQuery ReadAndUnread() 
        {
            return new NotificationsQuery(NotificationStatus.Read, NotificationStatus.Unread);
        }

        [Obsolete("Use WithStatuses()")]
        public static NotificationsQuery Read() 
        {
            return new NotificationsQuery(NotificationStatus.Read);
        }

        [Obsolete("Use WithStatuses()")]
        public static NotificationsQuery Unread() 
        {
            return new NotificationsQuery(NotificationStatus.Unread);
        }

        public static NotificationsQuery WithStatuses(params string[] statuses)
        {
            return new NotificationsQuery(statuses);
        }

        public static NotificationsQuery WithAllStatuses()
        {
            return new NotificationsQuery();
        }

        public NotificationsQuery OfAllTypes() {
            _types = AllTypes;
            return this;
        }

        public NotificationsQuery OfTypes(params string[] types)
        {
            _types = types;
            return this;
        }

        public NotificationsQuery WithLimit(int limit)
        {
            _limit = limit;
            return this;
        }

        public NotificationsQuery WithFilter(Filter filter, string notificationId)
        {
            _filter = filter;
            _notificationId = notificationId;
            return this;
        }

        public NotificationsQuery WithActions(params string[] actions)
        {
            _actions = actions;
            return this;
        }
        
#if UNITY_ANDROID
        public AndroidJavaObject ToAjo()
        {
            var query = new AndroidJavaClass("im.getsocial.sdk.pushnotifications.NotificationsQuery")
                .CallStaticAJO("withStatuses", _statuses.ToList().ToJavaStringArray());
            
            query.CallAJO("withLimit", _limit);
            
            if (_types.Length > 0)
            {
                query.CallAJO("ofTypes", _types.ToList().ToJavaStringArray() );    
            }
            
            if (_filter != Filter.NoFilter)
            {
                query.CallAJO("withFilter", _filter.ToAndroidJavaObject(), _notificationId);
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
                {"Limit", _limit},
                {"Filter", (int)_filter},
                {"FilteringNotificationId", _notificationId},
                {"Actions", _actions}
            };
            return GSJson.Serialize(json);
        }
#endif
    }
}
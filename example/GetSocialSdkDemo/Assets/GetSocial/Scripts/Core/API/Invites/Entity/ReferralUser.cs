using System;
using System.Collections.Generic;
#if UNITY_ANDROID
using UnityEngine;
#endif

namespace GetSocialSdk.Core
{
    public sealed class ReferralUser : PublicUser, IConvertableFromNative<ReferralUser>
    {

        /// <summary>
        /// Date of event.
        /// </summary>
        /// <value>Date of event.</value>
        public DateTime EventDate { get; private set; }
        
        /// <summary>
        /// Event name.
        /// </summary>
        /// <value>Event name.</value>
        public string Event { get; private set; }

        /// <summary>
        /// Event data.
        /// </summary>
        /// <value>Event data.</value>
        public Dictionary<string, string> EventData { get; private set; }

        public ReferralUser()
        {
            
        }
        
        internal ReferralUser(Dictionary<string, string> publicProperties, string id, string displayName, string avatarUrl, Dictionary<string, string> identities, DateTime eventDate, string eventName, Dictionary<string, string> eventData) : base(publicProperties, id, displayName, avatarUrl, identities)
        {
            Event = eventName;
            EventDate = EventDate;
            EventData = EventData;
        }

#if UNITY_ANDROID
        public new ReferralUser ParseFromAJO(AndroidJavaObject ajo)
        {
            if (ajo.IsJavaNull())
            {
                return null;
            }

            using (ajo)
            {
                base.ParseFromAJO(ajo);                 
                EventDate = DateUtils.FromUnixTime(ajo.CallLong("getEventDate"));
                Event = ajo.CallStr("getEvent");
                EventData = ajo.CallAJO("getEventData").FromJavaHashMap();
            }
            return this;
        }
#elif UNITY_IOS
        public new ReferralUser ParseFromJson(Dictionary<string, object> json)
        {
            base.ParseFromJson(json);
            EventDate = DateUtils.FromUnixTime((long) json["EventDate"]);
            Event = (string) json["Event"];
            EventData = (json["EventData"] as Dictionary<string, object>).ToStrStrDict();
            return this;
        }
#endif
    }
}
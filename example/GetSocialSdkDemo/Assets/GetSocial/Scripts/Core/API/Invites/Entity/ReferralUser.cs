using System;
using System.Collections.Generic;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public sealed class ReferralUser : User
    {

        /// <summary>
        /// Date of event.
        /// </summary>
        /// <value>Date of event.</value>
        [JsonSerializationKey("eventDate")]
        public long EventDate { get; internal set; }

        /// <summary>
        /// Event name.
        /// </summary>
        /// <value>Event name.</value>
        [JsonSerializationKey("event")]
        public string Event { get; internal set; }

        /// <summary>
        /// Event data.
        /// </summary>
        /// <value>Event data.</value>
        [JsonSerializationKey("eventData")]
        public Dictionary<string, string> EventData { get; internal set; }

        public override string ToString()
        {
            return $"{base.ToString()}, EventDate: {EventDate}, Event: {Event}, Data: {EventData.ToDebugString()}";
        }
    }
}
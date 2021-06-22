#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR

using System;
using System.Collections.Generic;

namespace GetSocialSdk.Core
{
    internal class AnalyticsEvent
    {
        public string Id { get; private set; }
        public string Name;
        public Dictionary<string, string> Properties;
        public DateTime CreatedAt = DateTime.Now;
        public bool IsCustom = false;

        public AnalyticsEvent()
        {
            Id = Guid.NewGuid().ToString();
        }

        public AnalyticsEvent ShiftTime(long diff)
        {
            return new AnalyticsEvent
            {
                Id = Id,
                Name = Name, 
                Properties = Properties,
                CreatedAt = CreatedAt.AddSeconds(diff),
                IsCustom = IsCustom
            };
        }
    }
}
#endif

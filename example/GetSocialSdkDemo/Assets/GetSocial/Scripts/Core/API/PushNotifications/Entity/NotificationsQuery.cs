using System.Collections.Generic;
using System.Linq;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public sealed class NotificationsQuery
    {        
        private static readonly string[] AllTypes = new string[0];

#pragma warning disable 414        
        [JsonSerializationKey("statuses")]
        internal readonly List<string> Statuses;

        [JsonSerializationKey("types")] 
        internal List<string> Types = new List<string>();

        [JsonSerializationKey("actions")]
        internal List<string> Actions = new List<string>();
#pragma warning restore 414
        
        private NotificationsQuery(List<string> statuses)
        {
            Statuses = statuses;
        }

        public static NotificationsQuery WithStatuses(params string[] statuses)
        {
            return new NotificationsQuery(statuses.ToList());
        }

        public static NotificationsQuery WithStatuses(List<string> statuses)
        {
            return new NotificationsQuery(statuses);
        }

        public static NotificationsQuery WithAllStatuses()
        {
            return new NotificationsQuery(new List<string>());
        }

        public NotificationsQuery OfAllTypes() {
            Types = AllTypes.ToList();
            return this;
        }

        public NotificationsQuery OfTypes(params string[] types)
        {
            Types = types.ToList();
            return this;
        }

        public NotificationsQuery WithActions(params string[] actions)
        {
            Actions = actions.ToList();
            return this;
        }

        public NotificationsQuery WithActions(List<string> actions)
        {
            Actions = actions;
            return this;
        }

        public NotificationsQuery OfTypes(List<string> types)
        {
            Types = types;
            return this;
        }
    }
}
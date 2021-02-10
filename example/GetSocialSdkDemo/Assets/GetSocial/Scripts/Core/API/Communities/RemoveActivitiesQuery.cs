using System.Collections.Generic;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Builder for a query to remove group members.
    /// </summary>
    public sealed class RemoveActivitiesQuery
    {
        [JsonSerializationKey("ids")]
        internal List<string> Ids { get; set; }

        internal RemoveActivitiesQuery(List<string> activityIds)
        {
            this.Ids = activityIds;
        }

        public static RemoveActivitiesQuery ActivityIds(List<string> activityIds)
        {
            return new RemoveActivitiesQuery(activityIds);
        }
    }
}
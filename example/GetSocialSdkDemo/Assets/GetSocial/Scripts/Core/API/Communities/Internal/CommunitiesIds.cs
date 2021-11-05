using System.Collections.Generic;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    internal class CommunitiesIds
    {
        [JsonSerializationKey("ids")]
        internal List<string> Ids;
        [JsonSerializationKey("type")] 
        internal CommunitiesEntityType? Type;

        private CommunitiesIds(CommunitiesEntityType? type, string id)
        {
            Type = type;
            Ids = new List<string> { id };
        }

        private CommunitiesIds(CommunitiesEntityType? type, List<string> ids)
        {
            Ids = ids;
            Type = type;
        }

        public static CommunitiesIds Topic(string topic)
        {
            return new CommunitiesIds(CommunitiesEntityType.Topic, topic);
        }

        public static CommunitiesIds Topics(List<string> topics)
        {
            return new CommunitiesIds(CommunitiesEntityType.Topic, topics);
        }

        public static CommunitiesIds Groups(List<string> groups)
        {
            return new CommunitiesIds(CommunitiesEntityType.Group, groups);
        }

        public static CommunitiesIds User(UserId userId)
        {
            return new CommunitiesIds(CommunitiesEntityType.User, userId.AsString());
        }

        public static CommunitiesIds Users(UserIdList userIdList)
        {
            return new CommunitiesIds(CommunitiesEntityType.User, userIdList.AsString());
        }

        public static CommunitiesIds Activity(string activityId)
        {
            return new CommunitiesIds(CommunitiesEntityType.Activity,  activityId);
        }

        public static CommunitiesIds App(string id)
        {
            return new CommunitiesIds(CommunitiesEntityType.App, id);
        }
        
        public static CommunitiesIds Group(string id)
        {
            return new CommunitiesIds(CommunitiesEntityType.Group, id);
        }

        public static CommunitiesIds Everywhere()
        {
            return new CommunitiesIds(null, new List<string>());
        }
    }
}
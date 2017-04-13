using System.Collections.Generic;
using System.Linq;
using GetSocialSdk.MiniJSON;

#if UNITY_IOS

namespace GetSocialSdk.Core
{
    public static class GSJsonUtils
    {
        public static InviteChannel[] ParseChannelsList(string json)
        {
            var result = new List<InviteChannel>();

            if (string.IsNullOrEmpty(json))
            {
                // return immediately in case of unexpected empty/null json
                GetSocialDebugLogger.E("ParseChannelsList is parsing null or empty json string");
                return result.ToArray();
            }

            var channels = GSJson.Deserialize(json) as List<object>;

            foreach (var channel in channels)
            {
                var currentChannel = new InviteChannel().ParseFromJson(channel as Dictionary<string, object>);
                result.Add(currentChannel);
            }

            return result.ToArray();
        }

        public static List<ActivityPost> ParseActivityPostsList(string json)
        {
            var result = new List<ActivityPost>();

            if (string.IsNullOrEmpty(json))
            {
                // return immediately in case of unexpected empty/null json
                GetSocialDebugLogger.E("ParseActivityPostsList is parsing null or empty json string");
                return result;
            }

            var posts = GSJson.Deserialize(json) as List<object>;

            foreach (var post in posts)
            {
                var currentPost = new ActivityPost().ParseFromJson(post as Dictionary<string, object>);
                result.Add(currentPost);
            }

            return result;
        }

        public static List<PublicUser> ParseUsersList(string json)
        {
            var result = new List<PublicUser>();

            if (string.IsNullOrEmpty(json))
            {
                // return immediately in case of unexpected empty/null json
                GetSocialDebugLogger.E("ParseUsersList is parsing null or empty json string");
                return result;
            }

            var users = GSJson.Deserialize(json) as List<object>;

            foreach (var user in users)
            {
                var parsedUser = new PublicUser().ParseFromJson(user as Dictionary<string, object>);
                result.Add(parsedUser);
            }

            return result;
        }

        public static Dictionary<string, string> ParseDictionary(string json)
        {
            var parsedDic = GSJson.Deserialize(json) as Dictionary<string, object>;

            var result = new Dictionary<string, string>();

            foreach (var key in parsedDic.Keys)
            {
                result[key] = (string) parsedDic[key];
            }

            return result;
        }

        public static Dictionary<string, object> ToDict(this string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                GetSocialDebugLogger.W("Trying to deserialize empty or null string as dictionary");
                return new Dictionary<string, object>();
            }

            return GSJson.Deserialize(json) as Dictionary<string, object>;
        }

        public static Dictionary<string, string> ToStrStrDict(this Dictionary<string, object> json)
        {
            if (json == null || json.Count == 0)
            {
                return new Dictionary<string, string>();
            }

            return json.ToDictionary(entry => entry.Key, entry => entry.Value as string);
        }
    }
}

#endif
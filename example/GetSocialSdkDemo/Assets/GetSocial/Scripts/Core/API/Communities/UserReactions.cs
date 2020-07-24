using System.Collections.Generic;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public class UserReactions
    {
        [JsonSerializationKey("user")]
        public User User { get; internal set; }
        [JsonSerializationKey("reactions")]
        internal List<string> ReactionsList { get; set; }

        public HashSet<string> Reactions
        {
            get
            {
                return new HashSet<string>(ReactionsList);
            }
        }
    }
}
using System.Collections.Generic;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public class UserVotes
    {
        [JsonSerializationKey("user")]
        public User User { get; internal set; }
        [JsonSerializationKey("votes")]
        internal List<string> VotesList { get; set; }

        public HashSet<string> Votes
        {
            get
            {
                return new HashSet<string>(VotesList);
            }
        }

        public override string ToString()
        {
            return $"User: {User}, Votes: {VotesList.ToDebugString()}";
        }

    }
}
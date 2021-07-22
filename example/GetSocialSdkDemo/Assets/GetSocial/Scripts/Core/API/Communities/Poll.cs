using System.Collections.Generic;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public class Poll
    {
        [JsonSerializationKey("allowMultipleVotes")]
        public bool AllowMultipleVotes { get; internal set; }

        [JsonSerializationKey("endDate")]
        public long EndDate { get; internal set; }

        [JsonSerializationKey("totalVotes")]
        public int TotalVotes { get; internal set; }

        [JsonSerializationKey("knownVoters")]
        public List<UserVotes> KnownVoters { get; internal set; }

        [JsonSerializationKey("options")]
        public List<PollOption> Options { get; internal set; }

        public Poll()
        {
        }


        public override string ToString()
        {
            return $"AllowMultipleVotes: {AllowMultipleVotes}, EndDate: {EndDate}, TotalVotes: {TotalVotes}, KnownVoters: {KnownVoters.ToDebugString()}, Options: {Options.ToDebugString()}";
        }
    }
}
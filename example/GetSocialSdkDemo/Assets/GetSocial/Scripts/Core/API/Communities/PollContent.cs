using System;
using System.Collections.Generic;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public class PollContent
    {
        [JsonSerializationKey("allowMultipleVotes")]
        public bool AllowMultipleVotes { get; set; }

        public System.DateTime? EndDate
        {
            set
            {
                _endDate = value.HasValue ? value.Value.ToUnixTimestamp() : 0;
            }
            internal get
            {
                return EndDate;
            }
        }
        [JsonSerializationKey("endDate")]
        internal long _endDate;

        [JsonSerializationKey("options")]
        internal List<PollOptionContent> Options { get; set; }


        public PollContent()
        {
            Options = new List<PollOptionContent>();
        }


    public PollContent AddPollOption(PollOptionContent optionContent)
        {
            Options.Add(optionContent);
            return this;
        }

        public PollContent AddPollOptions(IEnumerable<PollOptionContent> optionContents)
        {
            Options.AddAll(optionContents);
            return this;
        }

        public override string ToString()
        {
            return $"AllowMultipleVotes: {AllowMultipleVotes}, EndDate: {_endDate}, Options: {Options}";
        }
    }
}
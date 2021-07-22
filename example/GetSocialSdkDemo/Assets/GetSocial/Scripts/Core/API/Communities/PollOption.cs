using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public class PollOption
    {
        [JsonSerializationKey("optionId")]
        public string OptionId { get; internal set; }

        [JsonSerializationKey("text")]
        public string Text { get; internal set; }

        [JsonSerializationKey("attachment")]
        public MediaAttachment Attachment { get; internal set; }

        [JsonSerializationKey("voteCount")]
        public int VoteCount { get; internal set; }

        [JsonSerializationKey("isVotedByMe")]
        public bool IsVotedByMe { get; internal set; }

        public PollOption()
        {
        }

        public override string ToString()
        {
            return $"OptionId: {OptionId}, Text: {Text}, Attachment: {Attachment}, VoteCount: {VoteCount}, IsVotedByMe: {IsVotedByMe}";
        }
    }
}
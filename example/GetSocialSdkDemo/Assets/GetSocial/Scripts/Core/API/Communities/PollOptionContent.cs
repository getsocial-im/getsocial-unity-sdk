using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public class PollOptionContent
    {
        [JsonSerializationKey("optionId")]
        public string OptionId { get; set; }

        [JsonSerializationKey("text")]
        public string Text { get; set; }

        [JsonSerializationKey("attachment")]
        internal MediaAttachment Attachment { get; set; }


        public PollOptionContent()
        {
        }


        public override string ToString()
        {
            return $"OptionId: {OptionId}, Text: {Text}, Attachment: {Attachment}";
        }
    }
}
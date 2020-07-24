using GetSocialSdk.MiniJSON;
namespace GetSocialSdk.Core
{
    public class Mention 
    {
        public enum MentionType { App, User }

        /// <summary>
        /// ID of the mentioned user.
        /// </summary>
        /// <value>ID of the user.</value>
        [JsonSerializationKey("userId")]
        public string UserId { get; internal set; }

        /// <summary>
        /// Start position of the mention in the text.
        /// </summary>
        /// <value></value>
        [JsonSerializationKey("startIndex")]
        public int StartIndex { get; internal set; }

        /// <summary>
        /// End position of the mention in the text.
        /// </summary>
        /// <value></value>
        [JsonSerializationKey("endIndex")]
        public int EndIndex { get; internal set; }

        /// <summary>
        /// Type of the mention.
        /// </summary>
        /// <value>Type or user.</value>
        [JsonSerializationKey("type")]
        public MentionType Type { get; internal set; }
        
        public override string ToString()
        {
            return string.Format("{0} ({1}, {2}) - {3}", UserId, StartIndex, EndIndex, Type);
        }

        public Mention()
        {
            
        }
    }
}
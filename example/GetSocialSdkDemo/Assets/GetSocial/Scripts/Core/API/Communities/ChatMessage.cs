using System.Collections.Generic;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Chat message entity. Contains all information about message, its author and content.
    /// </summary>
    public sealed class ChatMessage 
    {
        /// <summary>
        /// Gets the message identifier.
        /// </summary>
        /// <value>The message identifier.</value>
        [JsonSerializationKey("id")]
        public string Id { get; internal set; }

        /// <summary>
        /// Gets the message text. Can be null if not text is set.
        /// </summary>
        /// <value>The message text.</value>
        [JsonSerializationKey("text")]
        public string Text { get; internal set; }

        /// <summary>
        /// Gets the message author.
        /// </summary>
        /// <value>The message author.</value>
        [JsonSerializationKey("author")]
        public User Author { get; internal set; }

        /// <summary>
        /// Get the list of attached media. Empty if no media was attached. Media is in the same order as it was attached while sending.
        /// </summary>
        [JsonSerializationKey("attachments")]
        public List<MediaAttachment> MediaAttachments { get; internal set; }


        /// <summary>
        /// Get custom data of the message attached when it was sent.
        /// </summary>
        /// <value>map of custom values, attached to the message.</value>
        [JsonSerializationKey("properties")]
        public Dictionary<string, string> Properties { get; internal set; }

        /// <summary>
        /// Message's sending date in seconds in Unix time.
        /// </summary>
        /// <value>message's sending date in seconds in Unix time.</value>
        [JsonSerializationKey("sentAt")]
        public long SentAt { get; internal set; }


        public ChatMessage()
        {
            
        }

        public override string ToString()
        {
            return $"Id: {Id}, Text: {Text}, Author:( {Author}), MediaAttachments: {MediaAttachments.ToDebugString()}, Properties: {Properties.ToDebugString()}, SentAt: {SentAt}";
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public class ChatMessageContent
    {
        /// <summary>
        /// Text of the message.
        /// </summary>
        /// <value>Text of the message.</value>
        [JsonSerializationKey("text")]
        public string Text { get; set; }

        [JsonSerializationKey("attachments")] 
        internal List<MediaAttachment> Attachments { get; } 

        [JsonSerializationKey("properties")] 
        internal Dictionary<string, string> Properties { get; }

        public ChatMessageContent()
        {
            Attachments = new List<MediaAttachment>();
            Properties = new Dictionary<string, string>();
        }

        /// <summary>
        /// Add custom property to the message.
        /// </summary>
        /// <param name="key">you can get the property value by this key in future.</param>
        /// <param name="value">arbitrary value.</param>
        /// <returns>same instance for a method chaining.</returns>
        public ChatMessageContent AddProperty(string key, string value)
        {
            Properties[key] = value;
            return this;
        }

        /// <summary>
        /// Add multiple properties.
        /// </summary>
        /// <param name="properties">map of properties.</param>
        /// <returns>same instance for a method chaining.</returns>
        public ChatMessageContent AddProperties(Dictionary<string, string> properties)
        {
            Properties.AddAll(properties);
            return this;
        }


        /// <summary>
        /// Add attachment to the message. Attachments will be returned in the same order as added.
        /// </summary>
        /// <param name="attachment">media attachment.</param>
        /// <returns>same instance for a method chaining.</returns>
        public ChatMessageContent AddMediaAttachment(MediaAttachment attachment)
        {
            Attachments.Add(attachment);
            return this;
        }

        /// <summary>
        /// Add multiple attachments to the message. Attachments will be returned in the same order as added.
        /// </summary>
        /// <param name="attachments"></param>
        /// <returns>same instance for a method chaining.</returns>
        public ChatMessageContent AddMediaAttachments(IEnumerable<MediaAttachment> attachments)
        {
            Attachments.AddAll(attachments);
            return this;
        }

        public override string ToString()
        {
            return $"Text: {Text}, Attachments: {Attachments}, Properties: {Properties.ToString()}";
        }
    }
}
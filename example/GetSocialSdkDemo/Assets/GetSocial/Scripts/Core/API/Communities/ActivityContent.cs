using System.Collections;
using System.Collections.Generic;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public class ActivityContent
    {
        /// <summary>
        /// Text of the post.
        /// </summary>
        /// <value>Text of the post.</value>
        [JsonSerializationKey("text")]
        public string Text { get; set; }

        [JsonSerializationKey("attachments")] 
        internal List<MediaAttachment> Attachments { get; } 

        /// <summary>
        /// Button attached to the post. 
        /// </summary>
        /// <value>Description of the button to be shown on the UI.</value>
        [JsonSerializationKey("button")]
        public ActivityButton Button { get; set; }

        [JsonSerializationKey("properties")] 
        internal Dictionary<string, string> Properties { get; }

        [JsonSerializationKey("poll")]
        public PollContent Poll { get; set; }

        public ActivityContent()
        {
            Attachments = new List<MediaAttachment>();
            Properties = new Dictionary<string, string>();
        }

        /// <summary>
        /// Add custom property to the post.
        /// </summary>
        /// <param name="key">you can get the property value by this key in future.</param>
        /// <param name="value">arbitrary value.</param>
        /// <returns>same instance for a method chaining.</returns>
        public ActivityContent AddProperty(string key, string value)
        {
            Properties[key] = value;
            return this;
        }

        /// <summary>
        /// Add multiple properties.
        /// </summary>
        /// <param name="properties">map of properties.</param>
        /// <returns>same instance for a method chaining.</returns>
        public ActivityContent AddProperties(Dictionary<string, string> properties)
        {
            Properties.AddAll(properties);
            return this;
        }


        /// <summary>
        /// Add attachment to the post. Attachments will be returned in the same order as added.
        /// </summary>
        /// <param name="attachment">media attachment.</param>
        /// <returns>same instance for a method chaining.</returns>
        public ActivityContent AddMediaAttachment(MediaAttachment attachment)
        {
            Attachments.Add(attachment);
            return this;
        }
        
        /// <summary>
        /// Set text of the post. Will override the previous value if it was set.
        /// </summary>
        /// <param name="attachments"></param>
        /// <returns>same instance for a method chaining.</returns>
        public ActivityContent AddMediaAttachments(IEnumerable<MediaAttachment> attachments)
        {
            Attachments.AddAll(attachments);
            return this;
        }

    }
}
using System;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    /// <summary>
    ///  Stores information about a way to send an invite and how it should be presented in a list.
    /// </summary>
    public sealed class InviteChannel
    {

        /// <summary>
        /// Gets the invite channel identifier.
        /// </summary>
        /// <value>The invite channel identifier.</value>
        [JsonSerializationKey("id")]
        public string Id { get; internal set; }

        /// <summary>
        /// Gets the invite channel name.
        /// </summary>
        /// <value>The invite channel name.</value>
        [JsonSerializationKey("name")]
        public string Name { get; internal set; }

        /// <summary>
        /// Gets the invite channel icon image URL.
        /// </summary>
        /// <value>The invite channel icon image URL.</value>
        [JsonSerializationKey("iconImageUrl")]
        public string IconImageUrl { get; internal set; }

        /// <summary>
        /// Gets invite channel the display order as configured on GetSocial Dashboard.
        /// </summary>
        /// <value>The invite channel the display order as configured on GetSocial Dashboard.</value>
        [JsonSerializationKey("displayOrder")]
        public int DisplayOrder { get; internal set; }

        override public string ToString()
        {
            return string.Format("Id = [{0}], name = [{1}], iconImageUrl = [{2}], displayOrder = [{3}]", Id, Name, IconImageUrl, DisplayOrder);
        }
    }
}
using System.Collections.Generic;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Invite content being sent along with smart invite.
    /// </summary>
    public sealed class InviteContent
    {
        [JsonSerializationKey("subject")]
        public string Subject { get; set; }

        [JsonSerializationKey("text")]
        public string Text { get; set; }

        [JsonSerializationKey("mediaAttachment")]
        public MediaAttachment MediaAttachment { get; set; }

        [JsonSerializationKey("linkParams")]
        public Dictionary<string, object> LinkParams { get; }

        public InviteContent()
        {
            LinkParams = new Dictionary<string, object>();
        }

        public InviteContent AddLinkParam(string key, object value)
        {
            this.LinkParams.Add(key, value);
            return this;
        }

        public InviteContent AddLinkParams(Dictionary<string, object> linkParams)
        {
            this.LinkParams.AddAll(linkParams);
            return this;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Builder for a query to retrieve chat messages.
    /// </summary>
    public sealed class ChatMessagesQuery 
    {
        [JsonSerializationKey("chatId")]
        internal ChatId Id;

        ChatMessagesQuery(ChatId id)
        {
            this.Id = id;
        }
        
        /// <summary>
        /// Get messages in a chet.
        /// </summary>
        /// <param name="id">Chat id.</param>
        /// <returns>new query</returns>
        public static ChatMessagesQuery InChat(ChatId id)  
        {
            return new ChatMessagesQuery(id);
        }
    }
}

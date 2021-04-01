using System.Collections.Generic;
using System.Linq;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{

    /// <summary>
    /// Builder for a query to retrieve groups.
    /// </summary>
    public sealed class ChatMessagesPagingQuery
    {
        [JsonSerializationKey("limit")]
        public int Limit { get; internal set; }

        [JsonSerializationKey("nextMessages")]
        public string NextMessages { get; internal set; }

        [JsonSerializationKey("previousMessages")]
        public string PreviousMessages { get; internal set; }

        [JsonSerializationKey("query")]
        public ChatMessagesQuery Query { get; }

        public ChatMessagesPagingQuery WithLimit(int limit)
        {
            Limit = limit;
            return this;
        }

        public ChatMessagesPagingQuery NextMessagesCursor(string next)
        {
            NextMessages = next;
            return this;
        }

        public ChatMessagesPagingQuery PreviousMessagesCursor(string previous)
        {
            PreviousMessages = previous;
            return this;
        }

        public ChatMessagesPagingQuery(ChatMessagesQuery query)
        {
            Query = query;
            Limit = 50;
        }

    }
}

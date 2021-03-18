using System.Collections.Generic;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{

    /// <summary>
    /// Chat messages pagination result.
    /// </summary>
    public sealed class ChatMessagesPagingResult
    {
        [JsonSerializationKey("entries")]
        public List<ChatMessage> Messages { get; internal set; }

        [JsonSerializationKey("next")]
        public string NextMessagesCursor { get; internal set; }

        [JsonSerializationKey("previous")]
        public string PreviousMessagesCursor { get; internal set; }

        [JsonSerializationKey("refreshCursor")]
        public string RefreshCursor { get; internal set; }
    }
}

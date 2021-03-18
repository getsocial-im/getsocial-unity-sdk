using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public class ChatId
    {
        [JsonSerializationKey("id")]
        public string Id { get; internal set; }
        
        [JsonSerializationKey("userId")]
        public UserId UserId { get; internal set; }

        /// <summary>
        /// Create a chat id.
        /// </summary>
        /// <param name="chatId">Chat identifier.</param>
        /// <returns></returns>
        public static ChatId Create(string chatId)
        {
            return new ChatId {Id= chatId, UserId = null};
        }

        /// <summary>
        /// Create a chat id with an User Id.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns></returns>
        public static ChatId Create(UserId userId)
        {
            return new ChatId { Id = null, UserId = userId};
        }

        public override string ToString()
        {
            var userIdString = UserId == null ? null : UserId.ToString();
            return $"Id: {Id}, UserId: {userIdString}";
        }

    }
}
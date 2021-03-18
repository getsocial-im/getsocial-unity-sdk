using System.Collections.Generic;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Chat entity. Contains all information about chat, its author and last message.
    /// </summary>
    public sealed class Chat
    {
        /// <summary>
        /// Gets the chat identifier.
        /// </summary>
        /// <value>The message identifier.</value>
        [JsonSerializationKey("id")]
        public string Id { get; internal set; }

        /// <summary>
        /// Gets the chat title. Can be null if not text is set.
        /// </summary>
        /// <value>The message text.</value>
        [JsonSerializationKey("title")]
        public string Title { get; internal set; }

        /// <summary>
        /// Gets the chat avatarURL. Can be null if not text is set.
        /// </summary>
        /// <value>The message text.</value>
        [JsonSerializationKey("avatarUrl")]
        public string AvatarUrl { get; internal set; }

        /// <summary>
        /// Chat's creation date in seconds in Unix time.
        /// </summary>
        /// <value>creation date in seconds in Unix time.</value>
        [JsonSerializationKey("createdAt")]
        public long CreatedAt { get; internal set; }

        /// <summary>
        /// Chat's last update date in seconds in Unix time.
        /// </summary>
        /// <value>last update date in seconds in Unix time.</value>
        [JsonSerializationKey("updatedAt")]
        public long UpdatedAt { get; internal set; }

        /// <summary>
        /// Number of members.
        /// </summary>
        /// <value>number of members.</value>
        [JsonSerializationKey("membersCount")]
        public int MembersCount { get; internal set; }

        /// <summary>
        /// Gets the other chat member.
        /// </summary>
        /// <value>The other member.</value>
        [JsonSerializationKey("otherMember")]
        public User OtherMember { get; internal set; }

        /// <summary>
        /// Last chat message. Can be null if there are no messages yet.
        /// </summary>
        [JsonSerializationKey("lastMessage")]
        public ChatMessage LastMessage { get; internal set; }

        public Chat()
        {
            
        }

        public override string ToString()
        {
            return $"Id: {Id}, Title: {Title}, AvatarUrl: {AvatarUrl}, MembersCount: {MembersCount}, OtherMember: {OtherMember}, LastMessage: {LastMessage}, CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
        }
    }
}
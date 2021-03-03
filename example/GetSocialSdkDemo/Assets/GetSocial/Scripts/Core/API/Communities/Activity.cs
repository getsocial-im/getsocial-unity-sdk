using System.Collections.Generic;
using GetSocialSdk.MiniJSON;
using System.Linq;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Activity post entity. Contains all information about post, its author and content.
    /// </summary>
    public sealed class Activity 
    {
        /// <summary>
        /// Gets the activity identifier.
        /// </summary>
        /// <value>The activity identifier.</value>
        [JsonSerializationKey("id")]
        public string Id { get; internal set; }

        /// <summary>
        /// Gets the activity text. Can be null if not text is set.
        /// </summary>
        /// <value>The activity text.</value>
        [JsonSerializationKey("text")]
        public string Text { get; internal set; }

        /// <summary>
        /// Gets the activity author.
        /// </summary>
        /// <value>The activity author.</value>
        [JsonSerializationKey("author")]
        public User Author { get; internal set; }

        /// <summary>
        /// Get the list of attached media. Empty if no media was attached. Media is in the same order as it was attached while posting.
        /// </summary>
        [JsonSerializationKey("attachments")]
        public List<MediaAttachment> MediaAttachments { get; internal set; }

        /// <summary>
        /// Type of the activity. One of the <see cref="ActivityTypes"/>.
        /// </summary>
        /// <value>Type of the activity.</value>
        [JsonSerializationKey("type")]
        public string Type { get; internal set; }

        /// <summary>
        /// Indicates if activity is announcement or just regular post.
        /// </summary>
        /// <value>true, if activity is announcement, false otherwise.</value>
        [JsonSerializationKey("announcement")]
        public bool Announcement { get; internal set; }

        /// <summary>
        /// Number of comments under the activity.
        /// </summary>
        /// <value>total number of comments to this activity.</value>
        [JsonSerializationKey("commentsCount")]
        public int CommentsCount { get; internal set; }

        /// <summary>
        /// Map of the reactions count. Key is reaction name - one of <see cref="Reactions"/>.
        /// Value is number of users reacted with this reaction.
        /// Use <see cref="GetReactionsCount(string)"/> or <see cref="GetTotalReactionsCount"/> for a null safety.
        /// </summary>
        /// <value>Map of reactions names and number of unique reactions per each.</value>
        [JsonSerializationKey("reactionsCount")]
        public Dictionary<string, int> ReactionsCount { get; internal set; }

        [JsonSerializationKey("myReactions")] 
        internal List<string> MyReactionsList { get; set; }

        /// <summary>
        /// Get set of reactions to the activity by a current user.
        /// </summary>
        /// <value>Reactions of a current user.</value>
        public HashSet<string> MyReactions
        {
            get { return new HashSet<string>(MyReactionsList);  }
        }

        /// <summary>
        /// Get custom data of the activity attached when it was posted.
        /// </summary>
        /// <value>map of custom values, attached to the activity.</value>
        [JsonSerializationKey("properties")]
        public Dictionary<string, string> Properties { get; internal set; }

        /// <summary>
        /// Activity's creation date in seconds in Unix time.
        /// </summary>
        /// <value>activity's creation date in seconds in Unix time.</value>
        [JsonSerializationKey("createdAt")]
        public long CreatedAt { get; internal set; }

        /// <summary>
        /// Get list of mentions in the activity.
        /// </summary>
        /// <value>List of mentioned user in the activity Text.</value>
        [JsonSerializationKey("mentions")]
        public List<Mention> Mentions { get; internal set; }

        /// <summary>
        /// Action button, attached to the activity.
        /// </summary>
        /// <value>button object, or null if there is no button.</value>
        [JsonSerializationKey("button")]
        public ActivityButton Button { get; internal set; }

        /// <summary>
        /// Source of the activity, describes where it was posted.
        /// </summary>
        /// <value> entity to which the activity was posted.</value>
        [JsonSerializationKey("source")]
        public CommunitiesEntity Source { get; internal set; }

        /// <summary>
        /// Status of the activity.
        /// Check <see cref="ActivityStatus"/> for possible values.
        /// </summary>
        /// <value> entity to which the activity was posted.</value>
        [JsonSerializationKey("status")]
        public string Status { get; internal set; }

        public Activity()
        {
            
        }

        public int GetReactionsCount(string reaction)
        {
            return ReactionsCount.ContainsKey(reaction) ? ReactionsCount[reaction] : 0;
        }

        public int GetTotalReactionsCount()
        {
            return ReactionsCount.Sum(x => x.Value);
        }

        public override string ToString()
        {
            return $"Id: {Id}, Text: {Text}, Author: {Author}, MediaAttachments: {MediaAttachments.ToDebugString()}, Type: {Type}, Announcement: {Announcement}, CommentsCount: {CommentsCount}, ReactionsCount: {ReactionsCount.ToDebugString()}, MyReactions: {MyReactionsList.ToDebugString()},  Properties: {Properties.ToDebugString()}, CreatedAt: {CreatedAt}, Mentions: {Mentions.ToDebugString()}, Button: {Button}, Source: {Source}, Status: {Status}";
        }
    }
}
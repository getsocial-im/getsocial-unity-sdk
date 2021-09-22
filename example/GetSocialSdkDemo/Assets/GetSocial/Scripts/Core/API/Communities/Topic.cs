using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Topic entity. Contains all information about a topic.
    /// </summary>
    public sealed class Topic
    {
        /// <summary>
        /// Gets the topic identifier.
        /// </summary>
        /// <value>The topic identifier.</value>
        [JsonSerializationKey("id")]
        public string Id { get; internal set; }

        /// <summary>
        /// Gets the topic title.
        /// </summary>
        /// <value>The topic title.</value>
        [JsonSerializationKey("title")]
        public string Title { get; internal set; }

        /// <summary>
        /// Gets the topic description.
        /// </summary>
        /// <value>The topic description.</value>
        [JsonSerializationKey("description")]
        public string Description { get; internal set; }

        /// <summary>
        /// Get the topic avatar url.
        /// </summary>
        /// <value>The topic avatar url.
        [JsonSerializationKey("avatarUrl")]
        public string AvatarUrl { get; internal set; }

        /// <summary>
        ///  Get topic's creation date in seconds in Unix time.
        /// </summary>
        /// <value>seconds from the topic creation time. UTC.</value>
        [JsonSerializationKey("createdAt")]
        public long CreatedAt { get; internal set; }

        /// <summary>
        /// Last date when topic was updated. UTC time in seconds.
        /// </summary>
        /// <value>seconds from the topic update time. UTC.</value>
        [JsonSerializationKey("updatedAt")]
        public long UpdatedAt { get; internal set; }

        /// <summary>
        /// Get settings for the topic.
        /// </summary>
        /// <value>topic settings.</value>
        [JsonSerializationKey("settings")]
        public CommunitiesSettings Settings { get; internal set; }

        /// <summary>
        /// Get the number of followers of a topic.
        /// </summary>
        /// <value>Number of users who follow the topic.</value>
        [JsonSerializationKey("followersCount")]
        public int FollowersCount { get; internal set; }

        /// <summary>
        /// Check if current user is a follower of the topic.
        /// </summary>
        /// <value>True if current user if a follower of the topic, false otherwise.</value>
        [JsonSerializationKey("isFollowedByMe")]
        public bool IsFollowedByMe { get; internal set; }

        [JsonSerializationKey("popularity")]
        public double Popularity { get; internal set; } = 0;

        public Topic()
        {

        }

        public override string ToString()
        {
            return $"Id: {Id}, Title: {Title}, Description: {Description}, AvatarUrl: {AvatarUrl}, CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}, Settings: {Settings}, FollowersCount: {FollowersCount}, isFollowedByMe: {IsFollowedByMe}, popularity: {Popularity}";
        }
    }
}
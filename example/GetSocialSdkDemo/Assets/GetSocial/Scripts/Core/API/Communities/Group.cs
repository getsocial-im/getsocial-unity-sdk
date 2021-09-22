using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Group entity. Contains all information about a group.
    /// </summary>
    public sealed class Group
    {
        /// <summary>
        /// Gets the group identifier.
        /// </summary>
        /// <value>The group identifier.</value>
        [JsonSerializationKey("id")]
        public string Id { get; internal set; }

        /// <summary>
        /// Gets the group title.
        /// </summary>
        /// <value>The group title.</value>
        [JsonSerializationKey("title")]
        public string Title { get; internal set; }

        /// <summary>
        /// Gets the group description.
        /// </summary>
        /// <value>The group description.</value>
        [JsonSerializationKey("description")]
        public string Description { get; internal set; }

        /// <summary>
        /// Get the group avatar url.
        /// </summary>
        /// <value>The group avatar url.
        [JsonSerializationKey("avatarUrl")]
        public string AvatarUrl { get; internal set; }

        [JsonSerializationKey("createdAt")]
        public long CreatedAt { get; internal set; }

        [JsonSerializationKey("updatedAt")]
        public long UpdatedAt { get; internal set; }

        [JsonSerializationKey("settings")]
        public CommunitiesSettings Settings { get; internal set; }

        [JsonSerializationKey("followersCount")]
        public int FollowersCount { get; internal set; }

        [JsonSerializationKey("isFollowedByMe")]
        public bool IsFollowedByMe { get; internal set; }

        [JsonSerializationKey("membersCount")]
        public int MembersCount { get; internal set; }

        [JsonSerializationKey("membership")]
        public Membership Membership { get; internal set; }

        [JsonSerializationKey("popularity")]
        public double Popularity { get; internal set; } = 0;

        public Group()
        {

        }

        public override string ToString()
        {
            return $"Id: {Id}, Title: {Title}, Description: {Description}, AvatarUrl: {AvatarUrl},  Settings: {Settings}, CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}, Membership: {Membership}, popularity: {Popularity}";
        }

    }
}
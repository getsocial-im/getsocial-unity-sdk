using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{

    /// <summary>
    /// Suggested friend entity.
    /// </summary>
    public class SuggestedFriend : User
    {
        [JsonSerializationKey("mutualFriendsCount")]
        public int MutualFriendsCount { get; internal set; }
    }
}
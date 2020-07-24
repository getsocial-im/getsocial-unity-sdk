using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public class FriendsQuery
    {
        [JsonSerializationKey("userId")]
        internal readonly UserId UserId;

        private FriendsQuery(UserId userId) {
            UserId = userId;
        }

        /// <summary>
        /// Get friends of the user.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <returns>new query.</returns>
        public static FriendsQuery OfUser(UserId userId) {
            return new FriendsQuery(userId);
        }
    }
}
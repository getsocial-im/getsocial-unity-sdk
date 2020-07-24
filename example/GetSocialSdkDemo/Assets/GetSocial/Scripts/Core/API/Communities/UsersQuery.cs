using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public class UsersQuery
    {
        [JsonSerializationKey("query")] 
        internal string Query { get; set; }

        [JsonSerializationKey("followedBy")] 
        internal UserId FollowedBy { get; set; }

        /// <summary>
        /// Find users with a name or part of the name.
        /// </summary>
        /// <param name="name">Part of the name. At least 3 symbols required.</param>
        /// <returns>new query.</returns>
        public static UsersQuery Find(string name)
        {
            return new UsersQuery {Query = name};
        }

        /// <summary>
        /// Find users that are followed by a user.
        /// </summary>
        /// <param name="user">ID of the user.</param>
        /// <returns>new query.</returns>
        public static UsersQuery FollowedByUser(UserId user)
        {
            return new UsersQuery {FollowedBy = user};
        }
 
    }
}
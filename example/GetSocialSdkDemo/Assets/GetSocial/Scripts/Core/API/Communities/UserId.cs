using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public class UserId
    {
        [JsonSerializationKey("provider")]
        public string Provider { get; internal set; }
        
        [JsonSerializationKey("userId")]
        public string Id { get; internal set; }

        /// <summary>
        /// Create an identifier of user with GetSocial ID.
        /// </summary>
        /// <param name="userId">GetSocial user ID.</param>
        /// <returns></returns>
        public static UserId Create(string userId)
        {
            return new UserId {Provider = null, Id = userId};
        }
        
        public static UserId CreateWithProvider(string provider, string userId)
        {
            return new UserId {Provider = provider, Id = userId};
        }

        public static UserId CurrentUser()
        {
            var currentUser = GetSocial.GetCurrentUser();
            return currentUser == null ? new UserId() : new UserId {Id = currentUser.Id};
        }

        internal string AsString()
        {
            if (this.Provider == null)
            {
                return Id;
            }
            return this.Provider + ":" + this.Id;
        }

        public override string ToString()
        {
            return AsString();
        }

        internal UserIdList ToUserIdList()
        {
            return UserIdList.CreateWithProvider(this.Provider, this.Id);
        }

    }
}
using System;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public sealed class ReferralUsersQuery
    {
        [JsonSerializationKey("eventName")]
        private string _eventName = "";

        ReferralUsersQuery(string eventName)
        {
            _eventName = eventName;
        }

        /// <summary>
        /// Creates new query to filter all the users.
        /// </summary>
        /// <returns>New query.</returns>
        public static ReferralUsersQuery AllUsers()
        {
            return new ReferralUsersQuery("");
        }

        /// <summary>
        /// Creates new query to filter users for the specified event.
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns>New query.</returns>
        public static ReferralUsersQuery UsersForEvent(string eventName)
        {
            return new ReferralUsersQuery(eventName);
        }

        public string GetEventName()
        {
            return _eventName;
        }
    }
}
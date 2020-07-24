
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public sealed class NotificationButton
    {

        public const string ConsumeAction = "consume";
        public const string IgnoreAction = "ignore";

        [JsonSerializationKey("title")]
        public string Title { get; private set; }

        /// <summary>
        /// One of constants(<see cref="ConsumeAction"/>, <see cref="IgnoreAction"/>) or any custom value.
        /// </summary>
        [JsonSerializationKey("actionId")]
        public string ActionId { get; private set; }

        internal NotificationButton()
        {
            
        }

        /// <summary>
        /// Create a new button.
        /// </summary>
        /// <param name="title">Title to be displayed</param>
        /// <param name="actionId">Action ID - could be one of constants(<see cref="ConsumeAction"/>, <see cref="IgnoreAction"/>) or any custom value.</param>
        /// <returns></returns>
        public static NotificationButton Create(string title, string actionId)
        {
            return new NotificationButton { Title = title, ActionId = actionId };
        }

        private bool Equals(NotificationButton other)
        {
            return string.Equals(Title, other.Title) && string.Equals(ActionId, other.ActionId);
        }
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is NotificationButton && Equals((NotificationButton) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Title != null ? Title.GetHashCode() : 0) * 397) ^ (ActionId != null ? ActionId.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return string.Format("Title: {0}, ActionId: {1}", Title, ActionId);
        }
    }
}
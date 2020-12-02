using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public sealed class NotificationContext
    {
        [JsonSerializationKey("action")]
        public string Action { get; private set; }
    }
}
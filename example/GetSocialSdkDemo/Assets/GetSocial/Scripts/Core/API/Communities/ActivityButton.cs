using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public class ActivityButton
    {
        /// <summary>
        /// Action to be performed when the button is clicked.
        /// </summary>
        /// <value>Button action.</value>
        [JsonSerializationKey("action")]
        public GetSocialAction Action { get; internal set; }

        /// <summary>
        /// Title of the button. Will be displayed on the UI.
        /// </summary>
        /// <value>Button title.</value>
        [JsonSerializationKey("title")]
        public string Title { get; internal set; }

        /// <summary>
        /// Create a new button.
        /// </summary>
        /// <param name="title">Title of the button.</param>
        /// <param name="action">Action to be performed when the button is clicked.</param>
        /// <returns>New button.</returns>
        public static ActivityButton Create(string title, GetSocialAction action)
        {
            return new ActivityButton
            {
                Action = action,
                Title = title
            };
        }

        public override string ToString()
        {
            return $"Action: {Action}, Title: {Title}";
        }
    }
}
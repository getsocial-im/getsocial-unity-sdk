using System.Collections.Generic;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{

    public sealed class NotificationContentPlaceholders
    {
        /// <summary>
        ///  Will be replaced with your actual display name in the notification text.
        /// </summary>
        public const string SenderDisplayName = "[SENDER_DISPLAY_NAME]";

        /// <summary>
        /// Will be replaced with received actual display name in the notification text.
        /// </summary>
        public const string ReceiverDisplayName = "[RECEIVER_DISPLAY_NAME]";
    }

    public sealed class NotificationContent
    {

#pragma warning disable 414
        [JsonSerializationKey("text")]
        internal string _text;

        [JsonSerializationKey("title")]
        internal string _title;

        [JsonSerializationKey("templateName")]
        internal string _templateName;

        [JsonSerializationKey("mediaAttachment")]
        internal MediaAttachment _mediaAttachment;

        [JsonSerializationKey("templatePlaceholders")]
        internal readonly Dictionary<string, string> _templatePlaceholders;

        [JsonSerializationKey("action")]
        internal GetSocialAction _action;

        [JsonSerializationKey("actionButtons")]
        internal readonly List<NotificationButton> _actionButtons;

        [JsonSerializationKey("customization")]
        internal NotificationCustomization _customization;

        [JsonSerializationKey("badge")]
        internal Badge _badge;
#pragma warning restore 414

        private NotificationContent()
        {
            _templatePlaceholders = new Dictionary<string, string>();
            _actionButtons = new List<NotificationButton>();
        }

        public override string ToString()
        {
            return string.Format(
                "Title: {0}, Text: {1}, Action: {2}, Template: {3}, TemplatePlaceholders: {4}, Customization: {5}"
                , _title, _text, _action, _templateName,
                _templatePlaceholders.ToDebugString(),
                _customization);
        }

        /// <summary>
        /// Create notification with text.
        /// </summary>
        /// <param name="text">text text to be displayed to receivers.</param>
        /// <returns>new notification content.</returns>
        public static NotificationContent CreateWithText(string text)
        {
            return new NotificationContent().WithText(text);
        }

        /// <summary>
        /// Create notification from the template configured on the GetSocial Dashboard.
        /// </summary>
        /// <param name="templateName">name of the template on the GetSocial Dashboard. Case-sensitive.</param>
        /// <returns>new notification content.</returns>
        public static NotificationContent CreateWithTemplate(string templateName)
        {
            return new NotificationContent().WithTemplateName(templateName);
        }

        /// <summary>
        /// Set notification title. If you use template, your title will be overriden by this.
        /// </summary>
        /// <param name="title">notification title.</param>
        /// <returns>notification content for methods chaining</returns>
        public NotificationContent WithTitle(string title)
        {
            _title = title;
            return this;
        }

        /// <summary>
        /// Set notification text. If you use template, your text will be overriden by this.
        /// </summary>
        /// <param name="text">notification text</param>
        /// <returns>notification content for methods chaining.</returns>
        public NotificationContent WithText(string text)
        {
            _text = text;
            return this;
        }

        /// <summary>
        /// Set template name. Notification will use values from the GetSocial Dashboard as title and text.
        /// But <see cref="WithTitle"/> or <see cref="WithText"/> have higher priority and will override template values.
        /// </summary>
        /// <param name="templateName">name of the template on the dashboard. Case-sensitive</param>
        /// <returns>notification content for methods chaining</returns>
        public NotificationContent WithTemplateName(string templateName)
        {
            _templateName = templateName;
            return this;
        }

        /// <summary>
        /// If you specified placeholders on the GetSocial Dashboard for your template title or text - you can replace it using this method.
        /// For example, if your template text it "Hello,  [USERNAME].", call notificationContent.addTemplatePlaceholder("USERNAME", "My actual name").
        /// Brackets should be omitted in key.
        /// Won't make any effect without template name.
        /// </summary>
        /// <param name="placeholder">placeholder on the GetSocial Dashboard.</param>
        /// <param name="replacement">actual text that should be used instead.</param>
        /// <returns>notification content for methods chaining</returns>
        public NotificationContent AddTemplatePlaceholder(string placeholder, string replacement)
        {
            _templatePlaceholders[placeholder] = replacement;
            return this;
        }

        /// <summary>
        /// Add all keys and values from map to template placeholders.
        /// </summary>
        /// <param name="templateData">template placeholders map.</param>
        /// <returns>notification content for methods chaining</returns>
        public NotificationContent AddTemplatePlaceholders(Dictionary<string, string> templateData)
        {
            _templatePlaceholders.AddAll(templateData);
            return this;
        }

        public NotificationContent WithMediaAttachment(MediaAttachment mediaAttachment)
        {
            _mediaAttachment = mediaAttachment;
            return this;
        }

        public NotificationContent WithAction(GetSocialAction action)
        {
            _action = action;
            return this;
        }

        public NotificationContent AddActionButton(NotificationButton actionButton)
        {
            _actionButtons.Add(actionButton);
            return this;
        }

        public NotificationContent AddActionButtons(IEnumerable<NotificationButton> actionButton)
        {
            _actionButtons.AddAll(actionButton);
            return this;
        }

        /// <summary>
        /// Set badge update for receiver. Works only if receiver's platform is iOS.
        /// </summary>
        /// <param name="badge">Change badge.</param>
        /// <returns>notification content for methods chaining</returns>
        public NotificationContent WithBadge(Badge badge)
        {
            _badge = badge;
            return this;
        }

        /// <summary>
        /// Customize notification, like change background image, title and text color.
        /// Supported only on Android.
        /// </summary>
        /// <param name="customization">customization parameters.</param>
        /// <returns>notification content for methods chaining</returns>
        public NotificationContent WithCustomization(NotificationCustomization customization)
        {
            _customization = customization;
            return this;
        }
    }
}
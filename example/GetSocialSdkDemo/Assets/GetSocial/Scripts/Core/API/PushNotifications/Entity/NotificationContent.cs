using System.Collections.Generic;

#if UNITY_ANDROID
using UnityEngine;
#endif

#if UNITY_IOS
using GetSocialSdk.MiniJSON;
#endif

namespace GetSocialSdk.Core
{
    public sealed class NotificationContent : IConvertableToNative
    {
        
#pragma warning disable 414      
        private Notification.Type? _actionType;
        private readonly Dictionary<string, string> _actionData;

        private string _text;
        private string _title;

        private string _templateName;
        private readonly Dictionary<string, string> _templatePlaceholders;
#pragma warning restore 414

        private NotificationContent()
        {
            _actionData = new Dictionary<string, string>();
            _templatePlaceholders = new Dictionary<string, string>();
        }

        public override string ToString()
        {
            return string.Format(
                "Title: {0}, Text: {1}, Action: {2}, ActionData: {3}, Template: {4}, TemplatePlaceholders: {5}"
                , _title, _text, _actionType, _actionData.ToDebugString(), _templateName,
                _templatePlaceholders.ToDebugString());
        }

        /// <summary>
        /// Create notification with text.
        /// </summary>
        /// <param name="text">text text to be displayed to receivers.</param>
        /// <returns>new notification content.</returns>
        public static NotificationContent NotificationWithText(string text)
        {
            return new NotificationContent().WithText(text);
        }

        /// <summary>
        /// Create notification from the template configured on the GetSocial Dashboard.
        /// </summary>
        /// <param name="templateName">name of the template on the GetSocial Dashboard. Case-sensitive.</param>
        /// <returns>new notification content.</returns>
        public static NotificationContent NotificationFromTemplate(string templateName)
        {
            return new NotificationContent().WithTemplateName(templateName);
        }

        /// <summary>
        /// Set notification action to be performed on click. One of <see cref="Notification.Type"/>.
        /// Default is <see cref="Notification.Type.Custom"/>
        /// </summary>
        /// <param name="actionType">what should be done when user clicks on notification.</param>
        /// <returns>content for methods chaining</returns>
        public NotificationContent WithAction(Notification.Type actionType)
        {
            _actionType = actionType;
            return this;
        }

        /// <summary>
        /// Add action data you could retrieve on the receiver side. Also you could specify some additional data for default actions.
        /// Use one of <see cref="Notification.Key"/> for default actions or pass your custom data.
        /// </summary>
        /// <param name="key">action key.</param>
        /// <param name="value">action value</param>
        /// <returns>notification content for methods chaining</returns>
        public NotificationContent AddActionData(string key, string value)
        {
            _actionData[key] = value;
            return this;
        }

        /// <summary>
        /// Add all keys and values from map to action data.
        /// </summary>
        /// <param name="actionData">map of action keys and values</param>
        /// <returns>notification content for methods chaining</returns>
        public NotificationContent AddActionData(Dictionary<string, string> actionData)
        {
            _actionData.AddAll(actionData);
            return this;
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
#if UNITY_ANDROID
        public AndroidJavaObject ToAjo()
        {
            var notificationContentClass =
                new AndroidJavaClass("im.getsocial.sdk.pushnotifications.NotificationContent");
            var notificationContent = notificationContentClass.CallStaticAJO("notificationWithText", _text)
                .CallAJO("withTitle", _title)
                .CallAJO("withTemplateName", _templateName)
                .CallAJO("addActionData", _actionData.ToJavaHashMap())
                .CallAJO("addTemplatePlaceholders", _templatePlaceholders.ToJavaHashMap());
            if (_actionType != null) 
            {
                notificationContent.CallAJO("withAction", (int) _actionType.Value);
            }
            return notificationContent;
        }
#elif UNITY_IOS
        public string ToJson()
        {
            var json = new Dictionary<string, object>
            {
                {"Title", _title},
                {"Text", _text},
                {"ActionData", _actionData},
                {"Template", _templateName},
                {"TemplatePlaceholders", _templatePlaceholders}
            };
            if (_actionType != null) 
            {
                json["Action"] = (int) _actionType.Value;
            }
            return GSJson.Serialize(json);
        }
#endif
    }
}
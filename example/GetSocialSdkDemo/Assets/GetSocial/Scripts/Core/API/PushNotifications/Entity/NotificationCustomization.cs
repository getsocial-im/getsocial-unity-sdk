using System.Collections.Generic;
using GetSocialSdk.MiniJSON;
using UnityEngine;

namespace GetSocialSdk.Core
{
    public class NotificationCustomization
    {
        [JsonSerializationKey("backgroundImage")]
        public string BackgroundImageConfiguration { get; internal set; }

        [JsonSerializationKey("titleColor")]
        public string TitleColor { get; internal set; }

        [JsonSerializationKey("textColor")]
        public string TextColor { get; internal set; }
        
        public NotificationCustomization() {}

        internal NotificationCustomization(string backgroundImageConfiguration, string titleColor, string textColor)
        {
            BackgroundImageConfiguration = backgroundImageConfiguration;
            TitleColor = titleColor;
            TextColor = textColor;
        }

        public static NotificationCustomization WithBackgroundImageConfiguration(string backgroundImageConfiguration)
        {
            var customization = new NotificationCustomization();
            customization.BackgroundImageConfiguration = backgroundImageConfiguration;
            return customization;
        }

        public NotificationCustomization WithTitleColor(string titleColor)
        {
            this.TitleColor = titleColor;
            return this;
        }

        public NotificationCustomization WithTextColor(string textColor)
        {
            this.TextColor = textColor;
            return this;
        }

        public override string ToString()
        {
            return string.Format("BackgroundImageConfiguration: {0}, TitleColor: {1}, TextColor: {2}", BackgroundImageConfiguration, TitleColor, TextColor);
        }
    }
}
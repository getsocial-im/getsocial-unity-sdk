using System.Collections.Generic;
using GetSocialSdk.MiniJSON;
using UnityEngine;

namespace GetSocialSdk.Core
{
    public class NotificationCustomization : IConvertableFromNative<NotificationCustomization>
    {
        public string BackgroundImageConfiguration { get; private set; }
        public string TitleColor { get; private set; }
        public string TextColor { get; private set; }
        
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

#if UNITY_ANDROID
        public NotificationCustomization ParseFromAJO (AndroidJavaObject ajo) {
            if (ajo == null) 
            {
                return null;    
            }
            BackgroundImageConfiguration = ajo.CallStr ("getBackgroundImageConfiguration");
            TitleColor = ajo.CallStr ("getTitleColor");
            TextColor = ajo.CallStr ("getTextColor");
            return this;
        }

        public AndroidJavaObject ToAjo()
        {
            var notificationCustomizationClass =
                new AndroidJavaClass("im.getsocial.sdk.pushnotifications.NotificationCustomization");
            var notificationCustomization = notificationCustomizationClass.CallStaticAJO("withBackgroundImageConfiguration", BackgroundImageConfiguration)
                .CallAJO("withTitleColor", TitleColor)
                .CallAJO("withTextColor", TextColor);
            
            return notificationCustomization;
        }

#elif UNITY_IOS
        public NotificationCustomization ParseFromJson (Dictionary<string, object> dictionary) {
            if (dictionary == null) 
            {
                return null;
            }
            BackgroundImageConfiguration = dictionary["BackgroundImageConfiguration"] as string;
            TitleColor = dictionary["TitleColor"] as string;
            TextColor = dictionary["TextColor"] as string;
            return this;
        }
        
        public string ToJson()
        {
            var json = new Dictionary<string, object>
            {
                {"BackgroundImageConfiguration", BackgroundImageConfiguration},
                {"TitleColor", TitleColor},
                {"TextColor", TextColor},
            };
            return GSJson.Serialize(json);
        }
        
#endif        
    }
}
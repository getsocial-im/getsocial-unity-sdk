using System.Collections.Generic;

namespace GetSocialSdk.Editor
{
    public class RemoteConfig
    {
        public bool IsSuccessful { get; private set; }
        public string ErrorMessage { get; private set; }
        public PlatformConfig Android { get; private set; }
        public PlatformConfig Ios { get; private set; }


        public RemoteConfig(bool isSuccessful, string errorMessage, PlatformConfig android, PlatformConfig ios)
        {
            IsSuccessful = isSuccessful;
            ErrorMessage = errorMessage;
            Android = android;
            Ios = ios;
        }

        public static RemoteConfig FromDict(Dictionary<string,object> valueDictionary)
        {
            var isSuccessful = valueDictionary.ContainsKey("success") ? (bool) valueDictionary["success"] : true;
            var errorMessage = string.Empty;
            PlatformConfig androidConfig = null;
            PlatformConfig iosConfig = null;
            
            if (isSuccessful)
            {
                androidConfig = PlatformConfig.FromDict(valueDictionary["android"] as Dictionary<string, object>);
                iosConfig = PlatformConfig.FromDict(valueDictionary["ios"] as Dictionary<string,object>);    
            }
            else
            {
                errorMessage = valueDictionary["error"] as string;
            }

            return new RemoteConfig(isSuccessful, errorMessage, androidConfig, iosConfig);
        }

        public class PlatformConfig
        {
            public bool IsEnabled { get; private set; }
            public bool IsPushNotificationEnabled { get; private set; }
            public string PushEnvironment { get; private set; }
            public List<string> DeepLinkDomains { get; private set; }
            public List<string> EnabledInviteChannels { get; private set; }

            private PlatformConfig(bool isEnabled, bool isPushNotificationEnabled, string pushEnvironment, List<string> deepLinkDomains, List<string> enabledInviteChannels)
            {
                IsEnabled = isEnabled;
                IsPushNotificationEnabled = isPushNotificationEnabled;
                PushEnvironment = pushEnvironment;
                DeepLinkDomains = deepLinkDomains;
                EnabledInviteChannels = enabledInviteChannels;
            }

            public static PlatformConfig FromDict(Dictionary<string,object> valueDictionary)
            {
                return new PlatformConfig(
                    (bool) valueDictionary["enabled"],
                    (bool) valueDictionary["push_enabled"],
                    (string) valueDictionary["push_environment"],
                    new List<string>(((List<object>) valueDictionary["domains"]).ConvertAll(v => (string)v)),
                    new List<string>(((List<object>) valueDictionary["providers"]).ConvertAll(v => (string)v))
                );
            }
        }
    }
}
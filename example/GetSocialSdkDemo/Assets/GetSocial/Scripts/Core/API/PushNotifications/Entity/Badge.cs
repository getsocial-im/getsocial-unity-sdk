
#if UNITY_ANDROID
using UnityEngine;
#elif UNITY_IOS
using System.Collections.Generic;
using GetSocialSdk.MiniJSON;
#endif

namespace GetSocialSdk.Core
{
    public sealed class Badge: IConvertableToNative
    {
        
        internal int Value = int.MinValue;
        internal int Increase;

        public Badge()
        {
            
        }
        
        /// <summary>
        /// Recipient badge will be increased by value.
        /// </summary>
        /// <param name="value">Value to increase badge by.</param>
        /// <returns>New badge instance.</returns>
        public static Badge IncreaseBy(int value)
        {
            var badge = new Badge();
            badge.Increase = value;
            return badge;
        }
        /// <summary>
        /// Recipient badge will be set to value.
        /// </summary>
        /// <param name="value"Value to be set as badge.></param>
        /// <returns>New badge instance.</returns>
        public static Badge SetTo(int value)
        {
            var badge = new Badge();
            badge.Value = value;
            return badge;
        }

#if UNITY_ANDROID
        
        public AndroidJavaObject ToAjo()
        {
            if (Value == int.MinValue)
            {
                return new AndroidJavaClass("im.getsocial.sdk.pushnotifications.NotificationContent$Badge")
                    .CallStaticAJO("increaseBy", Increase);
            } else 
            {
                return new AndroidJavaClass("im.getsocial.sdk.pushnotifications.NotificationContent$Badge")
                    .CallStaticAJO("setTo", Value);
            }
        }
#elif UNITY_IOS
        public string ToJson()
        {
            var dictionary = new Dictionary<string, object>();
            if (Value == int.MinValue)
            {
                dictionary["Increase"] = Increase;
            }
            else 
            {
                dictionary["Value"] = Value;
            }
            return GSJson.Serialize(dictionary);
        }
#endif
    }
}
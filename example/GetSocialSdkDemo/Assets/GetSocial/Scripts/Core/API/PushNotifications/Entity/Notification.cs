using System;
using System.Collections.Generic;

#if UNITY_ANDROID
using UnityEngine;
#endif

#if UNITY_IOS
using System.Linq;
#endif

namespace GetSocialSdk.Core
{
    public class Notification : IGetSocialBridgeObject<Notification>
    {
        /// <summary>
        /// Enumeration that allows you to have convenient switch for your action.
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// Custom action.
            /// </summary>
            Custom = 0,
            /// <summary>
            /// Activity with provided identifier should be opened.
            /// </summary>
            OpenActivity,

            /// <summary>
            /// Profile with provided identifier should be opened.
            /// </summary>
            OpenProfile
        }

        /// <summary>
        /// Contains all predefined keys for <see cref="ActionData"/> dictionary.
        /// </summary>
        public static class Key
        {
            public static class OpenActivity
            {
                public const string ActivityId = "$activity_id";
                public const string CommentId = "$comment_id";
            }

            public static class OpenProfile
            {
                public const string UserId = "$user_id";
            }
        }

        public Type Action { get; private set; }
        public string Title { get; private set; }
        public string Text { get; private set; }
        public Dictionary<string, string> ActionData { get; private set; }

#if UNITY_ANDROID

        public AndroidJavaObject ToAJO()
        {
            throw new NotImplementedException("NotificationAction is never sent to Android");
        }

        public Notification ParseFromAJO(AndroidJavaObject ajo)
        {
            Action = (Type) ajo.CallAJO("getAction").CallInt("ordinal");
            Title = ajo.CallStr("getTitle");
            Text = ajo.CallStr("getText");
            ActionData = ajo.CallAJO("getActionData").FromJavaHashMap();
            return this;
        }

#elif UNITY_IOS

        public string ToJson()
        {
            throw new NotImplementedException("NotificationAction is never sent to iOS");
        }

        public Notification ParseFromJson(Dictionary<string, object> dictionary)
        {
            Title = dictionary["Title"] as string;
            Text = dictionary["Text"] as string;
            ActionData = (dictionary["Data"] as Dictionary<string, object>).ToStrStrDict();
            Action = (Type) (long) dictionary["Type"];
            return this;
        }
#endif

        
    }
}
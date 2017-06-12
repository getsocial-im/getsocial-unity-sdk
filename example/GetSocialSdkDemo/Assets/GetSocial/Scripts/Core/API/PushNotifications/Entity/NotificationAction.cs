using System;
using System.Collections.Generic;
using UnityEngine;

namespace GetSocialSdk.Core
{
    public class NotificationAction : IGetSocialBridgeObject<NotificationAction>
    {
        /// <summary>
        /// Enumeration that allows you to have convenient switch for your action.
        /// </summary>
        public enum ActionType
        {
            /// <summary>
            /// Action is type of <see cref="OpenActivityAction"/>, Activity with provided identifier should be opened.
            /// </summary>
            OpenActivity = 0,

            /// <summary>
            /// Action is type of <see cref="OpenProfileAction"/>, Profile with provided identifier should be opened.
            /// </summary>
            OpenProfile
        }

        public virtual ActionType Type {
            get
            {
                throw new NotImplementedException("Should be overriden");
            }
        }

#if UNITY_ANDROID

        public AndroidJavaObject ToAJO()
        {
            throw new NotImplementedException("NotificationAction is never sent to Android");
        }

        public NotificationAction ParseFromAJO(AndroidJavaObject ajo)
        {
            AndroidJavaObject type = ajo.CallAJO("getAction");
            ActionType actionType = (ActionType) type.CallInt("ordinal");
            switch (actionType)
            {
                case ActionType.OpenActivity:
                    return new OpenActivityAction(ajo.CallStr("getActivityId"));

                case ActionType.OpenProfile:
                    return new OpenProfileAction(ajo.CallStr("getUserId"));

                default:
                    return null;
            }
        }

#elif UNITY_IOS

        public string ToJson()
        {
            throw new NotImplementedException("NotificationAction is never sent to iOS");
        }

        public NotificationAction ParseFromJson(Dictionary<string, object> dictionary)
        {
            switch (dictionary["Type"] as string)
            {
                case "OPEN_ACTIVITY":
                    return new OpenActivityAction(dictionary["ActivityId"] as string);

                case "OPEN_PROFILE":
                    return new OpenProfileAction(dictionary["UserId"] as string);

                default:
                    return null;
            }
        }
#endif

        
    }
}
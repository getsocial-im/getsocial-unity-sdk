
#if UNITY_ANDROID
using UnityEngine;
#elif UNITY_IOS
using System.Collections.Generic;
using GetSocialSdk.MiniJSON;
#endif

namespace GetSocialSdk.Core
{
    public sealed class ActionButton: IConvertableToNative, IConvertableFromNative<ActionButton>
    {

        public const string ConsumeAction = "consume";
        public const string IgnoreAction = "ignore";
        
        public string Title { get; private set; }
        
        /// <summary>
        /// One of constants(<see cref="ConsumeAction"/>, <see cref="IgnoreAction"/>) or any custom value.
        /// </summary>
        public string Id { get; private set; }

        public ActionButton()
        {
            
        }

        /// <summary>
        /// Create a new button.
        /// </summary>
        /// <param name="title">Title to be displayed</param>
        /// <param name="actionId">Action ID - could be one of constants(<see cref="ConsumeAction"/>, <see cref="IgnoreAction"/>) or any custom value.</param>
        /// <returns></returns>
        public static ActionButton Create(string title, string actionId)
        {
            return new ActionButton { Title = title, Id = actionId };
        }

        private bool Equals(ActionButton other)
        {
            return string.Equals(Title, other.Title) && string.Equals(Id, other.Id);
        }
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is ActionButton && Equals((ActionButton) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Title != null ? Title.GetHashCode() : 0) * 397) ^ (Id != null ? Id.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return string.Format("Title: {0}, Id: {1}", Title, Id);
        }

#if UNITY_ANDROID
        
        public AndroidJavaObject ToAjo()
        {
            return new AndroidJavaClass("im.getsocial.sdk.pushnotifications.ActionButton")
                .CallStaticAJO("create", Title, Id);
        }
        
        public ActionButton ParseFromAJO(AndroidJavaObject ajo)
        {
            Title = ajo.CallStr("getTitle");
            Id = ajo.CallStr("getId");
            return this;
        }
#elif UNITY_IOS
        public string ToJson()
        {
            var dictionary = new Dictionary<string, string>();
            dictionary["Title"] = Title;
            dictionary["Id"] = Id;
            return GSJson.Serialize(dictionary);
        }

        public ActionButton ParseFromJson(Dictionary<string, object> json)
        {
            Id = json["Id"] as string;
            Title = json["Title"] as string;
            
            return this;
        }
#endif
    }
}
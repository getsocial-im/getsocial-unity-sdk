using System;

#if UNITY_ANDROID
using UnityEngine;
#endif

#if UNITY_IOS
using System.Collections.Generic;
#endif

namespace GetSocialSdk.Core
{
    public class Mention : IConvertableFromNative<Mention>
    {
        public string UserId { get; private set; }
        public int StartIndex { get; private set; }
        public int EndIndex { get; private set; }
        
        /// <summary>
        /// Listed in <see cref="MentionTypes"/>.
        /// </summary>
        public string Type { get; private set; }
        
        public override string ToString()
        {
            return string.Format("{0} ({1}, {2}) - {3}", UserId, StartIndex, EndIndex, Type);
        }
        
#if UNITY_IOS
        public Mention ParseFromJson(Dictionary<string, object> json)
        {
            UserId = json["UserId"] as string;
            StartIndex = (int) (long) json["StartIndex"];
            EndIndex = (int) (long) json["EndIndex"];
            Type = json["Type"] as string;
            return this;
        }
#elif UNITY_ANDROID
        public Mention ParseFromAJO(AndroidJavaObject ajo)
        {
            UserId = ajo.CallStr("getUserId");
            StartIndex = ajo.CallInt("getStartIndex");
            EndIndex = ajo.CallInt("getEndIndex");
            Type = ajo.CallStr("getType");
            return this;
        }
#endif
    }
}
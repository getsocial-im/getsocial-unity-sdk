using System;
using System.Collections.Generic;

#if UNITY_ANDROID
using UnityEngine;
#endif

#if UNITY_IOS
using GetSocialSdk.MiniJSON;
#endif

namespace GetSocialSdk.Core
{
    public delegate bool ActionListener(GetSocialAction action);
    
    public class GetSocialAction : IConvertableToNative, IConvertableFromNative<GetSocialAction>
    {
        /// <summary>
        /// Action to perform.
        /// </summary>
        public string Type { get; private set; }
        
        /// <summary>
        /// Keys are one of <see cref="GetSocialActionKeys"/> or any custom.
        /// </summary>
        public Dictionary<string, string> Data { get; private set; }

        internal GetSocialAction()
        {
            //
        }

        public override string ToString()
        {
            return string.Format("Type: {0}, Data: {1}", Type, Data.ToDebugString());
        }

        protected bool Equals(GetSocialAction other)
        {
            return string.Equals(Type, other.Type) && Equals(Data, other.Data);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GetSocialAction) obj);
        }

        public override int GetHashCode()
        {
            return ((Type != null ? Type.GetHashCode() : 0) * 397) ^ (Data != null ? Data.GetHashCode() : 0);
        }

        private GetSocialAction(string type, Dictionary<string, string> data)
        {
            Type = type;
            Data = new Dictionary<string, string>(data);
        }

        /// <summary>
        /// Use one of <see cref="GetSocialActionType"/>.
        /// </summary>
        /// <param name="type">one of <see cref="GetSocialActionType"/></param>
        /// <returns></returns>
        public static Builder CreateBuilder(string type)
        {
            return new Builder(type);
        }

        public class Builder
        {
            private readonly string _type;
            private readonly Dictionary<string, string> _data;
            
            internal Builder(string type)
            {
                _type = type;
                _data = new Dictionary<string, string>();
            }

            public Builder AddActionData(string key, string value)
            {
                _data[key] = value;
                return this;
            }

            public Builder AddActionData(Dictionary<string, string> data)
            {
                _data.AddAll(data);
                return this;
            }

            public GetSocialAction Build()
            {
                return new GetSocialAction(_type, _data);
            }
        }

#if UNITY_ANDROID
        public AndroidJavaObject ToAjo()
        {
            return new AndroidJavaClass("im.getsocial.sdk.actions.Action")
                .CallStaticAJO("builder", Type)
                .CallAJO("addActionData", Data.ToJavaHashMap())
                .CallAJO("build");
        }
        
        public GetSocialAction ParseFromAJO(AndroidJavaObject ajo)
        {
            Type = ajo.CallStr("getType");
            Data = ajo.CallAJO("getData").FromJavaHashMap();

            return this;
        }
#elif UNITY_IOS
        
        public GetSocialAction ParseFromJson(Dictionary<string, object> json)
        {
            Data = (json["Data"] as Dictionary<string, object>).ToStrStrDict();
            Type = (string) json["Type"];
            return this;
        }

        public string ToJson()
        {
            var json = new Dictionary<string, object>
            {
                {"Data", Data},
                {"Type", Type}
            };
            return GSJson.Serialize(json);
        }
#endif
    }
}
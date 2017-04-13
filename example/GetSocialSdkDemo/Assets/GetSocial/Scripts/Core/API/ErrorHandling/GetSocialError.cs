using System;
using System.Collections.Generic;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Error object containing detailed information about error that happened.
    /// </summary>
    public sealed class GetSocialError : IGetSocialBridgeObject<GetSocialError>
    {
        public int ErrorCode { get; private set; }
        public string Message { get; private set; }

        // TODO Simple constructors for creating in Unity
        public GetSocialError(string message)
        {
            Message = message;
            ErrorCode = ErrorCodes.Unknown;
        }

        public GetSocialError()
        {
            ErrorCode = ErrorCodes.Unknown;
        }

        public override string ToString()
        {
            return string.Format("Error code: {0}. Message: {1}", ErrorCode, Message);
        }

#if UNITY_ANDROID
        public UnityEngine.AndroidJavaObject ToAJO()
        {
            throw new NotImplementedException("Error is only received from Android");
        }

        public GetSocialError ParseFromAJO(UnityEngine.AndroidJavaObject ajo)
        {
            using (ajo)
            {
                ErrorCode = ajo.CallInt("getErrorCode");
                Message = ajo.CallStr("getMessage");
            }
            return this;
        }
#elif UNITY_IOS
        public string ToJson()
        {
            throw new NotImplementedException("Error is only received from iOS");
        }

        public GetSocialError ParseFromJson(string json)
        {
            return ParseFromJson(json.ToDict());
        }

        public GetSocialError ParseFromJson(Dictionary<string, object> jsonDic)
        {
            Message = jsonDic[MessageFieldName] as string;
            ErrorCode = (int)(long) jsonDic[ErrorCodeFieldName];

            return this;
        }

        static string ErrorCodeFieldName
        {
            get { return ReflectionUtils.GetMemberName((GetSocialError c) => c.ErrorCode); }
        }

        static string MessageFieldName
        {
            get { return ReflectionUtils.GetMemberName((GetSocialError c) => c.Message); }
        }
#endif
    }
}
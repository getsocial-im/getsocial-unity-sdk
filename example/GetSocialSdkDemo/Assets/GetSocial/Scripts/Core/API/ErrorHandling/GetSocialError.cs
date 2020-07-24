using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Error object containing detailed information about error that happened.
    /// </summary>
    public sealed class GetSocialError 
    {
        [JsonSerializationKey("code")]
        public int ErrorCode { get; private set; }
        [JsonSerializationKey("message")]
        public string Message { get; private set; }

        public GetSocialError(int errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }

        public GetSocialError(string message) : this(ErrorCodes.Unknown, message){}
        
        public GetSocialError() : this(null) {}

        public override string ToString()
        {
            return string.Format("Error code: {0}. Message: {1}", ErrorCode, Message);
        }
    }
}
using System.Collections.Generic;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public delegate void ActionListener(GetSocialAction action);
    
    public class GetSocialAction
    {
        /// <summary>
        /// Action to perform.
        /// </summary>
        [JsonSerializationKey("type")]
        public string Type { get; internal set; }

        /// <summary>
        /// Keys are one of <see cref="GetSocialActionKeys"/> or any custom.
        /// </summary>
        [JsonSerializationKey("data")]
        public Dictionary<string, string> Data { get; internal set; }

        internal GetSocialAction()
        {
            
        }

        internal GetSocialAction(string type, Dictionary<string, string> data)
        {
            Type = type;
            Data = new Dictionary<string, string>(data);
        }

        /// <summary>
        /// Create a new action.
        /// </summary>
        /// <param name="type">Action type.</param>
        /// <param name="data">Custom data.</param>
        /// <returns></returns>
        public static GetSocialAction Create(string type, Dictionary<string, string> data)
        {
            return new GetSocialAction(type, data);
        }

        public override string ToString()
        {
            return $"Type: {Type}, Data: {Data.ToDebugString()}";
        }
    }
}

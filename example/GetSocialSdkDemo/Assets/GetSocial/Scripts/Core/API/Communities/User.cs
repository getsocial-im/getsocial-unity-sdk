using System.Collections.Generic;
using System.Linq;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public class User
    {
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        [JsonSerializationKey("userId")]
        public string Id { get; internal set; }
        
        [JsonSerializationKey("displayName")]
        public string DisplayName { get; internal set; } 
        
        [JsonSerializationKey("avatarUrl")]
        public string AvatarUrl { get; internal set; }
        
        [JsonSerializationKey("identities")]
        public Dictionary<string, string> Identities { get; internal set; }
        
        [JsonSerializationKey("publicProperties")]
        public Dictionary<string, string> PublicProperties { get; internal set; }
        
        [JsonSerializationKey("verified")]
        public bool Verified { get; internal set; }

        public bool IsAnonymous
        {
            get
            {
                return !Identities.Any();
            }
        }

        public bool IsApp
        {
            get
            {
                return "app".Equals(Id);
            }
        }

        public override string ToString()
        {
            return $"Id: {Id}, DisplayName: {DisplayName}, AvatarUrl: {AvatarUrl}, Identities: {Identities.ToDebugString()}, PublicProperties: {PublicProperties.ToDebugString()}, Verified: {Verified}";
        }
    }
}
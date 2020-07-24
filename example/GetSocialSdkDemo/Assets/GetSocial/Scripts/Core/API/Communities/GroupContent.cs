using System.Collections;
using System.Collections.Generic;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public class GroupContent
    {
        [JsonSerializationKey("id")]
        public string Id { get; set; }

        [JsonSerializationKey("title")]
        public string Title { get; set; }

        [JsonSerializationKey("description")] 
        internal string Description { get; set; } 

        [JsonSerializationKey("avatarUrl")]
        internal string AvatarUrl { get; set; } 

// FIXME: use proper type
        [JsonSerializationKey("avatar")]
        internal string Avatar { get; set; } 

        [JsonSerializationKey("permissions")]
        internal Dictionary<CommunitiesAction, MembershipRole> Permissions { get; } 

        [JsonSerializationKey("properties")]
        internal Dictionary<string, string> Properties { get; } 

        [JsonSerializationKey("isDiscoverable")]
        internal bool IsDiscoverable { get; set; } 

        [JsonSerializationKey("isPrivate")]
        internal bool IsPrivate { get; set; } 

        public GroupContent()
        {
            Permissions = new Dictionary<CommunitiesAction, MembershipRole>();
            Properties = new Dictionary<string, string>();
        }

        public GroupContent AddProperty(string key, string value)
        {
            Properties[key] = value;
            return this;
        }

        public GroupContent AddProperties(Dictionary<string, string> properties)
        {
            Properties.AddAll(properties);
            return this;
        }

        public GroupContent AddPermission(CommunitiesAction action, MembershipRole role)
        {
            Permissions[action] = role;
            return this;
        }

        public GroupContent AddPermissions(Dictionary<CommunitiesAction, MembershipRole> permissions)
        {
            Permissions.AddAll(permissions);
            return this;
        }

    }
}
using System.Collections.Generic;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public class GroupContent
    {
        [JsonSerializationKey("id")]
        public string Id { get; internal set; }

        [JsonSerializationKey("title")]
        public string Title { get; set; }

        [JsonSerializationKey("description")] 
        internal string Description { get; set; } 

        [JsonSerializationKey("avatar")]
        internal MediaAttachment Avatar { get; set; } 

        [JsonSerializationKey("permissions")]
        internal Dictionary<CommunitiesAction, MemberRole> Permissions { get; } 

        [JsonSerializationKey("properties")]
        internal Dictionary<string, string> Properties { get; } 

        [JsonSerializationKey("isDiscoverable")]
        internal bool IsDiscoverable { get; set; } 

        [JsonSerializationKey("isPrivate")]
        internal bool IsPrivate { get; set; }

        [JsonSerializationKey("labels")]
        internal List<string> Labels { get; set; }

        public GroupContent(string groupId)
        {
            Id = groupId;
            Permissions = new Dictionary<CommunitiesAction, MemberRole>();
            Properties = new Dictionary<string, string>();
            Labels = new List<string>();
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

        public GroupContent AddPermission(CommunitiesAction action, MemberRole role)
        {
            Permissions[action] = role;
            return this;
        }

        public GroupContent AddPermissions(Dictionary<CommunitiesAction, MemberRole> permissions)
        {
            Permissions.AddAll(permissions);
            return this;
        }

        public GroupContent AddLabel(string label)
        {
            Labels.Add(label);
            return this;
        }

        public GroupContent AddLabels(List<string> labels)
        {
            Labels.AddAll(labels);
            return this;
        }

    }
}
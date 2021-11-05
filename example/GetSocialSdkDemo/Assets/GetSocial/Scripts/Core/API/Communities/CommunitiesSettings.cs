using System;
using System.Collections.Generic;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{

    /// <summary>
    /// CommunitiesSettings entity.
    /// </summary>
    public sealed class CommunitiesSettings
    {
        /// <summary>
        /// Custom properties.
        /// </summary>
        [JsonSerializationKey("properties")]
        public Dictionary<string, string> Properties { get; internal set; }

        [JsonSerializationKey("allowedActions")]
        internal Dictionary<CommunitiesAction, bool> AllowedActions { get; set; }

        [JsonSerializationKey("isPrivate")]
        public bool IsPrivate { get; internal set; }

        [JsonSerializationKey("isDiscoverable")]
        public bool IsDiscoverable { get; internal set; }

        [JsonSerializationKey("permissions")]
        public Dictionary<CommunitiesAction, MemberRole> Permissions { get; internal set; }

        [JsonSerializationKey("labels")]
        public List<string> Labels { get; internal set; }

        /// <summary>
        /// Check if current user is allowed to perform a certain action.
        /// </summary>
        /// <param name="action">action to be checked.</param>
        /// <returns>true, if current user is allowed to perform action, false otherwise.</returns>
        public bool IsActionAllowed(CommunitiesAction action) {
            if (AllowedActions.ContainsKey(action)) {
                return AllowedActions[action];
            }
            return false;
        }

        public CommunitiesSettings()
        {

        }

        public override string ToString()
        {
            return $"Properties: {Properties.ToDebugString()}, AllowedActions: {AllowedActions.ToDebugString()}, IsPrivate: {IsPrivate}, IsDiscoverable: {IsDiscoverable}, Permissions: {Permissions.ToDebugString()}, Labels: {Labels.ToDebugString()}";
        }
    }
}
using System;
using System.Collections.Generic;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public class CommunitiesEntity
    {
        /// <summary>
        /// Type of the entity.
        /// </summary>
        /// <value>Type of the entity.</value>
        [JsonSerializationKey("type")]
        public CommunitiesEntityType Type { get; internal set; }

        /// <summary>
        /// Unique identifier of the entity. If <see cref="Type"/> is User, then it's a user ID. If Topic - topic ID.
        /// Could be null for type App and Unknown.
        /// </summary>
        /// <value>ID of the entity.</value>
        [JsonSerializationKey("id")]
        public string Id { get; internal set; }

        /// <summary>
        /// If current user follows the entity.
        /// </summary>
        /// <value>True if user follows the entity, false otherwise.</value>
        [JsonSerializationKey("isFollower")]
        public bool IsFollower { get; internal set; }

        /// <summary>
        /// Number of followers of the entity.
        /// </summary>
        /// <value>total followers count of this entity.</value>
        [JsonSerializationKey("followersCount")]
        public int FollowersCount { get; internal set; }

        /// <summary>
        /// Title of the topic/group. Display name of the user. Null string otherwise.
        /// </summary>
        /// <value>title/name of the entity.</value>
        [JsonSerializationKey("title")]
        public string Title { get; internal set; }

        /// <summary>
        /// Get avatar URL of the topic/user/group. Null if not set. Null for everything else.
        /// </summary>
        /// <value>avatar URL of entity if present.</value>
        [JsonSerializationKey("avatarUrl")]
        public string AvatarUrl { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        [JsonSerializationKey("availableActions")]
        internal Dictionary<CommunitiesAction, bool> AvailableActions { get; set; }

        /// <summary>
        /// Check if current user is allowed to perform a certain action.
        /// </summary>
        /// <param name="action">action to be checked.</param>
        /// <returns>true, if current user is allowed to perform action, false otherwise.</returns>
        public bool IsActionAllowed(CommunitiesAction action)
        {
            return AvailableActions.ContainsKey(action) && AvailableActions[action];
        }

        public override string ToString()
        {
            return $"Type: {Type}, Id: {Id}, IsFollower: {IsFollower}, FollowersCount: {FollowersCount}, Title: {Title}, AvatarUrl: {AvatarUrl}, AvailableActions: {AvailableActions.ToDebugString()}";
        }
    }
}
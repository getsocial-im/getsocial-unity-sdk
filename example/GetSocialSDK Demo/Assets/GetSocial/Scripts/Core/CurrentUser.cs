/**
 *     Copyright 2015-2016 GetSocial B.V.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Describes the current user.
    /// </summary>
    public class CurrentUser
    {
        /// <summary>
        /// Get social user identity conflict resolution strategy.
        /// </summary>
        public enum AddIdentityConflictResolutionStrategy
        {
            /// <summary>
            /// Current user is used and the new identity won't be added since it already belongs to the remote user
            /// </summary>
            Current = 1,
            /// <summary>
            /// Remote user that contains added identity
            /// </summary>
            Remote = 2
        }

        /// <summary>
        /// Possible results of adding user identity
        /// </summary>
        public enum AddIdentityResult
        {
            /// <summary>
            ///  Identity was successfully added without a conflict
            /// </summary>
            SuccessfullyAddedIdentity = 1,
            /// <summary>
            /// Current user is kept and the new identity won't be added since it already belongs to the remote user
            /// </summary>
            ConflictWasResolvedWithCurrent = 2,
            /// <summary>
            /// Current user is replaced with remote user
            /// </summary>
            ConflictWasResolvedWithRemote = 3
        }

        /// <summary>
        /// Delefate of the method that will be invoked when adding the identity to the user if the identity already belongs to another user
        ///
        /// Based on the received current and remote user information you can prompt user to decide or decide yourself.
        /// To finalize the callback you MUST ALWAYS call <code>resolveConflictAction</code> passing the resolution strategy you chose.
        /// </summary>
        public delegate void OnAddIdentityConflictDelegate(
            User currentUser,User remoteUser,Action<AddIdentityConflictResolutionStrategy> resolveConflictAction);

        private readonly IGetSocialNativeBridge nativeBridge;

        internal CurrentUser(IGetSocialNativeBridge nativeBridge)
        {
            this.nativeBridge = nativeBridge;
        }

        /// <summary>
        /// Unique Identifier of the user.
        /// </summary>
        public string Guid
        {
            get { return nativeBridge.UserGuid; }
        }

        /// <summary>
        /// Display name of the user.
        /// </summary>
        public string DisplayName
        {
            get { return nativeBridge.UserDisplayName; }
        }

        /// <summary>
        /// Sets a new display name for the user
        /// </summary>
        /// <param name="displayName">New display name of the user.</param>
        /// <param name="onSuccess">On success callback.</param>
        /// <param name="onFailure">On failure callback.</param>
        public void SetDisplayName(string displayName, Action onSuccess, Action<string> onFailure)
        {
            Check.Argument.IsStrNotNullOrEmpty(displayName, "displayName", "Display name can not be null or empty");
            Check.Argument.IsNotNull(onSuccess, "onSuccess", "Success callback cannot be null");
            Check.Argument.IsNotNull(onFailure, "onFailure", "Failure callback cannot be null");

            nativeBridge.SetDisplayName(displayName, onSuccess, onFailure);
        }

        /// <summary>
        /// The Url of the Avatar of the user. <code>null</code> if no avatar Url is available.
        /// </summary>
        public string AvatarUrl
        {
            get { return nativeBridge.UserAvatarUrl; }
        }

        /// <summary>
        /// Sets a new avatar url for the user.
        /// </summary>
        /// <param name="avatarUrl">New avatar url for the user</param>
        /// <param name="onSuccess">On success callback.</param>
        /// <param name="onFailure">On failure callback.</param>
        public void SetAvatarUrl(string avatarUrl, Action onSuccess, Action<string> onFailure)
        {
            Check.Argument.IsStrNotNullOrEmpty(avatarUrl, "avatarUrl", "Avatar url name can not be null or empty");
            Check.Argument.IsNotNull(onSuccess, "onSuccess", "Success callback cannot be null");
            Check.Argument.IsNotNull(onFailure, "onFailure", "Failure callback cannot be null");

            nativeBridge.SetAvatarUrl(avatarUrl, onSuccess, onFailure);
        }

        /// <summary>
        /// Indicates if the user has at least one identity available
        /// </summary>
        /// <value><code>true</code> if user does not have any identities; otherwise, <code>false</code>.</value>
        public bool IsAnonymous
        {
            get { return nativeBridge.IsUserAnonymous; }
        }

        /// <summary>
        /// Identities of the user
        /// </summary>
        public ReadOnlyCollection<string> Identities
        {
            get { return Array.AsReadOnly(nativeBridge.UserIdentities); }
        }

        /// <summary>
        /// Determines whether the user has identity for the specified provider.
        /// </summary>
        /// <returns><code>true</code> if this instance has identity for the specified provider; otherwise, <code>false</code>.</returns>
        public bool HasIdentityForProvider(string provider)
        {
            Check.Argument.IsStrNotNullOrEmpty(provider, "provider", "Provider can not be null or empty");

            return nativeBridge.UserHasIdentityForProvider(provider);
        }

        /// <summary>
        /// Gets the user id for the specified provider.
        /// </summary>
        /// <returns>The user id for provider, null if user doesn't have the specified provider present</returns>
        public string GetIdForProvider(string provider)
        {
            Check.Argument.IsStrNotNullOrEmpty(provider, "provider", "Provider can not be null or empty");
            
            return HasIdentityForProvider(provider) ? nativeBridge.GetUserIdForProvider(provider) : null;
        }

        /// <summary>
        /// Adds identity to the current user.
        ///
        /// If the identity you are trying to add already belongs to another user it will invoke <code>onConflict</code> callback passing
        /// information about current user, remote user the identity you are trying to add already belongs to, and action to finalize the callback.
        ///
        /// If you implement <code>onConflict</code> callback you MUST ALWAYS call finalize action pasing your conflict resolution strategy as a parameter.
        /// If you do not provide <code>onConflict</code> callback SDK will substitute the current user with the user identity already belongs to.
        ///
        /// After the finalized action is called you are guaranteed to receive either <code>onComplete</code> or <code>onFailure</code> callback.
        /// </summary>
        /// <param name="identity">Identity info to be added</param>
        /// <param name="onComplete">Invoked when adding identity is completed. The result describing how it completed will be passed as parameter <see cref="AddIdentityResult"/></param>
        /// <param name="onFailure">Invoked when adding identity failed</param>
        ///
        /// <param name="onConflict">
        /// Invoked when the identity already belongs to another user and the conflict needs to be resolved.
        /// </param>
        public void AddUserIdentity(UserIdentity identity,
                                    Action<AddIdentityResult> onComplete,
                                    Action<string> onFailure,
                                    CurrentUser.OnAddIdentityConflictDelegate onConflict = null)
        {
            Check.Argument.IsNotNull(identity, "identity", "Identity cannot be null");
            Check.Argument.IsNotNull(onComplete, "onComplete", "Complete callback cannot be null");
            Check.Argument.IsNotNull(onFailure, "onFailure", "Failure callback cannot be null");

            nativeBridge.AddUserIdentity(identity.Serialize().ToString(), onComplete, onFailure, onConflict);
        }

        /// <summary>
        /// Removes User identity for the specified provider.
        /// </summary>
        /// <param name="providerId">Id of the provider.</param>
        /// <param name="onSuccess">On success callback.</param>
        /// <param name="onFailure">On failure callback. Error message is passed as a parameter.</param>
        public void RemoveUserIdentity(string providerId,
                                       Action onSuccess,
                                       Action<string> onFailure)
        {
            Check.Argument.IsStrNotNullOrEmpty(providerId, "providerId", "Provider id can not be null or empty");
            
            Check.Argument.IsNotNull(onSuccess, "onSuccess", "Success callback cannot be null");
            Check.Argument.IsNotNull(onFailure, "onFailure", "Failure callback cannot be null");

            nativeBridge.RemoveUserIdentity(providerId, onSuccess, onFailure);
        }

        /// <summary>
        /// Resets the current user and generates a new anonymous user.
        /// </summary>
        /// <param name="onSuccess">On success callback.</param>
        /// <param name="onFailure">On failure callback. Error message is passed as a parameter.</param>
        public void Reset(Action onSuccess, Action<string> onFailure)
        {
            Check.Argument.IsNotNull(onSuccess, "onSuccess", "Success callback cannot be null");
            Check.Argument.IsNotNull(onFailure, "onFailure", "Failure callback cannot be null");

            nativeBridge.ResetUser(onSuccess, onFailure);
        }

        #region social_graph_API
        /// <summary>
        /// Adds specified user to list of users who the current user is following
        /// </summary>
        /// <param name="user">User to follow.</param>
        /// <param name="onSuccess">On success callback.</param>
        /// <param name="onFailure">On failure callback. Error message is passed as a parameter.</param>
        public void FollowUser(User user, Action onSuccess, Action<string> onFailure)
        {
            nativeBridge.FollowUser(user.Guid, onSuccess, onFailure);
        }
        /// <summary>
        /// Adds user with specified userId and provider to list of users who the current user is following
        /// </summary>
        /// <param name="provider">Provider Id.</param>
        /// <param name="userId">User id on provider</param>
        /// <param name="onSuccess">On success callback.</param>
        /// <param name="onFailure">On failure callback. Error message is passed as a parameter.</param>
        public void FollowUser(string provider, string userId, Action onSuccess, Action<string> onFailure)
        {
            nativeBridge.FollowUser(provider, userId, onSuccess, onFailure);
        }
        /// <summary>
        /// Removes specified user from the list of users who the current user is following
        /// </summary>
        /// <param name="user">User to unfollow.</param>
        /// <param name="onSuccess">On success callback.</param>
        /// <param name="onFailure">On failure callback. Error message is passed as a parameter.</param>
        public void UnfollowUser(User user, Action onSuccess, Action<string> onFailure)
        {
            nativeBridge.UnfollowUser(user.Guid, onSuccess, onFailure);
        }
        /// <summary>
        /// Removes user with specified userId and provider from the list of users who the current user is following
        /// </summary>
        /// <param name="provider">Provider Id.</param>
        /// <param name="userId">User id on provider</param>
        /// <param name="onSuccess">On success callback.</param>
        /// <param name="onFailure">On failure callback. Error message is passed as a parameter.</param>
        public void UnfollowUser(string provider, string userId, Action onSuccess, Action<string> onFailure)
        {
            nativeBridge.UnfollowUser(provider, userId, onSuccess, onFailure);
        }
        /// <summary>
        /// Gets the list of users that current user is following.
        /// </summary>
        /// <param name="offset">Offset from which users will be retrieved.</param>
        /// <param name="count">Number of users to retireve.</param>
        /// <param name="onSuccess">On success. List of retrieved users is passed as a parameter.</param>
        /// <param name="onFailure">On failure callback. Error message is passed as a parameter.</param>
        public void GetFollowing(int offset, int count, Action<List<User>> onSuccess, Action<string> onFailure)
        {
            nativeBridge.GetFollowing(offset, count, onSuccess, onFailure);
        }
        /// <summary>
        /// Gets the list of users that are following current user.
        /// </summary>
        /// <param name="offset">Offset from which users will be retrieved.</param>
        /// <param name="count">Number of users to retireve.</param>
        /// <param name="onSuccess">On success. List of retrieved users is passed as a parameter.</param>
        /// <param name="onFailure">On failure callback. Error message is passed as a parameter.</param>
        public void GetFollowers(int offset, int count, Action<List<User>> onSuccess, Action<string> onFailure)
        {
            nativeBridge.GetFollowers(offset, count, onSuccess, onFailure);
        }
        #endregion
    }
}

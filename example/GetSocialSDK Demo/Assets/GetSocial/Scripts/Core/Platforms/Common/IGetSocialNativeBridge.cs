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
using System.Collections.Generic;

namespace GetSocialSdk.Core
{
    public interface IGetSocialNativeBridge
    {
        IConfiguration Configuration { get; }

        #region properties
        bool IsInitialized { get; }

        string Version { get; }

        string ApiVersion { get; }

        string Environment { get; }
        #endregion

        #region localization
        void SetLanguage(string languageCode);
        #endregion

        #region sdk_initialization
        void Init(string key, Action onSuccess = null, Action onFailure = null);

        void RegisterPlugin(string providerId, IPlugin plugin);
        #endregion

        #region current_user
        string UserGuid { get; }

        string UserDisplayName { get; }

        string UserAvatarUrl { get; }

        bool IsUserAnonymous { get; }

        string[] UserIdentities { get; }

        bool UserHasIdentityForProvider(string provider);

        string GetUserIdForProvider(string provider);

        void SetDisplayName(string displayName, Action onSuccess, Action<string> onFailure);

        void SetAvatarUrl(string avatarUrl, Action onSuccess, Action<string> onFailure);

        void AddUserIdentity(string serializedIdentity,
            Action<CurrentUser.AddIdentityResult> onComplete,
            Action<string> onFailure,
            CurrentUser.OnAddIdentityConflictDelegate onConflict);

        void RemoveUserIdentity(string provider, Action onSuccess, Action<string> onFailure);

        void ResetUser(Action onSuccess, Action<string> onFailure);
        #endregion

        #region view_management
        void ShowView(string serializedViewBuilder, ViewBuilder.OnViewActionDelegate onViewAction = null);

        void CloseView(bool saveViewState);

        void RestoreView();

        void SetOnWindowStateChangeListener(Action onOpen, Action onClose);
        #endregion

        #region cloud_save
        void Save(string state, Action onSuccess = null, Action<string> onFailure = null);

        void GetLastSave(Action<string> onSuccess, Action<string> onFailure);
        #endregion

        void PostActivity(string text, byte[] image, string buttonText, string actionId, string[] tags, Action<string> onSuccess, Action onFailure);

        void SetOnInviteFriendsListener(Action onInviteFriendsIntent, Action<int> onFriendsInvited);

        void SetOnUserActionPerformedListener(OnUserActionPerformed onUserActionPerformed);

        void SetOnUserAvatarClickListener(OnUserAvatarClick onUserAvatarClick);

        void SetOnAppAvatarClickListener(OnAppAvatarClick onAppAvatarClick);

        void SetOnActivityActionClickListener(OnActivityActionClick onActivityActionClick);

        void SetOnInviteButtonClickListener(OnInviteButtonClick onInviteButtonClick);

        void SetOnUserGeneratedContentListener(OnUserGeneratedContent onUserGeneratedContent);

        void SetOnReferralDataReceivedListener(OnReferralDataReceived onReferralDataReceived);

        string[] GetSupportedInviteProviders();

        void InviteFriendsUsingProvider(string provider, String subject = null, string text = null, byte[] image = null, IDictionary<string, string> referralData = null);

        #region notifications
        int UnreadNotificationsCount { get; }

        void SetOnUnreadNotificationsCountChangeListener(Action<int> onUnreadNotificationsCountChange);
        #endregion

        #region leaderboards
        void GetLeaderboard(string leaderboardId, Action<string> onSuccess, Action<string> onFailure = null);

        void GetLeaderboards(HashSet<string> leaderboardIds, Action<string> onSuccess, Action<string> onFailure = null);

        void GetLeaderboards(int offset, int count, Action<string> onSuccess, Action<string> onFailure = null);

        void GetLeaderboardScores(string leaderboardId, int offset, int count, LeaderboardScoreType scoreType, Action<string> onSuccess, Action<string> onFailure = null);

        void SubmitLeaderboardScore(string leaderboardId, int score, Action<int> onSuccess = null, Action onFailure = null);
        #endregion

        #region social_graph_API
        void FollowUser(String guid, Action onSuccess, Action<string> onFailure);

        void FollowUser(string provider, string userId, Action onSuccess, Action<string> onFailure);
        
        void UnfollowUser(String guid, Action onSuccess, Action<string> onFailure);

        void UnfollowUser(string provider, string userId, Action onSuccess, Action<string> onFailure);
        
        void GetFollowing(int offset, int count, Action<List<User>> onSuccess, Action<string> onFailure);

        void GetFollowers(int offset, int count, Action<List<User>> onSuccess, Action<string> onFailure);
        #endregion
    }
}

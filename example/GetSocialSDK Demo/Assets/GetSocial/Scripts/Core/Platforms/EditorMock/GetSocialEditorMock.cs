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
using System.Reflection;

namespace GetSocialSdk.Core
{
    internal sealed class GetSocialEditorMock : IGetSocialNativeBridge
    {
        private static IGetSocialNativeBridge instance;

        private GetSocialEditorMock()
        {
        }

        internal static IGetSocialNativeBridge GetInstance()
        {
            return instance ?? (instance = new GetSocialEditorMock());
        }

        public bool IsInitialized
        {
            get { return false; }
        }

        public IConfiguration Configuration
        {
            get { return ConfigurationEditorMock.GetInstance(); }
        }

        public int UnreadNotificationsCount
        {
            get { return 0; }
        }

        public string Version
        {
            get { return "mock"; }
        }

        public string ApiVersion
        {
            get { return "mock"; }
        }

        public string Environment
        {
            get { return "mock"; }
        }

        #region current_user
        public string UserGuid { get; private set; }

        public string UserDisplayName { get; private set; }

        public string UserAvatarUrl { get; private set; }

        public bool IsUserAnonymous { get; private set; }

        public string[] UserIdentities { get; private set; }

        public bool UserHasIdentityForProvider(string provider)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), provider);
            return false;
        }

        public string GetUserIdForProvider(string provider)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), provider);
            return string.Empty;
        }

        public void SetDisplayName(string displayName, Action onSuccess, Action<string> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), displayName, onSuccess, onFailure);
        }

        public void SetAvatarUrl(string avatarUrl, Action onSuccess, Action<string> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), avatarUrl, onSuccess, onFailure);
        }

        public void AddUserIdentity(string serializedIdentity, Action<CurrentUser.AddIdentityResult> onComplete, Action<string> onFailure,
                                    CurrentUser.OnAddIdentityConflictDelegate onConflict)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), serializedIdentity, onComplete, onFailure, onConflict);
        }

        public void RemoveUserIdentity(string provider, Action onSuccess, Action<string> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), provider, onSuccess, onFailure);
        }

        public void ResetUser(Action onSuccess, Action<string> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), onSuccess, onFailure);
        }

        public void FollowUser(String guid, Action onSuccess, Action<string> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), guid, onSuccess, onFailure);
        }

        public void FollowUser(string provider, string userId, Action onSuccess, Action<string> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), provider, userId, onSuccess, onFailure);
        }

        public void UnfollowUser(String guid, Action onSuccess, Action<string> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), guid, onSuccess, onFailure);
        }

        public void UnfollowUser(string provider, string userId, Action onSuccess, Action<string> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), provider, userId, onSuccess, onFailure);
        }

        public void GetFollowing(int offset, int count, Action<List<User>> onSuccess, Action<string> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), offset, count, onSuccess, onFailure);
        }

        public void GetFollowers(int offset, int count, Action<List<User>> onSuccess, Action<string> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), offset, count, onSuccess, onFailure);
        }
        #endregion

        public void Init(string key, Action onSuccess, Action onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), key, onSuccess, onFailure);
        }

        public void RegisterPlugin(string providerId, IPlugin plugin)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), providerId, plugin);
        }

        public void ShowView(string serializedViewBuilder, ViewBuilder.OnViewActionDelegate onViewAction)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), serializedViewBuilder, onViewAction);
        }

        public void CloseView(bool saveViewState)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), saveViewState);
        }

        public void RestoreView()
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod());
        }

        public void Login(string serializedIdentityInfo, Action onSuccess, Action<string> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), serializedIdentityInfo, onSuccess, onFailure);
        }

        public void Logout(Action onComplete)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), onComplete);
        }

        public void AddUserIdentityInfo(string serializedIdentityInfo, Action onSuccess, Action<string> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), serializedIdentityInfo, onSuccess, onFailure);
        }

        public void RemoveUserIdentityInfo(string providerId, Action onSuccess, Action<string> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), providerId, onSuccess, onFailure);
        }

        public void Save(string state, Action onSuccess = null, Action<string> onFailure = null)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), state, onSuccess, onFailure);
        }

        public void GetLastSave(Action<string> onSuccess, Action<string> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), onSuccess, onFailure);
        }

        public void PostActivity(string text, byte[] image, string buttonText, string actionId, string[] tags,
                                 Action<string> onSuccess, Action onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), text, image, buttonText, actionId, tags, onSuccess,
                onFailure);
        }

        public void SetOnWindowStateChangeListener(Action onOpen, Action onClose)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), onOpen, onClose);
        }

        public void SetOnInviteFriendsListener(Action onInviteFriendsIntent, Action<int> onFriendsInvited)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), onInviteFriendsIntent, onFriendsInvited);
        }

        public void SetOnUserActionPerformedListener(OnUserActionPerformed onUserActionPerformed)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), onUserActionPerformed);
        }

        public void SetOnUserAvatarClickListener(OnUserAvatarClick onUserAvatarClick)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), onUserAvatarClick);
        }

        public void SetOnAppAvatarClickListener(OnAppAvatarClick onAppAvatarClick)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), onAppAvatarClick);
        }

        public void SetOnActivityActionClickListener(OnActivityActionClick onActivityActionClick)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), onActivityActionClick);
        }

        public void SetOnUnreadNotificationsCountChangeListener(Action<int> onUnreadNotificationsCountChange)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), onUnreadNotificationsCountChange);
        }

        public void SetOnInviteButtonClickListener(OnInviteButtonClick onInviteButtonClick)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), onInviteButtonClick);
        }

        public void SetOnUserGeneratedContentListener(OnUserGeneratedContent onUserGeneratedContent)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), onUserGeneratedContent);
        }

        public void SetOnReferralDataReceivedListener(OnReferralDataReceived onReferralDataReceived)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), onReferralDataReceived);
        }

        public void SetLanguage(string languageCode)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), languageCode);
        }

        public string[] GetSupportedInviteProviders()
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod());
            return new string[0];
        }

        public void InviteFriendsUsingProvider(string provider, string subject = null, string text = null,
                                               byte[] image = null, IDictionary<string, string> referralData = null)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), provider, subject, text, image, referralData);
        }

        public void GetLeaderboard(string leaderboardId, Action<string> onSuccess, Action<string> onFailure = null)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), leaderboardId, onSuccess, onFailure);
        }

        public void GetLeaderboards(HashSet<string> leaderboardIds, Action<string> onSuccess,
                                    Action<string> onFailure = null)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), leaderboardIds, onSuccess, onFailure);
        }

        public void GetLeaderboards(int offset, int count, Action<string> onSuccess, Action<string> onFailure = null)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), offset, count, onSuccess, onFailure);
        }

        public void GetLeaderboardScores(string leaderboardId, int offset, int count, LeaderboardScoreType scoreType,
                                         Action<string> onSuccess, Action<string> onFailure = null)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), leaderboardId, offset, count, scoreType, onSuccess,
                onFailure);
        }

        public void SubmitLeaderboardScore(string leaderboardId, int score, Action<int> onSuccess = null,
                                           Action onFailure = null)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), leaderboardId, score, onSuccess, onFailure);
        }
    }
}

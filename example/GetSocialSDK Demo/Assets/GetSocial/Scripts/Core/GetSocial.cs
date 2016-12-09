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
    /// <summary>
    /// Delegate of the method that will be invoked when the user clicks on another user avatar.
    /// </summary>
    /// <param name="user">User whose avatar was clicked</param>
    /// <param name="source">View where avatar was clicked</param>
    public delegate bool OnUserAvatarClick(User user, GetSocial.SourceView source);

    /// <summary>
    /// Delegate of the method that will be invoked when the user clicks on application avatar.
    /// </summary>
    public delegate bool OnAppAvatarClick();

    /// <summary>
    /// Delegate of the method that will be invoked when the user clicks on the invite button.
    /// </summary>
    public delegate bool OnInviteButtonClick();

    /// <summary>
    /// Delegate of the method that will be invoked when the referral data is received.
    /// </summary>
    public delegate void OnReferralDataReceived(List<Dictionary<string, string>> referralData);

    /// <summary>
    /// Delegate of the method that will be invoked when the user clicks on activity action.
    ///
    /// Action id is specified on GetSocial Dashboard when the post with action is created
    /// </summary>
    /// <param name="actionId">Id of the activity action the user clicked</param>
    public delegate void OnActivityActionClick(string actionId);

    /// <summary>
    /// Delegate of the method that will be invoked when the user tries to perform any of the <see cref="GetSocialSdk.Core.GetSocial.UserPerformedAction"/>.
    /// You can control user permissions to perform certain actions by passing a <c>bool</c> to <c>finalizeAction</c>.
    ///
    /// Finalize action MUST ALWAYS be called to finalize the callback
    /// </summary>
    /// <param name="action">Action that the user is trying to perform</param>
    /// <param name="finalizeAction">
    ///     Action that must be invoked in order to finalize.
    ///     Pass a <c>bool</c> to specify whether the the action will be performed.
    /// </param>
    public delegate void OnUserActionPerformed(GetSocial.UserPerformedAction action, Action<bool> finalizeAction);

    /// <summary>
    /// Delegate of the method that will be called when a user has generated content and wants to post this to your GetSocial community.
    ///
    /// Implementing this callback allows you to verify the content adheres to your standards or modify/refuse
    /// the content if it does not.
    /// </summary>
    /// <returns>The approved content to send or null if the content should not be sent.</returns>
    public delegate string OnUserGeneratedContent(GetSocial.ContentSource contentSource, string content);

    /// <summary>
    /// Singleton class to interact with GetSocial SDK.
    /// </summary>
    public sealed class GetSocial
    {
        #region constants
        /// <summary>
        /// Scale mode for windows.
        /// </summary>
        public enum ScaleMode
        {
            /// <summary>
            /// Makes UI elements bigger the bigger the screen is.
            /// </summary>
            ScaleWithScreenSize = 1
        }

        /// <summary>
        /// Animation style for windows.
        /// </summary>
        public enum AnimationStyle
        {
            /// <summary>
            /// Windows are not instantly shown and hidden
            /// </summary>
            None = 0,
            /// <summary>
            /// Scale animation
            /// </summary>
            Scale = 1,
            /// <summary>
            /// Fade animation
            /// </summary>
            Fade = 2,
            /// <summary>
            /// Fade and Scale animation
            /// </summary>
            FadeAndScale = 3
        }

        /// <summary>
        /// GetSocial Source View.
        /// </summary>
        public enum SourceView
        {
            /// <summary>
            /// Notifications View.
            /// </summary>
            Notifications = 1,
            /// <summary>
            /// Chat List View.
            /// </summary>
            ChatList = 2,
            /// <summary>
            /// Chat View.
            /// </summary>
            Chat = 3,
            /// <summary>
            /// Chat Room View.
            /// </summary>
            ChatRoom = 4,
            /// <summary>
            /// Activities View.
            /// </summary>
            Activities = 5,
            /// <summary>
            /// Activity Comments View.
            /// </summary>
            Comments = 6,
            /// <summary>
            /// Like List View.
            /// </summary>
            LikeList = 7,
            /// <summary>
            /// Following List View.
            /// </summary>
            FollowingList = 8
        }

        /// <summary>
        /// Possible actions that may be performed by GetSocial user.
        /// </summary>
        public enum UserPerformedAction
        {
            /// <summary>
            /// Open activities
            /// </summary>
            OpenActivities = 1,
            /// <summary>
            /// Open activity
            /// </summary>
            OpenActivityDetails = 2,
            /// <summary>
            /// Post activity
            /// </summary>
            PostActivity = 3,
            /// <summary>
            /// Post comment
            /// </summary>
            PostComment = 4,
            /// <summary>
            /// Like activity
            /// </summary>
            LikeActivity = 5,
            /// <summary>
            /// Like comment
            /// </summary>
            LikeComment = 6,
            /// <summary>
            /// Open chat list
            /// </summary>
            OpenChatList = 7,
            /// <summary>
            /// Open private chat
            /// </summary>
            OpenPrivateChat = 8,
            /// <summary>
            /// Send private chat message
            /// </summary>
            SendPrivateChatMessage = 9,
            /// <summary>
            /// Open public chat
            /// </summary>
            OpenPublicChat = 10,
            /// <summary>
            /// Send public chat message
            /// </summary>
            SendPublicChatMessage = 11,
            /// <summary>
            /// Open notifications
            /// </summary>
            OpenNotifications = 12,
            /// <summary>
            /// Open friends list
            /// </summary>
            OpenFriendsList = 13,
            /// <summary>
            /// Open smart invites
            /// </summary>
            OpenSmartInvites = 14
        }

        /// <summary>
        /// Content type the user is trying to post. Used for moderation.
        /// <see cref="GetSocialSdk.Core.OnUserGeneratedContent"/>
        /// </summary>
        public enum ContentSource
        {
            /// <summary>
            /// The activity post.
            /// </summary>
            Activity = 1,
            /// <summary>
            /// The comment.
            /// </summary>
            Comment = 2,
            /// <summary>
            /// The private chat message.
            /// </summary>
            PrivateChatMessage = 3,
            /// <summary>
            /// The public chat message.
            /// </summary>
            PublicChatMessage = 4
        }

        /// <summary>
        /// This tag is replaced with url to download the app.
        /// </summary>
        public const string AppInviteUrlPlaceholder = "[APP_INVITE_URL]";

        /// <summary>
        /// This tag is replaced with app name configured on the GetSocial Dashboard.
        /// </summary>
        public const string AppNamePlaceholder = "[APP_NAME]";

        /// <summary>
        /// This tag is replaced by the user name.
        /// </summary>
        public const string UserDisplayNamePlaceholder = "[USER_NAME]";

        /// <summary>
        /// This tag is replaced by the app icon URL
        /// </summary>
        public const string AppIconUrlPlaceholder = "[APP_ICON_URL]";

        /// <summary>
        /// This tag is replaced with the package string e.g. <code>im.getsocial.testapp.unity</code>
        /// </summary>
        public const string AppPackageNamePlaceholder = "[APP_PACKAGE_NAME]";
        #endregion

        private static GetSocial instance;
        private IGetSocialNativeBridge getSocialImpl;
        private CurrentUser currentUser;
        private Action onViewOpen;
        private Action onViewClose;

        #region initialization

        private GetSocial()
        {
            getSocialImpl = GetSocialFactory.InstantiateGetSocial();
            currentUser = new CurrentUser(getSocialImpl);

            getSocialImpl.SetOnWindowStateChangeListener(
                () =>
                {
                    IsViewOpened = true;
                    if(onViewOpen != null)
                    {
                        onViewOpen();
                    }
                },
                () =>
                {
                    IsViewOpened = false;
                    if(onViewClose != null)
                    {
                        onViewClose();
                    }
                }
            );
        }

        /// <value><see cref="GetSocialSdk.Core.GetSocial"/> instance.</value>
        public static GetSocial Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new GetSocial();
                }
                return instance;
            }
        }

        #endregion

        #region Unity SDK specific public methods

        /// <summary>
        /// Gets a value indicating whether any GetSocial view is opened.
        /// </summary>
        /// <value><c>true</c> if any GetSocial view is opened; otherwise, <c>false</c>.</value>
        public bool IsViewOpened { get; private set; }

        #endregion

        /// <summary>
        /// Provides the status of the GetSocial initialisation.
        /// Returns true if <see cref="GetSocialSdk.Core.GetSocial.Init"/> completed successfully.
        /// </summary>
        public bool IsInitialized
        {
            get { return getSocialImpl.IsInitialized; }
        }

        /// <summary>
        /// Returns the current user.
        /// </summary>
        public CurrentUser CurrentUser
        {
            get
            {
                return currentUser;
            }
        }

        /// <summary>
        /// Returns class responsible for GetSocial UI configuration
        /// </summary>
        public IConfiguration Configuration
        {
            get { return getSocialImpl.Configuration; }
        }

        /// <summary>
        /// Gets the number of unread activities notifications.
        /// </summary>
        public int UnreadNotificationsCount
        {
            get { return getSocialImpl.UnreadNotificationsCount; }
        }

        /// <summary>
        /// Returns the current SDK version
        /// </summary>
        public string UnitySdkVersion
        {
            get { return BuildConfig.UnitySdkVersion; }
        }

        /// <summary>
        /// Gets the current API version.
        /// </summary>
        public string ApiVersion
        {
            get { return getSocialImpl.ApiVersion; }
        }

        /// <summary>
        /// Returns the underlying native SDK version
        /// </summary>
        public string NativeSdkVersion
        {
            get { return getSocialImpl.Version; }
        }

        #region init_sdk
        /// <summary>
        /// The method sets your app login data, it must be called before using any other functionality offered by the SDK.
        /// Make sure App Key is set up in GetSocial settings before calling this method.
        /// </summary>
        /// <param name="onSuccess">Action called if operation was successful</param>
        /// <param name="onFailure">Action called if operation failed to complete. Optional.</param>
        public void Init(Action onSuccess, Action onFailure = null)
        {
            Init(null, onSuccess, onFailure);
        }

        /// <summary>
        /// The method sets your app login data, it must be called before using any other functionality offered by the SDK.
        /// </summary>
        /// <param name="key">Your app's key, can be found in the GetSocial developer console</param>
        /// <param name="onSuccess">Action called if operation was successful</param>
        /// <param name="onFailure">Action called if operation failed to complete. Optional.</param>
        public void Init(string key, Action onSuccess, Action onFailure = null)
        {
            Check.Argument.IsNotNull(onSuccess, "onSuccess", "Success callback cannot be null");

            if(string.IsNullOrEmpty(key))
            {
                key = GetSocialSettings.AppKey;
            }

            Log.D(string.Format("Init with key {0}", key));
            getSocialImpl.Init(key, onSuccess, onFailure);
        }

        /// <summary>
        /// Register a new instance of a plugin for a specified provider.
        /// All plugins MUST be registered before calling <see cref="Init"/>
        /// </summary>
        /// <param name="providerId">Id of the provider for the plugin implementation.</param>
        /// <param name="plugin">An instance of a plugin implementation</param>
        public void RegisterPlugin(string providerId, IPlugin plugin)
        {
            Check.Argument.IsStrNotNullOrEmpty(providerId, "providerId", "Provider id can't be null or empty");
            Check.Argument.IsNotNull(plugin, "plugin", "Plugin cannot be null or empty");

            getSocialImpl.RegisterPlugin(providerId, plugin);
        }
        #endregion

        #region view_management
        /// <summary>
        /// Creates activity view builder used to open the Activities View.
        /// </summary>
        /// <returns>The ActivitiesViewBuilder instance</returns>
        public ActivitiesViewBuilder CreateActivitiesView()
        {
            return ActivitiesViewBuilder.Construct(getSocialImpl);
        }

        /// <summary>
        /// Creates activity view builder used to open the Activities View.
        /// </summary>
        /// <returns>The ActivitiesViewBuilder instance</returns>
        /// <param name="group">Activities group</param>
        /// <param name="tag">Activities tag</param>
        public ActivitiesViewBuilder CreateActivitiesView(string group, string tag)
        {
            Check.Argument.IsStrNotNullOrEmpty(group, "group", "Group can't be null or empty");
            Check.Argument.IsStrNotNullOrEmpty(tag, "tag", "Tag can't be null or empty");

            return ActivitiesViewBuilder.Construct(getSocialImpl, group, tag);
        }

        /// <summary>
        /// Creates activity view builder used to open the Activities View.
        /// </summary>
        /// <returns>The ActivitiesViewBuilder instance</returns>
        /// <param name="group">Activities group</param>
        /// <param name="tags">Activities tags</param>
        public ActivitiesViewBuilder CreateActivitiesView(string group, params string[] tags)
        {
            Check.Argument.IsStrNotNullOrEmpty(group, "group", "Group can't be null or empty");
            Check.Argument.IsNotNull(tags, "tags", "Tags can't be null");

            return ActivitiesViewBuilder.Construct(getSocialImpl, group, tags);
        }

        /// <summary>
        /// Creates notifications view builder used to open the Notifications View.
        /// </summary>
        /// <returns>The NotificationsViewBuilder instance</returns>
        public NotificationsViewBuilder CreateNotificationsView()
        {
            return NotificationsViewBuilder.Construct(getSocialImpl);
        }

        /// <summary>
        /// Creates smart invite view builder used to open the SmartInvite View.
        /// </summary>
        /// <returns>The SmartInviteViewBuilder instance</returns>
        public SmartInviteViewBuilder CreateSmartInviteView()
        {
            return SmartInviteViewBuilder.Construct(getSocialImpl);
        }

        /// <summary>
        /// Creates the user list view builder to open list of following users.
        /// </summary>
        /// <param name="onUserSelected">Callback when the user was selected.</param>
        /// <param name="onCancel">Callback when selecting the user was cancelled.</param>
        /// <returns>The UserListViewBuilder instance</returns>
        [Obsolete("Use CreateUserListView(UserListType type, Action<User> onUserSelected, Action onCancel) instead")]
        public UserListViewBuilder CreateUserListView(Action<User> onUserSelected, Action onCancel)
        {
            Check.Argument.IsNotNull(onUserSelected, "onUserSelected", "Callback cannot be null");
            Check.Argument.IsNotNull(onCancel, "onCancel", "Callback cannot be null");

            return UserListViewBuilder.Construct(getSocialImpl, UserListViewBuilder.UserListType.Following, onUserSelected, onCancel);
        }

        /// <summary>
        /// Creates the user list view builder to open list of following/follower users.
        /// </summary>
        /// <param name="type">Type of the list of users to be showed.</param>
        /// <param name="onUserSelected">Callback when the user was selected.</param>
        /// <param name="onCancel">Callback when selecting the user was cancelled.</param>
        /// <returns>The UserListViewBuilder instance</returns>
        public UserListViewBuilder CreateUserListView(UserListViewBuilder.UserListType type, Action<User> onUserSelected, Action onCancel)
        {
            Check.Argument.IsNotNull(onUserSelected, "onUserSelected", "Callback cannot be null");
            Check.Argument.IsNotNull(onCancel, "onCancel", "Callback cannot be null");

            return UserListViewBuilder.Construct(getSocialImpl, type, onUserSelected, onCancel);
        }

        /// <summary>
        /// Close GetSocial view.
        /// </summary>
        public void CloseView(bool saveViewState = false)
        {
            getSocialImpl.CloseView(saveViewState);
        }

        /// <summary>
        /// Restore last view after calling CloseView()
        /// </summary>
        public void RestoreView()
        {
            getSocialImpl.RestoreView();
        }
        #endregion

        /// <summary>
        /// Create a new save state.
        /// </summary>
        /// <param name="state">The state data to be saved.</param>
        /// <param name="onSuccess">Action called if operation was successful. Optional.</param>
        /// <param name="onFailure">Action called if operation failed to complete. Optional.</param>
        public void Save(string state, Action onSuccess = null, Action<string> onFailure = null)
        {
            Check.Argument.IsStrNotNullOrEmpty(state, "state", "State to save can't be null or empty");

            getSocialImpl.Save(state, onSuccess, onFailure);
        }

        /// <summary>
        /// Gets the last saved state.
        /// </summary>
        /// <param name="onSuccess">Action called if operation was successful.</param>
        /// <param name="onFailure">Action called if operation failed to complete. Optional.</param>
        public void GetLastSave(Action<string> onSuccess, Action<string> onFailure = null)
        {
            Check.Argument.IsNotNull(onSuccess, "onSuccess", "Success callback cannot be null");

            getSocialImpl.GetLastSave(onSuccess, onFailure);
        }

        /// <summary>
        /// Post an activity on the activity feed.
        /// </summary>
        /// <param name="text">The text of the activity, or null if no text should be shown.</param>
        /// <param name="image">The image of the activity, or null if no image should be shown.</param>
        /// <param name="buttonText">The text of the activity button, or null if no button text should be shown.</param>
        /// <param name="actionId">The action generated on click, or null if no button or image is shown.</param>
        /// <param name="tags">Tags for user segmentation.</param>
        /// <param name="onSuccess">Action called if operation was successful. Optional.</param>
        /// <param name="onFailure">Action called if operation failed to complete. Optional.</param>
        public void PostActivity(string text, byte[] image = null, string buttonText = null, string actionId = null,
                                 string[] tags = null, Action<string> onSuccess = null, Action onFailure = null)
        {
            getSocialImpl.PostActivity(text, image, buttonText, actionId, tags, onSuccess, onFailure);
        }

        /// <summary>
        /// Register an actions to be invoked when the GetSocial window state changes.
        /// </summary>
        /// <param name="onOpen">Action called when GetSocial window is opened.</param>
        /// <param name="onClose">Action called when GetSocial window is closed. Optional.</param>
        public void SetOnWindowStateChangeListener(Action onOpen, Action onClose = null)
        {
            Check.Argument.IsNotNull(onOpen, "onOpen", "Listener cannot be null");

            onViewOpen = onOpen;
            onViewClose = onClose;
        }

        /// <summary>
        /// Sets the actions to be called when smart invites are triggered
        /// </summary>
        /// <param name="onInviteFriendsIntent">Action to be called when user clicked on specific invite provider.</param>
        /// <param name="onFriendsInvited">Action to be called when user send invites. Gets number of invitations sent as a parameter.</param>
        public void SetOnInviteFriendsListener(Action onInviteFriendsIntent, Action<int> onFriendsInvited)
        {
            Check.Argument.IsNotNull(onInviteFriendsIntent, "onInviteFriendsIntent", "Listener cannot be null");
            Check.Argument.IsNotNull(onFriendsInvited, "onFriendsInvited", "Listener cannot be null");

            getSocialImpl.SetOnInviteFriendsListener(onInviteFriendsIntent, onFriendsInvited);
        }

        /// <summary>
        /// Sets the callback to be invoked when user tries to perform one of <see cref="GetSocialSdk.Core.GetSocial.UserPerformedAction"/>.
        /// Can be used to contol user permissions to perform certain actions.
        ///
        /// See <see cref="GetSocialSdk.Core.OnUserActionPerformed"/> for the delegate details
        /// </summary>
        /// <param name="onUserActionPerformed">On user action performed callback.</param>
        public void SetOnUserPerformedActionListener(OnUserActionPerformed onUserActionPerformed)
        {
            Check.Argument.IsNotNull(onUserActionPerformed, "onUserActionPerformed", "Listener cannot be null");

            getSocialImpl.SetOnUserActionPerformedListener(onUserActionPerformed);
        }

        /// <summary>
        /// Provide a method that implements <c>OnUserAvatarClick</c> delegate if you would like to know when the user wants to open a profile.
        /// Return true if action was handled by the app. Return false for default GetSocial behavior.
        /// </summary>
        public void SetOnUserAvatarClickListener(OnUserAvatarClick onUserAvatarClick)
        {
            Check.Argument.IsNotNull(onUserAvatarClick, "onUserAvatarClick", "Listener cannot be null");

            getSocialImpl.SetOnUserAvatarClickListener(onUserAvatarClick);
        }

        /// <summary>
        /// Provide a method that implements <c>OnAppAvatarClick</c> delegate if you would like to know when the user clicks on the app's avatar.
        /// Return true if action was handled by the app. Return false for default GetSocial behavior.
        /// </summary>
        public void SetOnAppAvatarClickListener(OnAppAvatarClick onAppAvatarClick)
        {
            Check.Argument.IsNotNull(onAppAvatarClick, "onAppAvatarClick", "Listener cannot be null");

            getSocialImpl.SetOnAppAvatarClickListener(onAppAvatarClick);
        }

        /// <summary>
        /// Provide a method that implements <c>OnActivityActionClick</c> delegate if you would like to know when the user clicks on an activity action.
        /// </summary>
        public void SetOnActivityActionClickListener(OnActivityActionClick onActivityActionClick)
        {
            Check.Argument.IsNotNull(onActivityActionClick, "onActivityActionClick", "Listener cannot be null");

            getSocialImpl.SetOnActivityActionClickListener(onActivityActionClick);
        }

        /// <summary>
        /// Provide a method that implements <c>OnInviteButtonClick</c> delegate if you would like to customize the behavior when the user clicks on the Invite Button.
        /// Return true if action was handled by the app. Return false for default GetSocial behavior.
        /// </summary>
        public void SetOnInviteButtonClickListener(OnInviteButtonClick onInviteButtonClick)
        {
            Check.Argument.IsNotNull(onInviteButtonClick, "onInviteButtonClick", "Listener cannot be null");

            getSocialImpl.SetOnInviteButtonClickListener(onInviteButtonClick);
        }

        /// <summary>
        /// Register a listener to handle user generated content. This listener is called when a user sends any type of content through activities or chat.
        /// </summary>
        /// <param name="onUserGeneratedContent">Callback to be executed when the content is ready to be send. Return the approved content to send or null if the content shouldn't be send.</param>
        public void SetOnUserGeneratedContentListener(OnUserGeneratedContent onUserGeneratedContent)
        {
            Check.Argument.IsNotNull(onUserGeneratedContent, "onUserGeneratedContent", "Listener cannot be null");

            getSocialImpl.SetOnUserGeneratedContentListener(onUserGeneratedContent);
        }

        /// <summary>
        /// Provide a method that implements <c>OnReferralDataReceived</c> delegate if you would like to get referral data if the user installs or opens the app after opening a Smart Invite .
        /// </summary>
        public void SetOnReferralDataReceivedListener(OnReferralDataReceived onReferralDataReceived)
        {
            Check.Argument.IsNotNull(onReferralDataReceived, "onReferralDataReceived", "Listener cannot be null");
            
            getSocialImpl.SetOnReferralDataReceivedListener(onReferralDataReceived);
        }

        /// <summary>
        /// Sets the action to be called when new chat message or notification arrives. First param is new notifications number, second - unread chat messages number.
        /// </summary>
        public void SetOnUnreadNotificationsCountChangeListener(Action<int> onUnreadNotificationsCountChange)
        {
            Check.Argument.IsNotNull(onUnreadNotificationsCountChange, "onUnreadNotificationsCountChange", "Listener cannot be null");
            
            getSocialImpl.SetOnUnreadNotificationsCountChangeListener(onUnreadNotificationsCountChange);
        }

        /// <summary>
        /// Sets the language for all GetSocial views. To take effect views should be reopened.
        /// </summary>
        /// <param name="languageCode">Language code. See supported languages in documentation.</param>
        public void SetLanguage(string languageCode)
        {
            Check.Argument.IsStrNotNullOrEmpty(languageCode, "languageCode", "Language code can't be null or empty");

            getSocialImpl.SetLanguage(languageCode);
        }

        /// <summary>
        /// Returns all supported invite providers.
        /// </summary>
        /// <returns>Array containing constants id's for all supported providers</returns>
        public string[] GetSupportedInviteProviders()
        {
            return getSocialImpl.GetSupportedInviteProviders();
        }

        /// <summary>
        /// Invite friends through a specific invite provider.
        /// </summary>
        /// <param name="provider">The provider through which the invite will be sent.</param>
        /// <param name="subject">The subject to be sent with the invite. If it is null the default subject set on GetSocial Dashboard is sent.</param>
        /// <param name="text">The text to be sent with the invite. NOTE: you can use following tags to customize text: 
        ///     [APP_INVITE_URL] - tag is replaced with url to download the app;
        ///     [APP_NAME] - tag is replaced with app name configured on the GetSocial Dashboard. If it is null the default text set on GetSocial Dashboard is sent.</param>
        /// <param name="image">The image to be sent with the invite. If it is null the default image set on GetSocial Dashboard will be sent.</param>
        /// <param name="referralData">The bundle to be sent with the invite. It can be null.</param>
        public void InviteFriendsUsingProvider(string provider, String subject = null, string text = null,
                                               byte[] image = null, IDictionary<string, string> referralData = null)
        {
            Check.Argument.IsStrNotNullOrEmpty(provider, "provider", "Provider can't be null or empty");

            getSocialImpl.InviteFriendsUsingProvider(provider, subject, text, image, referralData);
        }

        #region leaderboards
        /// <summary>
        /// Gets the leaderboard by id.
        /// </summary>
        /// <param name="leaderboardId">Leaderboard id.</param>
        /// <param name="onSuccess">Action called if operation was successful.</param>
        /// <param name="onFailure">Action called if operation failed to complete. Optional.</param>
        public void GetLeaderboard(string leaderboardId, Action<Leaderboard> onSuccess, Action onFailure = null)
        {
            Check.Argument.IsStrNotNullOrEmpty(leaderboardId, "leaderboardId", "Leaderboard id can't be null or empty");
            Check.Argument.IsNotNull(onSuccess, "onSuccess", "Success callback cannot be null");

            getSocialImpl.GetLeaderboard(leaderboardId,
                GetLeaderboardOnSuccessAdapter(onSuccess),
                GetLeaderboardOnFailureAdapter(onFailure)
            );
        }

        /// <summary>
        /// Gets list of the leaderboards for provided ids.
        /// </summary>
        /// <param name="leaderboardIds">Set of leaderboard ids.</param>
        /// <param name="onSuccess">Action called if operation was successful.</param>
        /// <param name="onFailure">Action called if operation failed to complete. Optional.</param>
        public void GetLeaderboards(HashSet<string> leaderboardIds, Action<List<Leaderboard>> onSuccess,
                                    Action onFailure = null)
        {
            Check.Argument.IsNotNull(leaderboardIds, "leaderboardIds", "Leaderboard ids cannot be null");
            Check.Argument.IsNotNull(onSuccess, "onSuccess", "Success callback cannot be null");
            
            getSocialImpl.GetLeaderboards(leaderboardIds,
                GetLeaderboardsOnSuccessAdapter(onSuccess),
                GetLeaderboardOnFailureAdapter(onFailure)
            );
        }

        /// <summary>
        /// Gets the leaderboards by page.
        /// </summary>
        /// <param name="offset">Offset from which leaderboards will be retrieved.</param>
        /// <param name="count">Count of count of the leaderboards. Could be less than expected if there are less leaderboards.</param>
        /// <param name="onSuccess">Action called if operation was successful.</param>
        /// <param name="onFailure">Action called if operation failed to complete. Optional.</param>
        public void GetLeaderboards(int offset, int count, Action<List<Leaderboard>> onSuccess, Action onFailure = null)
        {
            Check.Argument.IsNotNull(onSuccess, "onSuccess", "Success callback cannot be null");

            getSocialImpl.GetLeaderboards(offset, count,
                GetLeaderboardsOnSuccessAdapter(onSuccess),
                GetLeaderboardOnFailureAdapter(onFailure)
            );
        }

        /// <summary>
        /// Gets scores page by page.
        /// </summary>
        /// <param name="leaderboardId">Leaderboard id.</param>
        /// <param name="offset">Offset from which scores will be retrieved.</param>
        /// <param name="count">Count of the scores. Could be less than expected if there are less scores.</param>
        /// <param name="scoreType">Type of the score</param>
        /// <param name="onSuccess">Action called if operation was successful.</param>
        /// <param name="onFailure">Action called if operation failed to complete. Optional.</param>
        public void GetLeaderboardScores(string leaderboardId, int offset, int count, LeaderboardScoreType scoreType,
                                         Action<List<LeaderboardScore>> onSuccess, Action onFailure = null)
        {
            Check.Argument.IsNotNull(onSuccess, "onSuccess", "Success callback cannot be null");
            
            getSocialImpl.GetLeaderboardScores(leaderboardId, offset, count, scoreType,
                GetLeaderboardScoresOnSuccessAdapter(onSuccess),
                GetLeaderboardOnFailureAdapter(onFailure)
            );
        }

        /// <summary>
        /// Submits score for the leaderboard with provided id.
        /// </summary>
        /// <param name="leaderboardId">Leaderboard id.</param>
        /// <param name="score">Score to submit.</param>
        /// <param name="onSuccess">Action called if operation was successful. New user leaderboard rank is passed as parameter. Optional.</param>
        /// <param name="onFailure">Action called if operation failed to complete. Optional.</param>
        public void SubmitLeaderboardScore(string leaderboardId, int score, Action<int> onSuccess = null,
                                           Action onFailure = null)
        {
            Check.Argument.IsStrNotNullOrEmpty(leaderboardId, "leaderboardId", "Leaderboard id can't be null or empty");
            
            getSocialImpl.SubmitLeaderboardScore(leaderboardId, score, onSuccess, onFailure);
        }

        private Action<string> GetLeaderboardOnSuccessAdapter(Action<Leaderboard> onSuccess)
        {
            return rawData =>
            {
                if(onSuccess != null)
                {
                    onSuccess(new Leaderboard(new JSONObject(rawData)));
                }
            };
        }

        private Action<string> GetLeaderboardsOnSuccessAdapter(Action<List<Leaderboard>> onSuccess)
        {
            return rawData =>
            {
                if(onSuccess != null)
                {
                    var leaderboards = new List<Leaderboard>();
                    var jsonLeaderboards = new JSONObject(rawData);

                    foreach(JSONObject jsonLeaderboard in jsonLeaderboards["data"].list)
                    {
                        leaderboards.Add(new Leaderboard(jsonLeaderboard));
                    }

                    onSuccess(leaderboards);
                }
            };
        }

        private Action<string> GetLeaderboardScoresOnSuccessAdapter(Action<List<LeaderboardScore>> onSuccess)
        {
            return rawData =>
            {
                if(onSuccess != null)
                {
                    var leaderboardScores = new List<LeaderboardScore>();
                    var jsonLeaderboardScores = new JSONObject(rawData);

                    foreach(JSONObject jsonLeaderboardScore in jsonLeaderboardScores["data"].list)
                    {
                        var score = new LeaderboardScore(jsonLeaderboardScore["data"]);
                        leaderboardScores.Add(score);
                    }

                    onSuccess(leaderboardScores);
                }
            };
        }

        private static Action<string> GetLeaderboardOnFailureAdapter(Action onFailure)
        {
            return rawData =>
            {
                if(onFailure != null)
                {
                    onFailure();
                }
            };
        }
        #endregion
    }
}

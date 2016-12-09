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

using GetSocialSdk.Core;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

#if ENABLE_GETSOCIAL_CHAT
using GetSocialSdk.Chat;
#endif

public class GetSocialDemoController : MonoBehaviour
{
    private Vector2 scrollPos;

    // container class to store current user info
    private class CurrentUserInfo
    {
        public string guid;
        public string displayName;
        public string avatarUrl;
        public Dictionary<string, string> identities = new Dictionary<string, string>();

        public override string ToString()
        {
            return string.Format("[User: Guid={0}, DisplayName={1}, AvatarUrl={2}, Identities={3}]", guid, displayName,
                avatarUrl, identities.ToDebugString());
        }
    }

    private const string MainMenuTitle = "Main Menu";

    private enum TopLevelMenuSection
    {
        Main,
        UserManagement,
        FollowUnfollow,
        Activities,
        SmartInvites,
        Chat,
        UiCustomization,
        Leaderboards,
        CloudSave,
        Settings,
    }

    [SerializeField]
    private Texture2D avatar;

    private string getSocialUnityVersion;
    private string getSocialUnderlyingNativeSdkVersion;
    private string apiVersion;
    private string unreadNotificationsCount = "0";
    #if ENABLE_GETSOCIAL_CHAT
    private static string unreadPublicConversationsCount = "0";
    private static string unreadPrivateConversationsCount = "0";
    #endif
    private ScreenOrientation screenOrientation;
    private DemoAppConsole console;
    private bool isInMainMenu = true;
    private Dictionary<TopLevelMenuSection, DemoMenuSection> menuSections;

    protected GetSocial getSocial;
    protected bool isGetSocialInitialized = false;

    #if ENABLE_GETSOCIAL_CHAT
    private GetSocialChat getSocialChat;
    private bool isChatEnabled;
    #endif

    private string referralDataToString;

    public string CurrentViewTitle { set; get; }

    // store user info to avoid constant calls to native
    private CurrentUserInfo currentUserInfo = new CurrentUserInfo();

    #region lifecycle
    private void Awake()
    {
        console = gameObject.GetComponent<DemoAppConsole>();
        screenOrientation = ScreenOrientation.Unknown;

        SetupGetSocial();
        SetupMenuSections();
    }

    private void SetupMenuSections()
    {
        menuSections = new Dictionary<TopLevelMenuSection, DemoMenuSection> {
            { TopLevelMenuSection.UserManagement, GetComponentInChildren<AuthSection>() },
            { TopLevelMenuSection.FollowUnfollow, GetComponentInChildren<FollowUnfollowSection>() },
            { TopLevelMenuSection.Activities, GetComponentInChildren<ActivitiesSection>() },
            { TopLevelMenuSection.SmartInvites, GetComponentInChildren<SmartInvitesSection>() },
#if ENABLE_GETSOCIAL_CHAT
            { TopLevelMenuSection.Chat, GetComponentInChildren<ChatSection>() },
#endif
            { TopLevelMenuSection.UiCustomization, GetComponentInChildren<UiCustomizationSection>() },
            { TopLevelMenuSection.CloudSave, GetComponentInChildren<CloudSaveSection>() },
            { TopLevelMenuSection.Leaderboards, GetComponentInChildren<LeaderboardSection>() },
            { TopLevelMenuSection.Settings, GetComponentInChildren<SettingsSection>() }
        };
        foreach(var section in menuSections.Values)
        {
            section.Initialize(this, getSocial, console);
        }
        ShowMainMenu();
    }

    protected void Start()
    {
        // Initialize FB SDK
        FB.Init(OnFacebookInited, OnHideUnity);
    }

    protected void Update()
    {
        if(Screen.orientation != screenOrientation)
        {
            screenOrientation = Screen.orientation;
            OnOrientationChanged();
        }

        HandleAndroidBackButton();
    }

    private void HandleAndroidBackButton()
    {
        if(Application.platform == RuntimePlatform.Android && Input.GetKeyDown(KeyCode.Escape))
        {
            if(isInMainMenu)
            {
                Application.Quit();
            }
        }
    }

    protected void OnGUI()
    {
        // disable other gui if resoliving conflict
        #if !UNITY_EDITOR
        if(!isGetSocialInitialized)
        {
            return;
        }
        #endif

        DemoGuiUtils.DrawHeaderWrapper(DrawHeader);
        scrollPos = DemoGuiUtils.DrawScrollBodyWrapper(scrollPos, DrawBody);
        DemoGuiUtils.DrawFooter(DrawFooter);
    }
    #endregion

    #region callbacks
    protected void OnFacebookInited()
    {
        Debug.Log("Facebook successfully initialized");
    }

    /// <summary>
    /// NOTE: We do not support changing orientation so we need to close GetSocial view.
    /// </summary>
    protected void OnOrientationChanged()
    {
        if(getSocial.IsInitialized)
        {
            getSocial.CloseView();
        }
    }

    protected void OnHideUnity(bool isGameShown)
    {
        if(isGameShown)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
    #endregion

    #region private methods
    protected void SetupGetSocial()
    {
        Log.D("Setup GetSocial");
        getSocial = GetSocial.Instance;
        getSocialUnityVersion = getSocial.UnitySdkVersion;
        getSocialUnderlyingNativeSdkVersion = getSocial.NativeSdkVersion;
        apiVersion = getSocial.ApiVersion;

#if ENABLE_GETSOCIAL_CHAT
        getSocialChat = GetSocialChat.Instance;
        unreadPublicConversationsCount = getSocialChat.UnreadPublicRoomsCount.ToString();
        unreadPrivateConversationsCount = getSocialChat.UnreadPrivateRoomsCount.ToString();

        getSocialChat.SetOnUnreadConversationsCountChangeListener(count => console.LogD("Unread conversations count changed: " + count)); // deprecated API
        // listen to unread counter changes and log them
        getSocialChat.AddOnUnreadRoomCountChangedListener(new UnreadRoomsCountChangedLogger(console));
        // listen to messages and log them
        getSocialChat.AddMessageListener(new ReceviedMessagesLogger(console));
        // listen to typing status changes
        getSocialChat.AddTypingStatusListener(new ReceivedTypingStatusLogger(console));
#endif

        getSocial.RegisterPlugin(UserIdentity.ProviderFacebook, new FacebookInvitePlugin());

        // NOTE: This is only included on android due to conflicts with packaging an extra facebook library in
        // the iOS build along side the Unity facebook library
        // It is possible to manually do but hard to package with the GetSocial distributable
#if UNITY_ANDROID
        getSocial.RegisterPlugin(FacebookMessengerInvitePlugin.ProviderName, new FacebookMessengerInvitePlugin());
#endif

        unreadNotificationsCount = getSocial.UnreadNotificationsCount.ToString();
        getSocial.SetOnUnreadNotificationsCountChangeListener(
            unreadNotifications => unreadNotificationsCount = unreadNotifications.ToString());
        getSocial.SetOnWindowStateChangeListener(
            () => Debug.Log("Social window opened"),
            () => Debug.Log("Social window closed"));
        getSocial.SetOnInviteFriendsListener(
            () => Debug.Log("Invite intent"),
            numberOfInvites => Debug.Log("Number of invites: " + numberOfInvites));
        getSocial.SetOnReferralDataReceivedListener(referralData =>
        {
            referralDataToString = "\n";

            foreach(Dictionary<string, string> dictionary in referralData)
            {
                foreach(KeyValuePair<string, string> entry in dictionary)
                {
                    referralDataToString += entry.Key + ": " + entry.Value + "\n";
                }

                referralDataToString += "---";
            }

            console.LogD("Referral data received: \n" + referralDataToString);
        });

        getSocial.Init(
            () =>
            {
                Debug.Log("GetSocial successfully initialized.");
                isGetSocialInitialized = true;
                FetchCurrentUserData();
#if ENABLE_GETSOCIAL_CHAT
                isChatEnabled = getSocialChat.IsEnabled;
#endif
            },
            () => Debug.Log("GetSocial initialization FAILED")
        );
    }

    #if ENABLE_GETSOCIAL_CHAT
    private class UnreadRoomsCountChangedLogger : IUnreadRoomCountChangedListener
    {
        private readonly DemoAppConsole console;

        public UnreadRoomsCountChangedLogger(DemoAppConsole console)
        {
            this.console = console;
        }

        public void OnUnreadPublicRoomsCountChanged(int count)
        {
            console.LogD("Unread public rooms is now: " + count, false);
            unreadPublicConversationsCount = count.ToString();
        }

        public void OnUnreadPrivateRoomsCountChanged(int count)
        {
            console.LogD("Unread private rooms is now: " + count, false);
            unreadPrivateConversationsCount = count.ToString();
        }
    }

    private class ReceviedMessagesLogger : IChatMessageListener
    {
        private readonly DemoAppConsole console;

        public ReceviedMessagesLogger(DemoAppConsole console)
        {
            this.console = console;
        }

        public void OnPublicRoomMessage(IPublicChatRoom publicRoom, ChatMessage message)
        {
            console.LogD(string.Format("Received message to public room: {0}, room: {1}", publicRoom, message), false);
        }

        public void OnPrivateRoomMessage(IPrivateChatRoom privateRoom, ChatMessage message)
        {
            console.LogD(string.Format("Received message to private room: {0}, room: {1}", privateRoom, message), false);
        }
    }

    private class ReceivedTypingStatusLogger : ITypingStatusListener
    {
        private readonly DemoAppConsole console;

        public ReceivedTypingStatusLogger(DemoAppConsole console)
        {
            this.console = console;
        }

        public void OnPublicRoomTypingStatusReceived(IPublicChatRoom room, User user, TypingStatus typingStatus)
        {
            console.LogD(string.Format("Received typing status to public room: {0}, status: {1}, user: {2}",
                room, typingStatus, user), false);
        }

        public void OnPrivateRoomTypingStatusReceived(IPrivateChatRoom room, User user, TypingStatus typingStatus)
        {
            console.LogD(string.Format("Received typing status to private room: {0}, status: {1}, user: {2}",
                room, typingStatus, user), false);
        }
    }
    #endif

    protected void DrawHeader()
    {
        DrawConsoleToggle();

        GUILayout.BeginVertical(GUILayout.MaxWidth(150));
        GUILayout.Box(avatar, GUILayout.Width(100), GUILayout.Height(100));
        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), avatar, ScaleMode.StretchToFill);

        // Click area for avatar
        if(GUI.Button(new Rect(0, 0, 120, 120), string.Empty, new GUIStyle()))
        {
            console.LogD(FetchCurrentUserData());
        }
        GUILayout.EndVertical();

        if(!string.IsNullOrEmpty(currentUserInfo.guid))
        {
            DrawUserInfo();
        }
    }

    void DrawUserInfo()
    {
        GUILayout.BeginVertical();
        GUILayout.Label(currentUserInfo.displayName, GSStyles.NormalLabelText);
        GUILayout.Label("GUID: " + currentUserInfo.guid, GSStyles.NormalLabelText);
        GUILayout.Label("Is anonymous? " + (currentUserInfo.identities.Count == 0), GSStyles.NormalLabelText);
        GUILayout.EndVertical();
    }

    private void DrawConsoleToggle()
    {
        if(GUI.Button(new Rect(Screen.width - 115, 15, 100, 100), "Toggle\nConsole"))
        {
            console.Toggle();
        }
    }

    protected void DrawBody()
    {
        if(console.IsVisible || !isInMainMenu)
        {
            return;
        }

        GUILayout.Label(CurrentViewTitle, GSStyles.BigLabelText);
        DrawMainView();
    }

    private void DrawFooter()
    {
        GUILayout.Label(GetFooterText(), GSStyles.NormalLabelText);
    }

    protected virtual string GetFooterText()
    {
        return string.Format("Native SDK: v{0}, Unity SDK v{1}, API {2}", getSocialUnderlyingNativeSdkVersion,
            getSocialUnityVersion, apiVersion);
    }

    protected void DrawMainView()
    {
        Button("User Management", () => ShowMenuSection(TopLevelMenuSection.UserManagement));
        Button("Activities", () => ShowMenuSection(TopLevelMenuSection.Activities));
        Button("Smart Invites", () => ShowMenuSection(TopLevelMenuSection.SmartInvites));

#if ENABLE_GETSOCIAL_CHAT
        if(isChatEnabled)
        {
            Button(string.Format("Chat (Unread Public:{0}/Private:{1})", unreadPublicConversationsCount, unreadPrivateConversationsCount),
                () => ShowMenuSection(TopLevelMenuSection.Chat));
        }
#endif

        GUILayout.BeginHorizontal();
        Button(string.Format("Notifications ({0})", unreadNotificationsCount), CreateNotificationsView);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        Button("Follow / Unfollow", () => ShowMenuSection(TopLevelMenuSection.FollowUnfollow));
        GUILayout.EndHorizontal();
        GSStyles.Button.fixedWidth = Screen.width / 2;

        GUILayout.BeginHorizontal();
        Button("UI Customization", () => ShowMenuSection(TopLevelMenuSection.UiCustomization));
        Button("Leaderboards", () => ShowMenuSection(TopLevelMenuSection.Leaderboards));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        Button("Cloud Save", () => ShowMenuSection(TopLevelMenuSection.CloudSave));
        Button("Settings", () => ShowMenuSection(TopLevelMenuSection.Settings));
        GUILayout.EndHorizontal();
        GSStyles.Button.fixedWidth = 0;
    }

    protected void Button(string text, Action onClickCallback, bool isEnabled = true)
    {
        DemoGuiUtils.DrawButton(text, onClickCallback, isEnabled, GSStyles.Button);
    }

    protected void PauseGame()
    {
        Time.timeScale = 0;
        Debug.Log("Paused game");
    }

    protected void ResumeGame()
    {
        Debug.Log("Resuming game");
        Time.timeScale = 1;
    }
    #endregion

    #region button_actions
    private void CreateNotificationsView()
    {
        getSocial.CreateNotificationsView().SetTitle("Notifications").Show();
    }
    #endregion

    #region navigation
    public void ShowMainMenu()
    {
        CurrentViewTitle = MainMenuTitle;
        ShowMenuSection(TopLevelMenuSection.Main);
    }

    private void ShowMenuSection(TopLevelMenuSection menuSection)
    {
        foreach(var section in menuSections)
        {
            section.Value.gameObject.SetActive(false);
        }

        if(menuSection == TopLevelMenuSection.Main)
        {
            isInMainMenu = true;
            CurrentViewTitle = MainMenuTitle;
        }
        else
        {
            menuSections[menuSection].gameObject.SetActive(true);
            isInMainMenu = false;
        }
    }
    #endregion

    #region helpers
    public string FetchCurrentUserData()
    {
        var identities = new Dictionary<string, string>();
        foreach(var provider in getSocial.CurrentUser.Identities)
        {
            identities[provider] = getSocial.CurrentUser.GetIdForProvider(provider);
        }
        currentUserInfo = new CurrentUserInfo {
            guid = getSocial.CurrentUser.Guid,
            displayName = getSocial.CurrentUser.DisplayName,
            avatarUrl = getSocial.CurrentUser.AvatarUrl,
            identities = identities
        };
        StartCoroutine(DownloadAvatar(currentUserInfo.avatarUrl));

        return currentUserInfo.ToString();
    }

    private IEnumerator DownloadAvatar(string url)
    {
        Debug.Log("Downloading avatar: " + url);
        var www = new WWW(url);
        yield return www;
        if(!string.IsNullOrEmpty(www.error))
        {
            Debug.LogError("Failed to download avatar, reason: " + www.error);
        }
        avatar = www.texture;
        FindObjectOfType<RotatingCube>().gameObject.GetComponent<MeshRenderer>().material.mainTexture = avatar;
    }
    #endregion
}

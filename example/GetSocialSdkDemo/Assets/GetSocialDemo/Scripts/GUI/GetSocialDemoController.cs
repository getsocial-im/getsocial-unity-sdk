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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Assets.GetSocialDemo.Scripts.Utils;
using Facebook.Unity;
using GetSocialDemo.Scripts.GUI.Sections;
using GetSocialSdk.Core;
using GetSocialSdk.Ui;
using UnityEngine;
using GetSocialSdk.MiniJSON;

public class GetSocialDemoController : MonoBehaviour {
    const string MainMenuTitle = "Main Menu";

    Texture2D _avatar;
    string _getSocialUnitySdkVersion;
    string _getSocialUnderlyingNativeSdkVersion;

    bool _isInMainMenu = true;

    Vector2 _scrollPos;
    DemoAppConsole _console;

    public string CurrentViewTitle { set; get; }
    private string _latestReferralData;
    private string _latestChatId;
    private bool _isTestDevice;

    // store user info to avoid constant calls to native
    private CurrentUser _currentUser;

    #region lifecycle

    [SuppressMessage ("ReSharper", "UnusedMember.Local")]
    void Awake () {
        // change default framerate
        Application.targetFrameRate = 100;
        _console = gameObject.GetComponent<DemoAppConsole> ().Init ();
        SetupAppsFlyer ();
        ShowMainMenu();
        SetupGetSocial();
#if UNITY_IOS
        OnApplicationPause (false);
#endif
    }

    void SetupAppsFlyer () {
        AppsFlyer.setAppsFlyerKey ("AP8P3GvwHgw9NBdBTWAqrb");
        /* For detailed logging */
        /* AppsFlyer.setIsDebug (true); */
#if UNITY_IOS
        /* Mandatory - set your apple app ID
           NOTE: You should enter the number only and not the "ID" prefix */
        AppsFlyer.setAppID ("313131310");
        AppsFlyer.trackAppLaunch ();
#elif UNITY_ANDROID
        /* Mandatory - set your Android package name */
        AppsFlyer.setAppID ("im.getsocial.demo.unity");
        AppsFlyer.init ("AP8P3GvwHgw9NBdBTWAqrb");;
#endif
    }

    private void OnApplicationPause (bool pauseStatus) {
        if (!pauseStatus) {
            GetSocial.AddOnInitializedListener (() => {
                if (_latestChatId != null) {
                    ShowChat ();
                } 
            });
        } else {
            GetSocialUi.CloseView (false);
        }
    }

    protected virtual void Start () {
        // Initialize FB SDK
        FB.Init (OnFacebookInited, OnHideUnity);
    }

    protected void Update () {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        HandleAndroidBackButton ();
        if (_notification != null) {
            var notification = _notification;
            _notification = null;
            var popup = new MNPopup (notification.Title, notification.Text);
            notification.ActionButtons.ForEach (button => {
                popup.AddAction (button.Title, () => { ProcessAction (button.ActionId, notification); });
            });
            popup.AddAction ("Dismiss", () => { });
            popup.Show ();
        }
    }

    void HandleAndroidBackButton () {
        if (Application.platform == RuntimePlatform.Android && Input.GetKeyDown (KeyCode.Escape)) {
            if (_isInMainMenu) {
                Application.Quit ();
            }
        }
    }

    protected void OnGUI () {
        DemoGuiUtils.DrawHeaderWrapper (DrawHeader);
        _scrollPos = DemoGuiUtils.DrawScrollBodyWrapper (_scrollPos, DrawBody);
        DemoGuiUtils.DrawFooter (DrawFooter);
    }

    #endregion

    #region callbacks

    protected void OnFacebookInited () {
        _console.LogD ("Facebook successfully initialized");
    }

    protected void OnHideUnity (bool isGameShown) {
        if (isGameShown) {
            ResumeGame ();
        } else {
            PauseGame ();
        }
    }

    #endregion

    #region methods

    public void SetupGetSocial () {
        Notifications.SetOnTokenReceivedListener (deviceToken => {
            _console.LogD (string.Format ("DeviceToken: {0}", deviceToken), false);
        });
        Notifications.SetOnNotificationReceivedListener(notification => {
            // handle chat message
            if (notification.Action.Type.Equals("open_chat_message"))
            {
                _latestChatId = notification.Action.Data["open_messages_for_id"];
                if (GetSocial.IsInitialized)
                {
                    ShowChat();
                }
            }
            if (notification.Action.Type.Equals(GetSocialActionType.AddFriend))
            {
                ShowPopup(notification);
            }
            _console.LogD(string.Format("Notification received : {0}", notification));
        });
        Notifications.SetOnNotificationClickedListener((notification, context) => {
            _console.LogD(string.Format("Notification wasClicked : {0}", notification));
            HandleAction (notification.Action);
        });

        _console.LogD ("Setting up GetSocial...");
        _getSocialUnitySdkVersion = GetSocial.UnitySdkVersion;
        _getSocialUnderlyingNativeSdkVersion = GetSocial.NativeSdkVersion;

        GetSocialFBMessengerPluginHelper.RegisterFBMessengerPlugin ();
        if (GetSocial.IsInitialized)
        {
            _console.LogD("GetSocial is initialized and user is retrieved");
            _currentUser = GetSocial.GetCurrentUser();
            _isTestDevice = GetSocial.Device.IsTestDevice;
            FetchCurrentUserData();
        } else
        {
            GetSocial.AddOnCurrentUserChangedListener(user => {
                _console.LogD("GetSocial is initialized and user is retrieved");
                _currentUser = user;
                _isTestDevice = GetSocial.Device.IsTestDevice;
                FetchCurrentUserData();
            });
        }

        Invites.SetOnReferralDataReceivedListener((referralData) => {
            var logMessage = string.Empty;

            if (referralData != null)
            {
                logMessage += string.Format("Token: {0}\n", referralData.Token);
                logMessage += string.Format("Referrer user id: {0}\n", referralData.ReferrerUserId);
                logMessage += string.Format("Referrer channel: {0}\n", referralData.ReferrerChannelId);
                logMessage += string.Format("Is first match: {0}\n", referralData.IsFirstMatch);
                logMessage += string.Format("Is guarateed match: {0}\n", referralData.IsGuaranteedMatch);
                logMessage += "Referral Link params:\n" + referralData.LinkParams.ToDebugString();
            }
            _console.LogD("Referral data: \n" + logMessage);
        });

        GetSocial.Init();
    }

    private void ShowChat () {
        MainThreadExecutor.Queue (() => {
            //todo
            // ShowMenuSection<SocialGraphSection> ();
            // _menuSections.ForEach (section => {
            //     if (section is SocialGraphSection) {
            //         ((SocialGraphSection) section).MessageFriend (_latestChatId);
            //         _latestChatId = null;
            //     }
            // });
            // if (_console.IsVisible) {
            //     _console.Toggle ();
            // }
        });
    }

    private void ShowPopup (Notification notification) {
        _notification = notification;
    }

    public static void ProcessAction (string actionButtonId, Notification notification) {
        if (actionButtonId.Equals (NotificationButton.ConsumeAction)) {
            Notifications.SetStatus (NotificationStatus.Consumed, new List<string> { notification.Id }, () => { }, Debug.LogError);
            GetSocial.Handle (notification.Action);
        } else if (actionButtonId.Equals (NotificationButton.IgnoreAction)) {
            Notifications.SetStatus (NotificationStatus.Ignored, new List<string> { notification.Id }, () => { }, Debug.LogError);
        }
    }

    public void HandleAction (GetSocialAction action) {
        _console.LogD("Action: " + action.ToString(), false);
        if (action.Type == GetSocialActionType.OpenProfile && action.Data.ContainsKey(GetSocialActionKeys.OpenProfile.UserId)) 
        {
            newFriend (action.Data[GetSocialActionKeys.OpenProfile.UserId]);
        }
    }

    void newFriend (string userId) {
        Communities.GetUser(UserId.Create(userId), user => {
            DemoUtils.ShowPopup ("You have a new friend!", user.DisplayName + " is now your friend.");
        }, error => {
            _console.LogE ("Failed to get user: " + error.Message + ", code: " + error.ErrorCode);
        });
    }

    protected void DrawHeader () {
        DrawConsoleToggle ();

        GUILayout.BeginVertical (GUILayout.MaxWidth (150));
        GUILayout.Box (_avatar ?? Texture2D.whiteTexture, GUILayout.Width (100), GUILayout.Height (100));

        GUI.DrawTexture (GUILayoutUtility.GetLastRect (), _avatar ?? Texture2D.blackTexture, ScaleMode.StretchToFill);
        // Click area for avatar
        if (GUI.Button (new Rect (0, 0, 120, 120), string.Empty, GUIStyle.none)) {
            DemoUtils.ShowPopup ("Info", _currentUser.ToString ());
            _console.LogD (_currentUser.ToString ());
        }
        GUIStyle fpsStyle = new GUIStyle {
            margin = new RectOffset (4, 4, 4, 4),
            fontSize = 16,
            alignment = TextAnchor.UpperLeft,
            wordWrap = true,
            richText = false,
            stretchWidth = true
        };
        GUILayout.Label ("FPS: " + GetFPS (), fpsStyle);
        if (_isTestDevice)
        {
            GUILayout.Label("TestDevice", fpsStyle);
        }
        GUILayout.EndVertical ();

        if (_currentUser != null) {
            DrawUserInfo ();
        }
    }

    void DrawUserInfo () {
        GUILayout.BeginVertical ();
        GUILayout.Label ("GUID: " + _currentUser.Id, GSStyles.NormalLabelText);
        GUILayout.Label ("Display Name: " + _currentUser.DisplayName, GSStyles.NormalLabelText);
        GUILayout.Label ("AvatarUrl: " + _currentUser.AvatarUrl, GSStyles.NormalLabelText);
        GUILayout.Label ("Is anonymous? " + _currentUser.IsAnonymous, GSStyles.NormalLabelText);
        GUILayout.EndVertical ();
    }

    void DrawConsoleToggle () {
        if (GUI.Button (new Rect (Screen.width - 115, 15, 100, 100), "Toggle\nConsole")) {
            _console.Toggle ();
        }
    }

    protected void DrawBody () {
        if (_console.IsVisible || !_isInMainMenu) {
            return;
        }

        GUILayout.Label (CurrentViewTitle, GSStyles.BigLabelText);
        DrawMainView ();
    }

    void DrawFooter () {
        GUILayout.Label (GetFooterText (), GSStyles.NormalLabelText);
    }

    float deltaTime = 0.0f;
    private Notification _notification;

    protected virtual string GetFPS () {
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        return string.Format ("{0:0.} fps ({1:0.} ms)", fps, msec);
    }

    protected virtual string GetFooterText () {
        return string.Format ("Native: v{0}, Unity: v{1}", _getSocialUnderlyingNativeSdkVersion,
            _getSocialUnitySdkVersion);
    }

    void DrawMainView () {
        GUILayout.Label ("API", GSStyles.NormalLabelText);
        Button ("User Management", ShowMenuSection<AuthSection>);
        Button ("InApp Purchase Api", ShowMenuSection<InAppPurchaseApiSection>);
        Button ("Promo Codes", ShowMenuSection<PromoCodesSection>);
        Button ("Custom Analytics Events", ShowMenuSection<CustomAnalyticsEventSection>);
        GUILayout.Space (30f);
        GUILayout.Label ("Invites", GSStyles.NormalLabelText);
        Button ("Smart Invites", ShowMenuSection<SmartInvitesApiSection>);
        Button ("Set Referrer", ShowMenuSection<SetReferrerSection>);
        Button ("Referred Users", ShowMenuSection<ReferredUsersSection>);
        Button ("Referrer Users", ShowMenuSection<ReferrerUsersSection>);
        GUILayout.Space (30f);
        GUILayout.Label ("Notification", GSStyles.NormalLabelText);
        Button ("Send Notification", ShowMenuSection<SendNotificationSection>);
        Button ("Notifications", ShowMenuSection<NotificationsSection>);
        GUILayout.Space (30f);
        GUILayout.Label ("Communities", GSStyles.NormalLabelText);
        Button ("Timeline", () => PushMenuSection<ActivitiesSection>(section => section.Query = ActivitiesQuery.Timeline() ));
        Button ("Find Tags", ShowMenuSection<TagsSection>);
        Button ("Post to Timeline", () => PushMenuSection<PostActivitySection>(section => section.Target = PostActivityTarget.Timeline() ));
        Button ("My Friends", () => PushMenuSection<FriendsSection>(section => section.User = UserId.CurrentUser()));
        Button ("Suggested Friends", ShowMenuSection<SuggestedFriendsSection>);
        Button ("My Followers", () => PushMenuSection<FollowersSection>(section =>
        {
            section.Name = "Me";
            section.Query = FollowersQuery.OfUser(UserId.CurrentUser());
        }));
        Button ("Followed By Me", () => PushMenuSection<FollowingSection>(section =>
        {
            section.Name = "Me";
            section.User = UserId.CurrentUser();
        }));
        Button ("Search Topics", ShowMenuSection<TopicsSearchSection>);
        Button ("My Topics", () => PushMenuSection<FollowedTopicsSection>(section =>
        {
            section.User = UserId.CurrentUser();
        }));
        Button ("Search Users", ShowMenuSection<UsersSearchSection>);
        GUILayout.Space (30f);
        GUILayout.Label ("UI", GSStyles.NormalLabelText);
        Button ("Activity Feed", ShowMenuSection<ActivityFeedUiSection>);
        Button ("Notification Center", ShowMenuSection<NotificationUiSection>);
        Button ("UI Customization", ShowMenuSection<UiCustomizationSection>);
        GUILayout.Space (30f);
        GUILayout.Label ("Other", GSStyles.NormalLabelText);
        DrawSettings();
    }

    protected virtual void DrawSettings()
    {
        Button ("Settings", ShowMenuSection<SettingsSection>);
    }
    #endregion

    #region navigation

    public void ShowMainMenu () {
        _isInMainMenu = true;
        CurrentViewTitle = MainMenuTitle;
    }

    public void ShowMenuSection<Menu> () where Menu : DemoMenuSection {
        PushMenuSection<Menu>(section => { });
    }
    
    Stack<DemoMenuSection> _stack = new Stack<DemoMenuSection>();
    
    public void PushMenuSection<Menu>(Action<Menu> onCreate) where Menu : DemoMenuSection
    {
        Debug.Log("### PUSHING MENU SECTION " + this);
        ShowMainMenu();
        _isInMainMenu = false;
        if (_stack.Count > 0)
        {
            _stack.Peek().gameObject.SetActive(false);
        }

        var gameObj = new GameObject();
        gameObj.SetActive(false);
        var section = gameObj.AddComponent<Menu>();
        section.enabled = false;
        onCreate(section);
        gameObj.transform.parent = gameObject.transform;
        section.Initialize(this, _console);
        gameObj.SetActive(true);
        section.enabled = true;
        
        _stack.Push(section);
    }

    public void PopMenuSection()
    {
        var section = _stack.Pop();
        section.gameObject.SetActive(false); 
        Destroy(section.gameObject);
        if (_stack.Count > 0)
        {
            _stack.Peek().gameObject.SetActive(true);
        }
        else
        {
            ShowMainMenu();
        }
    }

    #endregion

    #region helpers

    public void FetchCurrentUserData ()
    {
        _currentUser = GetSocial.GetCurrentUser();
        _avatar = Resources.Load<Texture2D> ("GUI/default_demo_avatar");
        if (!string.IsNullOrEmpty (_currentUser.AvatarUrl)) {
            StartCoroutine (DownloadAvatar (_currentUser.AvatarUrl));
        }
    }

    public bool HasIdentity (string provider) {
        return _currentUser.Identities.ContainsKey (provider);
    }

#pragma warning disable 0618
    private IEnumerator DownloadAvatar (string url) {
        Debug.Log ("Downloading avatar: " + url);
        var www = new WWW (url);
        yield return www;
        if (!string.IsNullOrEmpty (www.error)) {
            Debug.LogError ("Failed to download avatar, reason: " + www.error);
        }
        _avatar = www.texture;
    }
#pragma warning restore 0618

    protected static void Button (string text, Action onClickCallback, bool isEnabled = true) {
        DemoGuiUtils.DrawButton (text, onClickCallback, isEnabled, GSStyles.Button);
    }

    public ActionDialog Dialog()
    {
        return GetComponentInChildren<ActionDialog>();
    }

    static void PauseGame () {
        Time.timeScale = 0;
        Debug.Log ("Paused game");
    }

    static void ResumeGame () {
        Debug.Log ("Resuming game");
        Time.timeScale = 1;
    }
    #endregion
}
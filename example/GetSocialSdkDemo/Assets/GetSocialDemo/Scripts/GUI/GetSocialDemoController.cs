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
using GetSocialSdk.MiniJSON;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using com.adjust.sdk;
using Facebook.Unity;
using TheNextFlow.UnityPlugins;

public class GetSocialDemoController : MonoBehaviour
{
    const string MainMenuTitle = "Main Menu";

    Texture2D _avatar;
    string _getSocialUnitySdkVersion;
    string _getSocialUnderlyingNativeSdkVersion;

    bool _isInMainMenu = true;
    
    Vector2 _scrollPos;
    DemoAppConsole _console;

    Dictionary<TopLevelMenuSection, DemoMenuSection> _menuSections;

    public string CurrentViewTitle { set; get; }
    private string _latestReferralData;

    // store user info to avoid constant calls to native
    CurrentUserInfo _currentUserInfo = new CurrentUserInfo();

    #region lifecycle

    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    void Awake()
    {
        _console = gameObject.GetComponent<DemoAppConsole>().Init();
        Adjust.start(new AdjustConfig("suzggk2586io", AdjustEnvironment.Production));
        SetupGetSocial();
        SetupMenuSections();
    }


    private void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            GetSocial.WhenInitialized(() =>
            {
                GetSocial.GetReferralData(
                    data =>
                    {
                        string referralToken = "";
                        string message = "Referral data: " + data;
                        if (data == null)
                        {
                            message = "No referral data";
                        }
                        else
                        {
                            referralToken = data.Token;
                        }

                        if (referralToken != _latestReferralData)
                        {
                            MobileNativePopups.OpenAlertDialog("Info", message, "OK", () => {
                            });
                            _console.LogD(message);
                            _latestReferralData = referralToken;
                        }
                    },
                    error => _console.LogE("Failed to get referral data: " + error.Message)
                );
            });
        }
    }

    void SetupMenuSections()
    {
        _menuSections = new Dictionary<TopLevelMenuSection, DemoMenuSection>
        {
            {TopLevelMenuSection.UserManagement, GetComponentInChildren<AuthSection>()},
			{TopLevelMenuSection.SmartInvitesApi, GetComponentInChildren<SmartInvitesApiSection>()},
			{TopLevelMenuSection.ActivityFeedApi, GetComponentInChildren<ActivityFeedApiSection>()},
			{TopLevelMenuSection.SocialGraphApi, GetComponentInChildren<SocialGraphSection>()},
#if USE_GETSOCIAL_UI
            {TopLevelMenuSection.SmartInvitesUi, GetComponentInChildren<SmartInvitesUiSection>()},
            {TopLevelMenuSection.ActivityFeedUi, GetComponentInChildren<ActivityFeedUiSection>()},
            {TopLevelMenuSection.UiCustomization, GetComponentInChildren<UiCustomizationSection>()},
#endif
            {TopLevelMenuSection.Settings, GetComponentInChildren<SettingsSection>()}
        };
        foreach (var section in _menuSections.Values)
        {
            section.Initialize(this, _console);
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
        HandleAndroidBackButton();
    }

    void HandleAndroidBackButton()
    {
        if (Application.platform == RuntimePlatform.Android && Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isInMainMenu)
            {
                Application.Quit();
            }
        }
    }

    protected void OnGUI()
    {
        DemoGuiUtils.DrawHeaderWrapper(DrawHeader);
        _scrollPos = DemoGuiUtils.DrawScrollBodyWrapper(_scrollPos, DrawBody);
        DemoGuiUtils.DrawFooter(DrawFooter);
    }

    #endregion

    #region callbacks

    protected void OnFacebookInited()
    {
        _console.LogD("Facebook successfully initialized");
    }

    protected void OnHideUnity(bool isGameShown)
    {
        if (isGameShown)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    #endregion

    #region methods

    public void SetupGetSocial()
    {
        GetSocial.SetNotificationListener((notification, wasClicked) =>
        {
            if (!wasClicked)
            {
                _console.LogD(string.Format("Notification: {0}, Title: {1}, Text: {2}, Data: {3}", notification.Action, notification.Title, notification.Text, notification.ActionData.ToDebugString()));
                return false;
            }
            _console.LogD("Notification received: " + notification.Action + ", with data: " + notification.ActionData.ToDebugString());
            if (notification.Action == Notification.Type.OpenProfile)
            {
                newFriend(notification.ActionData[Notification.Key.OpenProfile.UserId]);
                return true;
            }
            return false;
        });

        // Set custom listener for global errors
        GetSocial.SetGlobalErrorListener(error =>
        {
            _console.LogE("Global error listener invoked with message: " + error.Message);
        });

        _console.LogD("Setting up GetSocial...");
        _getSocialUnitySdkVersion = GetSocial.UnitySdkVersion;
        _getSocialUnderlyingNativeSdkVersion = GetSocial.NativeSdkVersion;

        RegisterInvitePlugins();

        GetSocial.User.SetOnUserChangedListener(() =>
        {
            _console.LogD(string.Format("GetSocial is initialized and user is retrieved"));
            FetchCurrentUserData();
        });
    }

    void RegisterInvitePlugins()
    {
        RegisterFacebookSharePlugin();
    }

    void RegisterFacebookSharePlugin()
    {
        var fbResult = GetSocial.RegisterInviteChannelPlugin(InviteChannelIds.Facebook,
            new FacebookSharePlugin());
        if (fbResult)
        {
            _console.LogD("Registered Facebook share plugin.");
        }
        else
        {
            _console.LogE("Failed to register Facebook share plugin.");
        }
    }

    void newFriend(string userId)
    {
        GetSocial.GetUserById(userId, user =>
        {
            MobileNativePopups.OpenAlertDialog("You have a new friend!", user.DisplayName +" is now your friend.", "OK", () => { });
        }, error =>
        {
            _console.LogE("Failed to get user: " + error.Message);
        });
    }

    protected void DrawHeader()
    {
        DrawConsoleToggle();

        GUILayout.BeginVertical(GUILayout.MaxWidth(150));
        GUILayout.Box(_avatar ?? Texture2D.whiteTexture, GUILayout.Width(100), GUILayout.Height(100));

        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), _avatar ?? Texture2D.blackTexture, ScaleMode.StretchToFill);
        // Click area for avatar
        if (GUI.Button(new Rect(0, 0, 120, 120), string.Empty, GUIStyle.none))
        {
            FetchCurrentUserData();
            MobileNativePopups.OpenAlertDialog("Info", _currentUserInfo.ToString(), "OK", () => { });
            _console.LogD(_currentUserInfo.ToString());
        }
        GUILayout.EndVertical();

        if (!string.IsNullOrEmpty(_currentUserInfo.Guid))
        {
            DrawUserInfo();
        }
    }

    void DrawUserInfo()
    {
        GUILayout.BeginVertical();
        GUILayout.Label("GUID: " + _currentUserInfo.Guid, GSStyles.NormalLabelText);
        GUILayout.Label("Display Name: " + _currentUserInfo.DisplayName, GSStyles.NormalLabelText);
        GUILayout.Label("AvatarUrl: " + _currentUserInfo.AvatarUrl, GSStyles.NormalLabelText);
        GUILayout.Label("Is anonymous? " + _currentUserInfo.IsAnonymous, GSStyles.NormalLabelText);
        GUILayout.EndVertical();
    }

    void DrawConsoleToggle()
    {
        if (GUI.Button(new Rect(Screen.width - 115, 15, 100, 100), "Toggle\nConsole"))
        {
            _console.Toggle();
        }
    }

    protected void DrawBody()
    {
        if (_console.IsVisible || !_isInMainMenu)
        {
            return;
        }

        GUILayout.Label(CurrentViewTitle, GSStyles.BigLabelText);
        DrawMainView();
    }

    void DrawFooter()
    {
        GUILayout.Label(GetFooterText(), GSStyles.NormalLabelText);
    }

    protected virtual string GetFooterText()
    {
        return string.Format("Native: v{0}, Unity: v{1}", _getSocialUnderlyingNativeSdkVersion,
            _getSocialUnitySdkVersion);
    }

    void DrawMainView()
    {
        GUILayout.Label("API", GSStyles.NormalLabelText);
		Button("Smart Invites", () => ShowMenuSection(TopLevelMenuSection.SmartInvitesApi));
		Button("Activity Feed", () => ShowMenuSection(TopLevelMenuSection.ActivityFeedApi));
        Button("User Management", () => ShowMenuSection(TopLevelMenuSection.UserManagement));
		Button("Social Graph", () => ShowMenuSection(TopLevelMenuSection.SocialGraphApi));
#if USE_GETSOCIAL_UI
        GUILayout.Space(30f);
        GUILayout.Label("UI", GSStyles.NormalLabelText);
        Button("Smart Invites", () => ShowMenuSection(TopLevelMenuSection.SmartInvitesUi));
        Button("Activity Feed", () => ShowMenuSection(TopLevelMenuSection.ActivityFeedUi));
        Button("UI Customization", () => ShowMenuSection(TopLevelMenuSection.UiCustomization));
#endif
        GUILayout.Space(30f);
        GUILayout.Label("Other", GSStyles.NormalLabelText);
        Button("Settings", () => ShowMenuSection(TopLevelMenuSection.Settings));
    }
    #endregion

    #region navigation

    public void ShowMainMenu()
    {
        CurrentViewTitle = MainMenuTitle;
        ShowMenuSection(TopLevelMenuSection.Main);
    }

    void ShowMenuSection(TopLevelMenuSection menuSection)
    {
        foreach (var section in _menuSections)
        {
            section.Value.gameObject.SetActive(false);
        }

        if (menuSection == TopLevelMenuSection.Main)
        {
            _isInMainMenu = true;
            CurrentViewTitle = MainMenuTitle;
        }
        else
        {
            _menuSections[menuSection].gameObject.SetActive(true);
            _isInMainMenu = false;
        }
    }

    #endregion

    #region helpers

    public void FetchCurrentUserData()
    {
        _currentUserInfo = new CurrentUserInfo
        {
            Guid = GetSocial.User.Id,
            DisplayName = GetSocial.User.DisplayName,
            AvatarUrl = GetSocial.User.AvatarUrl,
            IsAnonymous = GetSocial.User.IsAnonymous,
            Identities = GetSocial.User.AuthIdentities,
            PublicProperties = GetSocial.User.AllPublicProperties,
            PrivateProperties = GetSocial.User.AllPrivateProperties
        };

        _avatar = Resources.Load<Texture2D>("GUI/default_demo_avatar");
        if (!string.IsNullOrEmpty(_currentUserInfo.AvatarUrl))
        {
            StartCoroutine(DownloadAvatar(_currentUserInfo.AvatarUrl));
        }
    }

    public bool HasIdentity(string provider)
    {
        return _currentUserInfo.Identities.ContainsKey(provider);
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
        _avatar = www.texture;
    }

    static void Button(string text, Action onClickCallback, bool isEnabled = true)
    {
        DemoGuiUtils.DrawButton(text, onClickCallback, isEnabled, GSStyles.Button);
    }

    static void PauseGame()
    {
        Time.timeScale = 0;
        Debug.Log("Paused game");
    }

    static void ResumeGame()
    {
        Debug.Log("Resuming game");
        Time.timeScale = 1;
    }
    #endregion

    #region helper classes
    // container class to store current user info
    class CurrentUserInfo
    {
        public string DisplayName;
        public string AvatarUrl;
        public string Guid;
        public bool IsAnonymous;
        public Dictionary<string, string> Identities = new Dictionary<string, string>();
        public Dictionary<string, string> PublicProperties = new Dictionary<string, string>();
        public Dictionary<string, string> PrivateProperties = new Dictionary<string, string>();
        
        public override string ToString()
        {
            var dict = new Dictionary<string, object>
            {
                {"DisplayName", DisplayName},
                {"AvatarUrl", AvatarUrl},
                {"Guid", Guid},
                {"IsAnonymous", IsAnonymous},
                {"Identities", Identities},
                {"PublicProperties", PublicProperties},
                {"PrivateProperties", PrivateProperties}
            };
			return GSJson.Serialize (dict);
        }
    }

    enum TopLevelMenuSection
    {
        Main,
        UserManagement,
        SocialGraphApi,
        // Smart Invites
        SmartInvitesApi,
        SmartInvitesUi,

        // Activity Feed
        ActivityFeedApi,
        ActivityFeedUi,

        UiCustomization,
        Settings
    }
    #endregion
}

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
using com.adjust.sdk;
using Facebook.Unity;
using GetSocialDemo.Scripts.GUI.Sections;
using GetSocialSdk.Core;
using GetSocialSdk.MiniJSON;
using GetSocialSdk.Ui;
using UnityEngine;

public class GetSocialDemoController : MonoBehaviour {
    const string MainMenuTitle = "Main Menu";

    Texture2D _avatar;
    string _getSocialUnitySdkVersion;
    string _getSocialUnderlyingNativeSdkVersion;

    bool _isInMainMenu = true;

    Vector2 _scrollPos;
    DemoAppConsole _console;

    List<DemoMenuSection> _menuSections;

    public string CurrentViewTitle { set; get; }
    private string _latestReferralData;
    private string _latestChatId;

    // store user info to avoid constant calls to native
    CurrentUserInfo _currentUserInfo = new CurrentUserInfo ();

    #region lifecycle

    [SuppressMessage ("ReSharper", "UnusedMember.Local")]
    void Awake () {
        // change default framerate
        Application.targetFrameRate = 100;
        _console = gameObject.GetComponent<DemoAppConsole> ().Init ();
        Adjust.start (new AdjustConfig ("suzggk2586io", AdjustEnvironment.Production));
        SetupAppsFlyer ();
        SetupGetSocial ();
        SetupMenuSections ();
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
            GetSocial.WhenInitialized (() => {
                if (_latestChatId != null) {
                    ShowChat ();
                } else {
                    GetSocial.GetReferralData (
                        data => {
                            var referralToken = "";
                            var message = "Referral data: " + data;
                            string promoCode = null;
                            if (data == null) {
                                message = "No referral data";
                            } else {
                                promoCode = data.ReferralLinkParams.ContainsKey (LinkParams.KeyPromoCode) ? data.ReferralLinkParams[LinkParams.KeyPromoCode] : null;
                                referralToken = data.Token;
                            }

                            if (!referralToken.Equals (_latestReferralData)) {
                                // show popup only if chat is not shown
                                if (_latestChatId == null) {
                                    if (promoCode != null) {
                                        message += "\n\nPROMO CODE: " + promoCode;
                                    }
                                    var popup = new MNPopup ("Referral Data", message);
                                    popup.AddAction ("OK", () => { });
                                    if (promoCode != null) {
                                        popup.AddAction ("Claim Promo Code", () => PromoCodesSection.ClaimPromoCode (promoCode));
                                    }
                                    popup.Show ();
                                }
                                _console.LogD (message);
                                _latestReferralData = referralToken;
                            }
                        },
                        error => _console.LogE ("Failed to get referral data: " + error.Message)
                    );
                }
            });
        } else {
            GetSocialUi.CloseView (false);
        }
    }

    void SetupMenuSections () {
        _menuSections = new List<DemoMenuSection> {
            GetComponentInChildren<AuthSection> (),
            GetComponentInChildren<SmartInvitesApiSection> (),
            GetComponentInChildren<ActivityFeedApiSection> (),
            GetComponentInChildren<SocialGraphSection> (),
            GetComponentInChildren<NotificationsApiSection> (),
            GetComponentInChildren<InAppPurchaseApiSection> (),
            GetComponentInChildren<SendNotificationSection> (),
            GetComponentInChildren<PromoCodesSection> (),
            GetComponentInChildren<CustomAnalyticsEventSection> (),
            GetComponentInChildren<SetReferrerSection> (),
#if USE_GETSOCIAL_UI
            GetComponentInChildren<SmartInvitesUiSection> (),
            GetComponentInChildren<ActivityFeedUiSection> (),
            GetComponentInChildren<NotificationUiSection> (),
            GetComponentInChildren<UiCustomizationSection> (),
#endif
            GetComponentInChildren<SettingsSection> ()
        };

        _menuSections.ForEach (section => section.Initialize (this, _console));
        ShowMainMenu ();
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
                popup.AddAction (button.Title, () => { ProcessAction (button.Id, notification); });
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
        GetSocial.SetPushNotificationTokenListener (deviceToken => {
            _console.LogD (string.Format ("DeviceToken: {0}", deviceToken), false);
        });
        GetSocial.SetNotificationListener ((notification, wasClicked) => {
            // handle chat message
            if (notification.NotificationAction.Type.Equals ("open_chat_message")) {
                _latestChatId = notification.NotificationAction.Data["open_messages_for_id"];
                if (GetSocial.IsInitialized) {
                    ShowChat ();
                }
                return true;
            }
            if (notification.NotificationAction.Type.Equals (GetSocialActionType.AddFriend)) {
                ShowPopup (notification);
                return true;
            }
            _console.LogD (string.Format ("Notification(wasClicked : {0}): {1}", wasClicked, notification));
            if (!wasClicked) {
                return false;
            }

            return HandleAction (notification.NotificationAction);
        });

        // Set custom listener for global errors
        GetSocial.SetGlobalErrorListener (error => {
            _console.LogE ("Global error listener invoked with message: " + error.Message);
        });

        _console.LogD ("Setting up GetSocial...");
        _getSocialUnitySdkVersion = GetSocial.UnitySdkVersion;
        _getSocialUnderlyingNativeSdkVersion = GetSocial.NativeSdkVersion;

        RegisterInvitePlugins ();

        GetSocial.User.SetOnUserChangedListener (() => {
            _console.LogD ("GetSocial is initialized and user is retrieved");
            GetSocial.User.IsPushNotificationsEnabled (isEnabled => ((SettingsSection) _menuSections.Find (section => section is SettingsSection)).PnEnabled = isEnabled, error => Debug.LogError ("Failed to get PN status " + error));
            FetchCurrentUserData ();
        });
    }

    private void ShowChat () {
        MainThreadExecutor.Queue (() => {
            ShowMenuSection<SocialGraphSection> ();
            _menuSections.ForEach (section => {
                if (section is SocialGraphSection) {
                    ((SocialGraphSection) section).MessageFriend (_latestChatId);
                    _latestChatId = null;
                }
            });
            if (_console.IsVisible) {
                _console.Toggle ();
            }
        });
    }

    private void ShowPopup (Notification notification) {
        _notification = notification;
    }

    public static void ProcessAction (string actionButtonId, Notification notification) {
        if (actionButtonId.Equals (ActionButton.ConsumeAction)) {
            GetSocial.User.SetNotificationsStatus (new List<string> { notification.Id }, NotificationStatus.Consumed, () => { }, Debug.LogError);
            GetSocial.ProcessAction (notification.NotificationAction);
        } else if (actionButtonId.Equals (ActionButton.IgnoreAction)) {
            GetSocial.User.SetNotificationsStatus (new List<string> { notification.Id }, NotificationStatus.Ignored, () => { }, Debug.LogError);
        }
    }

    public bool HandleAction (GetSocialAction action) {
        if (action.Type == GetSocialActionType.OpenProfile) {
            newFriend (action.Data[GetSocialActionKeys.OpenProfile.UserId]);
            return true;
        }
        return false;
    }

    void RegisterInvitePlugins () {
        GetSocialFBMessengerPluginHelper.RegisterFBMessengerPlugin ();
        RegisterFacebookSharePlugin ();
    }

    void RegisterFacebookSharePlugin () {
        var fbResult = GetSocial.RegisterInviteChannelPlugin (InviteChannelIds.Facebook,
            new FacebookSharePlugin ());
        if (fbResult) {
            _console.LogD ("Registered Facebook share plugin.");
        } else {
            _console.LogE ("Failed to register Facebook share plugin.");
        }
    }

    void newFriend (string userId) {
        GetSocial.GetUserById (userId, user => {
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
            FetchCurrentUserData ();
            DemoUtils.ShowPopup ("Info", _currentUserInfo.ToString ());
            _console.LogD (_currentUserInfo.ToString ());
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
        if (GetSocial.Device.IsTestDevice)
        {
            GUILayout.Label("TestDevice", fpsStyle);
        }
        GUILayout.EndVertical ();

        if (!string.IsNullOrEmpty (_currentUserInfo.Guid)) {
            DrawUserInfo ();
        }
    }

    void DrawUserInfo () {
        GUILayout.BeginVertical ();
        GUILayout.Label ("GUID: " + _currentUserInfo.Guid, GSStyles.NormalLabelText);
        GUILayout.Label ("Display Name: " + _currentUserInfo.DisplayName, GSStyles.NormalLabelText);
        GUILayout.Label ("AvatarUrl: " + _currentUserInfo.AvatarUrl, GSStyles.NormalLabelText);
        GUILayout.Label ("Is anonymous? " + _currentUserInfo.IsAnonymous, GSStyles.NormalLabelText);
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
        Button ("Smart Invites", ShowMenuSection<SmartInvitesApiSection>);
        Button ("Activity Feed", ShowMenuSection<ActivityFeedApiSection>);
        Button ("User Management", ShowMenuSection<AuthSection>);
        Button ("Social Graph", ShowMenuSection<SocialGraphSection>);
        Button ("Notifications Api", ShowMenuSection<NotificationsApiSection>);
        Button ("InApp Purchase Api", ShowMenuSection<InAppPurchaseApiSection>);
        Button ("Send Notification", ShowMenuSection<SendNotificationSection>);
        Button ("Promo Codes", ShowMenuSection<PromoCodesSection>);
        Button ("Custom Analytics Events", ShowMenuSection<CustomAnalyticsEventSection>);
        Button ("Set Referrer", ShowMenuSection<SetReferrerSection>);
#if USE_GETSOCIAL_UI
        GUILayout.Space (30f);
        GUILayout.Label ("UI", GSStyles.NormalLabelText);
        Button ("Smart Invites", ShowMenuSection<SmartInvitesUiSection>);
        Button ("Activity Feed", ShowMenuSection<ActivityFeedUiSection>);
        Button ("Notification Center", ShowMenuSection<NotificationUiSection>);
        Button ("UI Customization", ShowMenuSection<UiCustomizationSection>);
#endif
        GUILayout.Space (30f);
        GUILayout.Label ("Other", GSStyles.NormalLabelText);
        Button ("Settings", ShowMenuSection<SettingsSection>);
    }
    #endregion

    #region navigation

    public void ShowMainMenu () {
        _isInMainMenu = true;
        _menuSections.ForEach (section => section.gameObject.SetActive (false));
        CurrentViewTitle = MainMenuTitle;
    }

    public void ShowMenuSection<Menu> () where Menu : DemoMenuSection {
        _isInMainMenu = false;
        _menuSections.ForEach (section => section.gameObject.SetActive (section is Menu));
    }

    #endregion

    #region helpers

    public void FetchCurrentUserData () {
        _currentUserInfo = new CurrentUserInfo {
            Guid = GetSocial.User.Id,
            DisplayName = GetSocial.User.DisplayName,
            AvatarUrl = GetSocial.User.AvatarUrl,
            IsAnonymous = GetSocial.User.IsAnonymous,
            Identities = GetSocial.User.AuthIdentities,
            PublicProperties = GetSocial.User.AllPublicProperties,
            PrivateProperties = GetSocial.User.AllPrivateProperties
        };

        _avatar = Resources.Load<Texture2D> ("GUI/default_demo_avatar");
        if (!string.IsNullOrEmpty (_currentUserInfo.AvatarUrl)) {
            StartCoroutine (DownloadAvatar (_currentUserInfo.AvatarUrl));
        }
    }

    public bool HasIdentity (string provider) {
        return _currentUserInfo.Identities.ContainsKey (provider);
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

    static void Button (string text, Action onClickCallback, bool isEnabled = true) {
        DemoGuiUtils.DrawButton (text, onClickCallback, isEnabled, GSStyles.Button);
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

    #region helper classes
    // container class to store current user info
    class CurrentUserInfo {
        public string DisplayName;
        public string AvatarUrl;
        public string Guid;
        public bool IsAnonymous;
        public Dictionary<string, string> Identities = new Dictionary<string, string> ();
        public Dictionary<string, string> PublicProperties = new Dictionary<string, string> ();
        public Dictionary<string, string> PrivateProperties = new Dictionary<string, string> ();

        public override string ToString () {
            var dict = new Dictionary<string, object> { { "DisplayName", DisplayName },
                    { "AvatarUrl", AvatarUrl },
                    { "Guid", Guid },
                    { "IsAnonymous", IsAnonymous },
                    { "Identities", Identities },
                    { "PublicProperties", PublicProperties },
                    { "PrivateProperties", PrivateProperties }
                };
            return GSJson.Serialize (dict);
        }
    }
    #endregion
}
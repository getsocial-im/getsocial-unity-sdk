
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

using UnityEngine;
using GetSocialSdk.Core;
using System;
using System.Linq;
using Facebook.Unity;

public class AuthSection : DemoMenuSection
{
    static readonly string[] HeroNames =
    {
        "Batman",
        "Spiderman",
        "Captain America",
        "Green Lantern",
        "Wolverine",
        "Catwomen",
        "Iron Man",
        "Superman",
        "Wonder Woman",
        "Aquaman",
        "Captain Linger"
    };

    static readonly string[] FacebookPermissions = {"public_profile", "user_friends"};
    const string CustomProviderId = "custom";

    string _customUserId = "UnityUser";
    string _customProviderToken = "custom_provider_token";

    protected override void InitGuiElements()
    {
    }

    protected override string GetTitle()
    {
        return "User Management";
    }

    protected override void DrawSectionBody()
    {
        DrawUserManagement();
    }

    void DrawUserManagement()
    {
        // facebook
        DrawAddRemoveProvider("Facebook Identity", () =>
        {
            DemoGuiUtils.DrawButton("Add", AddFacebookUserIdentity, !UserHasFacebookIdentity(), GSStyles.Button);
            DemoGuiUtils.DrawButton("Remove", RemoveFacebookUserIdentity, UserHasFacebookIdentity(), GSStyles.Button);
            DemoGuiUtils.DrawButton("Switch User", SwitchToFacebookUser, true, GSStyles.Button);
        });

        // custom provider
        DrawAddRemoveCustomProvider();
    }

    void DrawAddRemoveCustomProvider()
    {
        DemoGuiUtils.Space();
        GUILayout.BeginVertical("Box");
        GUILayout.Label("Custom Provider Identity", GSStyles.NormalLabelText);

        // user id
        GUILayout.BeginHorizontal();
        GUILayout.Label("UserID: ", GSStyles.NormalLabelText, GUILayout.Width(100f));
        _customUserId = GUILayout.TextField(_customUserId, GSStyles.TextField);
        GUILayout.EndHorizontal();

        if (string.IsNullOrEmpty(_customUserId))
        {
            GUILayout.Label("Custom Provider User ID cannot be empty", GSStyles.NormalLabelTextRed);
        }

        // user display name
        GUILayout.BeginHorizontal();
        var displayName = GetSocial.IsInitialized ? GetSocial.User.DisplayName : "not initialized";
        GUILayout.Label("User DisplayName: " + displayName, GSStyles.NormalLabelText, GUILayout.Width(300f));
        DemoGuiUtils.DrawButton("Change Display Name", () => GetSocial.User.SetDisplayName(GetDisplayNameForUser(), () =>
        {
            _console.LogD("User display name has been changed");
            demoController.FetchCurrentUserData();
        }, error =>
        {
            _console.LogE("Failed to change user DisplayName: " + error);
        }), true, GSStyles.Button);
        GUILayout.EndHorizontal();

        //user avatar
        GUILayout.BeginHorizontal();
        GUILayout.Label("User Avatar: ", GSStyles.NormalLabelText, GUILayout.Width(100f));
        DemoGuiUtils.DrawButton("Change Avatar", () => GetSocial.User.SetAvatarUrl(GetAvatarForUser(), () =>
        {
            _console.LogD("User avatar url has been changed");
            demoController.FetchCurrentUserData();
        }, error =>
        {
            _console.LogE("Failed to change user AvatarURL: " + error);
        }), true, GSStyles.Button);
        GUILayout.EndHorizontal();


        // token
        GUILayout.BeginHorizontal();
        GUILayout.Label("Token: ", GSStyles.NormalLabelText, GUILayout.Width(100f));
        _customProviderToken = GUILayout.TextField(_customProviderToken, GSStyles.TextField);
        GUILayout.EndHorizontal();

        if (string.IsNullOrEmpty(_customProviderToken))
        {
            GUILayout.Label("Custom Provider token cannot be empty", GSStyles.NormalLabelTextRed);
        }
        GUILayout.BeginHorizontal();
        GUI.enabled = !string.IsNullOrEmpty(_customProviderToken) && !string.IsNullOrEmpty(_customUserId);
        DemoGuiUtils.DrawButton("Add", AddCustomUserIdentity, !UserHasCustomIdentity(), GSStyles.Button);
        GUI.enabled = true;
        DemoGuiUtils.DrawButton("Remove", RemoveCustomUserIdentity, UserHasCustomIdentity(), GSStyles.Button);
        DemoGuiUtils.DrawButton("Switch User", SwitchToCustomIdentityUser, true, GSStyles.Button);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        DemoGuiUtils.Space();
    }

    static void DrawAddRemoveProvider(string title, Action drawButtonsAction)
    {
        DemoGuiUtils.Space();
        GUILayout.BeginVertical("Box");
        GUILayout.Label(title, GSStyles.NormalLabelText);
        GUILayout.BeginHorizontal();
        drawButtonsAction();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    bool UserHasFacebookIdentity()
    {
        return demoController.HasIdentity(AuthIdentityProvider.Facebook);
    }

    bool UserHasCustomIdentity()
    {
        return demoController.HasIdentity(CustomProviderId);
    }

    #region switch_user

    void SwitchToFacebookUser()
    {
        if (FB.IsLoggedIn)
        {
            SwitchToFacebookUserInternal();
        }
        else
        {
            FB.LogInWithReadPermissions(FacebookPermissions, result =>
            {
                if (result.Cancelled)
                {
                    _console.LogE("Facebook login was cancelled");
                    return;
                }

                if (!string.IsNullOrEmpty(result.Error))
                {
                    _console.LogE("Error happened during Facebook login: " + result.Error);
                    return;
                }

                _console.LogD("Logged in to Facebook: " + result.RawResult);
                SwitchToFacebookUserInternal();
            });
        }
    }

    void SwitchToFacebookUserInternal()
    {
        _console.LogD(string.Format("Switching to FB user, token: {0}", AccessToken.CurrentAccessToken.TokenString));
        GetSocial.User.SwitchUser(AuthIdentity.CreateFacebookIdentity(AccessToken.CurrentAccessToken.TokenString),
            () =>
            {
                _console.LogD("Successfully switched to FB user");
                demoController.FetchCurrentUserData();
            },
            error => _console.LogE("Switching to FB user failed, reason: " + error));
    }

    #endregion

    public void AddFacebookUserIdentity()
    {
        if (!UserHasFacebookIdentity())
        {
            FB.LogInWithReadPermissions(FacebookPermissions, result =>
            {
                if (result.Cancelled)
                {
                    _console.LogE("Facebook login was cancelled");
                    return;
                }

                if (!string.IsNullOrEmpty(result.Error))
                {
                    _console.LogE("Error happened during Facebook login: " + result.Error);
                    return;
                }

                _console.LogD("Logged in to Facebook: " + result.RawResult);
                AddFacebookUserIdentityInternal();
            });
        }
        else
        {
            _console.LogD("User already has Facebook identity");
        }
    }

    #region fb_add_remove_identity

    void AddFacebookUserIdentityInternal()
    {
        _console.LogD(string.Format("Adding FB identity, token: {0}", AccessToken.CurrentAccessToken.TokenString));
        GetSocial.User.AddAuthIdentity(AuthIdentity.CreateFacebookIdentity(AccessToken.CurrentAccessToken.TokenString),
            () =>
            {
                _console.LogD("Successfully added Facebook Identity");
                demoController.FetchCurrentUserData();
            },
            error => _console.LogE("Adding Facebook identity failed, reason: " + error),
            OnAddUserIdentityConflict);
    }

    void RemoveFacebookUserIdentity()
    {
        if (UserHasFacebookIdentity())
        {
            RemoveFacebookUserIdentityInternal();
        }
        else
        {
            _console.LogD("User doesn't have Facebook identity");
        }
    }

    void RemoveFacebookUserIdentityInternal()
    {
        GetSocial.User.RemoveAuthIdentity(AuthIdentityProvider.Facebook,
            () =>
            {
                _console.LogD("Successfully removed Facebook user identity");
                demoController.FetchCurrentUserData();
            },
            error => _console.LogE("Removing Facebook identity failed, reason: " + error));
    }

    #endregion

    #region add_remove_identity_custom

    void AddCustomUserIdentity()
    {
        GetSocial.User.AddAuthIdentity(AuthIdentity.CreateCustomIdentity(CustomProviderId, _customUserId, _customProviderToken),
            () =>
            {
                _console.LogD("Successfully added custom identity");
                demoController.FetchCurrentUserData();
            },
            error => _console.LogE(string.Format("Failed to add user identity '{0}', reason: {1}", CustomProviderId,
                error)),
            OnAddUserIdentityConflict);
    }

    void RemoveCustomUserIdentity()
    {
        GetSocial.User.RemoveAuthIdentity(CustomProviderId,
            () =>
            {
                _console.LogD(string.Format("Successfully removed user identity '{0}'", CustomProviderId));
                demoController.FetchCurrentUserData();
            },
            error => _console.LogE(string.Format("Failed to remove user identity '{0}', reason: {1}", CustomProviderId,
                error))
        );
    }

    void SwitchToCustomIdentityUser()
    {
        GetSocial.User.SwitchUser(AuthIdentity.CreateCustomIdentity(CustomProviderId, _customUserId, _customProviderToken),
            () =>
            {
                _console.LogD("Successfully switched to Custom provider user");
                demoController.FetchCurrentUserData();
            },
            error => _console.LogE("Switching to custom provider user failed, reason: " + error));
    }

    void OnAddUserIdentityConflict(ConflictUser user)
    {
        _console.LogW("User identity conflict: " + user);
    }

    #endregion

    static string GetAvatarForUser()
    {
        var randomStr = RandomString(5);
        int index = randomStr.GetHashCode() % 10;
        return string.Format("http://api.adorable.io/avatars/150/{0}", index);
    }

    static string GetDisplayNameForUser()
    {
        var randomStr = RandomString(5);
        int index = randomStr.GetHashCode() % 10;
        return HeroNames[Mathf.Abs(index)] + " Unity " + randomStr;
    }

    static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new System.Random();
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
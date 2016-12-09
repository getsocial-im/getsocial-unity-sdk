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

#if UNITY_ANDROID
using GooglePlayGames;
#endif

public class AuthSection : DemoMenuSection
{
    // object to store conflict state
    private class ConflictResolutionInfo
    {
        public User current;
        public User remote;
        public Action<CurrentUser.AddIdentityConflictResolutionStrategy> conflictResolverAction;
    }

    private static readonly string[] HeroNames = 
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

    public const string FacebookPermissions = "public_profile,user_friends";
    private const string ProviderCustom = "custom";

    private ConflictResolutionInfo conflictInfo = null;
    private string customProviderToken = "custom_provider_token";

    public bool IsInAddIdentityConflict
    {
        get
        {
            return conflictInfo != null;
        }
    }

    #region implemented abstract members of DemoMenuSection

    protected override void InitGuiElements()
    {
    }

    protected override string GetTitle()
    {
        return IsInAddIdentityConflict ? "Conflict Resolution" : "User Management";
    }

    protected override void DrawSectionBody()
    {
        if(IsInAddIdentityConflict)
        {
            DrawAddUserIdentityConflictResolutionDialog();
            return;
        }

        DrawUserManagement();
    }
    
    protected override bool IsBackButtonActive()
    {
        return !IsInAddIdentityConflict;
    }
    
    protected override void GoBack()
    {
        if(IsInAddIdentityConflict)
        {
            return;
        }

        base.GoBack();
    }

    private void DrawUserManagement()
    {
        GUILayout.BeginHorizontal();
        DemoGuiUtils.DrawButton("Set New Avatar", SetNewAvatar, style: GSStyles.Button);
        DemoGuiUtils.DrawButton("Set New Display Name", SetNewDisplayName, style: GSStyles.Button);
        GUILayout.EndHorizontal();

        // facebook
        DrawAddRemoveProvider("Facebook Identity", () => {
            DemoGuiUtils.DrawButton("Add", AddFacebookUserIdentity, !UserHasFacebookIdentity(), GSStyles.Button);
            DemoGuiUtils.DrawButton("Remove", RemoveFacebookUserIdentity, UserHasFacebookIdentity(), GSStyles.Button);
        });
        
        // google play
#if UNITY_ANDROID
        DrawAddRemoveProvider("Google Play Identity", () => {
            DemoGuiUtils.DrawButton("Add", AddGooglePlayUserIdentity, !UserHasGooglePlayIdentity(), GSStyles.Button);
            DemoGuiUtils.DrawButton("Remove", RemoveGooglePlayUserIdentity, UserHasGooglePlayIdentity(), GSStyles.Button);
        });
#endif
        
        // custom provider
        DrawAddRemoveCustomProvider();
        
        // reset user
        DemoGuiUtils.DrawButton("Reset User", ResetCurrentUser, style: GSStyles.Button);
    }
    
    private void DrawAddRemoveCustomProvider()
    {
        DemoGuiUtils.Space();
        GUILayout.BeginVertical("Box");
        GUILayout.Label("Custom Provider Identity", GSStyles.NormalLabelText);
        
        // token
        GUILayout.BeginHorizontal();
        GUILayout.Label("Token: ", GSStyles.NormalLabelText, GUILayout.Width(100f));
        customProviderToken = GUILayout.TextField(customProviderToken, GSStyles.TextField);
        GUILayout.EndHorizontal();
        
        if(string.IsNullOrEmpty(customProviderToken))
        {
            GUILayout.Label("Custom Provider token cannot be empty", GSStyles.NormalLabelTextRed);
        }
        GUILayout.BeginHorizontal();
        GUI.enabled = !string.IsNullOrEmpty(customProviderToken);
        DemoGuiUtils.DrawButton("Add", AddCustomUserIdentity, !UserHasCustomIdentity(), GSStyles.Button);
        GUI.enabled = true;
        DemoGuiUtils.DrawButton("Remove", RemoveCustomUserIdentity, UserHasCustomIdentity(), GSStyles.Button);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        DemoGuiUtils.Space();
    }
    
    private void DrawAddRemoveProvider(string title, Action drawButtonsAction)
    {
        DemoGuiUtils.Space();
        GUILayout.BeginVertical("Box");
        GUILayout.Label(title, GSStyles.NormalLabelText);
        GUILayout.BeginHorizontal();
        drawButtonsAction();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    private void SetNewAvatar()
    {
        getSocial.CurrentUser.SetAvatarUrl(GetAvatarForUser(RandomString(10)), 
            () =>
            {
                console.LogD("Avatar successfully updated");
                demoController.FetchCurrentUserData();
            },
            error => console.LogE("Updating the avatar failed"));
    }

    private void SetNewDisplayName()
    {
        getSocial.CurrentUser.SetDisplayName(GetDisplayNameForUser(), 
            () =>
            {
                console.LogD("Display name successfully updated");
                demoController.FetchCurrentUserData();
            },
            error => console.LogE("Updating display name failed"));
    }

    private void DrawAddUserIdentityConflictResolutionDialog()
    {
        GUILayout.Label("The identity you are trying to add already belongs to another user. \n\n" +
        "You can continue with the current user or continue with the user identity currently belongs to", GSStyles.NormalLabelText);
        GUILayout.Space(20f);
        GUILayout.Label("You must choose the user to continue with", GSStyles.NormalLabelTextRed);
        GUILayout.Space(10f);
        DrawConflictResolutionButton(conflictInfo.current, CurrentUser.AddIdentityConflictResolutionStrategy.Current);
        DrawConflictResolutionButton(conflictInfo.remote, CurrentUser.AddIdentityConflictResolutionStrategy.Remote);
    }

    private void DrawConflictResolutionButton(User user, CurrentUser.AddIdentityConflictResolutionStrategy strategy)
    {
        string userType = strategy == CurrentUser.AddIdentityConflictResolutionStrategy.Current ? "Current: " : "Remote: ";
        DemoGuiUtils.DrawButton(userType + user.ToButtonString(),
            () =>
            {
                conflictInfo.conflictResolverAction(strategy);
                conflictInfo = null;
            }, 
            true, GSStyles.Button);
    }

    #endregion

    private bool UserHasFacebookIdentity()
    {
        return getSocial.CurrentUser.HasIdentityForProvider(UserIdentity.ProviderFacebook);
    }
    
    #if UNITY_ANDROID
    private bool UserHasGooglePlayIdentity()
    {
        return getSocial.CurrentUser.HasIdentityForProvider(UserIdentity.ProviderGooglePlay);
    }
    #endif
    
    private bool UserHasCustomIdentity()
    {
        return getSocial.CurrentUser.HasIdentityForProvider(ProviderCustom);
    }
    
    public void AddFacebookUserIdentity()
    {
        if(!UserHasFacebookIdentity())
        {
            if(FB.IsLoggedIn)
            {
                AddFacebookUserIdentityInternal();
            }
            else
            {
                FB.Login(FacebookPermissions, result =>
                { 
                    console.LogD("Logged in to Facebook: " + result.Text, false);
                    AddFacebookUserIdentityInternal();
                });
            }
        }
        else
        {
            console.LogD("User already has Facebook identity");
        }
    }

    #region fb_add_remove_identity

    private void AddFacebookUserIdentityInternal()
    {
        var fbIdentity = UserIdentity.CreateFacebookIdentity(FB.AccessToken);
        
        getSocial.CurrentUser.AddUserIdentity(fbIdentity,
            result =>
            {
                console.LogD("Successfully completed with result: " + result, false);
                demoController.FetchCurrentUserData();
            },
            error => console.LogE("Adding Facebook identity failed, reason: " + error),
            OnAddUserIdentityConflict);
    }

    private void RemoveFacebookUserIdentity()
    {
        if(UserHasFacebookIdentity())
        {
            RemoveFacebookUserIdentityInternal();
        }
        else
        {
            console.LogD("User doesn't have Facebook identity");
        }
    }

    void RemoveFacebookUserIdentityInternal()
    {
        getSocial.CurrentUser.RemoveUserIdentity(UserIdentity.ProviderFacebook, 
            () =>
            {
                console.LogD("Successfully removed Facebook user identity");
                demoController.FetchCurrentUserData();
            },
            error => console.LogE("Removing Facebook identity failed, reason: " + error));
    }

    #endregion

    #if UNITY_ANDROID
    private void AddGooglePlayUserIdentity()
    {
        if(!UserHasGooglePlayIdentity())
        {
            PlayGamesPlatform.Instance.Authenticate((bool success) =>
            {
                if(success)
                {
                    AddGooglePlayUserIdentityInternal();
                }
            });
        }
        else
        {
            console.LogD("User already has Google Play identity");
        }
    }

    private void AddGooglePlayUserIdentityInternal()
    {
        string googlePlayToken = PlayGamesPlatform.Instance.GetToken();
        var googlePlayIdentity = UserIdentity.Create(UserIdentity.ProviderGooglePlay, googlePlayToken);

        getSocial.CurrentUser.AddUserIdentity(googlePlayIdentity,
            result =>
            {
                console.LogD("Successfully completed with result: " + result, false);
                demoController.FetchCurrentUserData();
            },
            error => console.LogE("Adding Google Play identity failed, reason: " + error),
            OnAddUserIdentityConflict);
    }

    private void RemoveGooglePlayUserIdentity()
    {
        if(UserHasGooglePlayIdentity())
        {
            RemoveGooglePlayUserIdentityInternal();
        }
        else
        {
            console.LogD("User doesn't have Facebook identity");
        }
    }

    void RemoveGooglePlayUserIdentityInternal()
    {
        getSocial.CurrentUser.RemoveUserIdentity(UserIdentity.ProviderGooglePlay, 
            () => console.LogD("Removed Google Play user identity"), 
            error => console.LogE("Removing Google Play identity failed, reason: " + error));
    }
    #endif

    #region add_remove_identity_custom

    private void AddCustomUserIdentity()
    {
        var identityInfo = CreateProviderCustomIdentityInfo();
        getSocial.CurrentUser.AddUserIdentity(identityInfo,
            result =>
            { 
                console.LogD("Successfully completed with result: " + result, false);
                demoController.FetchCurrentUserData();
            },
            error => console.LogE(string.Format("Failed to add user identity '{0}', reason: {1}", ProviderCustom, error))
        );
    }

    private void RemoveCustomUserIdentity()
    {
        getSocial.CurrentUser.RemoveUserIdentity(ProviderCustom,
            () =>
            {
                console.LogD(string.Format("Successfully removed user identity '{0}'", ProviderCustom));
                demoController.FetchCurrentUserData();
            },
            error => console.LogE(string.Format("Failed to remove user identity '{0}', reason: {1}", ProviderCustom, error))
        );
    }

    private void ResetCurrentUser()
    {
        getSocial.CurrentUser.Reset(
            () =>
            { 
                console.LogD("Successfully reseted current user");
                if(FB.IsLoggedIn)
                {
                    FB.Logout();
                }
#if UNITY_ANDROID
                PlayGamesPlatform.Instance.SignOut();
#endif
                demoController.FetchCurrentUserData();
            },
            error => console.LogE(string.Format("Failed to reset current user, reason: {0}", error))
        );
    }

    private void OnAddUserIdentityConflict(User currentUser, User remoteUser, 
                                           Action<CurrentUser.AddIdentityConflictResolutionStrategy> resolveConflictAction)
    {
        conflictInfo = new ConflictResolutionInfo {
            current = currentUser,
            remote = remoteUser,
            conflictResolverAction = resolveConflictAction
        };
    }
    #endregion

    #region helpers
    private UserIdentity CreateProviderCustomIdentityInfo()
    {
        var userId = SystemInfo.deviceUniqueIdentifier + "_getsocial";
        return UserIdentity.Create(ProviderCustom, userId, customProviderToken);
    }

    private static string GetAvatarForUser(string userId)
    {
        return string.Format("http://api.adorable.io/avatars/150/{0}", userId);
    }

    private static string GetDisplayNameForUser()
    {
        var randomStr = RandomString(5);
        int index = randomStr.GetHashCode() % 10;
        return HeroNames[Mathf.Abs(index)] + " Unity " + randomStr;
    }

    private static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new System.Random();
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }
    #endregion
}

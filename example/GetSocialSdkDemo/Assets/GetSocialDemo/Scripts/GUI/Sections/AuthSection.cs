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
using System.Collections.Generic;
using System.Linq;
using Facebook.Unity;
using Random = UnityEngine.Random;

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

    public static readonly string[] FacebookPermissions = {"public_profile", "user_friends"};
    public const string CustomProviderId = "custom";

    string _customUserId = "UnityUser";
    string _customProviderToken = "custom_provider_token";

    private string _key = "key", _val = "value";
    private string _keyPrivate = "key", _valPrivate = "value";
    private string _keyIncrement = "token", _valIncrement = "10.5";

    private readonly List<PropertiesUpdate> _batchUpdatePublicProperties = new List<PropertiesUpdate>();
    private readonly List<PropertiesUpdate> _batchUpdatePrivateProperties = new List<PropertiesUpdate>();
    private bool _useAvatarUrl;

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
        DrawAddRemoveProperty();
        DrawBatchUpdateSection();

        DrawGetUsersByAuthIdentitiesSection();

        DemoGuiUtils.DrawButton("Log Out", () =>
        {
            GetSocial.ResetUser(() =>
                {
                    _console.LogD("User has been successfully logged out.");
                    demoController.FetchCurrentUserData();
                },
                error => _console.LogE("Failed to log out user, error: " + error)
            );
        }, true, GSStyles.Button);

        DemoGuiUtils.DrawButton("Reset without init", () => 
        {
            GetSocial.Reset(() => 
            {
                demoController.PopMenuSection();
                demoController.FetchCurrentUserData();
            }, error => 
            {
                _console.LogE("Failed to reset user: " + error);
            });
        }, true, GSStyles.Button);
    }

    private void DrawBatchUpdateSection()
    {
        DemoGuiUtils.Space();
        GUILayout.BeginVertical("Box");
        
        DemoGuiUtils.DrawToggle(ref _useAvatarUrl, "Use Avatar URL");
        
        DrawBatchSection("Public", _batchUpdatePublicProperties);
        DrawBatchSection("Private", _batchUpdatePrivateProperties);
        
        DemoGuiUtils.DrawButton("Update User", () => GetSocial.GetCurrentUser().UpdateDetails(GetUpdateForUser(),
            () =>
            {
                _console.LogD("User updated");
                demoController.FetchCurrentUserData();
            }, error => { _console.LogE("Failed to update user SetUserDetails: " + error); }), true, GSStyles.Button);
        
        GUILayout.EndVertical();
    }

    private void DrawGetUsersByAuthIdentitiesSection()
    {
        DemoGuiUtils.Space();
        GUILayout.BeginVertical("Box");

        var providerUserIds = new List<string>();
        providerUserIds.Add(_customUserId);
        DemoGuiUtils.DrawButton("Get Users By Auth Identities", () => {
            Communities.GetUsers(UserIdList.CreateWithProvider(CustomProviderId, providerUserIds), users => {
                _console.LogD("GetUsersByAuthIdentities result: " + users.ToDebugString());
            }, error => {
                _console.LogE("Failed to get users by auth identities: " + error);
            });
        });
        GUILayout.EndVertical();
    }

    private static void DrawBatchSection(string name, List<PropertiesUpdate> updateList)
    {
        DemoGuiUtils.DrawButton("Add " + name + " Properties Update", () =>
        {
            updateList.Add(new PropertiesUpdate());
        }, true, GSStyles.Button);
        
        for (var i = 0; i < updateList.Count; i++)
        {
            var propertyUpdate = updateList[i];
            GUILayout.BeginHorizontal();
            propertyUpdate.Key = GUILayout.TextField(propertyUpdate.Key, GSStyles.TextField);
            propertyUpdate.Value = GUILayout.TextField(propertyUpdate.Value, GSStyles.TextField);
            DemoGuiUtils.DrawToggle(ref propertyUpdate.Remove, "Delete");
            DemoGuiUtils.DrawButton("Remove Row", () => { updateList.Remove(propertyUpdate); }, true, GSStyles.Button);
            GUILayout.EndHorizontal();
        }
    }
   

    private void DrawAddRemoveProperty()
    {
        DemoGuiUtils.Space();
        GUILayout.BeginVertical("Box");
        GUILayout.Label("Custom Properties", GSStyles.NormalLabelText);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Public property: ", GSStyles.NormalLabelText);
        _key = GUILayout.TextField(_key, GSStyles.TextField);
        _val = GUILayout.TextField(_val, GSStyles.TextField);
        
        DemoGuiUtils.DrawButton("Add", () =>
        {
            if (GetSocial.GetCurrentUser().PublicProperties.ContainsKey(_key))
            {
                _console.LogW("Property already exists " + _key + "=" + GetSocial.GetCurrentUser().PublicProperties[_key] +
                              ", remove old one if you're really want to override it.");
            }
            else
            {
                var update = new UserUpdate().AddPublicProperty(_key, _val);
                GetSocial.GetCurrentUser().UpdateDetails(update,
                    () => _console.LogD(
                        "Property added for key: " + _key + "=" + GetSocial.GetCurrentUser().PublicProperties[_key]),
                    error => _console.LogE("Failed to add property for key: " + _key + ", error: " + error)
                );
            }
        }, true, GSStyles.Button);
        DemoGuiUtils.DrawButton("Remove", () =>
        {
            var update = new UserUpdate().RemovePublicProperty(_key);
            GetSocial.GetCurrentUser().UpdateDetails(update,
                () => _console.LogD("Property removed for key: " + _key),
                error => _console.LogE("Failed to remove property for key: " + _key + ", error: " + error)
            );
        }, true, GSStyles.Button);
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        GUILayout.Label("Private property: ", GSStyles.NormalLabelText);
        _keyPrivate = GUILayout.TextField(_keyPrivate, GSStyles.TextField);
        _valPrivate = GUILayout.TextField(_valPrivate, GSStyles.TextField);

        DemoGuiUtils.DrawButton("Add", () =>
        {
            if (GetSocial.GetCurrentUser().PrivateProperties.ContainsKey(_keyPrivate))
            {
                _console.LogW("Property already exists " + _keyPrivate + "=" +
                              GetSocial.GetCurrentUser().PrivateProperties[_keyPrivate] +
                              ", remove old one if you're really want to override it.");
            }
            else
            {
                var update = new UserUpdate().AddPrivateProperty(_key, _val);
                GetSocial.GetCurrentUser().UpdateDetails(update, 
                    () => _console.LogD("Property added for key: " + _keyPrivate + "=" +
                                        GetSocial.GetCurrentUser().PrivateProperties[_keyPrivate]),
                    error => _console.LogE("Failed to add property for key: " + _keyPrivate + ", error: " + error)
                );
            }
        }, true, GSStyles.Button);
        DemoGuiUtils.DrawButton("Remove", () =>
        {
            var update = new UserUpdate().RemovePrivateProperty(_key);
            GetSocial.GetCurrentUser().UpdateDetails(update,
                () => _console.LogD("Property removed for key: " + _keyPrivate),
                error => _console.LogE("Failed to remove property for key: " + _keyPrivate + ", error: " + error)
            );
        }, true, GSStyles.Button);

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Increment property: ", GSStyles.NormalLabelText);
        _keyIncrement = GUILayout.TextField(_keyIncrement, GSStyles.TextField);
        _valIncrement = GUILayout.TextField(_valIncrement, GSStyles.TextField);

        DemoGuiUtils.DrawButton("Increment", () =>
        {
            var update = new UserUpdate().IncrementPublicProperty(_keyIncrement, double.Parse(_valIncrement));
            GetSocial.GetCurrentUser().UpdateDetails(update,
                () => _console.LogD("Property incremented for key: " + _keyIncrement + "=" +
                                    GetSocial.GetCurrentUser().PublicProperties[_keyIncrement]),
                error => _console.LogE("Failed to increment property for key: " + _keyIncrement + ", error: " + error)
            );
        }, true, GSStyles.Button);
        DemoGuiUtils.DrawButton("Decrement", () =>
        {
            var update = new UserUpdate().DecrementPublicProperty(_keyIncrement, double.Parse(_valIncrement));
            GetSocial.GetCurrentUser().UpdateDetails(update,
                () => _console.LogD("Property decremented for key: " + _keyIncrement + "=" +
                                    GetSocial.GetCurrentUser().PublicProperties[_keyIncrement]),
                error => _console.LogE("Failed to decrement property for key: " + _keyIncrement + ", error: " + error)
            );
        }, true, GSStyles.Button);

        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
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
        GUILayout.Label("User DisplayName: ", GSStyles.NormalLabelText, GUILayout.Width(300f));
        DemoGuiUtils.DrawButton("Change Display Name", () => GetSocial.GetCurrentUser().UpdateDetails(new UserUpdate{DisplayName = GetDisplayNameForUser()},
            () =>
            {
                _console.LogD("User display name has been changed");
                demoController.FetchCurrentUserData();
            }, error => { _console.LogE("Failed to change user DisplayName: " + error); }), true, GSStyles.Button);
        GUILayout.EndHorizontal();

        //user avatar
        GUILayout.BeginHorizontal();
        GUILayout.Label("User Avatar: ", GSStyles.NormalLabelText, GUILayout.Width(100f));
        DemoGuiUtils.DrawButton("Change Avatar URL", () => GetSocial.GetCurrentUser().UpdateDetails(new UserUpdate {AvatarUrl = GetAvatarUrlForUser()}, () =>
        {
            _console.LogD("User avatar url has been changed");
            demoController.FetchCurrentUserData();
        }, error => { _console.LogE("Failed to change user AvatarURL: " + error); }), true, GSStyles.Button);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        DemoGuiUtils.DrawButton("Change Avatar", () => GetSocial.GetCurrentUser().UpdateDetails(new UserUpdate{Avatar = GetAvatarForUser()}, () =>
        {
            _console.LogD("User avatar has been changed");
            demoController.FetchCurrentUserData();
        }, error => { _console.LogE("Failed to change user Avatar: " + error); }), true, GSStyles.Button);
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
        GetSocial.SwitchUser(Identity.Facebook(AccessToken.CurrentAccessToken.TokenString),
            () =>
            {
                _console.LogD("Successfully switched to FB user");
                demoController.FetchCurrentUserData();
                SetFacebookUserDetails();
            },
            error => _console.LogE("Switching to FB user failed, reason: " + error));
    }

    #endregion

    #region fb_add_remove_identity
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

    void AddFacebookUserIdentityInternal()
    {
        _console.LogD(string.Format("Adding FB identity, token: {0}", AccessToken.CurrentAccessToken.TokenString));
        GetSocial.GetCurrentUser().AddIdentity(Identity.Facebook(AccessToken.CurrentAccessToken.TokenString),
            () =>
            {
                _console.LogD("Successfully added Facebook Identity");
                demoController.FetchCurrentUserData();
                SetFacebookUserDetails();
            }, OnAddUserIdentityConflict, 
            error => _console.LogE("Adding Facebook identity failed, reason: " + error)
            );
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
        GetSocial.GetCurrentUser().RemoveIdentity(AuthIdentityProvider.Facebook,
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
        GetSocial.GetCurrentUser().AddIdentity(
            Identity.Custom(CustomProviderId, _customUserId, _customProviderToken),
            () =>
            {
                _console.LogD("Successfully added custom identity");
                demoController.FetchCurrentUserData();
            }, OnAddUserIdentityConflict, 
            error => _console.LogE(string.Format("Failed to add user identity '{0}', reason: {1}", CustomProviderId,
                error))
            );
    }

    void RemoveCustomUserIdentity()
    {
        GetSocial.GetCurrentUser().RemoveIdentity(CustomProviderId,
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
        GetSocial.SwitchUser(
            Identity.Custom(CustomProviderId, _customUserId, _customProviderToken),
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

    private void SetFacebookUserDetails()
    {
        FB.API("me?fields=first_name,last_name,picture", HttpMethod.GET, result =>
        {
            var dictionary = result.ResultDictionary;
            var displayName = dictionary["first_name"] + " " + dictionary["last_name"];
            var pictureDictionary = (IDictionary<string, object>) dictionary["picture"];
            var dataDictionary = (IDictionary<string, object>) pictureDictionary["data"];
            var pictureUrl = dataDictionary["url"].ToString();
            var update = new UserUpdate
            {
                DisplayName = displayName, AvatarUrl = pictureUrl
            };
            GetSocial.GetCurrentUser().UpdateDetails(update, () =>
            {
                _console.LogD("Display name is set to:" + displayName + ", avatar set to " + pictureUrl);
                demoController.FetchCurrentUserData();
            }, error =>
            {
                _console.LogE("Failed to set Facebook details");
            });
        });
    }

    static string GetAvatarUrlForUser()
    {
        var randomStr = RandomString(5);
        var index = randomStr.GetHashCode() % 10;
        return $"http://api.adorable.io/avatars/150/{index}";
    }

    static Texture2D GetAvatarForUser()
    {
        Texture2D t2 = new Texture2D(100, 100);

        for (int x = 0; x < t2.width; x++)
        {
            for (int y = 0; y < t2.height; y++)
            {
                var r = Random.Range(0f, 1f);
                var g = Random.Range(0f, 1f);
                var b = Random.Range(0f, 1f);
                t2.SetPixel(x, y, new Color(r, g, b));
            }
        }

        t2.Apply();

        return t2;
    }

    static string GetDisplayNameForUser()
    {
        var randomStr = RandomString(5);
        int index = randomStr.GetHashCode() % 10;
        return HeroNames[Mathf.Abs(index)] + " Unity " + randomStr;
    }

    UserUpdate GetUpdateForUser()
    {
        var updateBuilder = new UserUpdate {DisplayName = GetDisplayNameForUser()};

        if (_useAvatarUrl)
        {
            updateBuilder.AvatarUrl = GetAvatarUrlForUser();
        }
        else
        {
            updateBuilder.Avatar = GetAvatarForUser();
        }
        
        foreach (var propertyUpdate in _batchUpdatePrivateProperties)
        {
            if (propertyUpdate.Remove)
            {
                updateBuilder.RemovePrivateProperty(propertyUpdate.Key);
            }
            else
            {
                updateBuilder.AddPrivateProperty(propertyUpdate.Key, propertyUpdate.Value);
            }
        }
        
        foreach (var propertyUpdate in _batchUpdatePublicProperties)
        {
            if (propertyUpdate.Remove)
            {
                updateBuilder.RemovePublicProperty(propertyUpdate.Key);
            }
            else
            {
                updateBuilder.AddPublicProperty(propertyUpdate.Key, propertyUpdate.Value);
            }
        }

        return updateBuilder;
    }

    static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new System.Random();
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }
    
    private class PropertiesUpdate
    {
        public string Key = "";
        public string Value = "";
        public bool Remove;
    }
}
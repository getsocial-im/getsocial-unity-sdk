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
using UnityEngine;
using GetSocialSdk.Core;

public class FollowUnfollowSection : DemoMenuSection
{
    private string provider = UserIdentity.ProviderFacebook;
    private string userId = "facebook_user_id";

    public static User LastSelectedUser { set; private get; }

    #region implemented abstract members of DemoMenuSection

    protected override void InitGuiElements()
    {
    }

    protected override string GetTitle()
    {
        return "Follow/Unfollow API";
    }

    protected override void DrawSectionBody()
    {
        DemoGuiUtils.DrawButton("Open User List (Obsolete)", OpenUserList, true, GSStyles.Button);

        GUILayout.BeginHorizontal();
        DemoGuiUtils.DrawButton("Open Following List", OpenFollowingUI, true, GSStyles.Button);
        DemoGuiUtils.DrawButton("Open Followers List", OpenFollowersUI, true, GSStyles.Button);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        DemoGuiUtils.DrawButton("Get First 10 Following", GetFirst10Following, true, GSStyles.Button);
        DemoGuiUtils.DrawButton("Get First 10 Followers", GetFirst10Followers, true, GSStyles.Button);
        GUILayout.EndHorizontal();

        GUILayout.Space(5f);
        if(LastSelectedUser != null)
        {
            var btnTitleFollow = string.Format("Follow {0}", LastSelectedUser.DisplayName);
            DemoGuiUtils.DrawButton(btnTitleFollow, FollowLastSelectedUser, true, GSStyles.Button);
            var btnTitleUnfollow = string.Format("Unfollow {0}", LastSelectedUser.DisplayName);
            DemoGuiUtils.DrawButton(btnTitleUnfollow, UnfollowLastSelectedUser, true, GSStyles.Button);
        }

        DrawFollowUnfollowOnProvider();
    }

    #endregion

    [Obsolete("This method is kept to test obsolete method")]
    private void OpenUserList()
    {
        getSocial.CreateUserListView(
            ProcessUserClick, 
            () => console.LogE("User list closed", false)).SetTitle("Following").Show();
    }

    private void OpenFollowingUI()
    {
        getSocial.CreateUserListView(UserListViewBuilder.UserListType.Following, 
            ProcessUserClick, 
            () => console.LogE("User list closed", false)).SetTitle("Following").Show();
    }

    private void OpenFollowersUI()
    {
        getSocial.CreateUserListView(UserListViewBuilder.UserListType.Followers, 
            ProcessUserClick, 
            () => console.LogE("User list closed", false)).SetTitle("Followers").Show();
    }

    void ProcessUserClick(User user)
    {
        console.LogD("Clicked on " + user);
        LastSelectedUser = user;
        getSocial.CloseView();
    }
    
    private void ToggleFollow(User user, bool isFollowing)
    {
        if(isFollowing)
        {
            getSocial.CurrentUser.UnfollowUser(user, 
                () => console.LogD("Unfollowed " + user.DisplayName), 
                err => console.LogE(err));
        }
        else
        {
            getSocial.CurrentUser.FollowUser(user, 
                () => console.LogD("Followed " + user.DisplayName), 
                err => console.LogE(err));
        }
    }

    private void GetFirst10Following()
    {
        getSocial.CurrentUser.GetFollowing(0, 10,
            users => LogUserList(users, "You are not following any users"), err => console.LogE(err));
    }

    private void GetFirst10Followers()
    {
        getSocial.CurrentUser.GetFollowers(0, 10, 
            users => LogUserList(users, "You have no followers :'("), err => console.LogE(err));
    }

    private void LogUserList(List<User> users, String ifEmptyLogMessage)
    {
        if(users.Count == 0)
        {
            console.LogD(ifEmptyLogMessage);
        }
        else
        {
            users.ForEach(user => console.LogD(user.ToString()));
        }
    }

    private void DrawFollowUnfollowOnProvider()
    {
        DemoGuiUtils.Space();
        GUILayout.BeginVertical("Box");
        GUILayout.Label("Follow/Unfollow user on provider", GSStyles.NormalLabelText);
        DemoGuiUtils.Space();

        DrawUserOnProviderInputs();

        GUILayout.BeginHorizontal();
        GUI.enabled = !AreUserOnProviderFieldsValid();
        DemoGuiUtils.DrawButton("Follow User", FollowUserOnProvider, true, GSStyles.Button);
        DemoGuiUtils.DrawButton("Unfollow User", UnfollowUserOnProvider, true, GSStyles.Button);
        GUI.enabled = true;
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        DemoGuiUtils.Space();
    }

    private void DrawUserOnProviderInputs()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("User Id: ", GSStyles.NormalLabelText, GUILayout.Width(120f));
        userId = GUILayout.TextField(userId, GSStyles.TextField);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Provider: ", GSStyles.NormalLabelText, GUILayout.Width(120f));
        provider = GUILayout.TextField(provider, GSStyles.TextField);
        GUILayout.EndHorizontal();

        if(!AreUserOnProviderFieldsValid())
        {
            GUILayout.Label("User id or provider cannot be empty", GSStyles.NormalLabelTextRed);
        }
    }

    private void FollowUserOnProvider()
    {
        getSocial.CurrentUser.FollowUser(provider, userId,
            () => console.LogD("Successfully followed user"),
            error => console.LogE("Following user failed, reason " + error));
    }

    private void UnfollowUserOnProvider()
    {
        getSocial.CurrentUser.UnfollowUser(provider, userId,
            () => console.LogD("Successfully unfollowed user"),
            error => console.LogE("Unfollowing user failed, reason " + error));

    }

    void FollowLastSelectedUser()
    {
        Check.Argument.IsNotNull(LastSelectedUser, "user");
        getSocial.CurrentUser.FollowUser(LastSelectedUser, 
            () => console.LogD("Followed " + LastSelectedUser.DisplayName), 
            err => console.LogE(err));
    }

    void UnfollowLastSelectedUser()
    {
        Check.Argument.IsNotNull(LastSelectedUser, "user");
        getSocial.CurrentUser.UnfollowUser(LastSelectedUser, 
            () => console.LogD("Unfollowed " + LastSelectedUser.DisplayName), 
            err => console.LogE(err));
    }

    private bool AreUserOnProviderFieldsValid()
    {
        return !string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(provider);
    }
}

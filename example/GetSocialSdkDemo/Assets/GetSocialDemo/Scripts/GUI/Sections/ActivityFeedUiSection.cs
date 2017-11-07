using System;
using System.Collections.Generic;
using System.Linq;
using Facebook.Unity;
using GetSocialSdk.Core;
using TheNextFlow.UnityPlugins;
using UnityEngine;

#if USE_GETSOCIAL_UI
using GetSocialSdk.Ui;

public class ActivityFeedUiSection : DemoMenuSection
{
    string _feed = "default";

    protected override string GetTitle()
    {
        return "Activity Feed UI";
    }

    protected override void DrawSectionBody()
    {
        DemoGuiUtils.DrawButton("Global Feed", OpenGlobalFeed, true, GSStyles.Button);
        DemoGuiUtils.Space();
        
        DemoGuiUtils.DrawButton("My Global Feed", OpenMyGlobalFeed, true, GSStyles.Button);
        DemoGuiUtils.Space();
        
        DemoGuiUtils.DrawButton("My Custom Feed", OpenMyCustomFeed, true, GSStyles.Button);
        DemoGuiUtils.Space();
        
        DemoGuiUtils.DrawButton("My Friends Global Feed", OpenMyFriendsGlobalFeed, true, GSStyles.Button);
        DemoGuiUtils.Space();

        DemoGuiUtils.DrawButton("Open Activity Details", OpenActivityDetailsFunc(true), true, GSStyles.Button);
        DemoGuiUtils.Space();

        DemoGuiUtils.DrawButton("Open Activity Details(without feed)", OpenActivityDetailsFunc(false), true, GSStyles.Button);
        DemoGuiUtils.Space();

        // feed by id
        _feed = GUILayout.TextField(_feed, GSStyles.TextField);
        DemoGuiUtils.DrawButton("Feed By Id", OpenFeedWithId, true, GSStyles.Button);
    }

    private void OpenMyCustomFeed()
    {
        GetSocialUi.CreateActivityFeedView(_feed)
                    .SetWindowTitle("My Custom Feed")
                    .SetButtonActionListener(OnActivityActionClicked)
                    .SetFilterByUser(GetSocial.User.Id)
                    .Show();
    }

    private void OpenMyFriendsGlobalFeed()
    {
        GetSocialUi.CreateGlobalActivityFeedView()
            .SetWindowTitle("My Friends Feed")
            .SetButtonActionListener(OnActivityActionClicked)
            .SetShowFriendsFeed(true)
            .Show();
    }

    private void OpenMyGlobalFeed()
    {
        OpenFiteredGlobalFeedAction("My Global Feed", GetSocial.User.Id);
    }

    private void OpenUserGlobalFeed(PublicUser user)
    {
        OpenFiteredGlobalFeedAction(user.DisplayName + " Global Feed", user.Id);
    }
    
    private void OpenFiteredGlobalFeedAction(string title, string userId)
    {
        GetSocialUi.CreateGlobalActivityFeedView()
            .SetWindowTitle(title)
            .SetButtonActionListener(OnActivityActionClicked)
            .SetFilterByUser(userId)
            .SetReadOnly(true)
            .Show();
    }

    private void OpenFeedWithId()
    {
        GetSocialUi.CreateActivityFeedView(_feed)
            .SetWindowTitle(_feed + " Feed")
            .SetButtonActionListener(OnActivityActionClicked)
            .Show();
    }

    private void OpenGlobalFeed()
    {
        GetSocialUi.CreateGlobalActivityFeedView()
            .SetWindowTitle("Unity Global")
            .SetViewStateCallbacks(() => _console.LogD("Global feed opened"), () => _console.LogD("Global feed closed"))
            .SetButtonActionListener(OnActivityActionClicked)
            .SetMentionClickListener(OnMentionClicked)
            .SetAvatarClickListener(OnUserAvatarClicked)
            .SetUiActionListener(OnUiAction)
            .Show();
    }

    private void OnMentionClicked(string userId)
    {
        GetSocial.GetUserById(userId, OnUserAvatarClicked, error => _console.LogE("Failed to get user details, error:" + error.Message));
    }

    private void OnUserAvatarClicked(PublicUser publicUser)
    {
        if (GetSocial.User.Id.Equals(publicUser.Id))
        {
            MobileNativePopups.OpenAlertDialog("Action", "Choose Action", "Show My Feed", "Cancel", OpenMyGlobalFeed,
                () => { });
        }
        else
        {
            GetSocial.User.IsFriend(publicUser.Id, isFriend =>
            {
                if (isFriend)
                {
                    MobileNativePopups.OpenAlertDialog("Action", "Choose Action", 
                        "Show " + publicUser.DisplayName + " Feed",
                        "Remove from Friends",
                        "Cancel",
                        () => OpenUserGlobalFeed(publicUser),
                        () => RemoveFriend(publicUser),
                        () => { });
                }
                else
                {
                    MobileNativePopups.OpenAlertDialog("Action", "Choose Action", 
                        "Show " + publicUser.DisplayName + " Feed",
                        "Add to Friends",
                        "Cancel",
                        () => OpenUserGlobalFeed(publicUser),
                        () => AddFriend(publicUser),
                        () => { });
                }
            }, error => _console.LogE("Failed to check if friends with " + publicUser.DisplayName + ", error:" + error.Message));
        }
    }

    private void AddFriend(PublicUser user)
    {
        GetSocial.User.AddFriend(user.Id,
            friendsCount =>
            {
                string message = user.DisplayName + " is now your friend."; 
                _console.LogD(message);
                MobileNativePopups.OpenAlertDialog("Info", message, "OK", () => { });
            },
            error => _console.LogE("Failed to add a friend " + user.DisplayName + ", error:" + error.Message));
    }

    private void RemoveFriend(PublicUser user)
    {
        GetSocial.User.RemoveFriend(user.Id,
            friendsCount =>
            {
                string message = user.DisplayName + " is not your friend anymore."; 
                _console.LogD(message);
                MobileNativePopups.OpenAlertDialog("Info", message, "OK",  () => { });
            },
            error => _console.LogE("Failed to remove a friend " + user.DisplayName + ", error:" + error.Message));
    }

    private Action OpenActivityDetailsFunc(bool showFeed)
    {
        return () =>
        {
            GetSocial.GetActivities(ActivitiesQuery.PostsForGlobalFeed().WithLimit(1), (posts) =>
            {
                if (posts.Count == 0)
                {
                    _console.LogW("No activities, post something to global feed!");
                    return;
                }
                GetSocialUi.CreateActivityDetailsView(posts.First().Id)
                    .SetWindowTitle("Unity Global")
                    .SetViewStateCallbacks(() => _console.LogD("Activity details opened"),
                        () => _console.LogD("Activity details closed"))
                    .SetButtonActionListener(OnActivityActionClicked)
                    .SetShowActivityFeedView(showFeed)
                    .SetUiActionListener((action, pendingAction) =>
                    {
                        Debug.Log("Action invoked: " + action);
                        pendingAction();
                    })
                    .Show();
            }, (error) => _console.LogE("Failed to get activities, error: " + error.Message));
        };
    }

    private void OnUiAction(UiAction action, Action pendingAction)
    {
        List<UiAction> forbiddenForAnonymous = new List<UiAction>()
        {
            UiAction.LikeActivity, UiAction.LikeComment, UiAction.PostActivity, UiAction.PostComment
        };
        if (forbiddenForAnonymous.Contains(action) && GetSocial.User.IsAnonymous)
        {
            string message = "Action " + action + " is not allowed for anonymous.";
#if UNITY_ANDROID
            MainThreadExecutor.Queue(() => MobileNativePopups.OpenAlertDialog("Info", message, "OK", () => { }));
#else
            MobileNativePopups.OpenAlertDialog("Info", message, "OK", () => { });
#endif
            _console.LogD(message);
        }
        else
        {
            pendingAction();
        }
    }

    void OnActivityActionClicked(string actionId, ActivityPost post)
    {
        string message = string.Format("Activity feed button clicked, action type: {0}", actionId); 
        MobileNativePopups.OpenAlertDialog("Info", message, "OK", () => { });
        _console.LogD(message);
    }
}

#endif
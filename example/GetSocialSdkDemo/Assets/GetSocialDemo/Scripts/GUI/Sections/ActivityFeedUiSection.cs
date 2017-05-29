using System;
using System.Collections.Generic;
using System.Linq;
using Facebook.Unity;
using GetSocialSdk.Core;
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

        DemoGuiUtils.DrawButton("Open Activity Details", OpenActivityDetailsFunc(true), true, GSStyles.Button);
        DemoGuiUtils.Space();

        DemoGuiUtils.DrawButton("Open Activity Details(without feed)", OpenActivityDetailsFunc(false), true, GSStyles.Button);
        DemoGuiUtils.Space();

        // feed by id
        _feed = GUILayout.TextField(_feed, GSStyles.TextField);
        DemoGuiUtils.DrawButton("Feed By Id", OpenFeedWithId, true, GSStyles.Button);
    }

    void OpenFeedWithId()
    {
        GetSocialUi.СreateActivityFeedView(_feed)
            .SetWindowTitle(_feed + " Title")
            .SetButtonActionListener(OnActivityActionClicked)
            .Show();
    }

    void OpenGlobalFeed()
    {
        GetSocialUi.СreateGlobalActivityFeedView()
            .SetWindowTitle("Unity Global")
            .SetViewStateCallbacks(() => _console.LogD("Global feed opened"), () => _console.LogD("Global feed closed"))
            .SetButtonActionListener(OnActivityActionClicked)
            .SetAvatarClickListener(OnUserAvatarClicked)
            .SetUiActionListener(OnUiAction)
            .Show();
    }

    void OnUserAvatarClicked(PublicUser publicUser)
    {
        if (GetSocial.User.Id.Equals(publicUser.Id))
        {
            _console.LogD("Tapped on yourself", false);
        }
        else
        {
            GetSocial.User.IsFriend(publicUser.Id, isFriend =>
            {
                if (isFriend)
                {
                    removeFriend(publicUser);
                }
                else
                {
                    addFriend(publicUser);
                }
            }, error => _console.LogE("Failed to check if friends with "+ publicUser.DisplayName + ", error:" + error.Message));
        }
    }

    void addFriend(PublicUser user)
    {
        GetSocial.User.AddFriend(user.Id,
            friendsCount => _console.LogD(user.DisplayName + " is now your friend."),
            error => _console.LogE("Failed to add a friend " + user.DisplayName + ", error:" + error.Message));
    }

    void removeFriend(PublicUser user)
    {
        GetSocial.User.RemoveFriend(user.Id,
            friendsCount => _console.LogD(user.DisplayName + " is not your friend anymore."),
            error => _console.LogE("Failed to remove a friend " + user.DisplayName + ", error:" + error.Message));
    }

    Action OpenActivityDetailsFunc(bool showFeed)
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
                GetSocialUi.СreateActivityDetailsView(posts.First().Id)
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
            _console.LogD("Action " + action + " is not allowed for anonymous.");
            GetSocialUi.CloseView();
        }
        else
        {
            pendingAction();
        }
    }

    void OnActivityActionClicked(string actionId, ActivityPost post)
    {
        _console.LogD(string.Format("[{0}] button clicked on post: {1}", actionId, post));
    }
}

#endif
using System;
using System.Collections.Generic;
using GetSocialSdk.Core;

using GetSocialSdk.Ui;

public class ActivityFeedUiSection : DemoMenuSection
{
    protected override string GetTitle()
    {
        return "Activity Feed UI";
    }

    protected override void DrawSectionBody()
    {
        DemoGuiUtils.DrawButton("Timeline", OpenGlobalFeed, true, GSStyles.Button);
        DemoGuiUtils.Space();
        
        DemoGuiUtils.DrawButton("My Feed", OpenMyCustomFeed, true, GSStyles.Button);
        DemoGuiUtils.Space();
    }

    private void OpenMyCustomFeed()
    {
#pragma warning disable 0618
        ActivityFeedViewBuilder.Create(ActivitiesQuery.FeedOf(UserId.CurrentUser()))
                    .SetWindowTitle("My Custom Feed")
                    .SetActionListener(OnAction)
                    .Show();
#pragma warning restore 0618
        
    }


    private void OpenUserGlobalFeed(User user)
    {
        ActivityFeedViewBuilder.Create(ActivitiesQuery.FeedOf(UserId.Create(user.Id)))
            .SetWindowTitle("Feed of " + user.DisplayName)
            .SetViewStateCallbacks(() => _console.LogD("Feed opened"), () => _console.LogD("Feed closed"))
            .SetActionListener(OnAction)
            .SetMentionClickListener(OnMentionClicked)
            .SetAvatarClickListener(OnUserAvatarClicked)
            .SetTagClickListener(OnTagClicked)
            .SetUiActionListener(OnUiAction)
            .Show();
    }

    private void OpenGlobalFeed()
    {
#pragma warning disable 0618
        ActivityFeedViewBuilder.Create(ActivitiesQuery.Timeline())
            .SetWindowTitle("My Unity Timeline")
            .SetViewStateCallbacks(() => _console.LogD("Timeline opened"), () => _console.LogD("Timeline closed"))
            .SetActionListener(OnAction)
            .SetMentionClickListener(OnMentionClicked)
            .SetAvatarClickListener(OnUserAvatarClicked)
            .SetTagClickListener(OnTagClicked)
            .SetUiActionListener(OnUiAction)
            .Show();
#pragma warning restore 0618
    }

    private void OnMentionClicked(string mention)
    {
        if (mention.Equals(MentionTypes.App))
        {
            _console.LogD("Application mention clicked.");
            return;
        }
        Communities.GetUser(UserId.Create(mention), OnUserAvatarClicked, error => _console.LogE("Failed to get user details, error:" + error.Message));
    }

    private void OnTagClicked(string tag) 
    {
        GetSocialUi.CloseView();
        demoController.PushMenuSection<ActivitiesSection>(section =>
        {
            section.Query = ActivitiesQuery.Everywhere().WithTag(tag);
        });
    }

    private void OnUserAvatarClicked(User publicUser)
    {
        if (GetSocial.GetCurrentUser().Id.Equals(publicUser.Id))
        {
            var popup = new MNPopup ("Action", "Choose Action");
            popup.AddAction("Show My Feed", () => OpenUserGlobalFeed(publicUser));
            popup.AddAction("Cancel", () => { });
            popup.Show();
        }
        else
        {
            Communities.IsFriend(UserId.Create(publicUser.Id), isFriend =>
            {
                if (isFriend)
                {
                    var popup = new MNPopup ("Action", "Choose Action");
                    popup.AddAction("Show " + publicUser.DisplayName + " Feed", () => OpenUserGlobalFeed(publicUser));
                    popup.AddAction("Remove from Friends", () => RemoveFriend(publicUser));
                    popup.AddAction("Cancel", () => { });
                    popup.Show();
                }
                else
                {
                    var popup = new MNPopup ("Action", "Choose Action");
                    popup.AddAction("Show " + publicUser.DisplayName + " Feed", () => OpenUserGlobalFeed(publicUser));
                    popup.AddAction("Add to Friends", () => AddFriend(publicUser));
                    popup.AddAction("Cancel", () => { });
                    popup.Show();
                }
            }, error => _console.LogE("Failed to check if friends with " + publicUser.DisplayName + ", error:" + error.Message));
        }
    }

    private void AddFriend(User user)
    {
        Communities.AddFriends(UserIdList.Create(user.Id), 
            friendsCount =>
            {
                var message = user.DisplayName + " is now your friend."; 
                _console.LogD(message);
            },
            error => _console.LogE("Failed to add a friend " + user.DisplayName + ", error:" + error.Message));
    }

    private void RemoveFriend(User user)
    {
        Communities.RemoveFriends(UserIdList.Create(user.Id), 
            friendsCount =>
            {
                var message = user.DisplayName + " is not your friend anymore."; 
                _console.LogD(message);
            },
            error => _console.LogE("Failed to remove a friend " + user.DisplayName + ", error:" + error.Message));
    }

    private void OnUiAction(UiAction action, Action pendingAction)
    {
        var forbiddenForAnonymous = new List<UiAction>()
        {
            UiAction.LikeActivity, UiAction.LikeComment, UiAction.PostActivity, UiAction.PostComment
        };
        if (forbiddenForAnonymous.Contains(action) && GetSocial.GetCurrentUser().IsAnonymous)
        {
            var message = "Action " + action + " is not allowed for anonymous.";
            _console.LogD(message);
        }
        else
        {
            pendingAction();
        }
    }

    void OnAction(GetSocialAction action)
    {
        demoController.HandleAction(action);
    }
}
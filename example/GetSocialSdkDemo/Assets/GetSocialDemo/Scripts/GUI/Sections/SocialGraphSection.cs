using Assets.GetSocialDemo.Scripts.Utils;
using UnityEngine;

using GetSocialSdk.Core;

public class SocialGraphSection : DemoMenuSection
{
    string _userId = "123";

    protected override string GetTitle()
    {
        return "Social Graph API";
    }

    protected override void DrawSectionBody()
    {
        GUILayout.Label("User ID: ", GSStyles.NormalLabelText);
        _userId = GUILayout.TextField(_userId, GSStyles.TextField);

        DemoGuiUtils.Space();

        DemoGuiUtils.DrawButton("Add Friend", AddFriend, true, GSStyles.Button);
        DemoGuiUtils.DrawButton("Remove Friend", RemoveFriend, true, GSStyles.Button);
        DemoGuiUtils.DrawButton("Check If Friends", CheckIfFriends, true,
            GSStyles.Button);
        DemoGuiUtils.DrawButton("Get Friends Count", GetFriendsCount, true, GSStyles.Button);
        DemoGuiUtils.DrawButton("Get Friends", GetFriends, true, GSStyles.Button);
        DemoGuiUtils.DrawButton("Get Suggested Friends", GetSuggestedFriends, true, GSStyles.Button);
    }

    void GetSuggestedFriends()
    {
        GetSocial.User.GetSuggestedFriends(0, 10, suggestedFriends =>
        {   
            _console.LogD(string.Format ("Suggested friends are: {0}", suggestedFriends.ToPrettyString()));
        }, OnError);
    }

    void AddFriend()
    {
        GetSocial.User.AddFriend (_userId, friendsCount =>
            {
                _console.LogD (string.Format ("Friend is added, friends cound {0}", friendsCount));
            }, OnError);
    }

    void RemoveFriend()
    {
        GetSocial.User.RemoveFriend (_userId, friendsCount =>
            {
                _console.LogD (string.Format ("Friend is removed, friends cound {0}", friendsCount));
            }, OnError);
    }

    void CheckIfFriends()
    {
        GetSocial.User.IsFriend (_userId, (isFriend) =>
            {
                _console.LogD (string.Format ("Checked if friends: [{0}]", isFriend));
            }, OnError);
    }

    void GetFriends()
    {
        GetSocial.User.GetFriends (0, 1000, (friends) =>
        {
            _console.LogD (string.Format ("You friends are: [\n{0}\n]", friends.ToPrettyString()));
        }, OnError);
    }

    void GetFriendsCount()
    {
        GetSocial.User.GetFriendsCount((friendsCount) =>
        {
            _console.LogD(string.Format("You have {0} friends", friendsCount));
        }, OnError);
    }

    void OnError(GetSocialError error)
    {
        _console.LogE("Error: " + error.Message);
    }
}

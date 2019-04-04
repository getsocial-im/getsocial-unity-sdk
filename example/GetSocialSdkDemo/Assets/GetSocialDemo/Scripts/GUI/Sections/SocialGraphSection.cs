using System.Collections.Generic;
using Assets.GetSocialDemo.Scripts.Utils;
using UnityEngine;

using GetSocialSdk.Core;
using UnityEditor;

public class SocialGraphSection : DemoMenuSection
{
    string _userId = "123";
    private MessageView _messageView;
    private List<PublicUser> _existingFriends;

    protected override void InitGuiElements()
    {
        _messageView = GetComponentInChildren<MessageView>();
        _messageView.enabled = false;
    }

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
        DemoGuiUtils.DrawButton("Message Friend", MessageFriend, true, GSStyles.Button);
        DemoGuiUtils.DrawButton("Get Friends Count", GetFriendsCount, true, GSStyles.Button);
        DemoGuiUtils.DrawButton("Get Friends", GetFriends, true, GSStyles.Button);
        DemoGuiUtils.DrawButton("Get Suggested Friends", GetSuggestedFriends, true, GSStyles.Button);
        DemoGuiUtils.DrawButton("Load Friends for Chat", LoadFriendsForChat, true, GSStyles.Button);
        DemoGuiUtils.Space();
        // show existing friends
        if (_existingFriends != null)
        {
            foreach (var friend in _existingFriends)
            {
                var friendId = friend.Id;
                DemoGuiUtils.DrawButton(friend.DisplayName, () =>
                {
                    MessageFriend(friendId);
                }, true, GSStyles.Button);
            }
        }
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

    void MessageFriend()
    {
        GetSocial.GetUserById(_userId, user =>
        {
            // show message view
            enabled = false;
            _messageView.Receiver = user;
            _messageView.enabled = true;
            _messageView.LoadMessages();
        }, error =>
        {
            _console.LogE("Could not get user: " + error);
        });
    }

    public void MessageFriend(string friendId)
    {
        _userId = friendId;
        MessageFriend();
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

    void LoadFriendsForChat()
    {
        
        GetSocial.User.GetFriends(0, 1000, friends => { _existingFriends = friends; }, OnError);
    }
    
}

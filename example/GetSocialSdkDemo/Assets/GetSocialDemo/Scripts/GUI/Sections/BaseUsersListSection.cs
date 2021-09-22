using System.Collections.Generic;
using GetSocialSdk.Core;
using UnityEngine;

public abstract class BaseUsersListSection<Q> : BaseListSection<Q, User>
{
    private readonly Dictionary<string, bool> _areFriends = new Dictionary<string, bool>();
    private readonly Dictionary<string, bool> _follows = new Dictionary<string, bool>();


    protected override void OnDataChanged(List<User> list)
    {
        if (list.Count == 0)
        {
            return;
        }
        var users = UserIdList.Create(list.ConvertAll(it => it.Id));
        Communities.AreFriends(users, areFriends =>
        {
            _areFriends.AddAll(areFriends);
        }, error => _console.LogE(error.Message));
        Communities.IsFollowing(UserId.CurrentUser(), FollowQuery.Users(users), follows =>
        {
            _follows.AddAll(follows);
        }, error => _console.LogE(error.Message));
    }

    protected override void DrawItem(User item)
    {
        GUILayout.Label(item.DisplayName, GSStyles.BigLabelText);
        GUILayout.Label(item.Id, GSStyles.NormalLabelText);
        GUILayout.Label(item.AvatarUrl, GSStyles.NormalLabelText);
        DemoGuiUtils.DrawButton("Actions", () =>
        {
            ShowActions(item);
        }, style: GSStyles.Button);
    }

    private new void ShowActions(User user)
    {
        var popup = Dialog().WithTitle("Actions");
        // Follow and friend functions are not available if `user` is current user
        if (!user.Id.Equals(GetSocial.GetCurrentUser().Id))
        {
            if (_follows.ContainsKey(user.Id) && _follows[user.Id])
            {
                popup.AddAction("Unfollow", () => Unfollow(user));
            }
            else
            {
                popup.AddAction("Follow", () => Follow(user));
            }
            if (_areFriends.ContainsKey(user.Id) && _areFriends[user.Id])
            {
                popup.AddAction("Remove Friend", () => RemoveFriend(user));
            }
            else
            {
                popup.AddAction("Add Friend", () => AddFriend(user));
            }
        }
        popup.AddAction("Info", () => Print(user));
        popup.AddAction("Followers", () => Followers(user));
        popup.AddAction("Friends", () => Friends(user));
        popup.AddAction("Feed", () => Feed(user));
        popup.AddAction("All posts by", () => AllPosts(user));
        popup.AddAction("Followed Topics", () => Topics(user));
        popup.AddAction("Open Chat", () => Chat(user));
        popup.AddAction("Cancel", () => { });
        popup.Show();   
    }

    private void Chat(User user)
    {
        demoController.PushMenuSection<ChatMessagesView>(section =>
        {
            var chatId = ChatId.Create(UserId.Create(user.Id));
            section.SetChatId(chatId);
        });
    }

    private void Feed(User user) 
    {
        demoController.PushMenuSection<ActivitiesSection>(section =>
        {
            section.ShowFilter = false;
            section.Query = ActivitiesQuery.FeedOf(UserId.Create(user.Id));
        });
    }

    private void AllPosts(User user) 
    {
        demoController.PushMenuSection<ActivitiesSection>(section =>
        {
            section.Query = ActivitiesQuery.Everywhere().ByUser(UserId.Create(user.Id));
            section.ShowFilter = false;
        });
    }
    private void AddFriend(User user)
    {
        Communities.AddFriends(UserIdList.Create(user.Id), count =>
        {
            _areFriends[user.Id] = true;
            _follows[user.Id] = true;
        }, error => _console.LogE(error.Message));
    }
    
    private void RemoveFriend(User user)
    {
        Communities.RemoveFriends(UserIdList.Create(user.Id), count =>
        {
            _areFriends[user.Id] = false;
            _follows[user.Id] = false;
        }, error => _console.LogE(error.Message));
    }

    private void Follow(User user)
    {
        Communities.Follow(FollowQuery.Users(UserIdList.Create(user.Id)), count =>
        {
            _follows[user.Id] = true;
        }, error => _console.LogE(error.Message));
    }
    
    private void Unfollow(User user)
    {
        Communities.Unfollow(FollowQuery.Users(UserIdList.Create(user.Id)), count =>
        {
            _follows[user.Id] = false;
        }, error => _console.LogE(error.Message));
    }
}

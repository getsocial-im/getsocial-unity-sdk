using System.Collections.Generic;
using GetSocialSdk.Core;
using UnityEngine;

public class UserByIdSection : DemoMenuSection
{
    private readonly List<Data> _userIds = new List<Data>();
    private string _provider;
    protected override string GetTitle()
    {
        return "Users by ID";
    }

    protected override void DrawSectionBody()
    {
        DemoGuiUtils.DrawButton("Actions", ShowActions, style:GSStyles.Button);
        _provider = GUILayout.TextField(_provider, GSStyles.TextField);

        DemoGuiUtils.DynamicRowFor(_userIds, "Users IDs");
    }

    private void ShowActions()
    {
        var popup = Dialog().WithTitle("Actions");
        popup.AddAction("Add Friends", AddFriends);
        popup.AddAction("Remove Friends", RemoveFriends);
        popup.AddAction("Are Friends", AreFriends);
        popup.AddAction("Set Friends", SetFriends);
        popup.AddAction("Send Notification", SendNotification);
        popup.AddAction("Get Users", GetUsers);
        popup.AddAction("Is Following", IsFollowing);
        popup.AddAction("Follow", Follow);
        popup.AddAction("UnFollow", Unfollow);
        popup.AddAction("Cancel", () => { });
        popup.Show();   
    }

    private void OnError(GetSocialError error)
    {
        _console.LogE(error.ToString());
    }

    private UserIdList FormUserList()
    {
        var ids = _userIds.ConvertAll(it => it.UserId);
        return _provider.Length > 0 ? UserIdList.CreateWithProvider(_provider, ids) : UserIdList.Create(ids);
    }

    private void Unfollow()
    {
        Communities.Unfollow(FollowQuery.Users(FormUserList()), newFollows =>
        {
            _console.LogD("Unfollowed user, total: " + newFollows);
        }, OnError);
    }

    private void Follow()
    {
        Communities.Follow(FollowQuery.Users(FormUserList()), newFollows =>
        {
            _console.LogD("Followed user, total: " + newFollows);
        }, OnError);
    }

    private void IsFollowing()
    {
        Communities.IsFollowing(UserId.CurrentUser(), FollowQuery.Users(FormUserList()), follows =>
        {
            _console.LogD("Current user is following: " + follows.ToDebugString());
        }, OnError);
    }

    private void GetUsers()
    {
        Communities.GetUsers(FormUserList(), users =>
        {
            _console.LogD("Users: " + users.ToDebugString());
        }, OnError);
    }

    private void SendNotification()
    {
        Notifications.Send(NotificationContent.CreateWithText("Hey, just testing a feature!"), SendNotificationTarget.Users(FormUserList()), () =>
        {
            _console.LogD("Notifications sent!");
        }, OnError);
    }

    private void SetFriends()
    {
        Communities.SetFriends(FormUserList(), total =>
        {
            _console.LogD("New friends total:" + total);
        }, OnError);
    }

    private void AreFriends()
    {
        Communities.AreFriends(FormUserList(), result =>
        {
            _console.LogD("Are friends: " + result.ToDebugString());
        }, OnError);
    }

    private void AddFriends()
    {
        Communities.AddFriends(FormUserList(), total =>
        {
            _console.LogD("New friends total:" + total);
        }, OnError);
    }
    private void RemoveFriends()
    {
        Communities.RemoveFriends(FormUserList(), total =>
        {
            _console.LogD("New friends total:" + total);
        }, OnError);
    }

    private class Data: DemoGuiUtils.IDrawableRow
    {
        public string UserId = "";
        
        public void Draw()
        {
            UserId = GUILayout.TextField(UserId, GSStyles.TextField);
        }
    }
}

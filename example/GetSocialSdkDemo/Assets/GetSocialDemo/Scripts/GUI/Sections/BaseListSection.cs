using System;
using System.Collections.Generic;
using GetSocialSdk.Core;
using UnityEngine;

public abstract class BaseListSection<Q, I> : DemoMenuSection
{

    private string _query = "";
    protected readonly List<I> _items = new List<I>();
    private bool _loadMore;
    private string _next = "";
    protected int _count = -1;

    protected override string GetTitle()
    {
        var title = GetSectionName();
        return _count == -1 ? title : title + "(" + _count + ")";
    }

    protected abstract string GetSectionName();

    protected override void InitGuiElements()
    {
        Reload();
    }

    protected void Reload()
    {
        _items.Clear();
        _query = null;
        _next = null;
        LoadMore();
        Count(CreateQuery(_query), count => _count = count, error => _console.LogE("Failed to load count: " + error.Message));
    }

    protected void Search()
    {
        _items.Clear();
        _next = null;
        LoadMore();
        Count(CreateQuery(_query), count => _count = count, error => _console.LogE("Failed to load count: " + error.Message));
    }

    protected virtual void DrawHeader()
    {
    }

    protected override void DrawSectionBody()
    {
        GUILayout.BeginVertical();
        DrawHeader();
        if (HasQuery())
        {
            GUILayout.BeginHorizontal();
            _query = GUILayout.TextField(_query, GSStyles.TextField);
            DemoGuiUtils.DrawButton("Search", Search, style: GSStyles.ShortButton);
            GUILayout.EndHorizontal();
            GUILayout.Space(20);
        }
        foreach (var item in _items)
        {
            DrawItem(item);
        }

        if (_loadMore)
        {
            DemoGuiUtils.DrawButton("Load more", LoadMore, style: GSStyles.Button);
        }
        GUILayout.EndVertical();
    }

    protected virtual bool HasQuery()
    {
        return true;
    }
    
    private void LoadMore()
    {
        _loadMore = false;
        var query = CreateQuery(_query);
        var paging = Paging(query, _next, 20);
        
        Load(paging, result =>
        {
            _items.AddAll(result.Entries);
            _loadMore = !result.IsLastPage;
            _next = result.NextCursor;
            OnDataChanged(_items);
        }, error => _console.LogE(error.Message));
    }

    protected virtual PagingQuery<Q> Paging(Q query, string next, int limit)
    {
        return new PagingQuery<Q>(query).WithLimit(limit).Next(next);
    }

    protected virtual void OnDataChanged(List<I> list)
    {
        
    }

    protected abstract void Load(PagingQuery<Q> query, Action<PagingResult<I>> success, Action<GetSocialError> error);
    protected abstract void Count(Q query, Action<int> success, Action<GetSocialError> error);

    protected abstract void DrawItem(I item);

    protected abstract Q CreateQuery(string query);

    protected void ShowActions(User user)
    {
        Communities.IsFollowing(UserId.CurrentUser(), FollowQuery.Users(UserIdList.Create(user.Id)), result =>
        {
            var isFollower = result.ContainsKey(user.Id) && result[user.Id];
            Communities.IsFriend(UserId.Create(user.Id), isFriend =>
            {
                ShowActions(user, isFollower, isFriend);
            }, error => _console.LogE(error.ToString()));
        }, error => _console.LogE(error.ToString()));
    }

    protected void ShowActions(User user, bool isFollower, bool isFriend)
    {
        var popup = Dialog().WithTitle("Actions");
        if (isFollower)
        {
            popup.AddAction("Unfollow", () => Unfollow(user));
        }
        else
        {
            popup.AddAction("Follow", () => Follow(user));
        }
        if (isFriend)
        {
            popup.AddAction("Remove Friend", () => RemoveFriend(user));
        }
        else
        {
            popup.AddAction("Add Friend", () => AddFriend(user));
        }
        popup.AddAction("Info", () => Print(user));
        popup.AddAction("Followers", () => Followers(user));
        popup.AddAction("Friends", () => Friends(user));
        popup.AddAction("Followed Topics", () => Topics(user));
        popup.AddAction("Following", () => Following(user));
        popup.AddAction("Cancel", () => { });
        popup.Show();   
    }

    protected void Topics(User user)
    {
        demoController.PushMenuSection<FollowedTopicsSection>(section => section.User = UserId.Create(user.Id));
    }
    
    protected void Following(User user)
    {
        demoController.PushMenuSection<FollowingSection>(section =>
        {
            section.User = UserId.Create(user.Id);
            section.Name = user.DisplayName;
        });
    }

    protected void Followers(User user)
    {
        demoController.PushMenuSection<FollowersSection>(section =>
        {
            section.Query = FollowersQuery.OfUser(UserId.Create(user.Id));
            section.Name = user.DisplayName;
        });
    }

    protected void Friends(User user)
    {
        demoController.PushMenuSection<FriendsSection>(section => section.User = UserId.Create(user.Id));
    }

    protected void Print(User user)
    {
        _console.LogD(user.ToString());
    }

    private void AddFriend(User user)
    {
        Communities.AddFriends(UserIdList.Create(user.Id), count =>
        {
            _console.LogD($"Friend Added: {count}", false);
        }, error => _console.LogE(error.Message));
    }
    
    private void RemoveFriend(User user)
    {
        Communities.RemoveFriends(UserIdList.Create(user.Id), count =>
        {
            _console.LogD($"Friend Removed: {count}", false);
        }, error => _console.LogE(error.Message));
    }

    private void Follow(User user)
    {
        Communities.Follow(FollowQuery.Users(UserIdList.Create(user.Id)), count => {
            _console.LogD($"Followed User: {count}", false);
        }, error => _console.LogE(error.Message));
    }
    
    private void Unfollow(User user)
    {
        Communities.Unfollow(FollowQuery.Users(UserIdList.Create(user.Id)), count => {
            _console.LogD($"Unfollowed User: {count}", false);
        }, error => _console.LogE(error.Message));
    }
}

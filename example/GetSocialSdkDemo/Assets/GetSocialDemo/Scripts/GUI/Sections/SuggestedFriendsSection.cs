using System;
using GetSocialSdk.Core;
using UnityEngine;

public class SuggestedFriendsSection : BaseListSection<object, SuggestedFriend>
{
    protected override string GetSectionName()
    {
        return "Suggested friends";
    }

    protected override void DrawItem(SuggestedFriend item)
    {
        GUILayout.Label(item.DisplayName + $"({item.MutualFriendsCount} mutual friends)", GSStyles.BigLabelText);
        GUILayout.Label(item.Id, GSStyles.NormalLabelText);
        GUILayout.Label(item.AvatarUrl, GSStyles.NormalLabelText);
        
        DemoGuiUtils.DrawButton("Actions", () =>
        {
            ShowActions(item);
        }, style: GSStyles.Button);
    }

    protected override void Load(PagingQuery<object> query, Action<PagingResult<SuggestedFriend>> success, Action<GetSocialError> error)
    {
        Communities.GetSuggestedFriends((SimplePagingQuery) query, success, error);
    }

    protected override void Count(object query, Action<int> success, Action<GetSocialError> error)
    {
        // not supported
    }

    protected override object CreateQuery(string query)
    {
        return null;
    }

    protected override PagingQuery<object> Paging(object query, string next, int limit)
    {
        return SimplePagingQuery.Simple(limit).Next(next);
    }

    protected override bool HasQuery()
    {
        return false;
    }
}


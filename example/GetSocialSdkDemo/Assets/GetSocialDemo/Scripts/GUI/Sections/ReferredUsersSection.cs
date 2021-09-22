using System;
using GetSocialSdk.Core;
using UnityEngine;

public class ReferredUsersSection : BaseListSection<ReferralUsersQuery, ReferralUser>
{
    protected override string GetSectionName()
    {
        return "Referred Users";
    }

    protected override void DrawItem(ReferralUser item)
    {
        GUILayout.Label(item.DisplayName, GSStyles.BigLabelText);
        GUILayout.Label(item.Id, GSStyles.NormalLabelText);
        GUILayout.Label(item.AvatarUrl, GSStyles.NormalLabelText);
        
        DemoGuiUtils.DrawButton("Actions", () =>
        {
            ShowActions(item);
        }, style: GSStyles.Button);
    }

    protected override void Load(PagingQuery<ReferralUsersQuery> query, Action<PagingResult<ReferralUser>> success, Action<GetSocialError> error)
    {
        Invites.GetReferredUsers(query, success, error);
    }

    protected override void Count(ReferralUsersQuery query, Action<int> success, Action<GetSocialError> error)
    {
        // not supported
    }

    protected override ReferralUsersQuery CreateQuery(string query)
    {
        return query == null || query.Length == 0 ? ReferralUsersQuery.AllUsers() : ReferralUsersQuery.UsersForEvent(query);
    }
}


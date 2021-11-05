using System;
using GetSocialSdk.Core;
using UnityEngine;

public class ReferrerUsersSection : BaseListSection<ReferralUsersQuery, ReferralUser>
{
    protected override string GetSectionName()
    {
        return "Referrer Users";
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
        Invites.GetReferrerUsers(query, success, error);
    }

    protected override void Count(ReferralUsersQuery query, Action<int> success, Action<GetSocialError> error)
    {
        // not supported
    }

    protected override ReferralUsersQuery CreateQuery(QueryObject queryObject)
    {
        return queryObject.SearchTerm == null || queryObject.SearchTerm.Length == 0 ? ReferralUsersQuery.AllUsers() : ReferralUsersQuery.UsersForEvent(queryObject.SearchTerm);
    }
}


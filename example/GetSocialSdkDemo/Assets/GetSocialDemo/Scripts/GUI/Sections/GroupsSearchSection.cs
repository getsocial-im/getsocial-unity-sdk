using System.Collections.Generic;
using GetSocialSdk.Core;
using UnityEngine;

public class GroupsSearchSection : BaseGroupsSection
{

    private bool _isTrending = false;

    protected override void DrawHeader()
    {
        GUILayout.BeginHorizontal();
        DemoGuiUtils.DrawButton(_isTrending ? "All" : "Only Trending", () =>
        {
            _isTrending = !_isTrending;
            Reload();
        }, style: GSStyles.Button);
        DemoGuiUtils.Space();
        GUILayout.EndHorizontal();
    }

    protected override GroupsQuery CreateQuery(string searchTerm)
    {
        var query = GroupsQuery.Find(searchTerm);
        query = query.OnlyTrending(_isTrending);
        return query;
    }
}

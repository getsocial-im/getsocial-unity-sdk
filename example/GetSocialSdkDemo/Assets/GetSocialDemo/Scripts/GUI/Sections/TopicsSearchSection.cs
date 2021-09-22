using System.Collections.Generic;
using GetSocialSdk.Core;
using UnityEngine;

public class TopicsSearchSection : BaseTopicsSection
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

    protected override TopicsQuery CreateQuery(string searchTerm)
    {
        var query = TopicsQuery.Find(searchTerm);
        query = query.OnlyTrending(_isTrending);
        return query;
    }
}

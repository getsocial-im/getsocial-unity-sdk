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

    protected override bool HasQuery()
    {
        return false;
    }

    protected override bool HasAdvancedQuery()
    {
        return true;
    }

    protected override TopicsQuery CreateQuery(QueryObject queryObject)
    {
        var query = TopicsQuery.Find(queryObject.SearchTerm);
        query.WithLabels(queryObject.SearchLabels);
        query.WithProperties(queryObject.SearchProperties);
        query = query.OnlyTrending(_isTrending);
        return query;
    }
}

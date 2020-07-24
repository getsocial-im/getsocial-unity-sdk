using System;
using GetSocialSdk.Core;
using UnityEngine;

public class TagsSection : BaseListSection<TagsQuery, string>
{
    protected override string GetSectionName()
    {
        return "Tags";
    }

    protected override void DrawItem(string item)
    {
        GUILayout.Label(item, GSStyles.NormalLabelText);
        
        DemoGuiUtils.DrawButton("Feed", () =>
        {
            demoController.PushMenuSection<ActivitiesSection>(section =>
            {
                section.Query = ActivitiesQuery.Everywhere().WithTag(item);
            });
        }, style: GSStyles.Button);
    }

    protected override void Load(PagingQuery<TagsQuery> query, Action<PagingResult<string>> success, Action<GetSocialError> error)
    {
        Communities.GetTags(query.Query, list => success(new PagingResult<string> { Entries = list }), error);
    }

    protected override void Count(TagsQuery query, Action<int> success, Action<GetSocialError> error)
    {
        // not supported
    }

    protected override TagsQuery CreateQuery(string query)
    {
        return TagsQuery.Find(query);
    }
}


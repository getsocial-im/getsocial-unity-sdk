using System;
using System.Linq;
using GetSocialSdk.Core;
using UnityEngine;

public class ReactionsListSection : BaseListSection<ReactionsQuery, UserReactions>
{
    public Activity Activity;
    protected override string GetSectionName()
    {
        return "Reactions";
    }

    protected override void Load(PagingQuery<ReactionsQuery> query, Action<PagingResult<UserReactions>> success, Action<GetSocialError> error)
    {
        Communities.GetReactions(query, success, error);
    }

    protected override void Count(ReactionsQuery query, Action<int> success, Action<GetSocialError> error)
    {
        // not supported
    }

    protected override void DrawItem(UserReactions item)
    {
        GUILayout.Label(item.User.DisplayName, GSStyles.BigLabelText);
        GUILayout.Label(item.Reactions.ToList().ToDebugString(), GSStyles.NormalLabelText);

        DemoGuiUtils.DrawButton("Actions", () => ShowActions(item.User), style:GSStyles.Button);
    }

    protected override ReactionsQuery CreateQuery(QueryObject queryObject)
    {
        return ReactionsQuery.ForActivity(Activity.Id);
    }

    protected override bool HasQuery()
    {
        return false;
    }
}

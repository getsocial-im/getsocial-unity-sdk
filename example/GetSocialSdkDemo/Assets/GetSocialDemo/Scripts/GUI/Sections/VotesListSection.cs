using System;
using System.Linq;
using GetSocialSdk.Core;
using UnityEngine;

public class VotesListSection : BaseListSection<VotesQuery, UserVotes>
{
    public Activity Activity;
    protected override string GetSectionName()
    {
        return "Votes";
    }

    protected override void Load(PagingQuery<VotesQuery> query, Action<PagingResult<UserVotes>> success, Action<GetSocialError> error)
    {
        Communities.GetVotes(query, success, error);
    }

    protected override void Count(VotesQuery query, Action<int> success, Action<GetSocialError> error)
    {
        // not supported
    }

    protected override void DrawItem(UserVotes item)
    {
        GUILayout.Label(item.User.DisplayName, GSStyles.BigLabelText);
        GUILayout.Label(item.Votes.ToList().ToDebugString(), GSStyles.NormalLabelText);

        DemoGuiUtils.DrawButton("Actions", () => ShowActions(item.User), style:GSStyles.Button);
    }

    protected override VotesQuery CreateQuery(string query)
    {
        return VotesQuery.ForActivity(Activity.Id);
    }

    protected override bool HasQuery()
    {
        return false;
    }
}

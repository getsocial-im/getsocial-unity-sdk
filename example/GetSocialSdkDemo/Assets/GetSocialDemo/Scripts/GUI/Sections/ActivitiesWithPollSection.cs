using System;
using System.Collections.Generic;
using System.Linq;
using GetSocialSdk.Core;
using GetSocialSdk.Ui;
using UnityEngine;

public class ActivitiesWithPollSection : BaseListSection<ActivitiesQuery, Activity>
{
    public ActivitiesQuery Query;
    public bool Comments;
    protected override string GetSectionName()
    {
        return "Activities with Poll";
    }

    protected override void Load(PagingQuery<ActivitiesQuery> query, Action<PagingResult<Activity>> success, Action<GetSocialError> error)
    {
        Communities.GetActivities(query, success, error);
    }

    protected override void Count(ActivitiesQuery query, Action<int> success, Action<GetSocialError> error)
    {
        // Not supported
    }

    protected override bool HasQuery()
    {
        return false;
    }

    protected override void DrawHeader()
    {
        GUILayout.BeginHorizontal();
        DemoGuiUtils.DrawButton("All", () => UpdateQuery(PollStatus.WithPoll), style: GSStyles.ShortButton);
        DemoGuiUtils.DrawButton("Voted", () => UpdateQuery(PollStatus.WithPollVotedByMe), style: GSStyles.ShortButton);
        DemoGuiUtils.DrawButton("Not Voted", () => UpdateQuery(PollStatus.WithPollNotVotedByMe), style: GSStyles.ShortButton);
        GUILayout.EndHorizontal();
        GUILayout.Space(20);
    }

    protected override void DrawItem(Activity item)
    {
        GUILayout.Label(item.Author.DisplayName, GSStyles.BigLabelText);
        GUILayout.Label(item.Text, GSStyles.NormalLabelText);
        GUILayout.Label("Total votes: " + item.Poll.TotalVotes, GSStyles.NormalLabelText);
        DemoGuiUtils.DrawButton("Actions", () => ShowActions(item), style:GSStyles.Button);
    }

    private void UpdateQuery(int pollStatus)
    {
        Query = Query.WithPollStatus(pollStatus);
        Reload();
    }

    private void ShowActions(Activity activity)
    {
        var popup = Dialog().WithTitle("Actions");
        popup.AddAction("Info", () => _console.LogD(activity.ToString()));
        popup.AddAction("All Votes", () => { ListVotes(activity); });
        popup.AddAction("Known voters", () => _console.LogD(activity.Poll.KnownVoters.ToDebugString()));
        if (activity.Source.IsActionAllowed(CommunitiesAction.React))
        {
            popup.AddAction("Vote", () => { Vote(activity); });
        }
        popup.AddAction("Cancel", () => { });
        popup.Show();
    }

    private void Vote(Activity activity)
    {
        demoController.PushMenuSection<VoteSection>(section =>
        {
            section.Activity = activity;
        });
    }

    private void ListVotes(Activity activity)
    {
        demoController.PushMenuSection<VotesListSection>(section =>
        {
            section.Activity = activity;
        });
    }

    protected override ActivitiesQuery CreateQuery(string query)
    {
        return Query;
    }
}

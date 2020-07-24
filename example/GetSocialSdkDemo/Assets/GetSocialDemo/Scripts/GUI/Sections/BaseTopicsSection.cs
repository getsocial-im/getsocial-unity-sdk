using System;
using System.Collections.Generic;
using GetSocialSdk.Core;
using GetSocialSdk.Ui;
using UnityEngine;

public abstract class BaseTopicsSection : BaseListSection<TopicsQuery, Topic>
{
    protected override string GetSectionName()
    {
        return "Topics";
    }

    protected override void Load(PagingQuery<TopicsQuery> query, Action<PagingResult<Topic>> success, Action<GetSocialError> error)
    {
        Communities.GetTopics(query, success, error);
    }

    protected override void Count(TopicsQuery query, Action<int> success, Action<GetSocialError> error)
    {
        Communities.GetTopicsCount(query, success, error);
    }

    protected override void DrawItem(Topic item)
    {
        GUILayout.Label(item.Title, GSStyles.BigLabelText);
        GUILayout.Label(item.Id, GSStyles.NormalLabelText);
        GUILayout.Label(item.AvatarUrl, GSStyles.NormalLabelText);
        DemoGuiUtils.DrawButton("Actions", () =>
        {
            ShowActions(item);
        }, style: GSStyles.Button);
    }

    private void ShowActions(Topic topic)
    {
        var popup = Dialog().WithTitle("Actions");
        if (topic.IsFollowedByMe)
        {
            popup.AddAction("Unfollow", () => Unfollow(topic));
        }
        else
        {
            popup.AddAction("Follow", () => Follow(topic));
        }
        popup.AddAction("Info", () => Print(topic));
        popup.AddAction("Followers", () => Followers(topic));
        popup.AddAction("Feed", () => Feed(topic));
        popup.AddAction("Feed UI", () => FeedUI(topic));
        popup.AddAction("Post To", () => PostTo(topic));
        popup.AddAction("Cancel", () => { });
        popup.Show();   
    }

    private void PostTo(Topic topic)
    {
        demoController.PushMenuSection<PostActivitySection>(section =>
        {
            section.Target = PostActivityTarget.Topic(topic.Id);
        });
    }

    private void Followers(Topic topic)
    {
        demoController.PushMenuSection<FollowersSection>(section =>
        {
            section.Query = FollowersQuery.OfTopic(topic.Id);
            section.Name = topic.Title;
        });
    }
    
    private void Feed(Topic topic)
    {
        demoController.PushMenuSection<ActivitiesSection>(section =>
        {
            section.Query = ActivitiesQuery.ActivitiesInTopic(topic.Id);
        });
    }

    private void FeedUI(Topic topic)
    {
        ActivityFeedViewBuilder.Create(ActivitiesQuery.ActivitiesInTopic(topic.Id))
                    .Show();
    }

    private void Print(Topic topic)
    {
        _console.LogD(topic.ToString());
    }

    private void Unfollow(Topic topic)
    {
        Communities.Unfollow(FollowQuery.Topics(topic.Id), count => 
        { 
            Refresh(topic);
        }, error => _console.LogE(error.Message));
    }
    
    private void Follow(Topic topic)
    {
        Communities.Follow(FollowQuery.Topics(topic.Id), count => 
        { 
            Refresh(topic);
        }, error => _console.LogE(error.Message));
    }

    private void Refresh(Topic topic)
    {
        Communities.GetTopic(topic.Id, refreshed =>
        {
            _items[_items.FindIndex(item => item == topic)] = refreshed;
        }, error => _console.LogE(error.ToString()));
    }
}

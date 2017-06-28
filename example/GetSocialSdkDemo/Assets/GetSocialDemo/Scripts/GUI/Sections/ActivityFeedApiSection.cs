using Assets.GetSocialDemo.Scripts.Utils;
using GetSocialSdk.Core;
using UnityEngine;

public class ActivityFeedApiSection : DemoMenuSection
{
    Texture2D _image;

    Texture2D Image
    {
        get { return _image ?? (_image = Resources.Load<Texture2D>("activityImage")); }
    }

    string _feed = ActivitiesQuery.GlobalFeed;
    string _activityId = "123";
    bool _isLiked;

    protected override string GetTitle()
    {
        return "Activity Feed API";
    }

    protected override void DrawSectionBody()
    {
        DrawFeedNameTextField();
        DrawActivityIdTextField();

        DemoGuiUtils.Space();

        DrawAnnouncementsBlock();
        DrawQueriesBlock();
        DrawPostAndLikeActivitiesBlock();
    }


    void DrawFeedNameTextField()
    {
        GUILayout.Label("Feed Name: ", GSStyles.NormalLabelText);
        _feed = GUILayout.TextField(_feed, GSStyles.TextField);
    }

    void DrawActivityIdTextField()
    {
        GUILayout.Label("Activity Feed Id: ", GSStyles.NormalLabelText);
        _activityId = GUILayout.TextField(_activityId, GSStyles.TextField);
    }

    void DrawQueriesBlock()
    {
        GUILayout.Label("Query", GSStyles.BigLabelText);

        GUILayout.BeginVertical("Box");
        GUILayout.Label("Settings for query: ", GSStyles.NormalLabelText);
        DrawFeedNameTextField();
        DrawActivityIdTextField();
        GUILayout.EndVertical();

        DemoGuiUtils.DrawButton("Get Global Feed Activities (No filter)", GetGlobalActivities, true, GSStyles.Button);
        DemoGuiUtils.DrawButton("Get Feed Activities (No filter)", GetFeedActivitiesNoFilter, true, GSStyles.Button);
        DemoGuiUtils.DrawButton("Get Feed Activities (Filter - Later)", GetFeedActivitiesFilterAfter, true,
            GSStyles.Button);
        DemoGuiUtils.DrawButton("Get Feed Activities (Filter - Before)", GetFeedActivitiesFilterBefore, true,
            GSStyles.Button);
        DemoGuiUtils.DrawButton("Get Feed Comments (No filter)", GetFeedComments, true, GSStyles.Button);
        DemoGuiUtils.DrawButton("Get Activity By Id ", GetActivityById, true, GSStyles.Button);
    }

    void DrawAnnouncementsBlock()
    {
        GUILayout.Label("Announcements", GSStyles.BigLabelText);
        DemoGuiUtils.DrawButton("Global Feed Announcements", GetGlobalFeedAnnouncements, true, GSStyles.Button);
        DemoGuiUtils.DrawButton("Feed Announcements", GetFeednnouncements, true, GSStyles.Button);
        DemoGuiUtils.Space();
    }

    void DrawPostAndLikeActivitiesBlock()
    {
        GUILayout.Label("Post Activities", GSStyles.BigLabelText);
        GUILayout.BeginVertical("Box");
        GUILayout.Label("Settings for posting: ", GSStyles.NormalLabelText);
        DrawFeedNameTextField();
        DrawActivityIdTextField();
        GUILayout.EndVertical();

        DemoGuiUtils.DrawButton("Post to Global Feed", PostToGlobalFeed, true, GSStyles.Button);
        DemoGuiUtils.DrawButton("Post to Feed", PostToFeed, true, GSStyles.Button);
        DemoGuiUtils.DrawButton("Post Comment", PostComment, true, GSStyles.Button);
        _isLiked = GUILayout.Toggle(_isLiked, "Like? ", GSStyles.Toggle);
        DemoGuiUtils.DrawButton("Like Activity", LikeActivity, true, GSStyles.Button);
        DemoGuiUtils.DrawButton("Get Activity Likers", GetLikers, true, GSStyles.Button);
    }

    #region announcements

    void GetGlobalFeedAnnouncements()
    {
        GetSocial.GetGlobalFeedAnnouncements(announcements =>
        {
            _console.LogD("Global feed announcements count: " + announcements.Count);
            _console.LogD(announcements.ToPrettyString());
        }, OnError);
    }

    void GetFeednnouncements()
    {
        GetSocial.GetAnnouncements(_feed, announcements =>
        {
            _console.LogD(string.Format("[{0}] Feed announcements count: {1}", _feed, announcements.Count));
            _console.LogD(announcements.ToPrettyString());
        }, OnError);
    }

    #endregion

    #region query_activities

    void GetGlobalActivities()
    {
        var query = ActivitiesQuery.PostsForGlobalFeed().WithLimit(5);
        GetSocial.GetActivities(query, posts =>
        {
            _console.LogD("Global feed posts count: " + posts.Count);
            _console.LogD(posts.ToPrettyString());
        }, OnError);
    }

    void GetFeedActivitiesNoFilter()
    {
        var query = ActivitiesQuery.PostsForFeed(_feed).WithLimit(5);
        GetSocial.GetActivities(query, posts =>
        {
            _console.LogD(string.Format("[{0}] Feed posts count: {1}", _feed, posts.Count));
            _console.LogD(posts.ToPrettyString());
        }, OnError);
    }

    void GetFeedActivitiesFilterAfter()
    {
        var query = ActivitiesQuery.PostsForFeed(_feed)
            .WithLimit(5)
            .WithFilter(ActivitiesQuery.Filter.Newer, _activityId);
        GetSocial.GetActivities(query, posts =>
        {
            _console.LogD(string.Format("[{0}] Feed posts newer count: {1}", _feed, posts.Count));
            _console.LogD(posts.ToPrettyString());
        }, OnError);
    }

    void GetFeedActivitiesFilterBefore()
    {
        var query = ActivitiesQuery.PostsForFeed(_feed)
            .WithLimit(5)
            .WithFilter(ActivitiesQuery.Filter.Older, _activityId);
        GetSocial.GetActivities(query, posts =>
        {
            _console.LogD(string.Format("[{0}] Feed posts older count: {1}", _feed, posts.Count));
            _console.LogD(posts.ToPrettyString());
        }, OnError);
    }

    void GetFeedComments()
    {
        var query = ActivitiesQuery.CommentsToPost(_activityId).WithLimit(5);
        GetSocial.GetActivities(query, comments =>
        {
            _console.LogD(string.Format("[{0}] Feed comments count: {1}", _feed, comments.Count));
            _console.LogD(comments.ToPrettyString());
        }, OnError);
    }


    void GetActivityById()
    {
        GetSocial.GetActivity(_activityId, activity => { _console.LogD(activity.ToString()); }, OnError);
    }

    #endregion

    #region posting_activities

    void PostToFeed()
    {
        GetSocial.PostActivityToFeed(_feed, GetPost(),
            activity => { _console.LogD(string.Format("Posted to [{0}] content: {1}", _feed, activity.ToString())); },
            OnError);
    }

    void PostToGlobalFeed()
    {
        if (GetSocial.User.IsAnonymous)
        {
            _console.LogD("Posting to Global Feed is not allowed for anonymous users.");
            return;
        }
        GetSocial.PostActivityToGlobalFeed(GetPost(),
            activity => { _console.LogD(string.Format("Posted to [{0}] content: {1}", _feed, activity.ToString())); },
            OnError);
    }


    void PostComment()
    {
        GetSocial.PostCommentToActivity(_activityId, GetPost(),
            activity => { _console.LogD(string.Format("Posted to [{0}] content: {1}", _feed, activity.ToString())); },
            OnError);
    }

    ActivityPostContent GetPost()
    {
        return ActivityPostContent.CreateBuilder()
            .WithText("My awesome post")
            .WithButton("Awesome Button", "action_id")
            .WithImage(Image)
            .Build();
    }

    void LikeActivity()
    {
        GetSocial.LikeActivity(_activityId, _isLiked,
            activity =>
            {
                _console.LogD(string.Format("Liked/Unliked [{0}] content: {1}", _activityId, activity.ToString()));
            },
            OnError);
    }

    void GetLikers()
    {
        GetSocial.GetActivityLikers(_activityId, 0, 5, likers =>
        {
            _console.LogD(string.Format("[{0}] Activity likers count: {1}", _activityId, likers.Count));
            _console.LogD(likers.ToPrettyString());
        }, OnError);
    }

    #endregion

    void OnActivityActionClicked(string actionId, ActivityPost post)
    {
        _console.LogD(string.Format("[{0}] button clicked on post: {1}", actionId, post));
    }

    void OnError(GetSocialError error)
    {
        _console.LogE("Error: " + error.Message);
    }
}
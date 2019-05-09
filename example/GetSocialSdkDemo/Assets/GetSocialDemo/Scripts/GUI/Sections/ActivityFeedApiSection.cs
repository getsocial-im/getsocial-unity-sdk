using System;
using System.Collections.Generic;
using System.Linq;
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

    byte[] _video;

    private byte[] Video
    {
        get { return _video ?? (_video = DemoUtils.LoadSampleVideoBytes()); }
    }

    private bool _postImage;
    private bool _postVideo;

    string _feed = ActivitiesQuery.GlobalFeed;
    string _activityId = "123";
    bool _isLiked;
    
    private string _action;
    private readonly List<Data> _actionData = new List<Data>();

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

        DemoGuiUtils.DrawButton("Fetch Last Activity", FetchLastActivity, true, GSStyles.Button);
        DemoGuiUtils.DrawButton("Get Global Feed Activities (No filter)", GetGlobalActivities, true, GSStyles.Button);
        DemoGuiUtils.DrawButton("Get Global Feed Activities (Only my posts)", GetMyGlobalActivities, true, GSStyles.Button);
        DemoGuiUtils.DrawButton("Get Global Friends Feed Activities", GetFriendsGlobalActivities, true, GSStyles.Button);
        DemoGuiUtils.DrawButton("Get Feed Activities (No filter)", GetFeedActivitiesNoFilter, true, GSStyles.Button);
        DemoGuiUtils.DrawButton("Get Feed Activities (Filter - Later)", GetFeedActivitiesFilterAfter, true,
            GSStyles.Button);
        DemoGuiUtils.DrawButton("Get Feed Activities (Filter - Before)", GetFeedActivitiesFilterBefore, true,
            GSStyles.Button);
        DemoGuiUtils.DrawButton("Get Feed Comments (No filter)", GetFeedComments, true, GSStyles.Button);
        DemoGuiUtils.DrawButton("Get Activity By Id ", GetActivityById, true, GSStyles.Button);
        DemoGuiUtils.DrawButton("Report Activity By Id ", ReportActivityById, true, GSStyles.Button);
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

        // Image/Video
        GUILayout.BeginHorizontal();
        GUILayout.Label("Image/Video", GSStyles.NormalLabelText, GUILayout.Width(Screen.width * 0.25f));

        if (GUILayout.Toggle(_postImage, "", GSStyles.ImageToggle))
        {
            _postImage = true; 
            _postVideo = false;
        }
        GUILayout.Label("Post Image", GSStyles.NormalLabelText, GUILayout.Width(Screen.width * 0.25f));

        if (GUILayout.Toggle(_postVideo, "", GSStyles.ImageToggle))
        {
            _postImage = false;
            _postVideo = true;
        }
        GUILayout.Label("Post Video", GSStyles.NormalLabelText, GUILayout.Width(Screen.width * 0.25f));
        
        if (GUILayout.Button("Clear", GSStyles.ClearButton))
        {
            _postImage = false;
            _postVideo = false;
        }
        GUILayout.EndHorizontal();
        
        GUILayout.Label("Action: " + (_action ?? "Default") , GSStyles.NormalLabelText);

        DemoGuiUtils.DrawRow(() =>
        {
            if (GUILayout.Button("Default", GSStyles.ShortButton))
            {
                _action = null;
            }
            var actions = new[] { GetSocialActionType.Custom, GetSocialActionType.OpenProfile, GetSocialActionType.OpenActivity, GetSocialActionType.OpenInvites, GetSocialActionType.OpenUrl };
            actions.ToList().ForEach(action =>
            {
                if (GUILayout.Button(action, GSStyles.ShortButton))
                {
                    _action = action;
                }
            });
        });
            
        DemoGuiUtils.DynamicRowFor(_actionData, "Action Data");
        
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
    

    void FetchLastActivity()
    {
        GetSocial.GetActivities(ActivitiesQuery.PostsForGlobalFeed().WithLimit(1), list =>
        {
            if (list.Count > 0)
            {
                _console.LogD(string.Format("Fetched activity: {0}", list[0]));
                _activityId = list[0].Id;
            }
        }, OnError);
    }

    void GetGlobalActivities()
    {
        var query = ActivitiesQuery.PostsForGlobalFeed().WithLimit(5);
        GetSocial.GetActivities(query, posts =>
        {
            _console.LogD("Global feed posts count: " + posts.Count);
            _console.LogD(posts.ToPrettyString());
        }, OnError);
    }
    
    void GetMyGlobalActivities()
    {
        var query = ActivitiesQuery.PostsForGlobalFeed().WithLimit(5).FilterByUser(GetSocial.User.Id);
        GetSocial.GetActivities(query, posts =>
        {
            _console.LogD("Global feed posts count: " + posts.Count);
            _console.LogD(posts.ToPrettyString());
        }, OnError);
    }
    
    void GetFriendsGlobalActivities()
    {
        var query = ActivitiesQuery.PostsForGlobalFeed().WithLimit(5).FriendsFeed(true);
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

    void ReportActivityById()
    {
        var popup = new MNPopup ("Report Activity", "What's wrong ?");
        popup.AddAction("Spam", _ReportActivityById(ReportingReason.Spam));
        popup.AddAction("Inappropriate Content", _ReportActivityById(ReportingReason.InappropriateContent));
        popup.AddAction("Cancel", () => { });
        popup.Show();            
    }

    private MNPopup.MNPopupAction _ReportActivityById(ReportingReason reportingReason)
    {
        return () =>
        {
            GetSocial.ReportActivity(_activityId, reportingReason, () =>
            {
                _console.LogD(string.Format("Activity {0} reported as {1}!", _activityId, reportingReason));
            }, OnError);
        };
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
        var mediaAttachment = _postImage ? MediaAttachment.Image(Image)
            : _postVideo ? MediaAttachment.Video(Video)
            : null;

#pragma warning disable 0618
        var content = ActivityPostContent.CreateBuilder()
            .WithText("My awesome post")
            .WithButton("Awesome Button", "action_id")
            .WithMediaAttachment(mediaAttachment);
#pragma warning restore 0618
                
        if (_action != null)
        {
            var action = GetSocialAction.CreateBuilder(_action)
                .AddActionData(_actionData.ToDictionary(data => data.Key, data => data.Val))
                .Build();

            content.WithButton("Awesome Button", action);
        }

        return content.Build();
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

    void OnError(GetSocialError error)
    {
        _console.LogE("Error: " + error.Message);
    }

    private class Data : DemoGuiUtils.IDrawableRow
    {
        public string Key = "";
        public string Val = "";
            
        public void Draw()
        {
            Key = GUILayout.TextField(Key, GSStyles.TextField);
            Val = GUILayout.TextField(Val, GSStyles.TextField);
        }
    }
}
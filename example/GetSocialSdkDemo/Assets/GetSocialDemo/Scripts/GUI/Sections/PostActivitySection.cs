using System;
using System.Collections.Generic;
using System.Linq;
using Assets.GetSocialDemo.Scripts.Utils;
using GetSocialSdk.Core;
using GetSocialSdk.Ui;
using NUnit.Framework;
using UnityEngine;

public class PostActivitySection : DemoMenuSection
{
    public string Activity;
    public PostActivityTarget Target;
    private string _text;
    private string _imageUrl = "";
    private string _videoUrl = "";

    private string _title;
    private string _action;
    private readonly List<Data> _actionData = new List<Data>();
    private readonly List<Data> _properties = new List<Data>();
    
    private bool _useCustomImage;
    private bool _useCustomVideo;
    public void SetActivity(Activity activity)
    {
        Activity = activity.Id;
        _text = activity.Text;
        if (activity.Button != null)
        {
            _title = activity.Button.Title;
            _action = activity.Button.Action.Type;
            foreach (var kv in activity.Button.Action.Data)
            {
                _actionData.Add(new Data{ Key = kv.Key, Val = kv.Value});
            }
        }
        foreach (var kv in activity.Properties)
        {
            _properties.Add(new Data{ Key = kv.Key, Val = kv.Value});
        }
        foreach (var attachment in activity.MediaAttachments)
        {
            if (attachment.VideoUrl != null)
            {
                if (_videoUrl.Length == 0)
                {
                    _videoUrl = attachment.VideoUrl;
                } else
                {
                    _useCustomVideo = true;
                }
            } else if (attachment.ImageUrl != null)
            {
                if (_imageUrl.Length == 0)
                {
                    _imageUrl = attachment.ImageUrl;
                } else
                {
                    _useCustomImage = true;
                }
            }
        }
    }

    protected override string GetTitle()
    {
        return "Post Activity";
    }

    protected override void DrawSectionBody()
    {
        DemoGuiUtils.DrawRow(() =>
        {
            GUILayout.Label("Text: ", GSStyles.NormalLabelText);
            _text = GUILayout.TextField(_text, GSStyles.TextField);
        });
        
        DemoGuiUtils.DrawRow(() =>
        {
            GUILayout.Label("Image url: ", GSStyles.NormalLabelText);
        });
        DemoGuiUtils.DrawRow(() =>
        {
            _imageUrl = GUILayout.TextField(_imageUrl, GSStyles.TextField);
        });
        
        DemoGuiUtils.DrawRow(() =>
        {
            GUILayout.Label("Video url: ", GSStyles.NormalLabelText);
        });
        DemoGuiUtils.DrawRow(() =>
        {
            _videoUrl = GUILayout.TextField(_videoUrl, GSStyles.TextField);
        });
        
        DemoGuiUtils.DrawRow(() =>
        {
            _useCustomImage = GUILayout.Toggle(_useCustomImage, "", GSStyles.Toggle);
            GUILayout.Label("Send Custom Image", GSStyles.NormalLabelText, GUILayout.Width(Screen.width * 0.25f));
        });
        
        DemoGuiUtils.DrawRow(() =>
        {
            _useCustomVideo = GUILayout.Toggle(_useCustomVideo, "", GSStyles.Toggle);
            GUILayout.Label("Send Custom Video", GSStyles.NormalLabelText, GUILayout.Width(Screen.width * 0.25f));
        });

        
        DemoGuiUtils.DrawRow(() => GUILayout.Label("Button", GSStyles.NormalLabelText));
        DemoGuiUtils.DrawRow(() =>
        {
            GUILayout.Label("Title: ", GSStyles.NormalLabelText);
            _title = GUILayout.TextField(_title, GSStyles.TextField);
        });
        GUILayout.Label("Action: " + (_action ?? "Default") , GSStyles.NormalLabelText);
        DemoGuiUtils.DrawRow(() =>
        {
            if (GUILayout.Button("Default", GSStyles.ShortButton))
            {
                _action = null;
            }

            var actions = new[] { GetSocialActionType.Custom, GetSocialActionType.OpenProfile, GetSocialActionType.OpenActivity, GetSocialActionType.OpenInvites, GetSocialActionType.OpenUrl,GetSocialActionType.AddFriend };
            actions.ToList().ForEach(action =>
            {
                if (GUILayout.Button(action, GSStyles.ShortButton))
                {
                    _action = action;
                }
            });
        });
        
        DemoGuiUtils.DynamicRowFor(_actionData, "Action Data");
        DemoGuiUtils.DynamicRowFor(_properties, "Properties");

        if (GUILayout.Button("Post", GSStyles.Button))
        {
            var content = new ActivityContent();
            content.Text = _text;

            if (_action != null && _title != null && _title.Length != 0)
            {
                var action = GetSocialAction.Create(_action, _actionData.ToDictionary(data => data.Key, data => data.Val));
                content.Button = ActivityButton.Create(_title, action);
            }
            if (_imageUrl.Length > 0)
            {
                content.AddMediaAttachment(MediaAttachment.WithImageUrl(_imageUrl));
            }
            if (_videoUrl.Length > 0)
            {
                content.AddMediaAttachment(MediaAttachment.WithVideoUrl(_videoUrl));
            }
            if (_useCustomImage)
            {
                content.AddMediaAttachment(MediaAttachment.WithImage(Resources.Load<Texture2D>("activityImage")));
            }
            if (_useCustomVideo)
            {
                content.AddMediaAttachment(MediaAttachment.WithVideo(DemoUtils.LoadSampleVideoBytes()));
            }
            if ((_text == null || _text.Length == 0) && content.Attachments.Count == 0 && (_action == null || (_title == null ||  _title.Length == 0)))
            {
                _console.LogE("Text, attachment or button is mandatory!");
                return;
            }

            content.AddProperties(_properties.ToDictionary(data => data.Key, data => data.Val));
            
            if (Activity == null) 
            {
                Communities.PostActivity(content, Target, posted => 
                {
                    _console.LogD("Posted: " + posted);
                    OpenFeed();
                }, error => 
                {
                    _console.LogE("Failed to post: " + error);
                });
            } else 
            {
                Communities.UpdateActivity(Activity, content, posted => 
                {
                    _console.LogD("Updated: " + posted);
                    OpenFeed();
                }, error => 
                {
                    _console.LogE("Failed to update: " + error);
                });
            }
        }
    }

    private void OpenFeed()
    {
        ActivitiesQuery query = null;
        if (Target.Ids.Type == CommunitiesEntityType.Group)
        {
            query = ActivitiesQuery.ActivitiesInGroup(Target.Ids.Ids.ToArray()[0]);
        }
        if (Target.Ids.Type == CommunitiesEntityType.Topic)
        {
            query = ActivitiesQuery.ActivitiesInTopic(Target.Ids.Ids.ToArray()[0]);
        }
        if (query != null)
        {
            ActivityFeedViewBuilder.Create(query)
                                .Show();
        }
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

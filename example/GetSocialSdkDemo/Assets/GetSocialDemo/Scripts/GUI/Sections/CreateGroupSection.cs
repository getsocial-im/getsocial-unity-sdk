using System;
using System.Collections.Generic;
using System.Linq;
using Assets.GetSocialDemo.Scripts.Utils;
using GetSocialSdk.Core;
using NUnit.Framework;
using UnityEngine;

public class CreateGroupSection : DemoMenuSection
{
    private string _id;
    private string _name;
    private string _description;
    private string _avatarUrl = "";
    private readonly List<Data> _properties = new List<Data>();
    
    private bool _useCustomImage = false;
    private bool _isPrivate;
    private bool _isDiscoverable;
    private Group oldGroup;
    private MemberRole _allowPost;
    private MemberRole _allowInteract;

    public void SetGroup(Group group)
    {
        oldGroup = group;
        _id = oldGroup.Id;
        _name = oldGroup.Title;
        _description = oldGroup.Description;
        _avatarUrl = oldGroup.AvatarUrl;
        _useCustomImage = false;
        foreach (var kv in oldGroup.Settings.Properties)
        {
            _properties.Add(new Data { Key = kv.Key, Val = kv.Value });
        }
        _isDiscoverable = oldGroup.Settings.IsDiscoverable;
        _isPrivate = oldGroup.Settings.IsPrivate;
        var canPost = oldGroup.Settings.Permissions[CommunitiesAction.Post];
        if (canPost == MemberRole.Owner) 
        {
            _allowPost = MemberRole.Owner;
        }
        if (canPost == MemberRole.Admin)
        {
            _allowPost = MemberRole.Admin;
        }
        if (canPost == MemberRole.Member)
        {
            _allowPost = MemberRole.Member;
        }
        var canInteract = oldGroup.Settings.Permissions[CommunitiesAction.React];
        if (canInteract == MemberRole.Owner)
        {
            _allowInteract = MemberRole.Owner;
        }
        if (canInteract == MemberRole.Admin)
        {
            _allowInteract = MemberRole.Admin;
        }
        if (canInteract == MemberRole.Member)
        {
            _allowInteract = MemberRole.Member;
        }
    }

    protected override string GetTitle()
    {
        return oldGroup == null ? "Create Group" : "Update Group";
    }

    protected override void DrawSectionBody()
    {
        DemoGuiUtils.DrawRow(() =>
        {
            GUILayout.Label("Id: ", GSStyles.NormalLabelText);
            if (oldGroup == null)
            {
                _id = GUILayout.TextField(_id, GSStyles.TextField);
            } else
            {
                GUILayout.Label(_id, GSStyles.NormalLabelText);
            }
        });
        DemoGuiUtils.DrawRow(() =>
        {
            GUILayout.Label("Name: ", GSStyles.NormalLabelText);
            _name = GUILayout.TextField(_name, GSStyles.TextField);
        });
        DemoGuiUtils.DrawRow(() =>
        {
            GUILayout.Label("Description: ", GSStyles.NormalLabelText);
            _description = GUILayout.TextField(_description, GSStyles.TextField);
        });
        DemoGuiUtils.DrawRow(() =>
        {
            GUILayout.Label("Avatar url: ", GSStyles.NormalLabelText);
        });
        DemoGuiUtils.DrawRow(() =>
        {
            _avatarUrl = GUILayout.TextField(_avatarUrl, GSStyles.TextField);
        });
        DemoGuiUtils.DrawRow(() =>
        {
            _useCustomImage = GUILayout.Toggle(_useCustomImage, "", GSStyles.Toggle);
            GUILayout.Label("Custom Avatar Image", GSStyles.NormalLabelText, GUILayout.Width(Screen.width * 0.25f));
        });
        GUILayout.Label("Allow post: " + (_allowPost), GSStyles.NormalLabelText);
        DemoGuiUtils.DrawRow(() =>
        {
            var actions = new[] { MemberRole.Owner, MemberRole.Admin, MemberRole.Member };
            actions.ToList().ForEach(action =>
            {
                if (GUILayout.Button(action.ToString(), GSStyles.ShortButton))
                {
                    _allowPost = action;
                }
            });
        });
        GUILayout.Label("Allow interact: " + (_allowInteract), GSStyles.NormalLabelText);
        DemoGuiUtils.DrawRow(() =>
        {
            var actions = new[] { MemberRole.Owner, MemberRole.Admin, MemberRole.Member };
            actions.ToList().ForEach(action =>
            {
                if (GUILayout.Button(action.ToString(), GSStyles.ShortButton))
                {
                    _allowInteract = action;
                }
            });
        });

        DemoGuiUtils.DrawRow(() =>
        {
            _isPrivate = GUILayout.Toggle(_isPrivate, "", GSStyles.Toggle);
            GUILayout.Label("Private?", GSStyles.NormalLabelText, GUILayout.Width(Screen.width * 0.25f));
        });
        DemoGuiUtils.DrawRow(() =>
        {
            _isDiscoverable = GUILayout.Toggle(_isDiscoverable, "", GSStyles.Toggle);
            GUILayout.Label("Discoverable?", GSStyles.NormalLabelText, GUILayout.Width(Screen.width * 0.25f));
        });

        DemoGuiUtils.DynamicRowFor(_properties, "Properties");

        if (GUILayout.Button(oldGroup == null ? "Create" : "Update", GSStyles.Button))
        {
            if (_id == null || _id.Length == 0)
            {
                _console.LogE("Group Id is mandatory");
                return;
            }
            if (_name == null || _name.Length == 0)
            {
                _console.LogE("Group Name is mandatory");
                return;
            }
            var content = new GroupContent(_id);
            content.Title = _name;
            content.Description = _description;
            if (_useCustomImage)
            {
                content.Avatar = MediaAttachment.WithImage(Resources.Load<Texture2D>("activityImage"));
            } else if (_avatarUrl != null && _avatarUrl.Length > 0)
            {
                content.Avatar = MediaAttachment.WithImageUrl(_avatarUrl);
            }
            content.AddProperties(_properties.ToDictionary(data => data.Key, data => data.Val));
            content.Permissions[CommunitiesAction.Post] = _allowPost;
            content.Permissions[CommunitiesAction.Comment] = _allowInteract;
            content.Permissions[CommunitiesAction.React] = _allowInteract;
            content.IsDiscoverable = _isDiscoverable;
            content.IsPrivate = _isPrivate;
            if (oldGroup == null) 
            {
                Communities.CreateGroup(content, created => 
                {
                    _console.LogD("Group created: " + Print(created));
                }, error => 
                {
                    _console.LogE("Failed to create: " + error);
                });
            } else 
            {
                Communities.UpdateGroup(oldGroup.Id, content, updated => 
                {
                    _console.LogD("Group updated: " + Print(updated));
                    SetGroup(updated);
                }, error => 
                {
                    _console.LogE("Failed to update: " + error);
                });
            }
        }
    }

    private string Print(Group group)
    {
        var groupInfo = group.ToString();
        if (group.CreatedAt != 0)
        {
            groupInfo += " [CreatedAt]: " + DateTimeOffset.FromUnixTimeSeconds(group.CreatedAt).ToString("yyyy:MM:dd HH:mm:ss") + ",";
        }
        if (group.UpdatedAt != 0)
        {
            groupInfo += " [UpdatedAt]: " + DateTimeOffset.FromUnixTimeSeconds(group.UpdatedAt).ToString("yyyy:MM:dd HH:mm:ss") + ",";
        }
        return groupInfo;
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

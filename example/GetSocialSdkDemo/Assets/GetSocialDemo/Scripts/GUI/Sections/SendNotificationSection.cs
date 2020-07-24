using System;
using System.Collections.Generic;
using System.Linq;
using Assets.GetSocialDemo.Scripts.Utils;
using GetSocialSdk.Core;
using NUnit.Framework;
using UnityEngine;

namespace GetSocialDemo.Scripts.GUI.Sections
{
    public class SendNotificationSection : DemoMenuSection
    {
        private static readonly Dictionary<string, string> Placeholders = new Dictionary<string, string>() {
            {"Receiver", NotificationContentPlaceholders.ReceiverDisplayName},
            {"Sender", NotificationContentPlaceholders.SenderDisplayName}
        };
        private string _title;
        private string _text;
        private string _imageUrl = "";
        private string _videoUrl = "";
        private string _backgroundImageConfiguration = "";
        private string _titleColor = "";
        private string _textColor = "";

        private string _action;
        private readonly List<Data> _actionData = new List<Data>();
        private readonly List<Data> _actionButtons = new List<Data>();
        
        private string _templateName;
        private readonly List<Data> _templatePlaceholders = new List<Data>();

        private bool _referrer;
        private bool _referredUsers;
        private bool _friends;
        private bool _me;
        private bool _useCustomImage;
        private bool _useCustomVideo;
        private readonly List<UserId> _userIds = new List<UserId>();
        private bool _sendBadgeValue;
        private bool _sendBadgeIncrease;
        private string _badgeValue = "";
        private string _badgeIncrease = "";

        private int _selectedPlaceholder;

        protected override string GetTitle()
        {
            return "Send Notification";
        }

        protected override void DrawSectionBody()
        {
            DemoGuiUtils.DrawRow(() =>
            {
                GUILayout.Label("Notification Title: ", GSStyles.NormalLabelText);
                _title = GUILayout.TextField(_title, GSStyles.TextField);
            });
            
            DemoGuiUtils.DrawRow(() =>
            {
                GUILayout.Label("Notification Text: ", GSStyles.NormalLabelText);
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

            DemoGuiUtils.DrawRow(() =>
            {
                GUILayout.Label("Background image: ", GSStyles.NormalLabelText);
            });
            DemoGuiUtils.DrawRow(() =>
            {
                _backgroundImageConfiguration = GUILayout.TextField(_backgroundImageConfiguration, GSStyles.TextField);
            });

            DemoGuiUtils.DrawRow(() =>
            {
                GUILayout.Label("Title color: ", GSStyles.NormalLabelText);
                _titleColor = GUILayout.TextField(_titleColor, GSStyles.TextField);
            });

            DemoGuiUtils.DrawRow(() =>
            {
                GUILayout.Label("Text color: ", GSStyles.NormalLabelText);
                _textColor = GUILayout.TextField(_textColor, GSStyles.TextField);
            });

            DemoGuiUtils.DrawRow(() =>
            {
                Placeholders.Keys.ToList().ForEach(key =>
                {
                    if (GUILayout.Button(key, GSStyles.Button))
                    {
                        _text += Placeholders[key];
                    }
                });
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
            DemoGuiUtils.DynamicRowFor(_actionButtons, "Action Buttons");

            DemoGuiUtils.DrawRow(() =>
            {
                GUILayout.Label("Template Name: ", GSStyles.NormalLabelText);
                _templateName = GUILayout.TextField(_templateName, GSStyles.TextField);
            });

            DemoGuiUtils.DynamicRowFor(_templatePlaceholders, "Template Placeholders");
            
            GUILayout.Label("Recipients:", GSStyles.NormalLabelText);
            
            DemoGuiUtils.DrawRow(() =>
            {
                _referrer = GUILayout.Toggle(_referrer, "", GSStyles.Toggle);
                GUILayout.Label("Referrer", GSStyles.NormalLabelText);
            });
            DemoGuiUtils.DrawRow(() =>
            {
                _referredUsers = GUILayout.Toggle(_referredUsers, "Referred Users", GSStyles.Toggle);
                GUILayout.Label("Referred Users", GSStyles.NormalLabelText);
            });
            DemoGuiUtils.DrawRow(() =>
            {
                _friends = GUILayout.Toggle(_friends, "Friends", GSStyles.Toggle);
                GUILayout.Label("Friends", GSStyles.NormalLabelText);
            });
            DemoGuiUtils.DrawRow(() =>
            {
                _me = GUILayout.Toggle(_me, "Me", GSStyles.Toggle);
                GUILayout.Label("Me", GSStyles.NormalLabelText);
            });
            
            DemoGuiUtils.DynamicRowFor(_userIds, "Custom Users ID");
            
            DemoGuiUtils.DrawRow(() =>
            {
                _sendBadgeValue = GUILayout.Toggle(_sendBadgeValue, "", GSStyles.Toggle);
                if (_sendBadgeValue)
                {
                    _sendBadgeIncrease = false;
                    _badgeIncrease = "";
                }
                GUILayout.Label("Badge Count", GSStyles.NormalLabelText);
            });

            if (_sendBadgeValue)
            {
                _badgeValue = GUILayout.TextField(_badgeValue, GSStyles.TextField);
            }

            DemoGuiUtils.DrawRow(() =>
            {
                _sendBadgeIncrease = GUILayout.Toggle(_sendBadgeIncrease, "", GSStyles.Toggle);
                if (_sendBadgeIncrease)
                {
                    _sendBadgeValue = false;
                    _badgeValue = "";
                }
                GUILayout.Label("Badge Increase", GSStyles.NormalLabelText);
            });

            if (_sendBadgeIncrease)
            {
                _badgeIncrease = GUILayout.TextField(_badgeIncrease, GSStyles.TextField);
            }

            if (GUILayout.Button("Send", GSStyles.Button))
            {
                var userIds = _userIds.ConvertAll(user => user.UserIdString);
                if (_me)
                {
                    userIds.Add(GetSocial.GetCurrentUser().Id);
                }
                var notificationTarget = new SendNotificationTarget(UserIdList.Create(userIds));
                if (_referrer)
                {
                    notificationTarget.AddPlaceholder(NotificationReceiverPlaceholders.Referrer);
                }
                if (_referredUsers)
                {
                    notificationTarget.AddPlaceholder(NotificationReceiverPlaceholders.ReferredUsers);
                }
                if (_friends)
                {
                    notificationTarget.AddPlaceholder(NotificationReceiverPlaceholders.Friends);
                }

                if (GetSocialActionType.AddFriend.Equals(_action) && string.IsNullOrEmpty(_text) && string.IsNullOrEmpty(_templateName))
                {
                    var addFriend = NotificationContent.CreateWithText(
                            NotificationContentPlaceholders.SenderDisplayName + " wants to become friends.")
                        .WithTitle("Friend request")
                        .WithAction(GetSocialAction.Create(_action, new Dictionary<string, string>() {
                            { GetSocialActionKeys.AddFriend.UserId, GetSocial.GetCurrentUser().Id },
                            { "user_name", GetSocial.GetCurrentUser().DisplayName }
                        }));
                
               
                    Notifications.Send(addFriend, 
                        notificationTarget, 
                        () => _console.LogD("Notification sending started"), 
                        error => _console.LogE("Error: " + error.Message)
                    );
                    return;
                }
                var mediaAttachment = _imageUrl.Length > 0 ? MediaAttachment.WithImageUrl(_imageUrl)
                    : _videoUrl.Length > 0 ? MediaAttachment.WithVideoUrl(_videoUrl)
                    : _useCustomImage ? MediaAttachment.WithImage(Resources.Load<Texture2D>("activityImage"))
                    : _useCustomVideo ? MediaAttachment.WithVideo(DemoUtils.LoadSampleVideoBytes())
                    : null;

                var customization = NotificationCustomization
                    .WithBackgroundImageConfiguration(_backgroundImageConfiguration.ToNullIfEmpty())
                    .WithTitleColor(_titleColor.ToNullIfEmpty())
                    .WithTextColor(_textColor.ToNullIfEmpty());
                var content = NotificationContent.CreateWithText(_text)
                    .WithTitle(_title)
                    .WithTemplateName(_templateName)
                    .AddTemplatePlaceholders(_templatePlaceholders.ToDictionary(data => data.Key, data => data.Val))
                    .WithMediaAttachment(mediaAttachment)
                    .WithCustomization(customization)
                    .AddActionButtons(_actionButtons.ConvertAll(item => NotificationButton.Create(item.Key, item.Val))
                 );

                if (_sendBadgeValue) 
                {
                    content.WithBadge(Badge.SetTo(int.Parse(_badgeValue)));
                } else if (_sendBadgeIncrease)
                {
                    content.WithBadge(Badge.IncreaseBy(int.Parse(_badgeIncrease)));
                }
                
                if (_action != null)
                {
                    var action = GetSocialAction.Create(_action, _actionData.ToDictionary(data => data.Key, data => data.Val));
                    content.WithAction(action);
                }
                
                Notifications.Send(content, 
                    notificationTarget, 
                    () => _console.LogD("Notifications sending started"), 
                    error => _console.LogE("Error: " + error.Message)
                );
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

        private class UserId : DemoGuiUtils.IDrawableRow
        {
            public string UserIdString = "";

            public void Draw()
            {
                UserIdString = GUILayout.TextField(UserIdString, GSStyles.TextField);
            }
        }
    }
    static class StringHelper 
    {
        public static string ToNullIfEmpty(this string str)
        {
            return str == null || str.Length == 0 ? null : str;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using GetSocialSdk.Core;
using UnityEngine;

namespace GetSocialDemo.Scripts.GUI.Sections
{
    public class SendNotificationSection : DemoMenuSection
    {
        private static readonly Dictionary<string, string> Placeholders = new Dictionary<string, string>() {
            {"Receiver", SendNotificationPlaceholders.CustomText.ReceiverDisplayName},
            {"Sender", SendNotificationPlaceholders.CustomText.SenderDisplayName}
        };
        private string _title;
        private string _text;

        private Notification.Type? _action;
        private readonly List<Data> _actionData = new List<Data>();
        
        private string _templateName;
        private readonly List<Data> _templatePlaceholders = new List<Data>();

        private bool _referrer;
        private bool _referredUsers;
        private bool _frieds;
        private readonly List<UserId> _userIds = new List<UserId>();

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
                Placeholders.Keys.ToList().ForEach(key =>
                {
                    if (GUILayout.Button(key, GSStyles.Button))
                    {
                        _text += Placeholders[key];
                    }
                });
            });
            
            GUILayout.Label("Action: " + (_action == null ? "Default" : _action.Value.ToString()) , GSStyles.NormalLabelText);
            DemoGuiUtils.DrawRow(() =>
            {
                if (GUILayout.Button("Default", GSStyles.ShortButton))
                {
                    _action = null;
                }
                var actions = Enum.GetValues(typeof(Notification.Type)).Cast<Notification.Type>();
                actions.ToList().ForEach(action =>
                {
                    if (GUILayout.Button(action.ToString(), GSStyles.ShortButton))
                    {
                        _action = action;
                    }
                });
            });
            
            DynamicRowFor(_actionData, "Action Data");

            DemoGuiUtils.DrawRow(() =>
            {
                GUILayout.Label("Template Name: ", GSStyles.NormalLabelText);
                _templateName = GUILayout.TextField(_templateName, GSStyles.TextField);
            });

            DynamicRowFor(_templatePlaceholders, "Template Placeholders");
            
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
                _frieds = GUILayout.Toggle(_frieds, "Friends", GSStyles.Toggle);
                GUILayout.Label("Friends", GSStyles.NormalLabelText);
            });
            
            DynamicRowFor(_userIds, "Custom Users ID");
            
            if (GUILayout.Button("Send", GSStyles.Button))
            {
                var recipients = _userIds.ConvertAll(user => user.UserIdString);
                if (_referrer)
                {
                    recipients.Add(SendNotificationPlaceholders.Receivers.Referrer);
                }
                if (_referredUsers)
                {
                    recipients.Add(SendNotificationPlaceholders.Receivers.ReferredUsers);
                }
                if (_frieds)
                {
                    recipients.Add(SendNotificationPlaceholders.Receivers.Friends);
                }

                var content = NotificationContent.NotificationWithText(_text)
                    .WithTitle(_title)
                    .AddActionData(_actionData.ToDictionary(data => data.Key, data => data.Val))
                    .WithTemplateName(_templateName)
                    .AddTemplatePlaceholders(_templatePlaceholders.ToDictionary(data => data.Key, data => data.Val));

                if (_action != null)
                {
                    content.WithAction(_action.Value);
                }
                
                GetSocial.User.SendNotification(recipients, 
                    content, 
                    summary => _console.LogD("Sent " + summary.SuccessfullySentCount + " notifications"), 
                    error => _console.LogE("Error: " + error.Message)
                );
            }
        }

        private static void DynamicRowFor<T>(List<T> list, string sectionName) where T:IDrawableRow, new() 
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(sectionName, GSStyles.NormalLabelText);
            if (GUILayout.Button("Add", GSStyles.Button))
            {
                list.Add(new T());
            }
            GUILayout.EndHorizontal();
            
            list.ForEach(data =>
            {
                GUILayout.BeginHorizontal();
                data.Draw();
                if (GUILayout.Button("Remove", GSStyles.Button))
                {
                    list.Remove(data);
                }
                GUILayout.EndHorizontal();
            });
        }

        private class Data : IDrawableRow
        {
            public string Key = "";
            public string Val = "";
            
            public void Draw()
            {
                Key = GUILayout.TextField(Key, GSStyles.TextField);
                Val = GUILayout.TextField(Val, GSStyles.TextField);
            }
        }

        private class UserId : IDrawableRow
        {
            public string UserIdString = "";

            public void Draw()
            {
                UserIdString = GUILayout.TextField(UserIdString, GSStyles.TextField);
            }
        }
        
        private interface IDrawableRow
        {
            void Draw();
        }
    }
}
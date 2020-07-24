using System;
using System.Collections.Generic;
using UnityEngine;
using GetSocialSdk.Core;


public class MessageView : DemoMenuSection
{

    public User Receiver;
    private string _message;
    
    private List<Activity> _messages;
    
    protected override void DrawSectionBody()
    {
        GUILayout.BeginHorizontal();
        _message = GUILayout.TextField(_message, GSStyles.TextField);
        DemoGuiUtils.DrawButton("Send", () =>
        {
            SendChatMessage(_message);
        }, true, GSStyles.Button);        
        GUILayout.EndHorizontal();

        foreach (var message in _messages)
        {
            DemoGuiUtils.DrawRow(DrawMessage(message));
        }
    }

    protected override void GoBack()
    {
        enabled = false;
        // GetComponentInParent<SocialGraphSection>().enabled = true;
    }

    private static Action DrawMessage(Activity message)
    {
        return () =>
        {
            if (message.Author.Id.Equals(GetSocial.GetCurrentUser().Id))
            {
                GUILayout.Label(message.Text, GSStyles.RightAlignedChatText);
            }
            else
            {
                GUILayout.Label(message.Text, GSStyles.LeftAlignedChatText);
            }
        };
    }

    protected override string GetTitle()
    {
        return "Chat with " + Receiver.DisplayName;
    }

    private void Awake()
    {
        _messages = new List<Activity>();
    }

    private static string GenerateChatId(string[] userIds)
    {
        Array.Sort(userIds);
        return "chat_" + string.Join("_", userIds);
    }

    public void LoadMessages()
    {
        // var chatId = GenerateChatId(new[] {GetSocial.GetCurrentUser().Id, Receiver.Id});
        // var query = ActivitiesQuery.ActivitiesInTopic(chatId);
        // GetSocial.GetActivities(query, list =>
        // {
        //     _messages = list;
        //     _messages.Reverse();
        // }, error =>
        // {
        //     _console.LogE("Failed to get messages, error: " + error);
        // });
    }

    private void SendChatMessage(string message)
    {
        // var feedId = GenerateChatId(new[] {GetSocial.GetCurrentUser().Id, Receiver.Id});
        // var messageContentBuilder = new ActivityContent
        // {
        //     Text = message
        // };
        // MNP.ShowPreloader("Sending message", "Please wait...");
        // GetSocial.PostActivityToFeed(feedId, messageContentBuilder.Build(), post =>
        // {
        //     SendNotification(message, Receiver.Id);
        //     LoadMessages();
        // }, error =>
        // {
        //     MNP.HidePreloader();
        //     _console.LogE("Failed to send message, error: " + error);
        // });
    }

    private void SendNotification(string message, string recepientId)
    {
        // var messageData = new Dictionary<string, string> {{"open_messages_for_id", GetSocial.GetCurrentUser().Id}};

        // var builder = GetSocialAction.CreateBuilder("open_chat_message");
        // builder.AddActionData(messageData);
        //
        // var notificationContent = NotificationContent.NotificationWithText(message)
        //     .WithTitle(GetSocial.GetCurrentUser().DisplayName)
        //     .WithAction(builder.Build());
        //
        // var recepients = new List<string> {recepientId};
        // GetSocial.GetCurrentUser().SendNotification(recepients, notificationContent, summary =>
        // {
        //     MNP.HidePreloader();
        // }, error =>
        // {
        //     MNP.HidePreloader();
        // });
    }
}

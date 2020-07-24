using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GetSocialSdk.Core;

public class NotificationsFilterSection : DemoMenuSection
{
    public static readonly List<string> AllStatuses = new List<string> { NotificationStatus.Read, NotificationStatus.Unread, NotificationStatus.Consumed, NotificationStatus.Ignored };

    private static readonly List<string> AllTypes = new List<string>
    {
        Notification.NotificationTypes.Comment,
        Notification.NotificationTypes.Direct,
        Notification.NotificationTypes.Sdk,
        Notification.NotificationTypes.Targeting,
        Notification.NotificationTypes.InviteAccepted,
        Notification.NotificationTypes.LikeActivity,
        Notification.NotificationTypes.LikeComment,
        Notification.NotificationTypes.NewFriendship,
        Notification.NotificationTypes.MentionInActivity,
        Notification.NotificationTypes.MentionInComment,
        Notification.NotificationTypes.ReplyToComment,
        Notification.NotificationTypes.CommentedInSameThread,
    };
    private readonly List<Data> _actions = new List<Data>();
    private readonly bool[] Statuses = new bool[AllStatuses.Count];
    private readonly bool[] Types = new bool[AllTypes.Count];

    public delegate void FilterSet(List<string> statuses, List<string> types, List<string> actions);

    public FilterSet Callback;
    
    protected override string GetTitle()
    {
        return "Setup Notifications";
    }

    protected override void DrawSectionBody()
    {
        GUILayout.BeginVertical();
        DemoGuiUtils.DrawButton("Save", () =>
        {
            Callback(
                AllStatuses.FindAll(it => Statuses[AllStatuses.IndexOf(it)]), 
                AllTypes.FindAll(it => Types[AllTypes.IndexOf(it)]),
                _actions.ConvertAll(it => it.Input)
            );
            GoBack();
        }, style: GSStyles.Button);
        DemoGuiUtils.DynamicRowFor(_actions, "Actions");
        DemoGuiUtils.Space();
        GUILayout.Label("Statuses", GSStyles.NormalLabelText);
        for (var i = 0; i < AllStatuses.Count; i++)
        {
            DemoGuiUtils.DrawRow(() =>
            {
                Statuses[i] = GUILayout.Toggle(Statuses[i], AllStatuses[i], GSStyles.Toggle);
                GUILayout.Label(AllStatuses[i], GSStyles.NormalLabelText);
            });
        }
        DemoGuiUtils.Space();
        GUILayout.Label("Types", GSStyles.NormalLabelText);
        for (var i = 0; i < AllTypes.Count; i++)
        {
            DemoGuiUtils.DrawRow(() =>
            {
                Types[i] = GUILayout.Toggle(Types[i], AllTypes[i], GSStyles.Toggle);
                GUILayout.Label(AllTypes[i], GSStyles.NormalLabelText);
            });
        }
        GUILayout.EndVertical();
    }

    private class Data: DemoGuiUtils.IDrawableRow
    {
        public string Input;

        public void Draw()
        {
            Input = GUILayout.TextField(Input, GSStyles.TextField);
        }
    }
}

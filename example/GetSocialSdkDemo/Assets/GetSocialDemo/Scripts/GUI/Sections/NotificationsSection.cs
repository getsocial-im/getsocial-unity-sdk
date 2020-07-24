using System;
using System.Collections.Generic;
using GetSocialSdk.Core;
using UnityEngine;

public class NotificationsSection : BaseListSection<NotificationsQuery, Notification>
{
    private NotificationsQuery Query = NotificationsQuery.WithAllStatuses();
    protected override string GetSectionName()
    {
        return "Notifications";
    }

    protected override void DrawHeader()
    {
        DemoGuiUtils.DrawButton("Filter", () =>
        {
            demoController.PushMenuSection<NotificationsFilterSection>(section =>
            {
                section.Callback = SetFilter;
            });
        }, style: GSStyles.Button);
    }

    private void SetFilter(List<string> statuses, List<string> types, List<string> actions)
    {
        Query = NotificationsQuery.WithStatuses(statuses).OfTypes(types).WithActions(actions);
        Reload();
    }

    protected override void Load(PagingQuery<NotificationsQuery> query, Action<PagingResult<Notification>> success, Action<GetSocialError> error)
    {
        Notifications.Get(query, success, error);
    }

    protected override void Count(NotificationsQuery query, Action<int> success, Action<GetSocialError> error)
    {
        Notifications.Count(query, success, error);
    }

    protected override void DrawItem(Notification item)
    {
        GUILayout.Label(item.Sender.DisplayName, GSStyles.BigLabelText);
        GUILayout.Label(item.Text, GSStyles.NormalLabelText);
        GUILayout.Label(item.Id, GSStyles.NormalLabelText);
        GUILayout.Label(item.Action.ToString(), GSStyles.NormalLabelText);
        DemoGuiUtils.DrawButton("Actions", () =>
        {
            ShowActions(item);
        }, style: GSStyles.Button);
    }
    private void ShowActions(Notification notification)
    {
        var popup = Dialog().WithTitle("Actions");
        popup.AddAction("Info", () => Print(notification));
        if (!notification.Status.Equals(NotificationStatus.Ignored) && !notification.Status.Equals(NotificationStatus.Consumed))
        {
            popup.AddAction("Change Status", () => ChangeStatus(notification));
        }
        popup.AddAction("Author", () => ShowActions(notification.Sender));
        popup.AddAction("Cancel", () => { });
        popup.Show();   
    }

    private void ChangeStatus(Notification notification) 
    {
        var popup = Dialog().WithTitle("Actions");
        foreach (var item in NotificationsFilterSection.AllStatuses)
        {
            if (notification.Status.Equals(item)) 
            {
                continue;
            }
            popup.AddAction(item, () => {
                Notifications.SetStatus(item, new List<string> { notification.Id }, () =>
                {
                    notification.Status = item;
                }, error => _console.LogE(error.ToString()));
            });
        }
        popup.Show();   
    }

    private void Print(Notification notification) 
    {
        _console.LogD(notification.ToString());
    }

    protected override NotificationsQuery CreateQuery(string query)
    {
        return Query;
    }

    protected override bool HasQuery()
    {
        return false;
    }
}

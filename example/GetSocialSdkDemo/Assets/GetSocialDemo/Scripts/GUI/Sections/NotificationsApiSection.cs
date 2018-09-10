using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Assets.GetSocialDemo.Scripts.Utils;
using GetSocialSdk.Core;
using UnityEngine;
using UnityEngine.UI;

public class NotificationsApiSection : DemoMenuSection
{
    private List<Notification> _notifications = new List<Notification>();
    private int _count = 0;

    private static readonly string[] Filters = { "Read and Unread", "Read", "Unread" };
    private static readonly string[] Types = { "All", "Custom" };

    private int _readUnread = 0;
    private int _types = 0;
    private int selectedListItem;
    
    protected override string GetTitle()
    {
        return string.Format("Notifications({0})", _count);
    }

    protected override void DrawSectionBody()
    {
        int readUnread = GUILayout.SelectionGrid(_readUnread, Filters, Filters.Length, GSStyles.Button);
        GUILayout.Space(10);
        int types = GUILayout.SelectionGrid(_types, Types, Types.Length, GSStyles.Button);
        GUILayout.Space(10);
        bool hasChanges = readUnread != _readUnread || _types != types;

        _readUnread = readUnread;
        _types = types;

        if (hasChanges)
        {
            Sync();
        }

        DemoGuiUtils.DrawButton("Refresh", Sync, true, GSStyles.Button);
        DemoGuiUtils.DrawButton("Load next", LoadNext, _notifications.Count > 0, GSStyles.Button);
        DemoGuiUtils.DrawButton("Toggle last read state", ToggleFirstReadState
, true, GSStyles.Button);
        DemoGuiUtils.DrawButton("Display Notifications", DisplayNotifications, true, GSStyles.Button);
    }

    void GetNotifications()
    {
        var query = _readUnread == 0 ? NotificationsQuery.ReadAndUnread() :
            _readUnread == 1 ? NotificationsQuery.Read() : NotificationsQuery.Unread();
        if (_types != 0)
        {
            query.OfTypes(Notification.NotificationTypes.Comment, Notification.NotificationTypes.CommentedInSameThread);
        }
        GetSocial.User.GetNotifications(query, notifications =>
        {
            _notifications = notifications;
            notifications.ForEach(notification => _console.LogD(notification.ToString(), false));
        }, Debug.LogError);
    }
    void LoadNext()
    {
        var query = _readUnread == 0 ? NotificationsQuery.ReadAndUnread() :
            _readUnread == 1 ? NotificationsQuery.Read() : NotificationsQuery.Unread();
        if (_types != 0)
        {
            query.OfTypes(Notification.NotificationTypes.Comment, Notification.NotificationTypes.CommentedInSameThread);
        }
        query.WithLimit(10);
        query.WithFilter(NotificationsQuery.Filter.Older, _notifications.Last().Id);
        GetSocial.User.GetNotifications(query, notifications =>
        {
            _notifications.AddRange(notifications);
            notifications.ForEach(notification => _console.LogD(notification.ToString(), false));
        }, Debug.LogError);
    }
    void UpdateCount()
    {
        var query = _readUnread == 0 ? NotificationsCountQuery.ReadAndUnread() :
            _readUnread == 1 ? NotificationsCountQuery.Read() : NotificationsCountQuery.Unread();
        if (_types != 0)
        {
            query.OfTypes(Notification.NotificationTypes.Comment, Notification.NotificationTypes.CommentedInSameThread);
        }
        GetSocial.User.GetNotificationsCount(query, i => _count = i, Debug.LogError);
    }

    void ToggleFirstReadState()
    {
        if (_notifications.Count > 0)
        {
            var notification = _notifications.First();
            GetSocial.User.SetNotificationsRead(new List<string> { notification.Id }, !notification.WasRead, Sync, Debug.LogError);
        }
    }
    void Sync() {
        UpdateCount();
        GetNotifications();
    }

    void DisplayNotifications()
    {
        var query = NotificationsQuery.ReadAndUnread();
        GetSocial.User.GetNotifications(query, notifications =>
        {
            _notifications = notifications;
            var message = "";
            notifications.ForEach(notification =>
            {
                message += notification.ToString() + '\n';
            });
            DemoUtils.ShowPopup("Notifications", message);
        }, Debug.LogError);
    }
}
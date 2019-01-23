using System.Collections.Generic;
using System.Linq;
using GetSocialSdk.Core;
using UnityEngine;

public class NotificationsApiSection : DemoMenuSection
{
    private static readonly string[] Tabs = {"Filter", "Notifications"};
    private int _currentTab = 0;

    private static List<Notification> _notifications = new List<Notification>();
    private int _count = 0;

    private static List<ActionRow> _actions = new List<ActionRow>();

    private static readonly string[] Filters = { NotificationStatus.Read, NotificationStatus.Unread, NotificationStatus.Consumed, NotificationStatus.Ignored };
    private static readonly bool[] _selectedFilters = CreateSelectedFiltersArray();

    private bool _likedAndComments = false;
    
    protected override string GetTitle()
    {
        return string.Format("Notifications({0})", _count);
    }

    private static bool[] CreateSelectedFiltersArray()
    {
        var arr = new bool[Filters.Length];
        for (var i = 0; i < arr.Length; i++)
        {
            arr[i] = false;
        }

        return arr;
    }
    
    protected override void DrawSectionBody()
    {
        _currentTab = GUILayout.SelectionGrid(_currentTab, Tabs, Tabs.Length, GSStyles.Button);
        if (_currentTab == 0)
        {
            DrawFilter();
        }
        else
        {
            DrawNotifications();
        }
    }

    private void DrawFilter()
    {
        DemoGuiUtils.DrawRow(() =>
        {
            for (var i = 0; i < Filters.Length; i++)
            {
                _selectedFilters[i] = GUILayout.Toggle(_selectedFilters[i], "", GSStyles.Toggle);
                GUILayout.Label(Filters[i], new GUIStyle {fontSize = 28});                
            }
        });
        DemoGuiUtils.DrawRow(() =>
        {
            _likedAndComments = GUILayout.Toggle(_likedAndComments, "", GSStyles.Toggle);
            GUILayout.Label("Likes and Comments only", new GUIStyle {fontSize = 28});
        });
        DemoGuiUtils.DynamicRowFor(_actions, "Actions");
        
        DemoGuiUtils.DrawButton("Save", Sync, style: GSStyles.Button);
    }

    private void DrawNotifications()
    {
        DemoGuiUtils.DrawButton("Load Newer", LoadPrev, style:GSStyles.Button);
        DemoGuiUtils.DrawLine();
        _notifications.ForEach(notification =>
        {
            GUILayout.BeginVertical();
            DemoGuiUtils.DrawRow(() =>
            {
                GUILayout.Label(notification.Title + "(" + notification.Status + ")", GSStyles.NormalLabelText);
            });
            DemoGuiUtils.DrawRow(() =>
            {
                GUILayout.Label(notification.Text, GSStyles.NormalLabelText);
            });
            if (notification.Status.Equals(NotificationStatus.Unread))
            {
                DemoGuiUtils.DrawRow(() =>
                {
                    DemoGuiUtils.DrawButton("Read", () =>
                    {
                        GetSocial.User.SetNotificationsStatus(new List<string> { notification.Id }, NotificationStatus.Read,
                            Sync, Debug.LogError);
                    }, style:GSStyles.ShortButton);
                });
            } else if (notification.Status.Equals(NotificationStatus.Read))
            {
                DemoGuiUtils.DrawRow(() =>
                {
                    DemoGuiUtils.DrawButton("Unread", () =>
                    {
                        GetSocial.User.SetNotificationsStatus(new List<string> { notification.Id }, NotificationStatus.Unread,
                            Sync, Debug.LogError);
                    }, style:GSStyles.ShortButton);
                });
            }
            
            DemoGuiUtils.DrawRow(() =>
            {
                if (notification.ImageUrl != null)
                {
                    DemoGuiUtils.DrawButton("Open Image", () =>
                    {
                        Application.OpenURL(notification.ImageUrl);
                    }, style:GSStyles.ShortButton);
                }
                if (notification.VideoUrl != null)
                {
                    DemoGuiUtils.DrawButton("Open Video", () =>
                    {
                        Application.OpenURL(notification.VideoUrl);
                    }, style:GSStyles.ShortButton);
                }
                DemoGuiUtils.DrawButton("Print to console", () =>
                {
                    _console.LogD(notification.ToString());
                }, style:GSStyles.ShortButton);
            });
            
            if (notification.ActionButtons.Count > 0)
            {
                DemoGuiUtils.DrawRow(() =>
                {
                    notification.ActionButtons.ForEach(actionButton =>
                    {
                        DemoGuiUtils.DrawButton(actionButton.Title, () =>
                        {
                            GetSocialDemoController.ProcessAction(actionButton.Id, notification);
                            Sync();
                        }, style:GSStyles.ShortButton);
                    });
                });
            }

            DemoGuiUtils.DrawLine();
            GUILayout.EndVertical();
        });
        DemoGuiUtils.DrawButton("Load Older", LoadNext, style:GSStyles.Button);
    }

    private NotificationsQuery CreateQuery()
    {
        var query = NotificationsQuery.WithStatuses(Filters.Where((t, i) => _selectedFilters[i]).ToArray());
        if (_likedAndComments)
        {
            query.OfTypes(Notification.NotificationTypes.Comment, Notification.NotificationTypes.LikeActivity, Notification.NotificationTypes.LikeComment);
        }

        if (_actions.Count > 0)
        {
            query.WithActions(_actions.ConvertAll(it => it.ActionId).ToArray());
        }

        return query;
    }
    
    void GetNotifications()
    {
        GetSocial.User.GetNotifications(CreateQuery(), notifications =>
        {
            _notifications.Clear();
            _notifications.AddAll(notifications);
        }, Debug.LogError);
    }
    
    void LoadNext()
    {
        var query = CreateQuery();
        query.WithLimit(10);
        query.WithFilter(NotificationsQuery.Filter.Older, _notifications.Last().Id);
        
        GetSocial.User.GetNotifications(query, notifications =>
        {
            _notifications.AddRange(notifications);
        }, Debug.LogError);
    }
    
    void LoadPrev()
    {
        var query = CreateQuery();
        query.WithLimit(10);
        query.WithFilter(NotificationsQuery.Filter.Newer, _notifications.First().Id);
        GetSocial.User.GetNotifications(query, notifications =>
        {
            _notifications.AddRange(notifications);
        }, Debug.LogError);
    }
    
    void UpdateCount()
    {
        var query = NotificationsCountQuery.WithStatuses(Filters.Where((t, i) => _selectedFilters[i]).ToArray());
        if (_likedAndComments)
        {
            query.OfTypes(Notification.NotificationTypes.Comment, Notification.NotificationTypes.LikeActivity, Notification.NotificationTypes.LikeComment);
        }

        if (_actions.Count > 0)
        {
            query.WithActions(_actions.ConvertAll(it => it.ActionId).ToArray());
        }
        
        GetSocial.User.GetNotificationsCount(query, i => _count = i, Debug.LogError);
    }
    
    void Sync() {
        UpdateCount();
        GetNotifications();
    }

    private class ActionRow : DemoGuiUtils.IDrawableRow
    {
        public string ActionId = "";

        public void Draw()
        {
            ActionId = GUILayout.TextField(ActionId, GSStyles.TextField);
        }
    }
}
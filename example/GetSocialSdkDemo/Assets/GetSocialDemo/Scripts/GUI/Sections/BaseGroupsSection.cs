using System;
using System.Collections.Generic;
using GetSocialSdk.Core;
using GetSocialSdk.Ui;
using UnityEngine;

public abstract class BaseGroupsSection : BaseListSection<GroupsQuery, Group>
{
    protected override string GetSectionName()
    {
        return "Groups";
    }

    protected override void Load(PagingQuery<GroupsQuery> query, Action<PagingResult<Group>> success, Action<GetSocialError> error)
    {
        Communities.GetGroups(query, success, error);
    }

    protected override void Count(GroupsQuery query, Action<int> success, Action<GetSocialError> error)
    {
        Communities.GetGroupsCount(query, success, error);
    }

    protected override void DrawItem(Group item)
    {
        GUILayout.Label(item.Title, GSStyles.BigLabelText);
        GUILayout.Label(item.Id, GSStyles.NormalLabelText);
        GUILayout.Label(item.AvatarUrl, GSStyles.NormalLabelText);
        DemoGuiUtils.DrawButton("Actions", () =>
        {
            ShowActions(item);
        }, style: GSStyles.Button);
    }

    private void ShowActions(Group group)
    {
        var popup = Dialog().WithTitle("Actions");
        popup.AddAction("Info", () => Print(group));

        var membership = group.Membership;
        if (membership == null)
        {
            popup.AddAction("Join", () => {
                Join(group, null);
            });
        }
        if (!group.Settings.IsPrivate || (membership != null && membership.Status == MemberStatus.Member))
        {
            popup.AddAction("Feed", () => Feed(group));
            popup.AddAction("Feed UI", () => FeedUI(group));
        }
        if (membership != null)
        {
            if (membership.Role != MemberRole.Owner)
            {
                popup.AddAction("Leave", () => {
                    Leave(group);
                });
            }
            if (membership.Status == MemberStatus.Member)
            {
                if (membership.Role == MemberRole.Admin || membership.Role == MemberRole.Owner)
                {
                    popup.AddAction("Add Member", () => {
                        ShowAddMember(group);
                    });

                }
                popup.AddAction("Show Members", () => {
                    ShowMembers(group);

                });
                if (group.Settings.IsActionAllowed(CommunitiesAction.Post))
                {
                    popup.AddAction("Post To", () => PostTo(group));
                }
                if (membership.Role == MemberRole.Admin || membership.Role == MemberRole.Owner)
                {
                    popup.AddAction("Edit", () => {
                        demoController.PushMenuSection<CreateGroupSection>(section =>
                        {
                            section.SetGroup(group);
                        });
                    });
                    popup.AddAction("Delete", () => {
                        DeleteGroup(group);
                    });
                }
            }
            if (membership.Status == MemberStatus.InvitationPending)
            {
                popup.AddAction("Approve invitation", () => {
                    Join(group, membership.InvitationToken);
                });
            }
        }
        popup.AddAction("Cancel", () => { });
        popup.Show();
    }

    private void ShowAddMember(Group group)
    {
        demoController.PushMenuSection<AddGroupMemberSection>(section =>
        {
            section.GroupId = group.Id;
        });
    }

    private void ShowMembers(Group group)
    {
        demoController.PushMenuSection<GroupMembersSection>(section =>
        {
            section.Query = MembersQuery.OfGroup(group.Id);
            section.GroupId = group.Id;
            section.CurrentUserRole = group.Membership.Role;
        });
    }

    private void DeleteGroup(Group group)
    {
        var groupIds = new List<string>();
        groupIds.Add(group.Id);
        Communities.RemoveGroups(groupIds, () => {
            _console.LogD("Group deleted");
            _items.Remove(group);
            _count -= 1;
        }, (error) => {
            _console.LogE(error.ToString());
        });
    }

    private void PostTo(Group group)
    {
        demoController.PushMenuSection<PostActivitySection>(section =>
        {
            section.Target = PostActivityTarget.Group(group.Id);
        });
    }
    
    private void Feed(Group group)
    {
        demoController.PushMenuSection<ActivitiesSection>(section =>
        {
            section.Query = ActivitiesQuery.ActivitiesInGroup(group.Id);
        });
    }

    private void FeedUI(Group group)
    {
        ActivityFeedViewBuilder.Create(ActivitiesQuery.ActivitiesInGroup(group.Id))
            .SetActionListener(OnAction)
            .SetMentionClickListener(OnMentionClicked)
            .SetAvatarClickListener(OnUserAvatarClicked)
            .SetTagClickListener(OnTagClicked)
            .Show();
    }

    private void OnMentionClicked(string mention)
    {
        if (mention.Equals(MentionTypes.App))
        {
            _console.LogD("Application mention clicked.");
            return;
        }
        Communities.GetUser(UserId.Create(mention), OnUserAvatarClicked, error => _console.LogE("Failed to get user details, error:" + error.Message));
    }

    private void OnTagClicked(string tag)
    {
        GetSocialUi.CloseView();
        demoController.PushMenuSection<ActivitiesSection>(section =>
        {
            section.Query = ActivitiesQuery.Everywhere().WithTag(tag);
        });
    }

    private void OnUserAvatarClicked(User publicUser)
    {
        if (!GetSocial.GetCurrentUser().Id.Equals(publicUser.Id))
        {
            Communities.IsFriend(UserId.Create(publicUser.Id), isFriend =>
            {
                if (isFriend)
                {
                    var popup = new MNPopup("Action", "Choose Action");
                    popup.AddAction("Remove from Friends", () => RemoveFriend(publicUser));
                    popup.AddAction("Cancel", () => { });
                    popup.Show();
                }
                else
                {
                    var popup = new MNPopup("Action", "Choose Action");
                    popup.AddAction("Add to Friends", () => AddFriend(publicUser));
                    popup.AddAction("Cancel", () => { });
                    popup.Show();
                }
            }, error => _console.LogE("Failed to check if friends with " + publicUser.DisplayName + ", error:" + error.Message));
        }
    }

    private void AddFriend(User user)
    {
        Communities.AddFriends(UserIdList.Create(user.Id),
            friendsCount =>
            {
                var message = user.DisplayName + " is now your friend.";
                _console.LogD(message);
            },
            error => _console.LogE("Failed to add a friend " + user.DisplayName + ", error:" + error.Message));
    }

    private void RemoveFriend(User user)
    {
        Communities.RemoveFriends(UserIdList.Create(user.Id),
            friendsCount =>
            {
                var message = user.DisplayName + " is not your friend anymore.";
                _console.LogD(message);
            },
            error => _console.LogE("Failed to remove a friend " + user.DisplayName + ", error:" + error.Message));
    }

    void OnAction(GetSocialAction action)
    {
        demoController.HandleAction(action);
    }

    private void Print(Group group)
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
        _console.LogD(groupInfo);
    }

    private void Join(Group group, string invitationToken)
    {
        var query = new JoinGroupQuery(group.Id);
        if (invitationToken != null)
        {
            query = query.WithInvitationToken(invitationToken);
        }
        Communities.JoinGroup(query, (member) => {
            _console.LogD("Joined to Group");
            this.Refresh(group);
        }, (error) => {
            _console.LogE(error.ToString());
        });
    }

    private void Leave(Group group)
    {
        var query = new RemoveGroupMembersQuery(group.Id, UserId.CurrentUser().ToUserIdList());
        Communities.RemoveGroupMembers(query, () => {
            _console.LogD("Group left");
            if (GetSectionName().Equals("MyGroups"))
            {
                _items.Remove(group);
                _count -= 1;
            } else
            {
                this.Refresh(group);
            }
        }, (error) => {
            _console.LogE(error.ToString());
        });
    }

    private void Refresh(Group group)
    {
        Communities.GetGroup(group.Id, refreshed =>
        {
            _items[_items.FindIndex(item => item == group)] = refreshed;
        }, error => _console.LogE(error.ToString()));
    }
}

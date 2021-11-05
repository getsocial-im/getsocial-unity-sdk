using System;
using GetSocialSdk.Core;
using UnityEngine;

public class GroupMembersSection : BaseListSection<MembersQuery, GroupMember>
{
    private CurrentUser _currentUser;
    public MemberRole CurrentUserRole;
    public string GroupId;
    public MembersQuery Query;

    protected override string GetSectionName()
    {
        return "Group Members";
    }

    protected override MembersQuery CreateQuery(QueryObject queryObject)
    {
        return Query;
    }

    protected override void Load(PagingQuery<MembersQuery> query, Action<PagingResult<GroupMember>> success, Action<GetSocialError> error)
    {
        _currentUser = GetSocial.GetCurrentUser();
        Communities.GetGroupMembers(query, success, error);
    }

    protected override void DrawItem(GroupMember item)
    {
        GUILayout.Label(item.DisplayName, GSStyles.BigLabelText);
        GUILayout.Label(item.Id, GSStyles.NormalLabelText);
        GUILayout.Label(item.AvatarUrl, GSStyles.NormalLabelText);
        DemoGuiUtils.DrawButton("Actions", () =>
        {
            ShowActions(item);
        }, style: GSStyles.Button);
    }

    private void ShowActions(GroupMember groupMember)
    {
        var popup = Dialog().WithTitle("Actions");
        popup.AddAction("Info", () => Print(groupMember));
        if ((CurrentUserRole == MemberRole.Admin || CurrentUserRole == MemberRole.Owner) && !groupMember.Id.Equals(_currentUser.Id) && groupMember.Membership.Role != MemberRole.Owner)
        {
            popup.AddAction("Remove", () => {
                RemoveMember(groupMember);
            });
            if (groupMember.Membership.Status == MemberStatus.ApprovalPending)
            {
                popup.AddAction("Approve", () => {
                    ApproveMember(groupMember);
                });
            }
        }
        if (groupMember.Id.Equals(_currentUser.Id) && groupMember.Membership.Role != MemberRole.Owner)
        {
            popup.AddAction("Leave", () => {
                RemoveMember(groupMember);
            });
        }

        popup.AddAction("Cancel", () => { });
        popup.Show();   
    }

    private void ApproveMember(GroupMember member)
    {
        var query = new UpdateGroupMembersQuery(GroupId, UserIdList.Create(member.Id))
            .WithMemberStatus(MemberStatus.Member)
            .WithMemberRole(MemberRole.Member);
        Communities.UpdateGroupMembers(query, (members) => {
            _console.LogD("Member approved");
            Refresh(member, members.ToArray()[0]);
        }, (error) => {
            _console.LogD("Failed to approve member, error: " + error);
        });
    }

    private void RemoveMember(GroupMember member)
    {
        var query = new RemoveGroupMembersQuery(GroupId, UserIdList.Create(member.Id));
        Communities.RemoveGroupMembers(query, () => {
            _console.LogD("Member removed from group");
            Refresh(member, null);
        }, (error) => {
            _console.LogD("Failed to remove user from group, error: " + error);
        });
    }

    private void Print(GroupMember groupMember)
    {
        var memberInfo = groupMember.ToString();
        memberInfo += " [CreatedAt]: " + DateTimeOffset.FromUnixTimeSeconds(groupMember.Membership.CreatedAt).ToString("yyyy:MM:dd HH:mm:ss") + ",";
        _console.LogD(memberInfo);
    }

    private void Refresh(GroupMember old, GroupMember groupMember)
    {
        if (groupMember == null)
        {
            _items.Remove(old);
        } else {
            _items[_items.FindIndex(item => item == old)] = groupMember;
        }
    }

    protected override void Count(MembersQuery query, Action<int> success, Action<GetSocialError> error)
    {
        // not implemented
    }
}

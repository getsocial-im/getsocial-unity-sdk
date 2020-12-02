using System.Linq;
using GetSocialSdk.Core;
using UnityEngine;

public class AddGroupMemberSection : DemoMenuSection
{
    public string GroupId;
    private string _userId;
    private string _providerId;
    private MemberStatus _status = MemberStatus.Member;
    private MemberRole _role = MemberRole.Member;

    protected override string GetTitle()
    {
        return "Add Group Member";
    }

    protected override void DrawSectionBody()
    {
        DemoGuiUtils.DrawRow(() =>
        {
            GUILayout.Label("User Id: ", GSStyles.NormalLabelText);
            _userId = GUILayout.TextField(_userId, GSStyles.TextField);
        });
        DemoGuiUtils.DrawRow(() =>
        {
            GUILayout.Label("Provider Id: ", GSStyles.NormalLabelText);
            _providerId = GUILayout.TextField(_providerId, GSStyles.TextField);
        });
        GUILayout.Label("Role: " + (_role), GSStyles.NormalLabelText);
        DemoGuiUtils.DrawRow(() =>
        {
            var roles = new[] { MemberRole.Admin, MemberRole.Member };
            roles.ToList().ForEach(action =>
            {
                if (GUILayout.Button(action.ToString(), GSStyles.ShortButton))
                {
                    _role = action;
                }
            });
        });
        GUILayout.Label("Status: " + (_status), GSStyles.NormalLabelText);
        DemoGuiUtils.DrawRow(() =>
        {
            var statuses = new[] { MemberStatus.InvitationPending, MemberStatus.Member };
            statuses.ToList().ForEach(action =>
            {
                if (GUILayout.Button(action.ToString(), GSStyles.ShortButton))
                {
                    _status = action;
                }
            });
        });

        if (GUILayout.Button("Create", GSStyles.Button))
        {
            UserIdList userIdList = null;
            if (_providerId == null || _providerId.Length == 0)
            {
                userIdList = UserIdList.Create(_userId);
            } else
            {
                userIdList = UserIdList.CreateWithProvider(_providerId, _userId);
            }
            var query = new AddGroupMembersQuery(GroupId, userIdList).WithMemberRole(_role).WithMemberStatus(_status);
            Communities.AddGroupMembers(query, (member) => {
                _console.LogD("Member added");
            }, (error) => {
                _console.LogD("Failed to add member, error: " + error);
            });
        }
    }
}

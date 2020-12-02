using GetSocialSdk.Core;

public class MyGroupsSection : BaseGroupsSection
{

    protected override string GetSectionName()
    {
        return "MyGroups";
    }

    protected override GroupsQuery CreateQuery(string query)
    {
        return GroupsQuery.Find(query).ByMember(UserId.CurrentUser());
    }
}

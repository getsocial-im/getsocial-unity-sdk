using GetSocialSdk.Core;

public class MyGroupsSection : BaseGroupsSection
{

    protected override string GetSectionName()
    {
        return "MyGroups";
    }

    protected override GroupsQuery CreateQuery(QueryObject queryObject)
    {
        return GroupsQuery.Find(queryObject.SearchTerm).ByMember(UserId.CurrentUser());
    }
}

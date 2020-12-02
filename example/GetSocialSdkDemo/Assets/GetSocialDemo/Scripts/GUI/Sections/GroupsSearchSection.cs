using GetSocialSdk.Core;

public class GroupsSearchSection : BaseGroupsSection
{
    protected override GroupsQuery CreateQuery(string query)
    {
        return GroupsQuery.Find(query);
    }
}

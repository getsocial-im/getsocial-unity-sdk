using GetSocialSdk.Core;

public class FollowedTopicsSection : BaseTopicsSection
{
    public UserId User;
    protected override TopicsQuery CreateQuery(QueryObject query)
    {
        return TopicsQuery.FollowedBy(User);
    }

    protected override bool HasQuery()
    {
        return false;
    }
}

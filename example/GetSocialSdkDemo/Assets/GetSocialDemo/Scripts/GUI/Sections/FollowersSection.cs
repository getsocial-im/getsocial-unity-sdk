using System;
using GetSocialSdk.Core;

public class FollowersSection: BaseUsersListSection<FollowersQuery>
{
    public string Name;
    public FollowersQuery Query;
    protected override string GetSectionName()
    {
        return "Followers of " + Name;
    }

    protected override void Load(PagingQuery<FollowersQuery> query, Action<PagingResult<User>> success, Action<GetSocialError> error)
    {
        Communities.GetFollowers(query, success, error);
    }

    protected override void Count(FollowersQuery query, Action<int> success, Action<GetSocialError> error)
    {
        Communities.GetFollowersCount(query, success, error);
    }

    protected override FollowersQuery CreateQuery(QueryObject queryObject)
    {
        return Query;
    }

    protected override bool HasQuery()
    {
        return false;
    }
}

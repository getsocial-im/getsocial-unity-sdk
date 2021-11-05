using System;
using GetSocialSdk.Core;

public class FollowingSection : BaseUsersListSection<UsersQuery>
{
    public string Name;
    public UserId User;
    
    protected override string GetSectionName()
    {
        return "Followed by " + Name;
    }

    protected override void Load(PagingQuery<UsersQuery> query, Action<PagingResult<User>> success, Action<GetSocialError> error)
    {
        Communities.GetUsers(query, success, error);
    }

    protected override void Count(UsersQuery query, Action<int> success, Action<GetSocialError> error)
    {
        Communities.GetUsersCount(query, success, error);
    }

    protected override UsersQuery CreateQuery(QueryObject queryObject)
    {
        return UsersQuery.FollowedByUser(User);
    }

    protected override bool HasQuery()
    {
        return false;
    }
}

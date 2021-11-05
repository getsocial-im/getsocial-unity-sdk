using System;
using GetSocialSdk.Core;

public class FriendsSection : BaseUsersListSection<FriendsQuery>
{
    public UserId User;
    protected override void Load(PagingQuery<FriendsQuery> query, Action<PagingResult<User>> success, Action<GetSocialError> error)
    {
        Communities.GetFriends(query, success, error);
    }

    protected override void Count(FriendsQuery query, Action<int> success, Action<GetSocialError> error)
    {
        Communities.GetFriendsCount(query, success, error);
    }

    protected override FriendsQuery CreateQuery(QueryObject queryObject)
    {
        return FriendsQuery.OfUser(User);
    }

    protected override string GetSectionName()
    {
        return "Friend";
    }

    protected override bool HasQuery()
    {
        return false;
    }
    
}

using System;
using System.Collections.Generic;
using GetSocialSdk.Core;
using UnityEngine;

public class UsersSearchSection : BaseUsersListSection<UsersQuery>
{
    protected override string GetSectionName()
    {
        return "Users";
    }

    protected override void Load(PagingQuery<UsersQuery> query, Action<PagingResult<User>> success, Action<GetSocialError> error)
    {
        Communities.GetUsers(query, success, error);
    }

    protected override void Count(UsersQuery query, Action<int> success, Action<GetSocialError> error)
    {
        Communities.GetUsersCount(query, success, error);
    }

    protected override UsersQuery CreateQuery(string query)
    {
        var searchTerm = query ?? "";
        return UsersQuery.Find(searchTerm);
    }
}

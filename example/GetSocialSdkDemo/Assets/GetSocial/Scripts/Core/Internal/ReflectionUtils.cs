using System;
using System.Linq.Expressions;

namespace GetSocialSdk.Core
{
    public static class ReflectionUtils
    {
        public static string GetMemberName<T, TValue>(this Expression<Func<T, TValue>> memberAccess)
        {
            return ((MemberExpression) memberAccess.Body).Member.Name;
        }
    }
}
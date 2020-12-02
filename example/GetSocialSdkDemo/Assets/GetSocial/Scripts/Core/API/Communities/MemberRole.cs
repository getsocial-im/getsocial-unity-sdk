namespace GetSocialSdk.Core
{
    public enum MemberRole
    {
        Owner = 0,
        Admin = 1,
        Member = 3,
        Follower = 4, // Used only in Topics permissions
        Everyone = 5 // Used only in Topics permissions
    }
}
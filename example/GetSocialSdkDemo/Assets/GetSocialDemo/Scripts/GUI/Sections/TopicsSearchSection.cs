using GetSocialSdk.Core;

public class TopicsSearchSection : BaseTopicsSection
{
    protected override TopicsQuery CreateQuery(string query)
    {
        return TopicsQuery.Find(query);
    }
}

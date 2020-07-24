namespace GetSocialSdk.MiniJSON
{
    [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property)]
    public class JsonSerializationKey : System.Attribute
    {
        public JsonSerializationKey(string name)
        {
            Name = name;
        }

        internal string Name { get; }
    }
}
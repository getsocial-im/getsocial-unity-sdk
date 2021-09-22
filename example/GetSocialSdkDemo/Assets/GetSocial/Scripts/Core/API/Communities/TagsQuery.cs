using GetSocialSdk.MiniJSON;
namespace GetSocialSdk.Core {
    public class TagsQuery {
        [JsonSerializationKey ("query")]
        internal string Query;

        [JsonSerializationKey ("target")]
        internal PostActivityTarget Target;

		[JsonSerializationKey("trending")]
		internal bool InternalTrending = false;

		private TagsQuery (string term) {
            Query = term;
        }

        public static TagsQuery Find (string term) {
            return new TagsQuery (term);
        }

		public TagsQuery OnlyTrending(bool trending)
		{
			this.InternalTrending = trending;
			return this;
		}

		public TagsQuery InTarget (PostActivityTarget target) {
            Target = target;
            return this;
        }

        public override string ToString()
        {
            return $"TagsQuery: Query: {Query}, target: {Target}, trending: {InternalTrending}";
        }

    }
}
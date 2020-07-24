using GetSocialSdk.MiniJSON;
namespace GetSocialSdk.Core {
    public class TagsQuery {
        [JsonSerializationKey ("query")]
        internal string Query;

        [JsonSerializationKey ("target")]
        internal PostActivityTarget Target;
        private TagsQuery (string term) {
            Query = term;
        }

        public static TagsQuery Find (string term) {
            return new TagsQuery (term);
        }

        public TagsQuery InTarget (PostActivityTarget target) {
            Target = target;
            return this;
        }
    }
}
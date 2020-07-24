using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public class PagingQuery<Q>
    {
        [JsonSerializationKey("limit")]
        public int Limit { get; internal set; }
        [JsonSerializationKey("next")]
        public string NextCursor { get; internal set;  }

        [JsonSerializationKey("query")]
        public Q Query { get; }

        internal PagingQuery()
        {
            Limit = 25;
        }

        public PagingQuery(Q query)
        {
            Query = query;
            Limit = 25;
        }

        public PagingQuery<Q> WithLimit(int limit)
        {
            Limit = limit;
            return this;
        }
        
        public PagingQuery<Q> Next(string next)
        {
            NextCursor = next;
            return this;
        }
    }

    public class PagingResult<R>
    {
        [JsonSerializationKey("next")]
        public string NextCursor { get; internal set; }

        public bool IsLastPage
        {
            get
            {
                return string.IsNullOrEmpty(NextCursor);
            }
        }

        [JsonSerializationKey("entries")]
        public List<R> Entries { get; internal set; }
    }

    public class SimplePagingQuery : PagingQuery<object>
    {
        public static SimplePagingQuery Simple(int limit)
        {
            return new SimplePagingQuery {Limit = limit};
        }
    }
}
using System;
using System.Collections.Generic;
using GetSocialSdk.MiniJSON;
using UnityEngine;

namespace GetSocialSdk.Core
{
    public sealed class PromoCodeContent
    {
        [JsonSerializationKey("code")]
        internal string Code;

        [JsonSerializationKey("data")]
        internal readonly Dictionary<string, string> Data;

        [JsonSerializationKey("maxClaimCount")]
        public uint MaxClaimCount = 0;

        public DateTime? StartDate
        {
            set
            {
                _startDate = value.HasValue ? value.Value.ToUnixTimestamp() : 0;
            }
        }

        [JsonSerializationKey("startDate")]
        internal long _startDate;

        public DateTime? EndDate 
        {
            set
            {
                _endDate = value.HasValue ? value.Value.ToUnixTimestamp() : 0;
            }
        }
        [JsonSerializationKey("endDate")]
        internal long _endDate;

        internal PromoCodeContent(string code)
        {
            Code = code;
            Data = new Dictionary<string, string>();
        }

        /// <summary>
        /// Create a Promo Code with a random code.
        /// </summary>
        /// <returns>New builder instance.</returns>
        public static PromoCodeContent CreateRandomCode()
        {
            return new PromoCodeContent(null);
        }

        /// <summary>
        /// Create a Promo Code with defined code.
        /// </summary>
        /// <param name="code">Code to be used as promo code.</param>
        /// <returns>New builder instance.</returns>
        public static PromoCodeContent CreateWithCode(string code)
        {
            return new PromoCodeContent(code);
        }


        /// <summary>
        /// Set the time range when this Promo Code is available.
        /// If not set - will be available from the creation moment and until manually disabled on the Dashboard.
        /// </summary>
        /// <param name="startDate">Date when the Promo Code should become available.</param>
        /// <param name="endDate">Date when the Promo Code should not be available anymore.</param>
        /// <returns>Instance of builder for method chaining.</returns>
        public PromoCodeContent SetTimeLimit(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
            return this;
        }

        /// <summary>
        /// Attach some custom data to the promo code.
        /// </summary>
        /// <param name="key">Data key.</param>
        /// <param name="value">Data value.</param>
        /// <returns>Instance of builder for method chaining.</returns>
        public PromoCodeContent AddProperty(string key, string value)
        {
            Data.Add(key, value);
            return this;
        }

        /// <summary>
        /// Attach some custom data to the promo code.
        /// </summary>
        /// <param name="data">Custom data.</param>
        /// <returns>Instance of builder for method chaining.</returns>
        public PromoCodeContent AddProperties(Dictionary<string, string> data)
        {
            Data.AddAll(data);
            return this;
        }
    }
}
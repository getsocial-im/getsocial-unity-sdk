using System;
using System.Collections.Generic;
using GetSocialSdk.MiniJSON;
using UnityEngine;

namespace GetSocialSdk.Core {
    public class PromoCodeBuilder : IConvertableToNative {
        internal string _code;
        internal readonly Dictionary<string, string> _data;
        internal uint _maxClaimCount = 0;
        internal DateTime? _startDate, _endDate;

        public PromoCodeBuilder (string code) {
            _code = code;
            _data = new Dictionary<string, string> ();
        }

        /// <summary>
        /// Create a Promo Code with a random code.
        /// </summary>
        /// <returns>New builder instance.</returns>
        public static PromoCodeBuilder CreateRandomCode () {
            return new PromoCodeBuilder (null);
        }

        /// <summary>
        /// Create a Promo Code with defined code.
        /// </summary>
        /// <param name="code">Code to be used as promo code.</param>
        /// <returns>New builder instance.</returns>
        public static PromoCodeBuilder CreateWithCode (string code) {
            return new PromoCodeBuilder (code);
        }
        
        /// <summary>
        /// The maximum number of times this code can be claimed.
        /// All the next attempts will receive an error.
        /// If `0` then no limits.
        /// </summary>
        /// <param name="maxClaimCount">Maximum claim times.</param>
        /// <returns>Instance of builder for method chaining.</returns>
        public PromoCodeBuilder WithMaxClaimCount (uint maxClaimCount) {
            _maxClaimCount = maxClaimCount;
            return this;
        }

        /// <summary>
        /// Set the time range when this Promo Code is available.
        /// If not set - will be available from the creation moment and until manually disabled on the Dashboard.
        /// </summary>
        /// <param name="startDate">Date when the Promo Code should become available.</param>
        /// <param name="endDate">Date when the Promo Code should not be available anymore.</param>
        /// <returns>Instance of builder for method chaining.</returns>
        public PromoCodeBuilder WithTimeLimit (DateTime startDate, DateTime endDate) {
            _startDate = startDate;
            _endDate = endDate;
            return this;
        }

        /// <summary>
        /// Attach some custom data to the promo code.
        /// </summary>
        /// <param name="key">Data key.</param>
        /// <param name="value">Data value.</param>
        /// <returns>Instance of builder for method chaining.</returns>
        public PromoCodeBuilder AddData (string key, string value) {
            _data.Add (key, value);
            return this;
        }

        /// <summary>
        /// Attach some custom data to the promo code.
        /// </summary>
        /// <param name="data">Custom data.</param>
        /// <returns>Instance of builder for method chaining.</returns>
        public PromoCodeBuilder AddData (Dictionary<string, string> data) {
            _data.AddAll (data);
            return this;
        }

#if UNITY_ANDROID
        public AndroidJavaObject ToAjo () {
            var ajc = new AndroidJavaClass ("im.getsocial.sdk.promocodes.PromoCodeBuilder");
            var ajo = _code == null ? ajc.CallStaticAJO ("createRandomCode") : ajc.CallStaticAJO ("createWithCode", _code);
            ajo.CallAJO ("withMaxClaimCount", (int) _maxClaimCount)
                .CallAJO ("addData", _data.ToJavaHashMap ())
                .CallAJO ("withTimeLimit", DateToAjo (_startDate), DateToAjo (_endDate));
            return ajo;
        }

        private AndroidJavaObject DateToAjo (DateTime? date) {
            if (!date.HasValue) {
                return null;
            }
            return new AndroidJavaObject ("java.text.SimpleDateFormat", "dd/MM/yyyy HH:mm:ss").CallAJO("parse", ConvertToTimestamp(date.Value));
        }
#elif UNITY_IOS

        public string ToJson () {
            var json = new Dictionary<string, object> ()
            {
                {"Code", _code},
                {"Data", _data},
                {"MaxClaimCount", _maxClaimCount},
            };
            if (_startDate.HasValue) {
                json["StartDate"] = ConvertToTimestamp(_startDate.Value);
            }
            if (_endDate.HasValue) {
                json["EndDate"] = ConvertToTimestamp(_endDate.Value);
            }
            return GSJson.Serialize (json);
        }
#endif
        private static string ConvertToTimestamp(DateTime value)
        {
            return value.ToString("dd/MM/yyyy HH:mm:ss");
        }
    }
}
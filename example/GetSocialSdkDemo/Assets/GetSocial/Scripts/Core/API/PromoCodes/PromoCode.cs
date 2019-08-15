using System;
using System.Collections.Generic;
#if UNITY_ANDROID
using UnityEngine;
#endif

namespace GetSocialSdk.Core {
    public class PromoCode : IConvertableFromNative<PromoCode> {

        /// <summary>
        /// Promo code.
        /// </summary>
        public string Code { get; private set; }
        /// <summary>
        /// Attached custom data.
        /// </summary>
        public Dictionary<string, string> Data { get; private set; }
        /// <summary>
        /// Maximum number of claims. If `0` then no limits.
        /// </summary>
        public uint MaxClaimCount { get; private set; }
        /// <summary>
        /// Date when Promo Code becomes active.
        /// </summary>
        public DateTime StartDate { get; private set; }
        /// <summary>
        /// Date when Promo Code is not active anymore. Null if Promo Code is available forever until manually disabled on the Dashboard.
        /// </summary>
        public DateTime? EndDate { get; private set; }
        /// <summary>
        /// Creator basic info.
        /// </summary>
        public UserReference Creator { get; private set; }
        /// <summary>
        /// Current number of claims.
        /// </summary>
        public uint ClaimCount { get; private set; }
        /// <summary>
        /// Is enabled on the Dashboard.
        /// </summary>
        public bool Enabled { get; private set; }
        /// <summary>
        /// Is claimable. True only if Promo Code is enabled and active in the current time.
        /// </summary>
        public bool Claimable { get; private set; }
        public PromoCode () { }

        internal PromoCode (string code, Dictionary<string, string> data, uint maxClaimCount, DateTime startDate, DateTime? endDate, UserReference creator, uint claimCount, bool enabled, bool claimable) {
            this.Code = code;
            this.MaxClaimCount = maxClaimCount;
            this.StartDate = startDate;
            this.Creator = creator;
            this.ClaimCount = claimCount;
            this.Enabled = enabled;
            this.Claimable = claimable;
            this.Data = data;
            this.EndDate = endDate;
        }
#if UNITY_IOS
        public PromoCode ParseFromJson (Dictionary<string, object> json) {
            Code = json["Code"] as string;
            Data = (json["Data"] as Dictionary<string, object>).ToStrStrDict ();
            MaxClaimCount = (uint) (long) json["MaxClaimCount"];
            StartDate = DateFromObject (json["StartDate"]).Value;
            EndDate = DateFromObject (json["EndDate"]);
            Creator = new UserReference ().ParseFromJson (json["Creator"] as Dictionary<string, object>);
            ClaimCount = (uint) (long) json["ClaimCount"];
            Enabled = (bool) json["Enabled"];
            Claimable = (bool) json["Claimable"];
            return this;
        }

        private static DateTime? DateFromObject (object date) {
            if (date == null) {
                return null;
            }
            return new DateTime (1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)
                .AddSeconds ((long) date)
                .ToLocalTime ();

        }
#elif UNITY_ANDROID
        public PromoCode ParseFromAJO (AndroidJavaObject ajo) {
            using (ajo) {
                Code = ajo.CallStr ("getCode");
                Data = ajo.CallAJO ("getData").FromJavaHashMap ();
                MaxClaimCount = (uint) ajo.CallInt ("getMaxClaimCount");
                StartDate = DateFromAjo (ajo.CallAJO ("getStartDate")).Value;
                EndDate = DateFromAjo (ajo.CallAJO ("getEndDate"));
                Creator = new UserReference ().ParseFromAJO (ajo.CallAJO ("getCreator"));
                ClaimCount = (uint) ajo.CallInt ("getClaimCount");
                Enabled = ajo.CallBool ("isEnabled");
                Claimable = ajo.CallBool ("isClaimable");
            }
            return this;
        }

        private static DateTime? DateFromAjo (AndroidJavaObject ajo) {
            if (ajo.IsJavaNull ()) {
                return null;
            }
            return new DateTime (1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)
                .AddSeconds (ajo.CallLong ("getTime") / 1000)
                .ToLocalTime ();
        }
#endif
    }
}
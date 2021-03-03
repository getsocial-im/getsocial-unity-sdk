using System;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Describes an in-app purchase
    /// </summary>
    public sealed class PurchaseData
    {

        public enum ProductType
        {
            Item = 0,
            Subscription = 1
        }

        [JsonSerializationKey("productId")]
        public string ProductId;

        [JsonSerializationKey("productTitle")]
        public string ProductTitle;

        [JsonSerializationKey("productType")]
        public ProductType PurchaseType;

        [JsonSerializationKey("price")]
        public float Price;

        [JsonSerializationKey("priceCurrency")]
        public string PriceCurrency;

        public DateTime PurchaseDate
        {
            get
            {
                return DateUtils.FromUnixTime(_purchaseDate);
            }
            set
            {
                _purchaseDate = value.ToUnixTimestamp();
            }
        }
        [JsonSerializationKey("purchaseDate")]
        private long _purchaseDate;

        [JsonSerializationKey("purchaseId")]
        public string PurchaseId;
    }
}
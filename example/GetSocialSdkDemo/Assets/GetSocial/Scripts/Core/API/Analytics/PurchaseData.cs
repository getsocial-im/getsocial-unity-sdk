using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using GetSocialSdk.MiniJSON;
using UnityEngine;

namespace GetSocialSdk.Core.Analytics
{
    /// <summary>
    /// Describes an in-app purchase
    /// </summary>
    public sealed class PurchaseData : IConvertableToNative
    {
        
        public enum ProductType
        {
            Item = 0,
            Subscription = 1
        }

#pragma warning disable 414
        private string _productId;
        private string _productTitle;
        private ProductType _productType;
        private float _price;
        private string _priceCurrency;
        private DateTime _purchaseDate;
        private string _purchaseId;
#pragma warning restore 414

        private PurchaseData()
        {
            
        }

        public static Builder CreateBuilder()
        {
            return new Builder();
        }

        public class Builder
        {
            internal Builder()
            {
                
            }
            
            readonly PurchaseData _purchaseData = new PurchaseData();
            
            public Builder WithProductId(string productId)
            {
                _purchaseData._productId = productId;
                return this;
            }

            public Builder WithProductTitle(string productTitle)
            {
                _purchaseData._productTitle = productTitle;
                return this;
            }
            
            public Builder WithProductType(ProductType productType)
            {
                _purchaseData._productType = productType;
                return this;
            }
            
            public Builder WithPrice(float price)
            {
                _purchaseData._price = price;
                return this;
            }
            
            public Builder WithPriceCurrency(string priceCurrency)
            {
                _purchaseData._priceCurrency = priceCurrency;
                return this;
            }
            
            public Builder WithPurchaseDate(DateTime purchaseDate)
            {
                _purchaseData._purchaseDate = purchaseDate;
                return this;
            }
            
            public Builder WithPurchaseId(string purchaseId)
            {
                _purchaseData._purchaseId = purchaseId;
                return this;
            }

            public PurchaseData Build()
            {
                return _purchaseData;
            }

        }

#if UNITY_ANDROID
        public AndroidJavaObject ToAjo()
        {   
            var purchaseDataBuilderAjo = new AndroidJavaObject("im.getsocial.sdk.iap.PurchaseData$Builder");
            purchaseDataBuilderAjo.CallAJO("withProductId", _productId);
            purchaseDataBuilderAjo.CallAJO("withProductType", _productType.ToAndroidJavaObject());
            purchaseDataBuilderAjo.CallAJO("withPrice", _price);
            purchaseDataBuilderAjo.CallAJO("withPriceCurrency", _priceCurrency);
            var epoxTime = _purchaseDate.ToUniversalTime();
            var correctTime = epoxTime.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            purchaseDataBuilderAjo.CallAJO("withPurchaseDate", (long)correctTime);
            purchaseDataBuilderAjo.CallAJO("withProductTitle", _productTitle);
            purchaseDataBuilderAjo.CallAJO("withPurchaseId", _purchaseId);
            return purchaseDataBuilderAjo.CallAJO("build");
        }

#endif    

#if UNITY_IOS

        public string ToJson()
        {
            var json = new Dictionary<string, object>
            {
                {"ProductId", _productId},
                {"ProductType", _productType},
                {"Price", _price},
                {"PriceCurrency", _priceCurrency},
                {"PurchaseDate", _purchaseDate},
                {"ProductTitle", _productTitle},
                {"PurchaseId", _purchaseId}
            };
            return GSJson.Serialize(json);
        }

#endif
    }
}
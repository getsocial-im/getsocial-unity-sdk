using System;
using GetSocialSdk.Core;
using GetSocialSdk.Core.Analytics;
using UnityEngine;

namespace GetSocialDemo.Scripts.GUI.Sections
{
    public class InAppPurchaseApiSection : DemoMenuSection
    {

        private string _productId = "com.demo.unity.item";
        private string _productTitle = "Amazing item";
        private string _productTypeStr = "item";
        private string _priceStr = "1.65";
        private string _priceCurrency = "USD";
        
        protected override string GetTitle()
        {
            return "InApp Purchase Tracking";
        }

        protected override void DrawSectionBody()
        {
        
            GUILayout.BeginHorizontal();
            GUILayout.Label("Product Id", GSStyles.NormalLabelText, GUILayout.Width(Screen.width * 0.25f));
            _productId = GUILayout.TextField(_productId, GSStyles.TextField,
                GUILayout.Width(Screen.width * 0.75f));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Product title", GSStyles.NormalLabelText, GUILayout.Width(Screen.width * 0.25f));
            _productTitle = GUILayout.TextField(_productTitle, GSStyles.TextField,
                GUILayout.Width(Screen.width * 0.75f));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Product Type", GSStyles.NormalLabelText, GUILayout.Width(Screen.width * 0.25f));
            _productTypeStr = GUILayout.TextField(_productTypeStr, GSStyles.TextField,
                GUILayout.Width(Screen.width * 0.75f));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Price", GSStyles.NormalLabelText, GUILayout.Width(Screen.width * 0.25f));
            _priceStr = GUILayout.TextField(_priceStr, GSStyles.TextField,
                GUILayout.Width(Screen.width * 0.75f));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Price currency", GSStyles.NormalLabelText, GUILayout.Width(Screen.width * 0.25f));
            _priceCurrency = GUILayout.TextField(_priceCurrency, GSStyles.TextField,
                GUILayout.Width(Screen.width * 0.75f));
            GUILayout.EndHorizontal();
            
            DemoGuiUtils.DrawButton("Track purchase", TrackPurchase, true, GSStyles.Button);
        }

        private void TrackPurchase()
        {
            var builder = PurchaseData.CreateBuilder();
            builder.WithProductId(_productId);
            builder.WithProductTitle(_productTitle);
            builder.WithPriceCurrency(_priceCurrency);
            builder.WithPrice(float.Parse(_priceStr));
            if (_productTypeStr.Equals("item"))
            {
                builder.WithProductType(PurchaseData.ProductType.Item);
            }
            else
            {
                builder.WithProductType(PurchaseData.ProductType.Subscription);
            }
            builder.WithPurchaseDate(DateTime.Now);
            builder.WithPurchaseId(System.Guid.NewGuid().ToString());
            
            GetSocial.TrackPurchase(builder.Build(), () =>
            {
                _console.LogD("Purchase was tracked");
            }, error =>
            {
                _console.LogD("Purchase tracking failed, error: " + error);
            });
            
        }
    }
}
using System;
using GetSocialSdk.Core;
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
            if (_priceStr.Length == 0)
            {
                _console.LogD("Price cannot be empty");
                return;
            }
            float priceFloat = 0;
            try
            {
                priceFloat = float.Parse(_priceStr);
            }
            catch (Exception exception)
            {
                _console.LogD("Invalid price, error: " + exception);
                return;
            }

            var purchaseData = new PurchaseData();
            purchaseData.ProductId = _productId;
            purchaseData.ProductTitle = _productTitle;
            purchaseData.PriceCurrency = _priceCurrency;
            purchaseData.Price = priceFloat;
            if (_productTypeStr.Equals("item"))
            {
                purchaseData.PurchaseType = PurchaseData.ProductType.Item;
            }
            else
            {
                purchaseData.PurchaseType = PurchaseData.ProductType.Subscription;
            }
            purchaseData.PurchaseDate = DateTime.Now;
            purchaseData.PurchaseId = System.Guid.NewGuid().ToString();
            
            if (Analytics.TrackPurchase(purchaseData))
            {
                _console.LogD("Purchase was tracked.");
            } else 
            {
                _console.LogD("Purchase tracking failed.");
            };
            
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using GetSocialSdk.Core;
using GetSocialSdk.Ui;
using UnityEngine;

public class PromoCodesSection : DemoMenuSection {

    private const string MyPromoCodeKey = "my_promo_code";
    private bool _createPromoCode = false;

    private string _promoCode = "";

    private string _code = "";
    private string _maxClaimCount = "0";
    private string _startDate = "";
    private string _endDate = "";
    private readonly List<Data> _data = new List<Data>();

    private bool _loading = false;

    public PromoCodesSection () { }
    
    protected override string GetTitle () {
        return "Promo Codes";
    }

    protected override void DrawSectionBody () {
        if (_createPromoCode) {
            DrawCreatePromoCodeBody();
            return;
        }
        DemoGuiUtils.DrawButton ("My Promo Code", ShowMyPromoCode, style : GSStyles.Button);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Promo code: ", GSStyles.NormalLabelText);
        _promoCode = GUILayout.TextField(_promoCode, GSStyles.TextField);
        GUILayout.EndHorizontal();
        DemoGuiUtils.DrawButton ("Claim Promo Code", ClaimPromoCode, style : GSStyles.Button);
        DemoGuiUtils.DrawButton ("Get Promo Code", GetPromoCode, style : GSStyles.Button);
        DemoGuiUtils.DrawButton ("Create Promo Code", () => {_createPromoCode = true; _loading = false;}, style : GSStyles.Button);
    }

    private void DrawCreatePromoCodeBody() {
        GUILayout.Label("Promo code(leave empty for random): ", GSStyles.NormalLabelText);
        _code = GUILayout.TextField(_code, GSStyles.TextField);

        GUILayout.Label("Max Claim Count(0 or empty for unlim): ", GSStyles.NormalLabelText);
        _maxClaimCount = GUILayout.TextField(_maxClaimCount, GSStyles.TextField);
        _maxClaimCount = Regex.Replace(_maxClaimCount, @"[^0-9]", "");

        GUILayout.Label("Start date('dd/MM/yyyy HH:mm:ss' or 'dd/MM/yyy'): ", GSStyles.NormalLabelText);
        _startDate = GUILayout.TextField(_startDate, GSStyles.TextField);
        
        GUILayout.Label("End date('dd/MM/yyyy HH:mm:ss' or 'dd/MM/yyy'): ", GSStyles.NormalLabelText);
        _endDate = GUILayout.TextField(_endDate, GSStyles.TextField);

        DemoGuiUtils.DynamicRowFor(_data, "Custom data");

        DemoGuiUtils.DrawButton ("Create", () => {
            if (_loading) {
                return;
            }
            _loading = true;
            var builder = (_code == null || _code.Length > 0 ? PromoCodeBuilder.CreateWithCode(_code) : PromoCodeBuilder.CreateRandomCode())
                    .AddData(_data.ToDictionary(data => data.Key, data => data.Val))
                    .WithMaxClaimCount(Convert.ToUInt32(_maxClaimCount));
            var startDate = ParseDate(_startDate);
            var endDate = ParseDate(_endDate);
            if (startDate.HasValue && endDate.HasValue) {
                Debug.LogFormat("{0} => {1}", startDate, endDate);
                builder.WithTimeLimit(startDate.Value, endDate.Value);
            }
            GetSocial.CreatePromoCode(builder,
                promoCode => {
                    _loading = false;
                    ShowFullInfo(promoCode);
                }, error => {
                    _loading = false;
                    ShowAlert("Create Promo Code", "Failed to create promo code: " + error.Message);
                    
                    _console.LogE (string.Format ("Failed to create promo code: {0}", error.Message), showImmediately : false);
                }
            );
        }, style : GSStyles.Button);

        DemoGuiUtils.DrawButton ("Back", () => {
            _createPromoCode = false;
        }, style : GSStyles.Button);
    }

    private static DateTime? ParseDate(string date) {
        if (date.Length == 0) {
            return null;
        }
        DateTime dateTime;
        CultureInfo provider = CultureInfo.InvariantCulture;  
        
        if (DateTime.TryParseExact(date, "dd/MM/yyyy HH:mm:ss", provider, new DateTimeStyles(), out dateTime) 
            || DateTime.TryParseExact(date, "dd/MM/yyyy", provider, new DateTimeStyles(), out dateTime)) {
            return dateTime;
        } else {
            return null;
        }
    }

    private void GetPromoCode()
    {
        GetSocial.GetPromoCode(_promoCode, promoCode => {
            ShowFullInfoWithoutActions(promoCode, "Get Promo Code");
        }, error => {
            ShowAlert("Get Promo Code", "Failed to get promo code: " + error.Message);
            
            _console.LogE (string.Format ("Failed to get promo code: {0}", error.Message), showImmediately : false);
        });
    }

    private void ClaimPromoCode()
    {
        ClaimPromoCode(_promoCode);
    }

    public static void ClaimPromoCode(string code) 
    {
        GetSocial.ClaimPromoCode(code, promoCode => {
            ShowFullInfoWithoutActions(promoCode, "Claim Promo Code");
        }, error => {
            ShowAlert("Claim Promo Code", "Failed to claim promo code: " + error.Message);
        });
    }

    private void ShowMyPromoCode () {
        if (GetSocial.User.HasPrivateProperty (MyPromoCodeKey)) {
            ShowPromoCode (GetSocial.User.GetPrivateProperty (MyPromoCodeKey));
        } else {
            GetSocial.CreatePromoCode (PromoCodeBuilder.CreateRandomCode ().AddData (MyPromoCodeKey, "true"), promoCode => {
                GetSocial.User.SetPrivateProperty(MyPromoCodeKey, promoCode.Code, ShowMyPromoCode,
                error => _console.LogE (string.Format ("Failed to set private property: {0}", error.Message), showImmediately : false));
            }, error => _console.LogE (string.Format ("Failed to create promo code: {0}", error.Message), showImmediately : false));
        }
    }

    private void ShowPromoCode (string promoCode) {
        var popup = new MNPopup ("Promo Code Info", promoCode);
        popup.AddAction("Share", () => SharePromoCode(promoCode));
        popup.AddAction("Info", () => LoadInfoAndShow(promoCode));
        popup.AddAction("Cancel", () => { });
        popup.Show();   
    }

    private void LoadInfoAndShow(string code)
    {
        GetSocial.GetPromoCode(code, promoCode => {
            ShowFullInfo(promoCode);
        }, error => {
            ShowAlert("Promo Code Info", "Failed to get promo code: " + error.Message);
            
            _console.LogE (string.Format ("Failed to get promo code: {0}", error.Message), showImmediately : false);
        });
    }

    private static void ShowFullInfo(PromoCode promoCode)
    {
        var code = promoCode.Code;
        var popup = new MNPopup ("Promo Code Info", FormatInfo(promoCode));
        popup.AddAction("Share", () => SharePromoCode(code));
        popup.AddAction("Copy", () => GUIUtility.systemCopyBuffer = code);
        popup.AddAction("Cancel", () => { });
        popup.Show();   
    }
    
    private static void ShowFullInfoWithoutActions(PromoCode promoCode, string title)
    {
        var code = promoCode.Code;
        var popup = new MNPopup (title, FormatInfo(promoCode));
        popup.AddAction("OK", () => { });
        popup.Show();   
    }
    

    private static string FormatInfo(PromoCode promoCode)
    {
        return string.Format(@"promoCode={0}
data={1}
maxClaim={2}
claimCount={3}
startDate={4}
endDate={5}
enabled={6}
claimable={7}
creator={8}
        ", promoCode.Code, promoCode.Data.ToDebugString(), promoCode.MaxClaimCount, promoCode.ClaimCount,
        promoCode.StartDate, promoCode.EndDate, promoCode.Enabled, promoCode.Claimable, 
        promoCode.Creator.DisplayName);
    }

    private static void SharePromoCode(string promoCode)
    {
        GetSocialUi.CreateInvitesView()
            .SetLinkParams(new LinkParams {
                {LinkParams.KeyPromoCode, promoCode}
            })
            .SetCustomInviteContent(InviteContent.CreateBuilder()
                .WithText("Use my Promo Code to get a personal discount: " + InviteTextPlaceholders.PlaceholderPromoCode + " . " + InviteTextPlaceholders.PlaceholderAppInviteUrl)
                .Build())
            .Show();
    }

    private static void ShowAlert(string title, string message) {
        var popup = new MNPopup (title, message);
        popup.AddAction("OK", () => {});
        popup.Show();
    }

    private class Data : DemoGuiUtils.IDrawableRow
    {
        public string Key = "";
        public string Val = "";
        
        public void Draw()
        {
            Key = GUILayout.TextField(Key, GSStyles.TextField);
            Val = GUILayout.TextField(Val, GSStyles.TextField);
        }
    }
}
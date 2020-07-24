using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Facebook.Unity.Example;
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

        DemoGuiUtils.DrawButton("Create", () => {
            if (_loading) {
                return;
            }
            _loading = true;
            var promoCode = (_code == null || _code.Length > 0 ? PromoCodeContent.CreateWithCode(_code) : PromoCodeContent.CreateRandomCode());
            promoCode.AddProperties(_data.ToDictionary(data => data.Key, data => data.Val));
            promoCode.MaxClaimCount = Convert.ToUInt32(_maxClaimCount);
            var startDate = ParseDate(_startDate);
            var endDate = ParseDate(_endDate);
            if (startDate.HasValue && endDate.HasValue) {
                Debug.LogFormat("{0} => {1}", startDate, endDate);
                promoCode.SetTimeLimit(startDate.Value, endDate.Value);
            }
            PromoCodes.Create(promoCode,
                code => {
                    _loading = false;
                    ShowFullInfoWithoutActions(code);
                }, error => {
                    _loading = false;
                    _console.LogE (string.Format ("Failed to create promo code: {0}", error.Message));
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
        PromoCodes.Get(_promoCode, promoCode => {
            ShowFullInfoWithoutActions(promoCode);
        }, error => {
            _console.LogE (string.Format ("Failed to get promo code: {0}", error.Message));
        });
    }

    private void ClaimPromoCode()
    {
        ClaimPromoCode(_promoCode);
    }

    public void ClaimPromoCode(string code) 
    {
        PromoCodes.Claim(code, promoCode => {
            ShowFullInfoWithoutActions(promoCode);
        }, error => {
            _console.LogE (string.Format ("Failed to claim promo code: {0}", error.Message));
        });
    }

    private void ShowMyPromoCode () {
        if (GetSocial.GetCurrentUser().PrivateProperties.ContainsKey(MyPromoCodeKey)) {
            ShowPromoCode (GetSocial.GetCurrentUser().PrivateProperties[MyPromoCodeKey]);
        } else {
            PromoCodes.Create (PromoCodeContent.CreateRandomCode ().AddProperty (MyPromoCodeKey, "true"), promoCode => {
                var userUpdate = new UserUpdate().AddPrivateProperty(MyPromoCodeKey, promoCode.Code);
                GetSocial.GetCurrentUser().UpdateDetails(userUpdate, ShowMyPromoCode,
                error => _console.LogE (string.Format ("Failed to set private property: {0}", error.Message), showImmediately : false));
            }, error => _console.LogE (string.Format ("Failed to create promo code: {0}", error.Message), showImmediately : false));
        }
    }

    private void ShowPromoCode (string promoCode) {
        var popup = Dialog().WithTitle(promoCode);
        popup.AddAction("Share", () => SharePromoCode(promoCode));
        popup.AddAction("Copy", () => GUIUtility.systemCopyBuffer = promoCode);
        popup.AddAction("Info", () => LoadInfoAndShow(promoCode));
        popup.AddAction("Cancel", () => { });
        popup.Show();   
    }

    private void LoadInfoAndShow(string code)
    {
        PromoCodes.Get(code, promoCode => {
            ShowFullInfoWithoutActions(promoCode);
        }, error => {
            _console.LogE (string.Format ("Failed to get promo code: {0}", error.Message));
        });
    }

    private void ShowFullInfoWithoutActions(PromoCode promoCode)
    {
        _console.LogD(promoCode.ToString());
    }

    private static void SharePromoCode(string promoCode)
    {
        var inviteContent = new InviteContent()
        {
            Text = "Use my Promo Code to get a personal discount: " + InviteTextPlaceholders.PlaceholderPromoCode + " . " + InviteTextPlaceholders.PlaceholderAppInviteUrl
        };
        // FIXME: use proper constants
        inviteContent.AddLinkParam(LinkParams.KeyPromoCode, promoCode);
        //todo
        InvitesViewBuilder.Create().SetCustomInviteContent(inviteContent).Show();
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
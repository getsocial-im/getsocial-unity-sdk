using System;

namespace GetSocialSdk.Core
{
    public static class PromoCodes
    {
        public static void Create(PromoCodeContent content, Action<PromoCode> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.CreatePromoCode(content, success, failure);
        }

        public static void Get(string code, Action<PromoCode> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.GetPromoCode(code, success, failure);
        }

        public static void Claim(string code, Action<PromoCode> success, Action<GetSocialError> failure)
        {
            GetSocialFactory.Bridge.ClaimPromoCode(code, success, failure);
        }
    }
}
using System;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Invite package containing the invite data.
    /// </summary>
    public sealed class InvitePackage : IGetSocialBridgeObject<InvitePackage>
    {
        /// <summary>
        /// Gets the invite subject.
        /// </summary>
        /// <value>The invite subject.</value>
        public string Subject { get; private set; }

        /// <summary>
        /// Gets the invite text.
        /// </summary>
        /// <value>The invite text.</value>
        public string Text { get; private set; }

        /// <summary>
        /// Gets the name of the user that sent the invite.
        /// </summary>
        /// <value>The name of the user that sent the invite.</value>
        public string UserName { get; private set; }

        /// <summary>
        /// Gets the referral data URL.
        /// </summary>
        /// <value>The referral data URL.</value>
        public string ReferralDataUrl { get; private set; }

        /// <summary>
        /// Gets the image of the invite.
        /// </summary>
        /// <value>The invite image.</value>
        public UnityEngine.Texture2D Image { get; private set; }

        /// <summary>
        /// Gets the url of the invite image.
        /// </summary>
        /// <value>The url of invite image.</value>
        public string ImageUrl { get; private set; }

        public override string ToString()
        {
            return string.Format(
                "[InvitePackage: Subject={0}, Text={1}, UserName={2}, HasImage={3}, ImageUrl={4}, ReferralDataUrl={5}]",
                Subject, Text, UserName, Image != null, ImageUrl, ReferralDataUrl);
        }

#if UNITY_ANDROID
        public UnityEngine.AndroidJavaObject ToAJO()
        {
            throw new NotImplementedException("This object is never passed to Android");
        }

        public InvitePackage ParseFromAJO(UnityEngine.AndroidJavaObject ajo)
        {
            JniUtils.CheckIfClassIsCorrect(ajo, "InvitePackage");

            using (ajo)
            {
                Subject = ajo.CallStr("getSubject");
                Text = ajo.CallStr("getText");
                UserName = ajo.CallStr("getUserName");
                ReferralDataUrl = ajo.CallStr("getReferralUrl");
                Image = ajo.CallAJO("getImage").FromAndroidBitmap();
                ImageUrl = ajo.CallStr("getImageUrl");
            }
            return this;
        }
#elif UNITY_IOS

        public string ToJson()
        {
            throw new NotImplementedException("This object is never passed to iOS");
        }

        public InvitePackage ParseFromJson(string json)
        {
            var dict = json.ToDict();
            Subject = dict[SubjectFieldName] as string;
            Text = dict[TextFieldName] as string;
            UserName = dict[UserNameFieldName] as string;
            ReferralDataUrl = dict[ReferralDataUrlFieldName] as string;
            Image = (dict[ImageFieldName] as string).FromBase64();
            ImageUrl = dict[ImageUrlFieldName] as string;
            return this;
        }

        static string SubjectFieldName
        {
            get { return ReflectionUtils.GetMemberName((InvitePackage c) => c.Subject); }
        }

        static string TextFieldName
        {
            get { return ReflectionUtils.GetMemberName((InvitePackage c) => c.Text); }
        }

        static string UserNameFieldName
        {
            get { return ReflectionUtils.GetMemberName((InvitePackage c) => c.UserName); }
        }

        static string ReferralDataUrlFieldName
        {
            get { return ReflectionUtils.GetMemberName((InvitePackage c) => c.ReferralDataUrl); }
        }

        static string ImageFieldName
        {
            get { return ReflectionUtils.GetMemberName((InvitePackage c) => c.Image); }
        }

        static string ImageUrlFieldName
        {
            get { return ReflectionUtils.GetMemberName((InvitePackage c) => c.ImageUrl); }
        }

#endif
    }
}
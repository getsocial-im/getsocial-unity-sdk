using System.Collections.Generic;
using GetSocialSdk.MiniJSON;
using UnityEngine;

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Invite content being sent along with smart invite.
    /// </summary>
    public sealed class InviteContent : IGetSocialBridgeObject<InviteContent>
    {
        public static Builder CreateBuilder()
        {
            return new Builder();
        }

        /// <summary>
        /// Gets the image URL.
        /// </summary>
        /// <value>The image URL.</value>
        public string ImageUrl { get; private set; }

        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <value>Invite content image.</value>
        public Texture2D Image { get; private set; }

        /// <summary>
        /// Gets the subject of ivite.
        /// </summary>
        /// <value>The subject of invite.</value>
        public string Subject { get; private set; }

        /// <summary>
        /// Gets the invite text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; private set; }

        public override string ToString()
        {
            return string.Format("[InviteContent: ImageUrl={0}, Subject={1}, Text={2}, HasImage={3}]", ImageUrl, Subject, Text, Image != null);
        }

        /// <summary>
        /// Builder to create <see cref="InviteContent" instance/>.
        /// </summary>
        public class Builder
        {
            readonly InviteContent _inviteContent;

            protected internal Builder()
            {
                _inviteContent = new InviteContent();
            }

            /// <summary>
            /// Sets the invite subject.
            /// </summary>
            /// <param name="subject">Invite subject.</param>
            /// <returns>The builder instance.</returns>
            public Builder WithSubject(string subject)
            {
                _inviteContent.Subject = subject;
                return this;
            }

            /// <summary>
            /// Sets the invite text.
            /// </summary>
            /// <param name="text">Invite text.</param>
            /// <returns>The builder instance.</returns>
            public Builder WithText(string text)
            {
                _inviteContent.Text = text;
                return this;
            }

            /// <summary>
            /// Sets the invite image url. Can not be used with <see cref="WithImage"/>.
            /// </summary>
            /// <param name="imageUrl">Invite image url.</param>
            /// <returns>The builder instance.</returns>
            public Builder WithImageUrl(string imageUrl)
            {
                _inviteContent.ImageUrl = imageUrl;
                _inviteContent.Image = null;
                return this;
            }

            /// <summary>
            /// Sets the invite image. Can not be used with <see cref="WithImageUrl"/>.
            /// </summary>
            /// <param name="image">Invite image</param>
            /// <returns>The builder instance.</returns>
            public Builder WithImage(Texture2D image)
            {
                _inviteContent.Image = image;
                _inviteContent.ImageUrl = null;
                return this;
            }

            /// <summary>
            /// Build this instance.
            /// </summary>
            public InviteContent Build()
            {
                return new InviteContent
                {
                    ImageUrl = _inviteContent.ImageUrl,
                    Image = _inviteContent.Image,
                    Subject = _inviteContent.Subject,
                    Text = _inviteContent.Text
                };
            }
        }


#if UNITY_ANDROID
        public UnityEngine.AndroidJavaObject ToAJO()
        {
            var inviteContentBuilderAJO = new UnityEngine.AndroidJavaObject("im.getsocial.sdk.invites.InviteContent$Builder");

            if (Subject != null)
            {
                inviteContentBuilderAJO.CallAJO("withSubject", Subject);
            }
            if (ImageUrl != null)
            {
                inviteContentBuilderAJO.CallAJO("withImageUrl", ImageUrl);
            }
            if (Image != null)
            {
                inviteContentBuilderAJO.CallAJO("withImage", Image.ToAjoBitmap());
            }
            if (Text != null)
            {
                inviteContentBuilderAJO.CallAJO("withText", Text);
            }
            return inviteContentBuilderAJO.CallAJO("build");
        }

        public InviteContent ParseFromAJO(AndroidJavaObject ajo)
        {
            using (ajo)
            {
                ImageUrl = ajo.CallStr("getImageUrl");

                var subjectAjo = ajo.CallAJO("getSubject");
                Subject = subjectAjo.IsJavaNull() ? null : subjectAjo.FromLocalizableText();

                var textAjo = ajo.CallAJO("getText");
                Text = textAjo.IsJavaNull() ? null : textAjo.FromLocalizableText();

                var image = ajo.CallAJO("getImage");
                Image = image.IsJavaNull() ? null : image.FromAndroidBitmap();
            }
            return this;
        }
#elif UNITY_IOS
        public string ToJson()
        {
            var jsonDic = new Dictionary<string, object>
            {
                {SubjectFieldName, Subject},
                {TextFieldName, Text},
                {ImageUrlFieldName, ImageUrl},
                {ImageFieldName, Image.TextureToBase64()}
            };
            return GSJson.Serialize(jsonDic);
        }
        
        public InviteContent ParseFromJson(Dictionary<string, object> jsonDic)
        {
            Subject = jsonDic[SubjectFieldName] as string;
            Text = jsonDic[TextFieldName] as string;
            ImageUrl = jsonDic[ImageUrlFieldName] as string;
            Image = (jsonDic[ImageFieldName] as string).FromBase64();

            return this;
        }

        static string SubjectFieldName
        {
            get { return ReflectionUtils.GetMemberName((InviteContent c) => c.Subject); }
        }

        static string TextFieldName
        {
            get { return ReflectionUtils.GetMemberName((InviteContent c) => c.Text); }
        }

        static string ImageUrlFieldName
        {
            get { return ReflectionUtils.GetMemberName((InviteContent c) => c.ImageUrl); }
        }

        static string ImageFieldName
        {
            get { return ReflectionUtils.GetMemberName((InviteContent c) => c.Image); }
        }
#endif
    }
}
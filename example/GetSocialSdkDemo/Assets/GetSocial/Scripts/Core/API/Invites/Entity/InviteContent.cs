using System;
using UnityEngine;

#if UNITY_IOS
using System.Collections.Generic;
using GetSocialSdk.MiniJSON;
#endif

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Invite content being sent along with smart invite.
    /// </summary>
    public sealed class InviteContent : IConvertableToNative
    {
        public static Builder CreateBuilder()
        {
            return new Builder();
        }

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
        
        /// <summary>
        /// Gets the image URL.
        /// </summary>
        /// <value>The image URL.</value>
        [Obsolete]
        public string ImageUrl 
        {
            get { return null; }
        }
        
        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <value>Invite content image.</value>
        [Obsolete]
        public Texture2D Image 
        {
            get { return null; }
        }
        
        /// <summary>
        /// Gets the video content.
        /// </summary>
        /// <value>Invite video content.</value>
        [Obsolete]
        public byte[] Video
        {
            get { return null; }
        }

        private MediaAttachment _mediaAttachment;

        public override string ToString()
        {
            return string.Format("[InviteContent: Subject={0}, Text={1}, HasAttachment={2}]", Subject, Text, _mediaAttachment != null);
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
            /// Sets the invite image url.
            /// </summary>
            /// <param name="imageUrl">Invite image url.</param>
            /// <returns>The builder instance.</returns>
            [Obsolete("Use WithMediaAttachment instead")]
            public Builder WithImageUrl(string imageUrl)
            {
                return WithMediaAttachment(MediaAttachment.ImageUrl(imageUrl));
            }

            /// <summary>
            /// Sets the invite image. 
            /// </summary>
            /// <param name="image">Invite image</param>
            /// <returns>The builder instance.</returns>
            [Obsolete("Use WithMediaAttachment instead")]
            public Builder WithImage(Texture2D image)
            {
                return WithMediaAttachment(MediaAttachment.Image(image));
            }

            /// <summary>
            /// Sets the invite video.
            /// </summary>
            /// <param name="videoBytes">Invite video</param>
            /// <returns>The builder instance.</returns>
            [Obsolete("Use WithMediaAttachment instead")]
            public Builder WithVideo(byte[] videoBytes)
            {
                return WithMediaAttachment(MediaAttachment.Video(videoBytes));
            }

            /// <summary>
            /// Add media attachment.
            /// </summary>
            /// <param name="mediaAttachment">Media attachment.</param>
            /// <returns>The builder instance.</returns>
            public Builder WithMediaAttachment(MediaAttachment mediaAttachment)
            {
                _inviteContent._mediaAttachment = mediaAttachment;
                return this;
            }

            /// <summary>
            /// Build this instance.
            /// </summary>
            public InviteContent Build()
            {
                return new InviteContent
                {
                    Subject = _inviteContent.Subject,
                    Text = _inviteContent.Text,
                    _mediaAttachment = _inviteContent._mediaAttachment
                };
            }
        }


#if UNITY_ANDROID
        public AndroidJavaObject ToAjo()
        {
            var inviteContentBuilderAJO = new AndroidJavaObject("im.getsocial.sdk.invites.InviteContent$Builder");

            if (Subject != null)
            {
                inviteContentBuilderAJO.CallAJO("withSubject", Subject);
            }
            if (Text != null)
            {
                inviteContentBuilderAJO.CallAJO("withText", Text);
            }
            if (_mediaAttachment != null)
            {
                inviteContentBuilderAJO.CallAJO("withMediaAttachment", _mediaAttachment.ToAjo());
            }
            return inviteContentBuilderAJO.CallAJO("build");
        }
#elif UNITY_IOS
        public string ToJson()
        {
            var jsonDic = new Dictionary<string, object>
            {
                {"Subject", Subject},
                {"Text", Text},
                {"MediaAttachment", _mediaAttachment == null ? "" : _mediaAttachment.ToJson()}
            };
            return GSJson.Serialize(jsonDic);
        }
#endif
    }
}
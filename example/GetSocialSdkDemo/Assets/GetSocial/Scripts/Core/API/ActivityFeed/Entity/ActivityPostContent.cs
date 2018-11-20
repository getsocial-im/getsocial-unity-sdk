using System;
using UnityEngine;
#if UNITY_IOS
using System.Collections.Generic;
using GetSocialSdk.MiniJSON;
#endif

namespace GetSocialSdk.Core
{

    /// <summary>
    ///
    /// </summary>
    public sealed class ActivityPostContent : IConvertableToNative
    {
#pragma warning disable 414
        string _text;
        string _buttonTitle;
        string _buttonAction;
        MediaAttachment _mediaAttachment;
        
#pragma warning restore 414
        ActivityPostContent()
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

            readonly ActivityPostContent _content = new ActivityPostContent();

            public Builder WithText(string text)
            {
                _content._text = text;
                return this;
            }

            [Obsolete("Use WithMediaAttachment instead")]
            public Builder WithImage(Texture2D image)
            {
                return WithMediaAttachment(MediaAttachment.Image(image));
            }

            public Builder WithButton(String title, String action)
            {
                _content._buttonTitle = title;
                _content._buttonAction = action;
                return this;
            }

            [Obsolete("Use WithMediaAttachment instead")]
            public Builder WithVideo(byte[] video)
            {
                return WithMediaAttachment(MediaAttachment.Video(video));
            }

            public Builder WithMediaAttachment(MediaAttachment mediaAttachment)
            {
                _content._mediaAttachment = mediaAttachment;
                return this;
            }

            public ActivityPostContent Build()
            {
                return _content;
            }
        }

#if UNITY_ANDROID
        public AndroidJavaObject ToAjo()
        {
            
            var activityPostContentBuilderAJO = new AndroidJavaObject("im.getsocial.sdk.activities.ActivityPostContent$Builder");

            if (_text != null)
            {
                activityPostContentBuilderAJO.CallAJO("withText", _text);
            }
            if (_buttonAction != null && _buttonTitle != null)
            {
                activityPostContentBuilderAJO.CallAJO("withButton", _buttonTitle, _buttonAction);
            }
            if (_mediaAttachment != null)
            {
                activityPostContentBuilderAJO.CallAJO("withMediaAttachment", _mediaAttachment.ToAjo());
            }
            return activityPostContentBuilderAJO.CallAJO("build");
        }
        
#elif UNITY_IOS

        public string ToJson()
        {
            var json = new Dictionary<string, object>
            {
                {"Text", _text},
                {"ButtonTitle", _buttonTitle},
                {"ButtonAction", _buttonAction},
                {"MediaAttachment", _mediaAttachment == null ? "" : _mediaAttachment.ToJson()}
            };
            return GSJson.Serialize(json);
        }

#endif
    }
}
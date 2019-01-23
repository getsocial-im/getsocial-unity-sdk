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
        GetSocialAction _action;
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

            [Obsolete("Use WithButton(string, GetSocialAction) instead")]
            public Builder WithButton(string title, string action)
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

            public Builder WithButton(string title, GetSocialAction action)
            {
                _content._buttonTitle = title;
                _content._action = action;
                return this;
            }

            public ActivityPostContent Build()
            {
                return new ActivityPostContent
                {
                    _buttonAction = _content._buttonAction,
                    _buttonTitle = _content._buttonTitle,
                    _action = _content._action,
                    _text = _content._text,
                    _mediaAttachment = _content._mediaAttachment
                };
            }
        }

#if UNITY_ANDROID
        public AndroidJavaObject ToAjo()
        {
            var activityPostContentBuilderAjo = new AndroidJavaObject("im.getsocial.sdk.activities.ActivityPostContent$Builder");

            if (_text != null)
            {
                activityPostContentBuilderAjo.CallAJO("withText", _text);
            }
            if (_buttonTitle != null)
            {
                if (_buttonAction != null)
                {
                    activityPostContentBuilderAjo.CallAJO("withButton", _buttonTitle, _buttonAction);
                }
                if (_action != null)
                {
                    activityPostContentBuilderAjo.CallAJO("withButton", _buttonTitle, _action.ToAjo());
                }
            }
            if (_mediaAttachment != null)
            {
                activityPostContentBuilderAjo.CallAJO("withMediaAttachment", _mediaAttachment.ToAjo());
            }
            return activityPostContentBuilderAjo.CallAJO("build");
        }
        
#elif UNITY_IOS

        public string ToJson()
        {
            var json = new Dictionary<string, object>
            {
                {"Text", _text},
                {"ButtonTitle", _buttonTitle},
                {"ButtonAction", _buttonAction},
                {"Action", _action == null ? "" : _action.ToJson()},
                {"MediaAttachment", _mediaAttachment == null ? "" : _mediaAttachment.ToJson()}
            };
            return GSJson.Serialize(json);
        }

#endif
    }
}
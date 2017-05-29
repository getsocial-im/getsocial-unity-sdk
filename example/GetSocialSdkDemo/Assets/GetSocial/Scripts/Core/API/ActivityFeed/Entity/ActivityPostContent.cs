using System;
using System.Collections.Generic;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    using UnityEngine;

    /// <summary>
    ///
    /// </summary>
    public sealed class ActivityPostContent : IGetSocialBridgeObject<ActivityPostContent>
    {
        string _text;

        Texture2D _image;

        string _buttonTitle;
        string _buttonAction;

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

            public Builder WithImage(Texture2D image)
            {
                _content._image = image;
                return this;
            }

            public Builder WithButton(String title, String action)
            {
                _content._buttonTitle = title;
                _content._buttonAction = action;
                return this;
            }

            public ActivityPostContent Build()
            {
                return _content;
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }

#if UNITY_ANDROID
        public UnityEngine.AndroidJavaObject ToAJO()
        {
            var activityPostContentBuilderAJO = new AndroidJavaObject("im.getsocial.sdk.activities.ActivityPostContent$Builder");

            if (_text != null)
            {
                activityPostContentBuilderAJO.CallAJO("withText", _text);
            }
            if (_image != null)
            {
                activityPostContentBuilderAJO.CallAJO("withImage", _image.ToAjoBitmap());
            }
            if (_buttonAction != null && _buttonTitle != null)
            {
                activityPostContentBuilderAJO.CallAJO("withButton", _buttonTitle, _buttonAction);
            }
            return activityPostContentBuilderAJO.CallAJO("build");
        }

        public ActivityPostContent ParseFromAJO(AndroidJavaObject ajo)
        {
            throw new NotImplementedException();
        }
#elif UNITY_IOS

        public string ToJson()
        {
            var jsonDic = new Dictionary<string, object>
            {
                {"Text", _text},
                {"ButtonTitle", _buttonTitle},
                {"ButtonAction", _buttonAction},
                {"Image", _image.TextureToBase64()}
            };
            return GSJson.Serialize(jsonDic);
        }

        public ActivityPostContent ParseFromJson(string json)
        {
            return this;
        }

#endif
    }
}
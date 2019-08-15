using System.Collections.Generic;
using UnityEngine;

#if UNITY_IOS
using GetSocialSdk.MiniJSON;
#endif

namespace GetSocialSdk.Core
{
    public sealed class MediaAttachment : IConvertableToNative
    {       
#pragma warning disable 414   
        internal  readonly string _method;
        internal  readonly object _object;
#pragma warning restore 414

        private MediaAttachment(string method, object obj)
        {
            _method = method;
            _object = obj;
        }
        
        public static MediaAttachment Image(Texture2D texture)
        {
            return texture == null ? null : new MediaAttachment("image", Convert(texture));
        }
        
        public static MediaAttachment ImageUrl(string imageUrl)
        {
            return imageUrl == null ? null : new MediaAttachment("imageUrl", imageUrl);
        }
        
        public static MediaAttachment Video(byte[] video)
        {
            return video == null ? null : new MediaAttachment("video", Convert(video));
        }
        
        public static MediaAttachment VideoUrl(string videoUrl)
        {
            return videoUrl == null ? null : new MediaAttachment("videoUrl", videoUrl);
        }

#if UNITY_ANDROID
        public AndroidJavaObject ToAjo()
        {
            return new AndroidJavaClass("im.getsocial.sdk.media.MediaAttachment").CallStaticAJO(_method, _object);
        }
#elif UNITY_IOS
        public string ToJson()
        {
            var dictionary = new Dictionary<string, string>();
            dictionary[_method] = _object as string;
            return GSJson.Serialize(dictionary);
        }
#endif
        
        private static object Convert(Texture2D texture)
        {
#if UNITY_ANDROID
            return texture.ToAjoBitmap();
#elif UNITY_IOS
            return texture.TextureToBase64();
#else
            return texture;
#endif
        }
        private static object Convert(byte[] video)
        {
#if UNITY_ANDROID
            return video;
#elif UNITY_IOS
            return video.ByteArrayToBase64();
#else
            return video;
#endif
        }
    }
}
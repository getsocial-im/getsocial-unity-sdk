using System.Collections.Generic;
using GetSocialSdk.MiniJSON;
using UnityEngine;

namespace GetSocialSdk.Core
{
    public sealed class MediaAttachment
    {
        [JsonSerializationKey("imageUrl")]
        public string ImageUrl { get; internal set; }

        public Texture2D Image {
            get {
                return _imageBase64.FromBase64();
            }
        }

        [JsonSerializationKey("gifUrl")]
        public string GifUrl { get; internal set; }

        [JsonSerializationKey("videoUrl")]
        public string VideoUrl { get; internal set; }

        [JsonSerializationKey("image")]
        internal string _imageBase64;

        [JsonSerializationKey("video")]
        internal string _videoDataBase64;

        internal MediaAttachment() { }

        public static MediaAttachment WithImage(Texture2D texture)
        {
            if (texture == null)
            {
                return null;
            }
            var bitmap = Convert(texture);
            if (bitmap == null)
            {
                return null;
            }
            return new MediaAttachment
            {
                _imageBase64 = bitmap
            };
        }
        
        public static MediaAttachment WithImageUrl(string imageUrl)
        {
            return new MediaAttachment
            {
                ImageUrl = imageUrl
            };
        }
        
        public static MediaAttachment WithVideo(byte[] video)
        {
            var bitmap = Convert(video);
            if (bitmap == null)
            {
                return null;
            }
            return new MediaAttachment
            {
                _videoDataBase64 = bitmap
            };
        }
        
        public static MediaAttachment WithVideoUrl(string videoUrl)
        {
            return new MediaAttachment
            {
                VideoUrl = videoUrl
            };
        }

        private static string Convert(Texture2D texture)
        {
            return texture.TextureToBase64();
        }

        private static string Convert(byte[] video)
        {
            return video.ByteArrayToBase64();
        }

        public override string ToString()
        {
            return $"ImageUrl={ImageUrl}, VideoUrl={VideoUrl}";
        }
    }
}
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace GetSocialSdk.Core
{
    internal class MediaUploader
    {
        private GetSocialStateController _stateController;
        private const int MaxRetryCount = 5;

        internal MediaUploader(GetSocialStateController stateController)
        {
            _stateController = stateController;
        }

        internal string UploadMedia(byte[] mediaData, string purpose)
        {
            var result = Task.Run(async () => await InternalUploadMedia(mediaData, purpose)).Result;
            if (result == null)
            {
                return null;
            }
            if (result.ContainsKey("png"))
            {
                return result["png"];
            }
            if (result.ContainsKey("jpg"))
            {
                return result["jpg"];
            }
            return null;
        }

        internal void UploadMedia(List<MediaAttachment> mediaAttachments, string purpose)
        {
            for (int i = 0; i < mediaAttachments.Count; i++)
            {
                var attachment = mediaAttachments[i];
                if (attachment._imageBase64 != null)
                {
                    var b64_bytes = Convert.FromBase64String(attachment._imageBase64);
                    var result = Task.Run(async () => await InternalUploadMedia(b64_bytes, purpose)).Result;
                    if (result.ContainsKey("png"))
                    {
                        attachment.ImageUrl = result["png"];
                    }
                    if (result.ContainsKey("jpg"))
                    {
                        attachment.ImageUrl = result["jpg"];
                    }
                }
                if (attachment._videoDataBase64 != null)
                {
                    var b64_bytes = Convert.FromBase64String(attachment._videoDataBase64);
                    var result = Task.Run(async () => await InternalUploadMedia(b64_bytes, purpose)).Result;
                    if (result.ContainsKey("png"))
                    {
                        attachment.ImageUrl = result["png"];
                    }
                    if (result.ContainsKey("jpg"))
                    {
                        attachment.ImageUrl = result["jpg"];
                    }
                    if (result.ContainsKey("gif"))
                    {
                        attachment.GifUrl = result["gif"];
                    }
                    if (result.ContainsKey("mp4"))
                    {
                        attachment.VideoUrl = result["mp4"];
                    }
                }
            }
        }

        private async Task<Dictionary<string, string>> InternalUploadMedia(byte[] mediaData, string purpose)
        {
            var endpoint = _stateController.UploadEndpoint;
            var metadata = new Dictionary<string, string>();
            metadata["purpose"] = purpose;
            metadata["filename"] = Guid.NewGuid().ToString();
            //metadata["os_version"] = _stateController.SuperProperties.DeviceOsVersion;
            //metadata["sdk_version"] = _stateController.SuperProperties.SdkVersion;
            //metadata["app"] = _stateController.SuperProperties.AppId;
            metadata["platform"] = "Unity Native";

            var memoryStream = new MemoryStream(mediaData);
            var client = new TUSClient();
            var fileURL = await client.Create(endpoint, memoryStream, metadata);

            var response = await client.Upload(fileURL, memoryStream);
            if (response == false)
            {
                return null;
            }
            var retryCount = 0;
            var downloadResponse = await client.Download(fileURL);
            while (downloadResponse == null && retryCount < MaxRetryCount)
            {
                Debug.Log("Retrying after 2 seconds");
                System.Threading.Thread.Sleep(2000);
                retryCount++;
                downloadResponse = await client.Download(fileURL);
            }
            if (downloadResponse == null)
            {
                return null;
            }
            Debug.Log("finished! ");
            return CreateUploadResult(downloadResponse.Headers["Getsocial-Resource"]);
        }

        private Dictionary<string, string> CreateUploadResult(string rawResult)
        {
            var result = new Dictionary<string, string>();
            var parts = rawResult.Split('|');
            for (var i = 0; i < parts.Length; i++)
            {
                var innerParts = parts[i].Split('=');
                result[innerParts[0]] = innerParts[1];
            }
            return result;
        }
    }
}
#endif
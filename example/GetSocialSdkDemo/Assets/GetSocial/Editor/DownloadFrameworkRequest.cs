using System;
using System.Collections;
using System.IO;
using System.Net;
using UnityEngine;

namespace GetSocialSdk.Editor
{
    public class DownloadFrameworkRequest
    {
        private Request _downloadRequest;

        private readonly string _url;
        private readonly string _destinationFolderPath;
        private readonly string _archiveFilePath;

        public static DownloadFrameworkRequest Create(string url, string destinationFolderPath, string archiveFilePath)
        {
            return new DownloadFrameworkRequest(url, destinationFolderPath, archiveFilePath); 
        }

        private DownloadFrameworkRequest(string url, string destinationFolderPath, string archiveFilePath)
        {
            _url = url;
            _destinationFolderPath = destinationFolderPath;
            _archiveFilePath = archiveFilePath;
        }

        public void Start(Action onSuccess, Action<float> onUpdate, Action<string> onFailure, bool sync = false)
        {
            Directory.CreateDirectory(_destinationFolderPath);
            if (sync)
            {
                StartSync(onSuccess, onFailure);                
            }
            else
            {
                EditorCoroutine.Start(StartRequest(onSuccess, onUpdate, onFailure));
            }
        }

        public void StartSync(Action onSuccess, Action<string> onFailure)
        {
            try
            {
                using (var wc = new WebClient())
                {
                    wc.DownloadFile(new Uri(_url), _archiveFilePath);
                    onSuccess();
                }
            }
            catch (WebException)
            {
                onFailure("Can not download GetSocial frameworks, check your internet connection");
            }
            
        }

        private IEnumerator StartRequest(Action onSuccess, Action<float> onUpdate, Action<string> onFailure)
        {
            _downloadRequest = RequestHelper.CreateDownloadRequest(_url);
            while (!_downloadRequest.IsDone())
            {
                onUpdate(_downloadRequest.Progress());
                yield return null;
            }
            if (_downloadRequest.GetErrorMessage() != null)
            {
                onFailure("Can not download GetSocial frameworks, check your internet connection");
            }
            else
            {
                File.WriteAllBytes(_archiveFilePath, _downloadRequest.GetBytes());
                onSuccess();
            }
        }
    }
}
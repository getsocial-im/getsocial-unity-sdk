using UnityEngine;
using UnityEngine.Networking;

namespace GetSocialSdk.Editor
{
    public interface Request
    {
        bool IsDone();
        string GetErrorMessage();
        byte[] GetBytes();
        string GetString();

        float Progress();
    }
   
    public class RequestHelper
    {
        public static Request CreateDownloadRequest(string requestURL)
        {
            #if UNITY_2018_1_OR_NEWER
                return CreateDownloadRequestUsingWebRequest(requestURL);    
            #else
                return CreateDownloadRequestUsingWWW(requestURL);    
            #endif
        }
        
        #region WWW methods

        private static WWWRequest CreateDownloadRequestUsingWWW(string requestURL)
        {
            return new WWWRequest(requestURL);
        }

        #endregion

#if UNITY_2018_1_OR_NEWER
        
        #region WebRequest methods

        private static WebRequest CreateDownloadRequestUsingWebRequest(string requestURL)
        {
            return new WebRequest(requestURL);
        }

        #endregion
        
#endif
        
    }
    
#pragma warning disable 0618
    public class WWWRequest : Request
    {
        private WWW _www;
        
        public WWWRequest(string requestURL)
        {
            _www = new WWW(requestURL);
        }
        
        public bool IsDone()
        {
            return _www.isDone;
        }

        public string GetErrorMessage()
        {
            return _www.error;
        }

        public byte[] GetBytes()
        {
            return _www.bytes;
        }

        public string GetString()
        {
            return _www.text;
        }

        public float Progress()
        {
            return _www.progress;
        }
    }
#pragma warning restore 0618

#if UNITY_2018_1_OR_NEWER
    
    public class WebRequest : Request
    {
        private UnityWebRequest _webRequest;
        
        public WebRequest(string requestURL)
        {
            _webRequest = UnityWebRequest.Get(requestURL);
            _webRequest.SendWebRequest();
        }
        
        public bool IsDone()
        {
            return _webRequest.isDone;
        }

        public string GetErrorMessage()
        {
            return _webRequest.error;
        }

        public byte[] GetBytes()
        {
            return _webRequest.downloadHandler.data;
        }

        public string GetString()
        {
            return _webRequest.downloadHandler.text;
        }
        
        public float Progress()
        {
            return _webRequest.downloadProgress;
        }
    }

#endif
    
}
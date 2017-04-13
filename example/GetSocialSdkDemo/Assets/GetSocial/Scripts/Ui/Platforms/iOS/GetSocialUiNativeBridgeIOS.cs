using System.IO;

#if UNITY_IOS && USE_GETSOCIAL_UI
using GetSocialSdk.Core;
using System.Runtime.InteropServices;
using UnityEngine;

namespace GetSocialSdk.Ui
{
    class GetSocialUiNativeBridgeIOS : IGetSocialUiNativeBridge
    {
        static IGetSocialUiNativeBridge _instance;

        public static IGetSocialUiNativeBridge Instance
        {
            get { return _instance ?? (_instance = new GetSocialUiNativeBridgeIOS()); }
        }

        #region IGetSocialUiNativeBridge implementation

        public bool LoadDefaultConfiguration()
        {
            return _loadDefaultConfiguration();
        }

        public bool LoadConfiguration(string filePath)
        {
            var fullPath = Path.Combine(Application.streamingAssetsPath, filePath);
            GetSocialDebugLogger.D("Loading configuration at path: " + fullPath);
            return _loadConfiguration(fullPath);
        }

        public bool CloseView(bool saveViewState)
        {
            return _closeView(saveViewState);
        }

        public bool RestoreView()
        {
            return _restoreView();
        }

        #endregion

        [DllImport("__Internal")]
        static extern bool _loadDefaultConfiguration();

        [DllImport("__Internal")]
        static extern bool _loadConfiguration(string filePath);

        [DllImport("__Internal")]
        static extern bool _closeView(bool saveViewState);

        [DllImport("__Internal")]
        static extern bool _restoreView();
    }
}
#endif

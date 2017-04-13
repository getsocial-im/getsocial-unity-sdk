#if USE_GETSOCIAL_UI
namespace GetSocialSdk.Ui
{
    interface IGetSocialUiNativeBridge
    {
        bool LoadDefaultConfiguration();

        bool LoadConfiguration(string filePath);

        bool CloseView(bool saveViewState);

        bool RestoreView();

#if UNITY_ANDROID
        bool OnBackPressed();
#endif
    }
}
#endif

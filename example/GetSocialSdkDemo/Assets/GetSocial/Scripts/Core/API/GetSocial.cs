using System;
using System.Collections.Generic;
using UnityEngine;

namespace GetSocialSdk.Core
{

    /// <summary>
    /// Listener to handle GetSocial notifications.
    /// Called if application was started by clicking on GetSocial push notification
    /// or notification was received while application is in foreground.
    /// </summary>
    /// <param name="notification">Received notification</param>
    /// <param name="wasClicked">is true, if notification was clicked by user in notification center, false if received while application was in foreground.</param>
    /// <returns>true, if you handle this notification, false otherwise</returns>
    public delegate bool NotificationListener(Notification notification, bool wasClicked);

    /// <summary>
    /// Called when GetSocial get the Push Notification device token.
    /// </summary>
    /// <param name="pushToken">Push Notification device token.</param>
    public delegate void PushTokenListener(string pushToken);

    public delegate void OnCurrentUserChangedListener(CurrentUser user);

    /// <summary>
    /// The GetSocial API
    /// </summary>
    public static class GetSocial
    {
        #region meta_data

        /// <summary>
        /// Gets the GetSocial Unity SDK version.
        /// </summary>
        /// <value>The GetSocial Unity SDK version.</value>
        public static string UnitySdkVersion
        {
            get { return BuildConfig.UnitySdkVersion; }
        }

        /// <summary>
        /// Gets the native SDK (Android/iOS) version.
        /// </summary>
        /// <value>The native SDK (Android/iOS) version.</value>
        public static string NativeSdkVersion
        {
            get { return GetSocialFactory.Bridge.GetNativeSdkVersion(); }
        }

        #endregion

        #region initialization

        /// <summary>
        /// Init the SDK. Use <see cref="AddOnInitializedListener"/> to be notified when SDK is initialized. Check errors in logs or in GlobalErrorListener using <see cref="SetGlobalErrorListener"/>.
        /// </summary>
        public static void Init()
        {
            GetSocialFactory.Bridge.Init(null);
        }
        
        /// <summary>
        /// Init the SDK. Use <see cref="AddOnInitializedListener"/> to be notified when SDK is initialized. Check errors in logs or in GlobalErrorListener using <see cref="SetGlobalErrorListener"/>.
        /// </summary>
        /// <param name="appId">Application ID</param>
        public static void Init(string appId)
        {
            GetSocialFactory.Bridge.Init(appId);
        }

        public static void Init(Identity identity, Action onSuccess, Action<GetSocialError> onError)
        {
            GetSocialFactory.Bridge.Init(identity, onSuccess, onError);
        }
        
        /// <summary>
        /// Set an action, which should be executed after SDK initialized.
        /// Executed immediately, if SDK is already initialized.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        public static void AddOnInitializedListener(Action action)
        {
            GetSocialFactory.Bridge.AddOnInitializedListener(action);
        }

        /// <summary>
        /// Provides the status of the GetSocial initialisation.
        /// </summary>
        /// <returns><c>true</c> if SDK has completed successfully; otherwise, <c>false</c>.</returns>
        public static bool IsInitialized
        {
            get { return GetSocialFactory.Bridge.IsInitialized(); }
        }

        #endregion

        #region localization

        /// <summary>
        /// The current language of GetSocial SDK, return value must be one of the language codes provided in <see cref="LanguageCodes"/>
        /// </summary>
        /// <returns>Current language of GetSocial SDK</returns>
        public static string GetLanguage()
        {
            return GetSocialFactory.Bridge.GetLanguage();
        }

        /// <summary>
        /// Set the language of GetSocial SDK. If language is incorrect, default language is set.
        /// </summary>
        /// <param name="languageCode">Must be one of language codes provided in <see cref="LanguageCodes"/></param>
        /// <returns><c>true</c> if the operation was successful; <c>false</c> otherwise</returns>
        public static bool SetLanguage(string languageCode)
        {
            Check.Argument.IsStrNotNullOrEmpty(languageCode, "languageCode");

            return GetSocialFactory.Bridge.SetLanguage(languageCode);
        }

        #endregion

        #region User

        public static string AddOnCurrentUserChangedListener(OnCurrentUserChangedListener listener) {
            return GetSocialFactory.Bridge.AddOnCurrentUserChangedListener(listener);
        }

        public static void RemoveOnCurrentUserChangedListener(string listenerId)
        {
            GetSocialFactory.Bridge.RemoveOnCurrentUserChangedListener(listenerId);
        }

        public static CurrentUser GetCurrentUser()
        {
            return GetSocialFactory.Bridge.GetCurrentUser();
        }

        public static void SwitchUser(Identity identity, Action success, Action<GetSocialError> failure) {
            GetSocialFactory.Bridge.SwitchUser(identity, success, failure);
        }

        public static void ResetUser(Action success, Action<GetSocialError> failure) {
            GetSocialFactory.Bridge.ResetUser(success, failure);
        }
        
        public static void Reset(Action success, Action<GetSocialError> failure) {
            GetSocialFactory.Bridge.ResetUserWithoutInit(success, failure);
        }


        #endregion

        #region Actions
        
        /// <summary>
        /// Process action by GetSocial SDK.
        /// </summary>
        /// <param name="action">Action to be processed</param>
        public static void Handle(GetSocialAction action)
        {
            GetSocialFactory.Bridge.Handle(action);
        }

        #endregion

        #region Device
        public static class Device 
        {
            public static bool IsTestDevice
            {
                get { return GetSocialFactory.Bridge.IsTestDevice(); }
            }

            public static string Identifier
            {
                get { return GetSocialFactory.Bridge.GetDeviceId(); }
            }
        }
        #endregion
    }
}
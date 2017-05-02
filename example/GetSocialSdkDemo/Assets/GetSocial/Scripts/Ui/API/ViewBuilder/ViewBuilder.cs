using System;
using GetSocialSdk.Core;
using UnityEngine;

#if USE_GETSOCIAL_UI

namespace GetSocialSdk.Ui
{
    /// <summary>
    /// Base class for GetSocial UI view builder
    /// </summary>
    /// <typeparam name="T">Type of view to create</typeparam>
    public abstract class ViewBuilder<T> where T : ViewBuilder<T>
    {
        protected Action _onOpen, _onClose;
        protected string _customWindowTitle;

        public T SetWindowTitle(string title)
        {
            _customWindowTitle = title;
            return (T) this;
        }

        public T SetViewStateCallbacks(Action onOpen, Action onClose)
        {
            _onOpen = onOpen;
            _onClose = onClose;
            return (T) this;
        }

        public abstract bool Show();

#if UNITY_ANDROID

        protected bool ShowBuilder(AndroidJavaObject builder)
        {
            SetTitleAJO(builder);
            SetViewStateListener(builder);
            // Make sure ui is instantiated at this point for ensuring OnResume was called before opening the view
            GetSocialUiFactory.InstantiateGetSocialUi();

            return JniUtils.RunOnUiThreadSafe(() =>
            {
                using (builder)
                {
                    builder.CallBool("show");
                }
            });
        }

        private void SetTitleAJO(AndroidJavaObject builderAJO)
        {
            if (_customWindowTitle != null)
            {
                builderAJO.CallAJO("setWindowTitle", _customWindowTitle);
            }
        }

        private void SetViewStateListener(AndroidJavaObject builderAJO)
        {
            if (_onOpen != null || _onClose != null)
            {
                builderAJO.CallAJO("setViewStateListener",
                    new ViewStateListener(_onOpen, _onClose));
            }
        }

#endif
    }
}

#endif
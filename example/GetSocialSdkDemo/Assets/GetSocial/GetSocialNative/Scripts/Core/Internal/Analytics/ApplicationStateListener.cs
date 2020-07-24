#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEngine;

namespace GetSocialSdk.Core
{
    internal class ApplicationStateListener : Singleton<ApplicationStateListener>
    {
        public DateTime AppStartTime { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            LoadInstance();
        }

        private void Awake()
        {
            AppStartTime = DateTime.Now;
        }

        protected override void OnDestroy()
        {
#if !UNITY_EDITOR
            TrackEvent(AnalyticsEventDetails.AppSessionEnd);
#endif
            base.OnDestroy();
        }

        private static void TrackEvent(string name, Dictionary<string, string> properties = null)
        {
            if (!GetSocial.IsInitialized)
            {
                return;
            }

            GetSocialFactory.Bridge.TrackCustomEvent(name, properties);
        } 
    }
}
#endif

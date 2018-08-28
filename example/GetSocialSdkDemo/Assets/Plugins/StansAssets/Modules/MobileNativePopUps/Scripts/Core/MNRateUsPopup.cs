using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MNRateUsPopup : MNPopup {

		private MNPopupAction remindListener;
		private MNPopupAction declineListener;
		private MNPopupAction rateUsListener;

		#if UNITY_ANDROID
		private string androidAppUrl = string.Empty;
		#elif UNITY_IOS
		private string appleId = string.Empty;
		#endif

		public MNRateUsPopup(string title, string message, string rateUs, string decline, string remind) : base (title, message) {
				AddAction (remind, () => {
						if (remindListener != null) {
								remindListener.Invoke();
						}
				});

				AddAction (decline, () => {
						if (declineListener != null) {
								declineListener.Invoke();
						}
				});

				AddAction (rateUs, () => {
						#if UNITY_ANDROID
						MNAndroidNative.RedirectStoreRatingPage(androidAppUrl);
						#elif UNITY_IOS
						MNIOSNative.RedirectToAppStoreRatingPage(appleId);
						#endif

						if (rateUsListener != null) {
								rateUsListener.Invoke();
						}
				});
		}

		public void SetAppleId (string id) {
				#if UNITY_IOS
				appleId = id;
				#endif
		}

		public void SetAndroidAppUrl (string appUrl) {
				#if UNITY_ANDROID
				androidAppUrl = appUrl;
				#endif
		}

		public void AddRateUsListener (MNPopupAction callback) {
				rateUsListener = callback;
		}

		public void AddRemindListener (MNPopupAction callback) {
				remindListener = callback;
		}

		public void AddDeclineListener (MNPopupAction callback) {
				declineListener = callback;
		}

}

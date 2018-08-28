//#define SA_DEBUG_MODE

using UnityEngine;
using System.Collections;
#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
using System.Runtime.InteropServices;
#endif

public class MNIOSNative {
	
	//--------------------------------------
	//  NATIVE FUNCTIONS
	//--------------------------------------
	
	#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE	
	[DllImport ("__Internal")]
	private static extern void _MNP_ShowMessage(string title, string message, string actions);
	
	[DllImport ("__Internal")]
	private static extern void _MNP_DismissCurrentAlert();

	[DllImport ("__Internal")]
	private static extern void _MNP_RedirectToAppStoreRatingPage(string appId);
	
	[DllImport ("__Internal")]
	private static extern void _MNP_ShowPreloader();	
	
	[DllImport ("__Internal")]
	private static extern void _MNP_HidePreloader();
	#endif

	public static void showMessage(string title, string message, string actions) {
		 #if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
		 _MNP_ShowMessage (title, message, actions);
		 #endif
	}

	public static void RedirectToAppStoreRatingPage(string appleId) {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
		_MNP_RedirectToAppStoreRatingPage(appleId);
		#endif
	}
		
	public static void ShowPreloader() {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
		_MNP_ShowPreloader();
		#endif
	}
	
	public static void HidePreloader() {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
		_MNP_HidePreloader();
		#endif
	}

	public static void dismissCurrentAlert() {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
		_MNP_DismissCurrentAlert();
		#endif
	}
}

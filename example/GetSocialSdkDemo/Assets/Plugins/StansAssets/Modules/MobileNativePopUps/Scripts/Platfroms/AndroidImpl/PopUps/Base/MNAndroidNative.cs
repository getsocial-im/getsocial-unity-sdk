////////////////////////////////////////////////////////////////////////////////
//  
// @module Common Android Native Lib
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class MNAndroidNative {

	private const string CLASS_NAME = "com.stansassets.mnp.NativePopupsManager";
	
	private static void CallActivityFunction(string methodName, params object[] args) {
		MNProxyPool.CallStatic(CLASS_NAME, methodName, args);
	}
	
	//--------------------------------------
	//  MESSAGING
	//--------------------------------------

	public static void showMessage(string title, string message, string actions, MNAndroidDialogTheme theme) {
		CallActivityFunction("ShowMessage", title, message, actions, (int)theme);
	}
	
	public static void ShowPreloader(string title, string message, MNAndroidDialogTheme theme) {
		CallActivityFunction("ShowPreloader",  title, message, (int)theme);
	}
	
	public static void HidePreloader() {
		CallActivityFunction("HidePreloader");
	}

	public static void RedirectStoreRatingPage(string url) {
		CallActivityFunction("OpenAppRatingPage", url);
	}

}

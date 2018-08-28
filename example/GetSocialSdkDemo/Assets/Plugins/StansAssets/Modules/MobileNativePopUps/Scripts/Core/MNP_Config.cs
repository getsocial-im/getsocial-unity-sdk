////////////////////////////////////////////////////////////////////////////////
//  
// @module Assets Common Lib
// @author Osipov Stanislav (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////


using UnityEngine;
using System;
using System.Reflection;
using System.Collections;

public static class MNP_Config
{
		public const string SUPPORT_EMAIL = "support@stansassets.com";
		public const string WEBSITE_ROOT_URL = "https://stansassets.com/";

		public const string BUNDLES_PATH = "Plugins/StansAssets/Bundles/";
		public const string MODULS_PATH = "Plugins/StansAssets/Modules/";
		public const string SUPPORT_MODULS_PATH = "Plugins/StansAssets/Support/";


		public const string COMMON_LIB_PATH = SUPPORT_MODULS_PATH + "Common/";
		public const string VERSION_INFO_PATH = SUPPORT_MODULS_PATH + "Versions/";
		public const string NATIVE_LIBRARIES_PATH = SUPPORT_MODULS_PATH + "NativeLibraries/";
		public const string EDITOR_TESTING_LIB_PATH = SUPPORT_MODULS_PATH + "EditorTesting/";
		public const string SETTINGS_REMOVE_PATH = SUPPORT_MODULS_PATH + "Settings/";
		public const string SETTINGS_PATH = SUPPORT_MODULS_PATH + "Settings/Resources/";

		
		public const string ANDROID_DESTANATION_PATH = "Plugins/Android/";
		public const string ANDROID_SOURCE_PATH = NATIVE_LIBRARIES_PATH + "Android/";


		public const string IOS_DESTANATION_PATH = "Plugins/IOS/";
		public const string IOS_SOURCE_PATH = NATIVE_LIBRARIES_PATH + "IOS/";

		
		public const string AN_VERSION_INFO_PATH = VERSION_INFO_PATH + "AN_VersionInfo.txt";
		public const string UM_VERSION_INFO_PATH = VERSION_INFO_PATH + "UM_VersionInfo.txt";
		public const string GMA_VERSION_INFO_PATH = VERSION_INFO_PATH + "GMA_VersionInfo.txt";
		public const string MSP_VERSION_INFO_PATH = VERSION_INFO_PATH + "MSP_VersionInfo.txt";
		public const string ISN_VERSION_INFO_PATH = VERSION_INFO_PATH + "ISN_VersionInfo.txt";
		public const string MNP_VERSION_INFO_PATH = VERSION_INFO_PATH + "MNP_VersionInfo.txt";
		public const string AMN_VERSION_INFO_PATH	= VERSION_INFO_PATH + "AMN_VersionInfo.txt";
}

////////////////////////////////////////////////////////////////////////////////
//  
// @module Stan's Assets Commons Lib
// @author Osipov Stanislav (Stan's Assets) 
// @support support@stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

public static class MNP_Instalation
{
				
		public static void IOS_UpdatePlugin ()
		{
				IOS_InstallPlugin (false);
		}

		public static void IOS_InstallPlugin (bool IsFirstInstall = true)
		{			
				IOS_CleanUp ();			
			
				MNP_Files.CopyFile (MNP_Config.IOS_SOURCE_PATH + "ISN_NativeCore.h.txt", MNP_Config.IOS_DESTANATION_PATH + "ISN_NativeCore.h");
				MNP_Files.CopyFile (MNP_Config.IOS_SOURCE_PATH + "ISN_NativeCore.mm.txt", MNP_Config.IOS_DESTANATION_PATH + "ISN_NativeCore.mm");
			
		}

		public static void IOS_CleanUp ()
		{			
				//New API
				RemoveIOSFile ("ISN_NativeCore");
		}

		
		public static void RemoveIOSFile (string filename)
		{
				MNP_Files.DeleteFile (MNP_Config.IOS_DESTANATION_PATH + filename + ".h");
				MNP_Files.DeleteFile (MNP_Config.IOS_DESTANATION_PATH + filename + ".m");
				MNP_Files.DeleteFile (MNP_Config.IOS_DESTANATION_PATH + filename + ".mm");
				MNP_Files.DeleteFile (MNP_Config.IOS_DESTANATION_PATH + filename + ".a");
		}

		
		public static void Android_UpdatePlugin ()
		{
				Android_InstallPlugin (false);
		}

		public static void Android_InstallPlugin (bool IsFirstInstall = true)
		{

				#if UNITY_4_6 || UNITY_4_7
			MNP_Files.CopyFile (MNP_Config.ANDROID_SOURCE_PATH + "mobile-native-popups.jar.txt",             MNP_Config.ANDROID_DESTANATION_PATH + "mobile-native-popups.jar");
				#else
				MNP_Files.CopyFile (MNP_Config.ANDROID_SOURCE_PATH + "mobile-native-popups.txt", MNP_Config.ANDROID_DESTANATION_PATH + "mobile-native-popups.aar");
				#endif

				//Clean up old legacy plugin resources
				MNP_Files.DeleteFile (MNP_Config.ANDROID_SOURCE_PATH + "mobilenativepopups.txt");
				MNP_Files.DeleteFile (MNP_Config.ANDROID_DESTANATION_PATH + "mobilenativepopups.jar");

				AssetDatabase.Refresh ();			
		}
}
#endif

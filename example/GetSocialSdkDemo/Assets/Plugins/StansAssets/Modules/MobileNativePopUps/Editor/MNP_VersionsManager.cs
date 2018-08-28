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
using System;
using System.Collections;

public static class MNP_VersionsManager
{

		//--------------------------------------
		// Mobile Native Pop Up
		//--------------------------------------

		public static bool Is_MNP_Installed {
				get {
						return MNP_Files.IsFileExists (MNP_Config.MNP_VERSION_INFO_PATH);
				} 
		}

		public static int MNP_Version {
				get {
						return GetVersionCode (MNP_Config.MNP_VERSION_INFO_PATH);
				} 
		}

		public static int MNP_MagorVersion {
				get {
						return GetMagorVersionCode (MNP_Config.MNP_VERSION_INFO_PATH);
				} 
		}

		public static string MNP_StringVersionId {
				get {
						return GetStringVersionId (MNP_Config.MNP_VERSION_INFO_PATH);
				}
		}

		//--------------------------------------
		// Utilities
		//--------------------------------------

		public static int ParceMagorVersion (string stringVersionId)
		{
				string[] versions = stringVersionId.Split (new char[] { '.', '/' });
				int intVersion = Int32.Parse (versions [0]) * 100;
				return  intVersion;
		}


		private static int GetMagorVersionCode (string versionFilePath)
		{
				string stringVersionId = MNP_Files.Read (versionFilePath);
				return ParceMagorVersion (stringVersionId);
		}



		public static int ParceVersion (string stringVersionId)
		{
				string[] versions = stringVersionId.Split (new char[] { '.', '/' });
				int intVersion = Int32.Parse (versions [0]) * 100 + Int32.Parse (versions [1]) * 10;
				return  intVersion;
		}



		private static int GetVersionCode (string versionFilePath)
		{
				string stringVersionId = MNP_Files.Read (versionFilePath);
				return ParceVersion (stringVersionId);
		}



		private static string GetStringVersionId (string versionFilePath)
		{
				if (MNP_Files.IsFileExists (versionFilePath)) {
						return MNP_Files.Read (versionFilePath);
				} else {
						return "0.0";
				}
		}


		public static string InstalledPluginsList {

				get {
						string allPluginsInstalled = "";

						if (MNP_Files.IsFolderExists (MNP_Config.BUNDLES_PATH)) {
								string[] bundles = MNP_Files.GetFoldersAt (MNP_Config.BUNDLES_PATH);
								foreach (string pluginPath in bundles) {
										string pluginName = System.IO.Path.GetFileName (pluginPath);
										allPluginsInstalled = allPluginsInstalled + " (" + pluginName + ")" + "\n";
								}
						}

						if (MNP_Files.IsFolderExists (MNP_Config.MODULS_PATH)) {

								string[] modules = MNP_Files.GetFoldersAt (MNP_Config.MODULS_PATH);
								foreach (string pluginPath in modules) {
										string pluginName = System.IO.Path.GetFileName (pluginPath);
										allPluginsInstalled = allPluginsInstalled + " (" + pluginName + ")" + "\n";
								}
						}

						return allPluginsInstalled;
				}
		}
}

#endif

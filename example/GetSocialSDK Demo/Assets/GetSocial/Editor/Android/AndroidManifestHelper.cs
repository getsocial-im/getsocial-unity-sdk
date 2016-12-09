/**
 *     Copyright 2015-2016 GetSocial B.V.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.IO;
using UnityEditor;
using UnityEngine;
using System;
using System.Xml;
using GetSocialSdk.Core;

namespace GetSocialSdk.Editors
{
    public static class AndroidManifestHelper
    {
        /// <summary>
        /// Relative path to your AndroidManifest.xml file.
        ///
        /// Change it if your manifest is not in the root of Plugins directory.
        /// </summary>
        private const string MainManifestPath = "Plugins/Android/AndroidManifest.xml";

        /// <summary>
        /// Relative path to default AndroidManifest.xml file that will be copied to Plugins folder if one does not exist there.
        /// </summary>
        private const string DefaultBackupManifestPath = "GetSocial/Editor/Android/BackupManifest/AndroidManifest.xml";

        public static readonly string ManifestPathInProject = Path.Combine(Application.dataPath, MainManifestPath);
        public static readonly string PluginsPathInProject = Path.Combine(Application.dataPath, "Plugins/Android");


        private const char Bullet = '\u2022';

        #region manifest_xml
        private const string MetaDataElementName = "meta-data";
        private const string PermissionElementName = "permission";
        private const string UsesPermissionElementName = "uses-permission";
        private const string ActivityElementName = "activity";
        private const string ReceiverElementName = "receiver";
        private const string IntentFilterElementName = "intent-filter";
        private const string ActionElementName = "action";
        private const string CategoryElementName = "category";
        private const string ProviderElementName = "provider";

        // used for deep linking and referral data retrieving
        private const string GetSocialDeepLinkingActivityName = "im.getsocial.sdk.core.unity.GetSocialDeepLinkingActivity";

        private const string ActivityActionMain = "android.intent.action.MAIN";
        private const string ActivityActionView = "android.intent.action.VIEW";

        private const string ActivityCategoryLauncher = "android.intent.category.LAUNCHER";
        private const string ActivityCategoryDefault = "android.intent.category.DEFAULT";
        private const string ActivityCategoryBrowsable = "android.intent.category.BROWSABLE";

        private const string GetSocialDeepLinkingScheme = "android:scheme=\"getsocial\"";

        // Core permissions
        private const string AccessNetoworkStatePermission = "android.permission.ACCESS_NETWORK_STATE";
        private const string PermissionInternet = "android.permission.INTERNET";

        // CGM permissions
        private const string WakeLockAndroidPemission = "android.permission.WAKE_LOCK";
        private const string ReceiveC2dmAndroidPemission = "com.google.android.c2dm.permission.RECEIVE";

        private const string GetSocialReceiverName = "im.getsocial.sdk.core.GetSocialReceiver";
        private const string InstallReferrerReceiverName = "im.getsocial.sdk.core.gms.InstallReferrerReceiver";

        private const string GetSocialImageContentProviderName = "im.getsocial.sdk.core.provider.ImageContentProvider";
        private const string GetSocialImageContentProviderFormat = "{0}.smartinvite.images.provider";
        #endregion

        private const string GetSocialDeepLinkingActivityMissingMsg = "You don't have the " + GetSocialDeepLinkingActivityName + " defined in your AndroidManifest.xml";

        private static bool areBasicPermissionsPresent = false;
        private static bool isGetSocialDeepLinkingActivityPresent = false;
        // install tracking & referral data
        private static bool isInstallReferrerReceiverPresent = false;
        private static bool arePlayServicesAdded = false;
        private static bool isImageContentProviderPresent = false;
        // push notifications
        private static bool arePermissionForPushesCorrect = false;
        private static bool isGetSocialReceiverPresent = false;

        // dependency libs
        private static bool areAllDependencyLibsPresent = false;


        public static void Refresh()
        {
            if(DoesManifestExist()) { RefreshAllManifestChecks(); }
        }

        private static void RefreshAllManifestChecks()
        {
            Debug.Log(string.Format("Rechecking your manifest at \n\t<color=green>{0}</color>", MainManifestPath));
            areBasicPermissionsPresent = CheckIfAllBasicPermissionsPresent();
            isGetSocialDeepLinkingActivityPresent = CheckIfGetSocialDeepLinkingActivityPresentAndCorrect();

            arePermissionForPushesCorrect = CheckIfAllPushPermissionsPresent();
            isGetSocialReceiverPresent = CheckIfReceiverPresent(GetSocialReceiverName, GetAndroidApplicationNode());
            isImageContentProviderPresent = CheckIfImageContentProviderPresent();

            arePlayServicesAdded = CheckIfPlayServicesPresent();
            isInstallReferrerReceiverPresent = CheckIfReceiverPresent(InstallReferrerReceiverName, GetAndroidApplicationNode());

            areAllDependencyLibsPresent = CheckIfAllDependencyLibsPresent();
        }

        private static void GenerateManifestIfNotPresent()
        {
            // only copy over a fresh copy of the AndroidManifest if one does not exist
            if(!DoesManifestExist())
            {
                Debug.Log("AndroidManifest.xml does not exist in Plugins folder, creating one for you...");
                var inputFile = Path.Combine(Application.dataPath, DefaultBackupManifestPath);
                File.Copy(inputFile, ManifestPathInProject);
            }
            AssetDatabase.Refresh();
            RefreshAllManifestChecks();
        }

        #region gui
        public static void DrawManifestCheckerGUI()
        {
            DrawCheckPackageGUI();

            if(!DoesManifestExist())
            {
                var message = string.Format(
                    "Failed to locate file '{0}'.\n\n" +
                    "Press a button to create new Android manifest or update constant MainManifestPath in AndroidManifestHelper.cs with correct path.",
                    MainManifestPath);
                DrawFixProjectConfigurationMessage(message, "Create AndroidManifest.xml", GenerateManifestIfNotPresent);
            }
            else
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.HelpBox("Click the button below to check your AndroidManifest.xml for possible issues.\n\n"  +
                    "Apply proposed updates ONLY if you are actually planning to use feature that requires them", MessageType.Info);
                if(GUILayout.Button("Check AndroidManifest.xml and Dependencies"))
                {
                    RefreshAllManifestChecks();
                }

                DrawManifestFixProposals();
                EditorGUILayout.EndVertical();
            }
        }

        private static void DrawManifestFixProposals()
        {
            DrawCoreFixes();
            DrawDeepLinkingFixes();
            DrawSmartInvitesFixes();
            DrawPushesAvailabelFixes();
            DrawDependencyLibsFixes();
        }

        static void DrawCoreFixes()
        {
            FixMessageGroupTitle("Core");
            Action drawFixAction = () => DrawFixProjectConfigurationMessage("You don't have all the required permissions for GetSocial to work", "Add missing permissions", AddBasicPermissions);
            DrawFixBox(areBasicPermissionsPresent, "All required basic permissions added", drawFixAction);
        }

        static void DrawDeepLinkingFixes()
        {
            FixMessageGroupTitle("Deep Linking & Install Tracking");
            Action drawFixAction = () => DrawFixProjectConfigurationMessage(GetSocialDeepLinkingActivityMissingMsg + " or it is misconfigured", "Add GetSocial Deep Linking Activity", AddGetSocialDeepLinkingActivity);
            DrawFixBox(isGetSocialDeepLinkingActivityPresent, "Deep linking & referral data setup is correct", drawFixAction);
        }

        static void DrawSmartInvitesFixes()
        {
            FixMessageGroupTitle("Install Tracking, Referral Data, Invite image (Smart Invites)");

            Action drawFixPlayServicesAction =
                () => DrawFixProjectConfigurationMessage("Google Play Services are required for install tracking and retrieving referral data", "Add Play Services", AddPlayServicesVersion);
            DrawFixBox(arePlayServicesAdded, "Google Play Services are present", drawFixPlayServicesAction);


            Action drawFixReferrerReceiver =
                () => DrawFixProjectConfigurationMessage("GetSocial install referrer receiver must be present to be able to retrieve referral data", "Add Install Referrer Receiver", AddInstallReferrerReceiver);
            DrawFixBox(isInstallReferrerReceiverPresent, "Install referrer receiver added", drawFixReferrerReceiver);

            Action drawFixImageContentProvider =
                () => DrawFixProjectConfigurationMessage("GetSocial image content provider must be present to attach images to your invites", "Add image content provider", AddImageContentProvider);
            DrawFixBox(isImageContentProviderPresent, "Image content provider added", drawFixImageContentProvider);
        }

        private static void DrawPushesAvailabelFixes()
        {
            FixMessageGroupTitle("Push Notifications");
            Action drawFixPushPermissions =
                () => DrawFixProjectConfigurationMessage("You don't have all the required permissions for push notifications in your manifest", "Add missing permissions", AddPushPermissions);
            DrawFixBox(arePermissionForPushesCorrect, "All required permissions for pushes are present", drawFixPushPermissions);

            Action drawFixPushReceiver =
                () => DrawFixProjectConfigurationMessage("You don't have GetSocial receiver in you manifest", "Add GetSocial receiver", AddGetSocialReceiver);
            DrawFixBox(isGetSocialReceiverPresent, "GetSocial receiver is already in your manifest", drawFixPushReceiver);
        }

        private static void DrawDependencyLibsFixes()
        {
            FixMessageGroupTitle("GetSocial Dependencies");
            Action drawFixDependencies =
                () => DrawFixProjectConfigurationMessage("You don't have all GetSocial dependencies libs in your Plugins/Android folder", "Add Libs", FixDependencies);
            DrawFixBox(areAllDependencyLibsPresent, "All GetSocial dependencies are present", drawFixDependencies);
        }

        private static void FixDependencies()
        {
            GetSocialSettingsEditor.CopyAndroidDependenciesToPlugins();
            RefreshAllManifestChecks();
        }

        private static void DrawFixBox(bool isFixed, string correctMsg, Action drawFixAction)
        {
            if(isFixed)
            {
                DrawAllCorrectBox(correctMsg);
            }
            else
            {
                drawFixAction();
            }
        }

        public static void DrawCheckPackageGUI()
        {
            var package = PlayerSettings.bundleIdentifier;
            if(string.IsNullOrEmpty(package))
            {
                EditorGUILayout.HelpBox("Package name is empty, please specify it in Android Player settings", MessageType.Warning);
            }

            if(package == "com.Company.ProductName")
            {
                EditorGUILayout.HelpBox("Please change the default Unity Bundle Identifier (com.Company.ProductName) to your package", MessageType.Warning);
            }
        }
        #endregion

        #region xml_parsing
        /*
         * <uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
         * <uses-permission android:name="android.permission.INTERNET" />
         */
        private static void AddBasicPermissions()
        {
            var appNode = GetAndroidApplicationNode();
            string ns = appNode.GetAndroidNameSpace();
            AddPermissionIfMissing(appNode.ParentNode, ns, UsesPermissionElementName, AccessNetoworkStatePermission);
            AddPermissionIfMissing(appNode.ParentNode, ns, UsesPermissionElementName, PermissionInternet);

            appNode.SaveOwnerDocument();
        }

        /*
         * Google Play Services:
         *
         *   <meta-data
         *       android:name="com.google.android.gms.version"
         *       android:value="@integer/google_play_services_version"/>
         */
        private static void AddPlayServicesVersion()
        {
            const string gmsVersionValue = "com.google.android.gms.version";

            var manifestNode = GetAndroidApplicationNode().ParentNode;
            string ns = manifestNode.GetAndroidNameSpace();

            XmlElement playServicesMetaData = FindElementWithAttribute(MetaDataElementName, "name", gmsVersionValue, ns, manifestNode);
            if(playServicesMetaData == null)
            {
                LogNotFoundMessage("Google Play Services");
                playServicesMetaData = CreateElementWithName(manifestNode.OwnerDocument, MetaDataElementName, gmsVersionValue, ns);
                playServicesMetaData.SetAttribute("value", ns, "@integer/google_play_services_version");
                manifestNode.AppendChild(playServicesMetaData);

                manifestNode.InsertBefore(manifestNode.OwnerDocument.CreateComment("Google Play Services"), playServicesMetaData);
            }

            manifestNode.SaveOwnerDocument();
        }

        /*
         *   <receiver
         *       android:name="im.getsocial.sdk.core.gms.InstallReferrerReceiver">
         *       <intent-filter>
         *           <action android:name="com.android.vending.INSTALL_REFERRER" />
         *       </intent-filter>
         *   </receiver>
         */
        private static void AddInstallReferrerReceiver()
        {
            var appNode = GetAndroidApplicationNode();
            string ns = appNode.GetAndroidNameSpace();

            XmlElement installReferrerReceiverElement = FindElementWithAttribute(ReceiverElementName, "name", InstallReferrerReceiverName, ns, appNode);
            if(installReferrerReceiverElement == null)
            {
                LogNotFoundMessage(InstallReferrerReceiverName);
                installReferrerReceiverElement = CreateElementWithName(appNode.OwnerDocument, ReceiverElementName, InstallReferrerReceiverName, ns);
                appNode.AppendChild(installReferrerReceiverElement);

                XmlElement intentFilter = appNode.OwnerDocument.CreateElement(IntentFilterElementName);
                installReferrerReceiverElement.AppendChild(intentFilter);

                var installReferrerAction = CreateElementWithName(appNode.OwnerDocument, ActionElementName, "com.android.vending.INSTALL_REFERRER", ns);
                intentFilter.AppendChild(installReferrerAction);
            }
            appNode.SaveOwnerDocument();
        }

        /*
         * <provider
         *     android:name="im.getsocial.sdk.core.provider.ImageContentProvider"
         *     android:authorities="YOUR_PACKAGE_NAME.smartinvite.images.provider"
         *     android:exported="true"/>
         */
        private static void AddImageContentProvider()
        {
            var appNode = GetAndroidApplicationNode();
            string ns = appNode.GetAndroidNameSpace();

            XmlElement provider = FindElementWithAttribute(ProviderElementName, "name", GetSocialImageContentProviderName, ns, appNode);
            if(provider == null)
            {
                LogNotFoundMessage(GetSocialImageContentProviderName);
                provider = CreateElementWithName(appNode.OwnerDocument, ProviderElementName, GetSocialImageContentProviderName, ns);
                provider.SetAttribute("authorities", ns, string.Format(GetSocialImageContentProviderFormat, PlayerSettings.bundleIdentifier));
                provider.SetAttribute("exported", ns, "true");
                appNode.AppendChild(provider);
            }

            appNode.SaveOwnerDocument();
        }

        /*
         * Adds the following permissions:
         *
         *      <uses-permission android:name="android.permission.WAKE_LOCK" />
         *      <uses-permission android:name="com.google.android.c2dm.permission.RECEIVE" />
         *
         *      <permission android:name="YOUR_PACKAGE_NAME.gcm.permission.C2D_MESSAGE" android:protectionLevel="signature" />
         *      <uses-permission android:name="YOUR_PACKAGE_NAME.gcm.permission.C2D_MESSAGE" />
         */
        private static void AddPushPermissions()
        {
            var appNode = GetAndroidApplicationNode();
            string ns = appNode.GetAndroidNameSpace();

            AddPermissionIfMissing(appNode.ParentNode, ns, UsesPermissionElementName, WakeLockAndroidPemission);
            AddPermissionIfMissing(appNode.ParentNode, ns, UsesPermissionElementName, ReceiveC2dmAndroidPemission);

            var c2dPermission = GetGcmPermission();
            XmlElement gcmPermission = AddPermissionIfMissing(appNode.ParentNode, ns, PermissionElementName, GetGcmPermission());
            gcmPermission.SetAttribute("protectionLevel", ns, "signature");
            AddPermissionIfMissing(appNode.ParentNode, ns, UsesPermissionElementName, c2dPermission);

            appNode.SaveOwnerDocument();
        }

        /*
         *  <receiver
         *       android:name="im.getsocial.sdk.core.GetSocialReceiver"
         *       android:permission="com.google.android.c2dm.permission.SEND"
         *
         *       <intent-filter>
         *           <action android:name="com.google.android.c2dm.intent.REGISTRATION" />
         *           <action android:name="com.google.android.c2dm.intent.RECEIVE" />
         *           <category android:name="YOUR_PACKAGE_NAME.gcm" />
         *       </intent-filter>
         *
         *       <intent-filter>
         *           <action android:name="im.getsocial.sdk.intent.RECEIVE" />
         *           <data android:scheme="getSocialNotificationId"/>
         *       </intent-filter>
         *   </receiver>
         */
        private static void AddGetSocialReceiver()
        {
            var appNode = GetAndroidApplicationNode();
            string ns = appNode.GetAndroidNameSpace();

            XmlElement receiverNode = FindElementWithAttribute(ReceiverElementName, "name", GetSocialReceiverName, ns, appNode);
            if(receiverNode == null)
            {
                LogNotFoundMessage(GetSocialReceiverName);
                receiverNode = CreateElementWithName(appNode.OwnerDocument, ReceiverElementName, GetSocialReceiverName, ns);
                receiverNode.SetAttribute(PermissionElementName, ns, "com.google.android.c2dm.permission.SEND");

                // GCM filter
                XmlElement gcmFilter = appNode.OwnerDocument.CreateElement(IntentFilterElementName);
                var registrationAction = CreateElementWithName(appNode.OwnerDocument, ActionElementName, "com.google.android.c2dm.intent.REGISTRATION", ns);
                var receiveAction = CreateElementWithName(appNode.OwnerDocument, ActionElementName, "com.google.android.c2dm.intent.RECEIVE", ns);
                var category = CreateElementWithName(appNode.OwnerDocument, CategoryElementName, string.Format("{0}.gcm", PlayerSettings.bundleIdentifier), ns);
                gcmFilter.AppendChild(registrationAction);
                gcmFilter.AppendChild(receiveAction);
                gcmFilter.AppendChild(category);
                receiverNode.AppendChild(gcmFilter);

                // GetSocial filter
                XmlElement gsFilter = appNode.OwnerDocument.CreateElement(IntentFilterElementName);
                var gsReceiveAction = CreateElementWithName(appNode.OwnerDocument, ActionElementName, "im.getsocial.sdk.intent.RECEIVE", ns);
                var scheme = CreateElementWithScheme(appNode.OwnerDocument, "data", "getSocialNotificationId", ns);
                gsFilter.AppendChild(gsReceiveAction);
                gsFilter.AppendChild(scheme);
                receiverNode.AppendChild(gsFilter);

                appNode.AppendChild(receiverNode);

                appNode.InsertBefore(appNode.OwnerDocument.CreateComment("GetSocial Receiver"), receiverNode);
            }

            appNode.SaveOwnerDocument();
        }

        /*
         * <activity android:name="im.getsocial.sdk.core.unity.GetSocialDeepLinkingActivity" android:exported="true">
         *     <intent-filter>
         *         <action android:name="android.intent.action.VIEW" />
         *         <category android:name="android.intent.category.DEFAULT" />
         *         <category android:name="android.intent.category.BROWSABLE" />
         *         <data android:scheme="getsocial" android:host="ti70h8r9x8Q74" />
         *     </intent-filter>
         * </activity>
         */
        private static void AddGetSocialDeepLinkingActivity()
        {
            var deepLinkingActivity = FindGetSocialDeepLinkingActivity();
            if(deepLinkingActivity == null)
            {
                Debug.Log("Adding <color=green>" + GetSocialDeepLinkingActivityName +  " </color>to your AndroidManifest.xml");
                var appNode = GetAndroidApplicationNode();
                string ns = appNode.GetAndroidNameSpace();

                XmlElement activity = appNode.OwnerDocument.CreateElement(ActivityElementName);
                activity.SetAttribute("name", ns, GetSocialDeepLinkingActivityName);
                activity.SetAttribute("exported", "true");

                XmlElement deepLinkingFilter = activity.OwnerDocument.CreateElement(IntentFilterElementName);

                var actionViewElem = CreateElementWithName(activity.OwnerDocument, ActionElementName, ActivityActionView, ns);
                var categoryDefault = CreateElementWithName(activity.OwnerDocument, CategoryElementName, ActivityCategoryDefault, ns);
                var categoryBrowsable = CreateElementWithName(activity.OwnerDocument, CategoryElementName, ActivityCategoryBrowsable, ns);
                var getSocialData = CreateElement(activity.OwnerDocument, "data", "scheme", "getsocial", ns);
                getSocialData.SetAttribute("host", ns, GetSocialSettings.AppId);

                deepLinkingFilter.AppendChild(actionViewElem);
                deepLinkingFilter.AppendChild(categoryDefault);
                deepLinkingFilter.AppendChild(categoryBrowsable);
                deepLinkingFilter.AppendChild(getSocialData);

                activity.AppendChild(deepLinkingFilter);
                appNode.AppendChild(activity);
                appNode.SaveOwnerDocument();
            }
        }

        private static XmlElement AddPermissionIfMissing(XmlNode manifestNode, string ns, string permissionElementName, string permission)
        {
            XmlElement permissionNode = FindElementWithAttribute(permissionElementName, "name", permission, ns, manifestNode);
            if(permissionNode == null)
            {
                LogNotFoundMessage(permission);
                permissionNode = CreateElementWithName(manifestNode.OwnerDocument, permissionElementName, permission, ns);
                manifestNode.AppendChild(permissionNode);
            }
            return permissionNode;
        }

        private static XmlNode GetAndroidApplicationNode()
        {
            var doc = new XmlDocument();
            doc.Load(ManifestPathInProject);

            if(doc == null)
            {
                Debug.LogError("Couldn't load " + ManifestPathInProject);
                return null;
            }

            XmlNode manNode = FindChildNode(doc, "manifest");
            XmlNode dict = FindChildNode(manNode, "application");

            if(dict == null)
            {
                Debug.LogError("Error parsing " + ManifestPathInProject);
                return null;
            }

            return dict;
        }

        private static XmlNode FindChildNode(XmlNode parent, string name)
        {
            XmlNode curr = parent.FirstChild;
            while(curr != null)
            {
                if(curr.Name.Equals(name))
                {
                    return curr;
                }
                curr = curr.NextSibling;
            }
            return null;
        }

        private static XmlElement FindElementWithAttribute(string elementName, string attributeName, string attributeValue, string ns, XmlNode parent)
        {
            var curr = parent.FirstChild;
            while(curr != null)
            {
                if(curr.Name.Equals(elementName) && curr is XmlElement && ((XmlElement)curr).GetAttribute(attributeName, ns) == attributeValue)
                {
                    return curr as XmlElement;
                }
                curr = curr.NextSibling;
            }
            return null;
        }

        private static XmlNode FindGetSocialDeepLinkingActivity()
        {
            var app = GetAndroidApplicationNode();
            for(int i = 0; i < app.ChildNodes.Count; i++)
            {
                if(app.ChildNodes[i].IsGetSocialDeepLinkingActivityNode())
                {
                    return app.ChildNodes[i];
                }
            }
            return null;
        }

        private static bool IsGetSocialDeepLinkingActivityNode(this XmlNode node)
        {
            return node.Name == ActivityElementName &&
                   node.Attributes["android:name"].Value == GetSocialDeepLinkingActivityName;
        }


        private static XmlElement CreateElementWithName(XmlDocument doc, string element, string name, string ns)
        {
            return CreateElement(doc, element, "name", name, ns);
        }

        private static XmlElement CreateElementWithScheme(XmlDocument doc, string element, string name, string ns)
        {
            return CreateElement(doc, element, "scheme", name, ns);
        }

        private static XmlElement CreateElement(XmlDocument doc, string element, string attribute, string attrName, string ns)
        {
            XmlElement newElement = doc.CreateElement(element);
            newElement.SetAttribute(attribute, ns, attrName);
            return newElement;
        }
        #endregion

        #region manifest_checks
        private static bool CheckIfAllBasicPermissionsPresent()
        {
            var appNode = GetAndroidApplicationNode();
            return
                CheckIfUsesPermissionPresent(AccessNetoworkStatePermission, appNode) &&
                CheckIfUsesPermissionPresent(PermissionInternet, appNode);
        }

        private static bool CheckIfGetSocialDeepLinkingActivityPresentAndCorrect()
        {
            var activity = FindGetSocialDeepLinkingActivity();
            return activity != null && activity.InnerXml.Contains(GetSocialDeepLinkingScheme);

        }

        private static bool DeepLinkingActivityHasCorrectIntentFilter(string activityNodeText)
        {
            return activityNodeText.Contains(ActivityActionView) &&
                   activityNodeText.Contains(ActivityCategoryDefault) &&
                   activityNodeText.Contains(ActivityCategoryBrowsable) &&

                   activityNodeText.Contains(GetSocialDeepLinkingScheme);
        }

        private static bool CheckIfImageContentProviderPresent()
        {
            var appNode = GetAndroidApplicationNode();
            return CheckIfElementPresent(ProviderElementName, "name", GetSocialImageContentProviderName, appNode);
        }

        // Push Permissions
        private static bool CheckIfAllPushPermissionsPresent()
        {
            var appNode = GetAndroidApplicationNode();
            return
                CheckIfUsesPermissionPresent(WakeLockAndroidPemission, appNode) &&
                CheckIfUsesPermissionPresent(ReceiveC2dmAndroidPemission, appNode) &&

                CheckIfUsesPermissionPresent(GetGcmPermission(), appNode) &&
                CheckIfPermissionPresent(GetGcmPermission(), appNode);
        }

        private static bool CheckIfPlayServicesPresent()
        {
            return CheckIfElementPresent(MetaDataElementName, "name", "com.google.android.gms.version", GetAndroidApplicationNode().ParentNode);
        }

        private static bool CheckIfElementPresent(string name, string attributeName, string attributeValue, XmlNode parent)
        {
            var appNode = GetAndroidApplicationNode();
            XmlElement element = FindElementWithAttribute(name, attributeName, attributeValue, appNode.GetAndroidNameSpace(), parent);
            return element != null;
        }

        private static bool CheckIfUsesPermissionPresent(string permission, XmlNode appNode)
        {
            return CheckIfElementPresent(UsesPermissionElementName, "name", permission, appNode.ParentNode);
        }

        private static bool CheckIfPermissionPresent(string permission, XmlNode appNode)
        {
            return CheckIfElementPresent(PermissionElementName, "name", permission, appNode.ParentNode);
        }

        private static bool CheckIfReceiverPresent(string receiver, XmlNode parent)
        {
            return CheckIfElementPresent(ReceiverElementName, "name", receiver, parent);
        }
        #endregion

        #region helpers
        private static bool DoesManifestExist()
        {
            return File.Exists(ManifestPathInProject);
        }

        private static void SaveOwnerDocument(this XmlNode xmlNode)
        {
            xmlNode.OwnerDocument.Save(ManifestPathInProject);
            RefreshAllManifestChecks();
            AssetDatabase.Refresh();
        }

        private static string GetAndroidNameSpace(this XmlNode node)
        {
            return node.GetNamespaceOfPrefix("android");
        }

        private static bool IsPushSetupComplete()
        {
            return arePermissionForPushesCorrect && isGetSocialReceiverPresent;
        }

        private static bool IsInstallTrackingAndReferralDataSetupComplete()
        {
            return arePlayServicesAdded && isInstallReferrerReceiverPresent;
        }

        private static string GetGcmPermission()
        {
            const string C2DCustomPermissionTemplate = "{0}.gcm.permission.C2D_MESSAGE";
            return string.Format(C2DCustomPermissionTemplate, PlayerSettings.bundleIdentifier);
        }
        #endregion

        #region dependency_libs
        private static bool CheckIfAllDependencyLibsPresent()
        {
            var dependencies = new [] { "gson*.jar" };
            var libJars = GetSocialEditorUtils.GetFiles(PluginsPathInProject, string.Join(";", dependencies), SearchOption.AllDirectories);
            return libJars.Length >= dependencies.Length;
        }
        #endregion

        #region draw_helpers
        private static void FixMessageGroupTitle(string title)
        {
            GUILayout.Space(7f);
            EditorGUILayout.LabelField(Bullet + " " + title + ":", EditorStyles.boldLabel);
        }

        private static void DrawFixProjectConfigurationMessage(string message, string fixButtonText, Action fixAction)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Space(5f);
            var labelStyle = EditorStyles.label;
            labelStyle.wordWrap = true;
            EditorGUILayout.LabelField(message, labelStyle);

            var backupColor = GUI.color;
            GUI.color = Color.green;
            EditorGUILayout.Space();
            if(GUILayout.Button(fixButtonText))
            {
                fixAction();
            }
            GUI.color = backupColor;
            EditorGUILayout.EndVertical();
        }

        private static void DrawAllCorrectBox(string text)
        {
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            GUILayout.Space(5f);
            var backupColor = GUI.color;
            GUI.color = Color.green;
            EditorGUILayout.LabelField(text);
            GUI.color = backupColor;

            EditorGUILayout.EndHorizontal();
        }
        #endregion

        private static void LogNotFoundMessage(string what, string color = "yellow")
        {
            Debug.Log(string.Format("<color={0}>{1}</color> not found in your manifest. \n\t Adding <color={0}>{1}</color>.", color, what));
        }
    }
}

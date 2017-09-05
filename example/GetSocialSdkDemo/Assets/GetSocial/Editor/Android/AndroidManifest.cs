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

using System;
using System.Collections.Generic;
using System.Xml;
using GetSocialSdk.Core;
using UnityEditor;
using UnityEngine;

namespace GetSocialSdk.Editor
{
    public class AndroidManifest
    {
        #region manifest constants

        const string MetaDataElementName = "meta-data";
        const string PermissionElementName = "permission";
        const string UsesPermissionElementName = "uses-permission";
        const string ActivityElementName = "activity";
        const string ReceiverElementName = "receiver";
        const string IntentFilterElementName = "intent-filter";
        const string ActionElementName = "action";
        const string CategoryElementName = "category";
        const string ProviderElementName = "provider";

        // Activity actions
        const string ActivityActionMain = "android.intent.action.MAIN";
        const string ActivityActionView = "android.intent.action.VIEW";

        // Activity categories
        const string ActivityCategoryLauncher = "android.intent.category.LAUNCHER";
        const string ActivityCategoryDefault = "android.intent.category.DEFAULT";
        const string ActivityCategoryBrowsable = "android.intent.category.BROWSABLE";

        // Permissions
        public const string InternetPermission = "android.permission.INTERNET";
        public const string AccessNetoworkStatePermission = "android.permission.ACCESS_NETWORK_STATE";

        // CGM permissions
        const string WakeLockAndroidPemission = "android.permission.WAKE_LOCK";
        const string ReceiveC2dmAndroidPemission = "com.google.android.c2dm.permission.RECEIVE";

        #endregion


        private readonly string _path;
        private readonly XmlDocument _xmlDocument;
        private readonly XmlNode _manifestNode;
        private readonly XmlNode _applicationNode;
        private readonly string _androidNamespace;


        #region public api

        public AndroidManifest(string path)
        {
            _path = path;

            _xmlDocument = new XmlDocument();
            _xmlDocument.Load(path);

            _manifestNode = FindChildNode(_xmlDocument, "manifest");
            _applicationNode = FindChildNode(_manifestNode, "application");

            if (_applicationNode == null)
            {
                throw new ArgumentException("Failed to parse AndroidManifest with path " + path);
            }

            _androidNamespace = _applicationNode.GetNamespaceOfPrefix("android");
        }

        public bool ContainsUsesPermissions(params string[] permissions)
        {
            var result = true;
            foreach (var permission in permissions)
            {
                result &= CheckIfUsesPermissionPresent(permission);
            }
            return result;
        }

        public void AddUsesPermissions(params string[] permissions)
        {
            foreach (var permission in permissions)
            {
                AddUsesPermissionIfMissing(permission);
            }
        }

        public void AddPermission(string permission, Dictionary<string, string> extraParams = null)
        {
            AddPermissionIfMissing(permission, extraParams);
        }
        
        
        public bool ContainsMetaTag(string name)
        {
            return FindElementWithAttribute(MetaDataElementName, "name", name, _androidNamespace, _applicationNode) != null;
        }

        public bool ContainsMetaTagWithValue(string name, string value)
        {
            XmlElement metaTag = FindElementWithAttribute(MetaDataElementName, "name", name, _androidNamespace, _applicationNode);
            if (metaTag != null)
            {
                var attribute = metaTag.GetAttribute("value", _androidNamespace);
                return value.Equals(attribute);
            }
            return false;
        }

        public void AddMetaTag(string name, string value)
        {
            XmlElement metaTag = FindElementWithAttribute(MetaDataElementName, "name", name, _androidNamespace, _applicationNode);
            if (metaTag == null)
            {
                LogNotFoundMessage(name);
                metaTag = CreateElementWithName(_xmlDocument, MetaDataElementName, name, _androidNamespace);
                _applicationNode.AppendChild(metaTag);
            }
            metaTag.SetAttribute("value", _androidNamespace, value);
        }

        public bool ContainsContentProvider(string name)
        {
            return FindElementWithAttribute(ProviderElementName, "name", name, _androidNamespace, _applicationNode) != null;
        }

        public bool ContainsContentProviderWithValues(string name, string authority, bool exported)
        {
            XmlElement contentProviderNode = FindElementWithAttribute(ProviderElementName, "name", name, _androidNamespace, _applicationNode);
            if (contentProviderNode != null)
            {
                var authorityTag = contentProviderNode.GetAttribute("authorities", _androidNamespace);
                var exportedTag = contentProviderNode.GetAttribute("exported", _androidNamespace);
                return authority.Equals(authorityTag) && exported.ToString().ToLower().Equals(exportedTag);
            }
            return false;
        }

        public void AddContentProvider(string name, string authority, bool exported = false)
        {
            XmlElement contentProviderNode = FindElementWithAttribute(ProviderElementName, "name", name, _androidNamespace, _applicationNode);
            if (contentProviderNode == null)
            {
                LogNotFoundMessage(name);
                contentProviderNode = CreateElementWithName(_xmlDocument, ProviderElementName, name, _androidNamespace);
                _applicationNode.AppendChild(contentProviderNode);
            }

            contentProviderNode.SetAttribute("authorities", _androidNamespace, authority);
            contentProviderNode.SetAttribute("exported", _androidNamespace, exported.ToString().ToLower());
            contentProviderNode.SetAttribute("enabled", _androidNamespace, "true");
        }

        public bool ContainsDeepLinkingActivity(string name)
        {
            return FindActivityNode(name) != null;
        }

        public void AddDeepLinkingActivity(string name, string hostForDeepLinkViaScheme, string hostForDeepLinkViaAppLinks, string altHostForDeepLinkViaAppLinks)
        {
            var deepLinkingActivity = FindActivityNode(name);
            var newActivityNode = CreateNewDeepLinkingActivity(name, hostForDeepLinkViaScheme, hostForDeepLinkViaAppLinks, altHostForDeepLinkViaAppLinks);

            if (deepLinkingActivity == null)
            {
                _applicationNode.AppendChild(newActivityNode);
                Debug.Log("[GetSocial] Adding `" + name + "` to the AndroidManifest.xml");
            }
            else
            {
                _applicationNode.ReplaceChild(newActivityNode, deepLinkingActivity);
                Debug.Log("[GetSocial] Updating `" + name + "` in the AndroidManifest.xml");
            }
        }

        public bool ContainsReceiver(string name, int atIndex = 0)
        {
            var receiverElement = FindElementWithAttribute(ReceiverElementName, "name", name, _androidNamespace, _applicationNode);
            if (receiverElement != null)
            {
                return _applicationNode.ChildNodes[atIndex] == receiverElement;
            }

            return false;
        }

        public void RemoveInstallReferrerReceiver(string name)
        {
            XmlElement installReferrerReceiverElement = FindElementWithAttribute(ReceiverElementName, "name", name, _androidNamespace, _applicationNode);
            if (installReferrerReceiverElement != null)
            {
                Debug.Log(string.Format("[GetSocial] Removed deprecated {0}", name));
                _applicationNode.RemoveChild(installReferrerReceiverElement);
            }
        }
        
        public void AddInstallReferrerReceiver(string name)
        {
            var installReferrerReceiverElement = CreateElementWithName(_xmlDocument, ReceiverElementName, name, _androidNamespace);
            _applicationNode.InsertAt(installReferrerReceiverElement, 0);

            XmlElement intentFilter = _xmlDocument.CreateElement(IntentFilterElementName);
            installReferrerReceiverElement.AppendChild(intentFilter);

            var installReferrerAction = CreateElementWithName(_xmlDocument, ActionElementName, "com.android.vending.INSTALL_REFERRER", _androidNamespace);
            intentFilter.AppendChild(installReferrerAction);
        }

        public void Save()
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };

            using (XmlWriter xmlWriter = XmlWriter.Create(_path, settings))
            {
                _xmlDocument.Save(xmlWriter);
            }
        }
        #endregion


        #region private methods

        bool CheckIfUsesPermissionPresent(string permission)
        {
            return CheckIfElementPresent(UsesPermissionElementName, "name", permission, _applicationNode.ParentNode);
        }


        void AddUsesPermissionIfMissing(string permission)
        {
            XmlElement permissionNode = FindElementWithAttribute(UsesPermissionElementName, "name", permission, _androidNamespace, _manifestNode);
            if (permissionNode == null)
            {
                LogNotFoundMessage(permission);
                permissionNode = CreateElementWithName(_manifestNode.OwnerDocument, UsesPermissionElementName, permission, _androidNamespace);
                _manifestNode.AppendChild(permissionNode);
            }
        }

        public bool CheckIfPermissionPresent(string permission)
        {
            return CheckIfElementPresent(PermissionElementName, "name", permission, _applicationNode.ParentNode);
        }

        void AddPermissionIfMissing(string permission, Dictionary<string, string> extraParams = null)
        {
            XmlElement permissionNode = FindElementWithAttribute(PermissionElementName, "name", permission, _androidNamespace, _manifestNode);
            if (permissionNode == null)
            {
                LogNotFoundMessage(permission);
                permissionNode = CreateElementWithName(_manifestNode.OwnerDocument, PermissionElementName, permission, _androidNamespace);
                if (extraParams != null)
                {
                    foreach (KeyValuePair<string, string> attribute in extraParams)
                    {
                        permissionNode.SetAttribute(attribute.Key, attribute.Value);
                    }
                }
                _manifestNode.AppendChild(permissionNode);
            }
        }

        bool CheckIfElementPresent(string name, string attributeName, string attributeValue, XmlNode parent)
        {
            XmlElement element = FindElementWithAttribute(name, attributeName, attributeValue, _androidNamespace, parent);
            return element != null;
        }

        XmlNode FindActivityNode(string name)
        {
            for (int i = 0; i < _applicationNode.ChildNodes.Count; i++)
            {
                var childNode = _applicationNode.ChildNodes[i];
                if (childNode.Name == ActivityElementName && childNode.Attributes["android:name"].Value == name)
                {
                    return childNode;
                }
            }
            return null;
        }
        
        private XmlElement CreateNewDeepLinkingActivity(string name, string hostForDeepLinkViaScheme, string hostForDeepLinkViaAppLinks, string altHostForDeepLinkViaAppLinks)
        {
            XmlElement activity = _xmlDocument.CreateElement(ActivityElementName);
            activity.SetAttribute("name", _androidNamespace, name);
            activity.SetAttribute("exported", "true");

            XmlElement deepLinkingFilter = _xmlDocument.CreateElement(IntentFilterElementName);

            var actionViewElem = CreateElementWithName(_xmlDocument, ActionElementName, ActivityActionView, _androidNamespace);
            var categoryDefault = CreateElementWithName(_xmlDocument, CategoryElementName, ActivityCategoryDefault, _androidNamespace);
            var categoryBrowsable = CreateElementWithName(_xmlDocument, CategoryElementName, ActivityCategoryBrowsable, _androidNamespace);

            deepLinkingFilter.AppendChild(actionViewElem);
            deepLinkingFilter.AppendChild(categoryDefault);
            deepLinkingFilter.AppendChild(categoryBrowsable);
            deepLinkingFilter.AppendChild(CreateDataTagWithSchemeAndHost("getsocial", hostForDeepLinkViaScheme));
            deepLinkingFilter.AppendChild(CreateDataTagWithSchemeAndHost("https", hostForDeepLinkViaAppLinks));
            deepLinkingFilter.AppendChild(CreateDataTagWithSchemeAndHost("https", altHostForDeepLinkViaAppLinks));
            
            // TODO: refactor
            // add assosiated domains for testing environement for demo app
            if (PlayerSettingsCompat.bundleIdentifier.Equals(GetSocialSettingsEditor.DemoAppPackage))
            {
                var testingHost = string.Format("{0}.testing.{1}", GetSocialSettings.GetSocialDomainPrefixForDeeplinking, GetSocialSettingsEditor.GetSocialSmartInvitesLinkDomain);
                deepLinkingFilter.AppendChild(CreateDataTagWithSchemeAndHost("https", testingHost));
                
                var altTestingHost = GetSocialSettings.UseCustomDomainForDeeplinking 
                    ? GetSocialSettings.CustomDomainForDeeplinking 
                    : string.Format("{0}-gsalt.testing.{1}", GetSocialSettings.GetSocialDomainPrefixForDeeplinking, GetSocialSettingsEditor.GetSocialSmartInvitesLinkDomain);
                deepLinkingFilter.AppendChild(CreateDataTagWithSchemeAndHost("https", altTestingHost));
            }

            activity.AppendChild(deepLinkingFilter);

            return activity;
        }

        XmlElement CreateDataTagWithSchemeAndHost(string scheme, string host)
        {
            var tagElement = CreateElement(_xmlDocument, "data", "scheme", scheme, _androidNamespace);
            tagElement.SetAttribute("host", _androidNamespace, host);

            return tagElement;
        }
        #endregion


        #region private static methods

        static XmlElement FindElementWithAttribute(string elementName, string attributeName, string attributeValue, string ns, XmlNode parent)
        {
            var curr = parent.FirstChild;
            while (curr != null)
            {
                if (curr.Name.Equals(elementName) && curr is XmlElement && ((XmlElement)curr).GetAttribute(attributeName, ns) == attributeValue)
                {
                    return curr as XmlElement;
                }
                curr = curr.NextSibling;
            }
            return null;
        }

        static XmlNode FindChildNode(XmlNode parent, string name)
        {
            XmlNode curr = parent.FirstChild;
            while (curr != null)
            {
                if (curr.Name.Equals(name))
                {
                    return curr;
                }
                curr = curr.NextSibling;
            }
            return null;
        }

        static XmlElement CreateElementWithName(XmlDocument doc, string element, string name, string ns)
        {
            return CreateElement(doc, element, "name", name, ns);
        }

        static XmlElement CreateElement(XmlDocument doc, string element, string attribute, string attrName, string ns)
        {
            XmlElement newElement = doc.CreateElement(element);
            newElement.SetAttribute(attribute, ns, attrName);
            return newElement;
        }


        static void LogNotFoundMessage(string what)
        {
            Debug.Log(string.Format("[GetSocial] `{0}` not found in your manifest. \n\t Adding `{0}` to AndroidManifest.xml.", what));
        }

        #endregion
    }
}
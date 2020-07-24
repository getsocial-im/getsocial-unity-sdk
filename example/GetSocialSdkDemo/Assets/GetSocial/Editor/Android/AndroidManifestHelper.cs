using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using System;
using System.Xml;
namespace GetSocialSdk.Core
{
    public static class AndroidManifestHelper
    {
        const string MainManifestPath = "Plugins/Android/AndroidManifest.xml";
        public static void RemoveSdk6Configs() 
        {
            var manifestPath = Path.Combine(Application.dataPath, MainManifestPath); 
            if (!File.Exists(manifestPath)) return;
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(manifestPath);
            RemoveGetSocial(xmlDocument);
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "    ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };

            using (XmlWriter xmlWriter = XmlWriter.Create(manifestPath, settings))
            {
                xmlDocument.Save(xmlWriter);
            }
        }

        private static bool RemoveGetSocial(XmlNode xmlNode)
        {
            if (xmlNode.Attributes != null)
            {
                var attributeNode = xmlNode.Attributes.GetNamedItem("android:name");
                var isGetSocial = attributeNode != null && attributeNode.Value.StartsWith("im.getsocial.sdk");
                if (isGetSocial)
                {
                    return true;
                } 
            }
            var toRemove = new List<XmlNode>();
            for (var i = 0; i < xmlNode.ChildNodes.Count; i++)
            {
                var child = xmlNode.ChildNodes[i];
                if (RemoveGetSocial(child)) toRemove.Add(child);
            }
            foreach (var child in toRemove)
            {
                xmlNode.RemoveChild(child);
            }
            return false;
        }
    }
}
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Hawkeye.Logging;
using System;

namespace Hawkeye.Configuration
{
    partial class SettingsManager
    {
        private class SettingsManagerImplementation
        {
            private const string implementationVersion = "1.0.0";

            private static readonly ILogService log = LogManager.GetLogger<SettingsManagerImplementation>();
            
            private Dictionary<string, SettingsStore> stores = new Dictionary<string, SettingsStore>();
            private XmlDocument settingsDocument = null;

            public ISettingsStore GetStore(string key)
            {
                return null;
            }

            public void CreateDefaultSettingsFile(string filename)
            {
                const string defaultContent = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<settings version=""1.0.0"">
  <hawkeye>
    <layouts></layouts>
    <configuration></configuration>
  </hawkeye>
  <plugins>
    <snapshot></snapshot>
    <reflector></reflector>
  </plugins>
</settings>";

                CreateBackup(filename);
                File.WriteAllText(filename, defaultContent, Encoding.UTF8);
            }

            public void Load(string filename)
            {
                if (settingsDocument == null)
                {
                    settingsDocument = new XmlDocument();
                    settingsDocument.Load(filename);
                }

                var rootNode = settingsDocument.ChildNodes.Cast<XmlNode>().SingleOrDefault(xn => xn.Name == "settings");
                if (rootNode == null)
                {
                    rootNode = settingsDocument.CreateElement("settings");
                    var versionNode = settingsDocument.CreateAttribute("version");
                    versionNode.Value = implementationVersion;
                    rootNode.Attributes.Append(versionNode);
                    return;
                }

                var children = rootNode.ChildNodes.Cast<XmlNode>();
                if (children.Count() == 0)
                {
                    // Add Hawkeye node
                    rootNode.AppendChild(settingsDocument.CreateElement(HawkeyeStoreKey));
                    return;
                }

                foreach (var node in children)
                {
                    if (node.Name == HawkeyeStoreKey)
                        LoadHawkeyeSettings(node);
                    else
                    {
                        var prefix = node.Name;
                        foreach (var subnode in node.ChildNodes.Cast<XmlNode>())
                            LoadSettings(subnode, string.Format("{0}/{1}", prefix, subnode.Name));
                    }
                }
            }

            private void LoadHawkeyeSettings(XmlNode node)
            {
                var children = node.ChildNodes.Cast<XmlNode>();
                if (children.Count() == 0) return;

                var configurationNode = children.SingleOrDefault(n => n.Name == "configuration");
                if (configurationNode != null) LoadSettings(configurationNode, HawkeyeStoreKey);

                var layoutNode = children.SingleOrDefault(n => n.Name == "layouts");
                if (layoutNode != null) LayoutManager.Load(layoutNode);
            }

            private void LoadSettings(XmlNode node, string storeKey)
            {
                var store = new SettingsStore();
                store.Content = node.InnerText;
                stores.Add(storeKey, store);
            }

            public void Save(string filename)
            {
                CreateBackup(filename);
                // TODO: save settings
                settingsDocument.Save(filename);
            }

            private void CreateBackup(string filename)
            {
                if (File.Exists(filename))
                {
                    // Create a backup copy
                    var backup = filename + ".bak";
                    try
                    {
                        File.Copy(filename, backup, true);
                    }
                    catch (Exception ex)
                    {
                        log.Error(string.Format(
                            "Could not createb backup copy of settings file: {0}", ex.Message), ex);
                    }
                }
            }
        }
    }
}

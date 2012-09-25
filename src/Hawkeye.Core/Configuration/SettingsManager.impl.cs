using System.IO;
using System.Collections.Generic;

using Hawkeye.Logging;

namespace Hawkeye.Configuration
{
    partial class SettingsManager
    {
        private class SettingsManagerImplementation
        {
            private static readonly ILogService log = LogManager.GetLogger<SettingsManagerImplementation>();            
            private static Dictionary<string, SettingsStore> stores = new Dictionary<string, SettingsStore>();
            
            public ISettingsStore GetStore(string key)
            {
                return null;
            }

            public void Load(string filename)
            {
                //TODO: load settings                
            }

            public void Save(string filename)
            {
                //TODO: save settings
                if (File.Exists(filename))
                {
                    // Create a backup copy
                    var backup = filename + ".bak";
                    File.Copy(filename, backup, true);
                }
            }
        }
    }
}

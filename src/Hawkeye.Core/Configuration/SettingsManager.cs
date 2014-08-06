using System;
using System.IO;
using System.Text;

using Hawkeye.Logging;

namespace Hawkeye.Configuration
{
    internal static partial class SettingsManager
    {
        private static readonly string defaultSettingsFileName = "hawkeye.settings";
        private static readonly ILogService log = LogManager.GetLogger(typeof(SettingsManager));
        private static string settingsFileName = string.Empty;
        private static SettingsManagerImplementation implementation = null;

        public static readonly string HawkeyeStoreKey = "hawkeye";

        private static SettingsManagerImplementation Implementation
        {
            get
            {
                if (implementation == null)
                    throw new ApplicationException("SettingsManager class was not initialized.");
                return implementation;
            }
        }

        public static string SettingsFileName
        {
            get { return settingsFileName; }
        }

        /// <summary>
        /// Initializes the Settings manager with the specified filename.
        /// </summary>
        /// <param name="filename">The Settings file name (optional).</param>
        public static void Initialize(string filename = "")
        {
            string resolved = filename;
            try
            {
                if (string.IsNullOrEmpty(resolved))
                    resolved = defaultSettingsFileName;

                if (!Path.IsPathRooted(resolved)) // combine with default directory
                    resolved = Path.Combine(
                        HawkeyeApplication.Shell.ApplicationInfo.ApplicationDataDirectory, resolved);

                settingsFileName = resolved; // This is the settings file

                implementation = new SettingsManagerImplementation();
                if (!File.Exists(settingsFileName)) // Check file exists
                    implementation.CreateDefaultSettingsFile(settingsFileName);
                implementation.Load(settingsFileName);
            }
            catch (Exception ex)
            {
                var builder = new StringBuilder();
                builder.AppendLine("There was an error during SettingsManager initialization:");
                builder.AppendFormat("\t- Provided settings file name was: '{0}'.", filename ?? "[NULL]");
                builder.AppendFormat("\t- Resolved settings file name was: '{0}'.", resolved ?? "[NULL]");
                builder.AppendFormat("\t- Error is: {0}.", ex.Message);
                log.Error(builder.ToString(), ex);
#if DEBUG
                throw;
#endif
            }
        }

        public static ISettingsStore GetStore(string key)
        {
            return Implementation.GetStore(key);
        }

        public static ISettingsStore GetHawkeyeStore()
        {
            return GetStore(HawkeyeStoreKey);
        }

        public static void Save()
        {
            Implementation.Save(settingsFileName);
        }
    }
}

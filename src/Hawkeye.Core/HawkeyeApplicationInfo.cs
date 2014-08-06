using System;
using System.IO;

namespace Hawkeye
{
    internal class HawkeyeApplicationInfo : IHawkeyeApplicationInfo
    {
        private static readonly string hawkeyeDataDirectory;

        /// <summary>
        /// Initializes the <see cref="HawkeyeApplicationInfo" /> class.
        /// </summary>
        static HawkeyeApplicationInfo()
        {
            hawkeyeDataDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), 
                "Hawkeye");
        }

        #region IHawkeyeApplicationInfo Members

        /// <summary>
        /// Gets the application data directory.
        /// </summary>
        public string ApplicationDataDirectory
        {
            get { return hawkeyeDataDirectory; }
        }

        #endregion
    }
}

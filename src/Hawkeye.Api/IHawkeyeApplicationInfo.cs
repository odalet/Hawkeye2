using System;

namespace Hawkeye
{
    /// <summary>
    /// Stores various information relative to the Hawkeye application
    /// </summary>
    public interface IHawkeyeApplicationInfo
    {
        /// <summary>
        /// Gets the application data directory.
        /// </summary>
        string ApplicationDataDirectory { get; }
    }
}

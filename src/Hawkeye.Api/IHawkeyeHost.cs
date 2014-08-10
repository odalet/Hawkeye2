using System;
using Hawkeye.Logging;
using Hawkeye.Configuration;

namespace Hawkeye
{
    /// <summary>
    /// Interface representing the Hawkeye application's host: used by plugins.
    /// </summary>
    public interface IHawkeyeHost
    {
        /// <summary>
        /// Occurs when the value of <see cref="CurrentWindowInfo"/> changes.
        /// </summary>
        event EventHandler CurrentWindowInfoChanged;

        /// <summary>
        /// Gets the <see cref="IWindowInfo"/> that represents the currently inspected control.
        /// </summary>
        IWindowInfo CurrentWindowInfo { get; }

        /// <summary>
        /// Gets a logger instance for the specified type.
        /// </summary>
        /// <param name="type">The type for which to log.</param>
        /// <returns>
        /// An instance of an object implementing <see cref="ILogService"/>.
        /// </returns>
        ILogService GetLogger(Type type);

        /// <summary>
        /// Gets the settings stored in Hawkeye configuration file matching the specified key.
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item>If the key does not match any saved settings, a new empty store is created.</item>
        /// <item>
        /// If the key is null or empty, a read-only store containing representing 
        /// the Hawkeye application settings is returned.
        /// </item>
        /// </list>
        /// </remarks>
        /// <param name="key">The settings store key.</param>
        /// <returns>An <see cref="ISettingsStore"/> containing the requested settings data.</returns>
        ISettingsStore GetSettings(string key);

        /// <summary>
        /// Gets a value containing information relative to the Hawkeye application.
        /// </summary>
        IHawkeyeApplicationInfo ApplicationInfo { get; }
    }
}

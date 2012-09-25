using System;
using Hawkeye.Logging;
using Hawkeye.Configuration;

namespace Hawkeye
{
    /// <summary>
    /// Exposes the Hawkeye application API to plugins
    /// </summary>
    public static class HawkeyeHost
    {
        /// <summary>
        /// Provides a Hawkeye application host implementation 
        /// to the <see cref="HawkeyeHost"/> public facade.
        /// </summary>
        internal interface IHost
        {
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
            ISettingsStore GetSettings(string key = "");
        }

        private static IHost implementation = null;

        #region Initialization

        /// <summary>
        /// Gets a value indicating whether the Hawkeye Host is initialized.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the Hawkeye Host is initialized; otherwise, <c>false</c>.
        /// </value>
        public static bool IsInitialized
        {
            get { return implementation != null; }
        }

        internal static void Initialize(IHost host)
        {
            implementation = host;
        }

        private static void EnsureInitialized()
        {
            if (!IsInitialized) throw new ApplicationException(
                "The Hawkeye application Host is not initialized.");
        }

        #endregion

        #region Logging

        /// <summary>
        /// Gets the default logger.
        /// </summary>
        /// <returns>An instance of an object implementing <see cref="ILogService"/>.</returns>
        public static ILogService GetLogger()
        {
            return GetLogger(null);
        }

        /// <summary>
        /// Gets a logger instance for the specified type.
        /// </summary>
        /// <typeparam name="T">The type for which to log.</typeparam>
        /// <returns>
        /// An instance of an object implementing <see cref="ILogService"/>.
        /// </returns>
        public static ILogService GetLogger<T>()
        {
            return GetLogger(typeof(T));
        }

        /// <summary>
        /// Gets a logger instance for the specified type.
        /// </summary>
        /// <param name="type">The type for which to log.</param>
        /// <returns>
        /// An instance of an object implementing <see cref="ILogService"/>.
        /// </returns>
        public static ILogService GetLogger(Type type)
        {
            EnsureInitialized();
            return implementation.GetLogger(type);
        }

        #endregion

        #region Settings

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
        public static ISettingsStore GetSettings(string key = "")
        {
            EnsureInitialized();
            return implementation.GetSettings(key);
        }

        /// <summary>
        /// Gets an object containing the Hawkeye settings object.
        /// </summary>
        /// <remarks>
        /// The returned settings store is read-only.
        /// </remarks>
        /// <returns>An <see cref="ISettingsStore"/> representing the Hawkeye application settings.</returns>
        public static ISettingsStore GetHawkeyeSettings()
        {
            return GetSettings();
        }

        #endregion
    }
}

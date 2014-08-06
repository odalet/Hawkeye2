using System;
using Hawkeye.Logging;
using Hawkeye.Configuration;

namespace Hawkeye
{
    /// <summary>
    /// Exposes the Hawkeye application API to plugins
    /// </summary>
    internal class HawkeyeHost : IHawkeyeHost
    {
        private IHawkeyeHost implementation = null;
        
        #region Initialization

        /// <summary>
        /// Gets a value indicating whether the Hawkeye Host is initialized.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the Hawkeye Host is initialized; otherwise, <c>false</c>.
        /// </value>
        public bool IsInitialized
        {
            get { return implementation != null; }
        }

        internal void Initialize(IHawkeyeHost host)
        {
            implementation = host;
            implementation.CurrentWindowInfoChanged += (s, e) =>
            {
                if (CurrentWindowInfoChanged != null)
                    CurrentWindowInfoChanged(null, e);
            };
        }

        private void EnsureInitialized()
        {
            if (!IsInitialized) throw new ApplicationException(
                "The Hawkeye application Host is not initialized.");
        }

        #endregion
        
        #region IHawkeyeHost Members

        /// <summary>
        /// Occurs when the value of <see cref="CurrentWindowInfo"/> changes.
        /// </summary>
        public event EventHandler CurrentWindowInfoChanged;

        /// <summary>
        /// Gets a logger instance for the specified type.
        /// </summary>
        /// <param name="type">The type for which to log.</param>
        /// <returns>
        /// An instance of an object implementing <see cref="ILogService"/>.
        /// </returns>
        public ILogService GetLogger(Type type)
        {
            EnsureInitialized();
            return implementation.GetLogger(type);
        }

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
        public ISettingsStore GetSettings(string key)
        {
            EnsureInitialized();
            return implementation.GetSettings(key);
        }

        /// <summary>
        /// Gets a value containing information relative to the Hawkeye application.
        /// </summary>
        public IHawkeyeApplicationInfo ApplicationInfo
        {
            get
            {
                EnsureInitialized();
                return implementation.ApplicationInfo;
            }
        }

        /// <summary>
        /// Gets the <see cref="IWindowInfo"/> that represents the currently inspected control.
        /// </summary>
        public IWindowInfo CurrentWindowInfo
        {
            get
            {
                EnsureInitialized();
                return implementation.CurrentWindowInfo;
            }
        }

        #endregion
    }
}

using Hawkeye.Logging;
using Hawkeye.Configuration;

namespace Hawkeye
{
    /// <summary>
    /// Extennsions methods on <see cref="IHawkeyeHost"/>.
    /// </summary>
    public static class HostExtensions
    {
        /// <summary>
        /// Gets the default logger.
        /// </summary>
        /// <returns>An instance of an object implementing <see cref="ILogService"/>.</returns>
        public static ILogService GetLogger(this IHawkeyeHost host)
        {
            return host.GetLogger(null);
        }

        /// <summary>
        /// Gets a logger instance for the specified type.
        /// </summary>
        /// <typeparam name="T">The type for which to log.</typeparam>
        /// <returns>
        /// An instance of an object implementing <see cref="ILogService"/>.
        /// </returns>
        public static ILogService GetLogger<T>(this IHawkeyeHost host)
        {
            return host.GetLogger(typeof(T));
        }

        /// <summary>
        /// Gets an object containing the Hawkeye settings object.
        /// </summary>
        /// <remarks>
        /// The returned settings store is read-only.
        /// </remarks>
        /// <returns>An <see cref="ISettingsStore"/> representing the Hawkeye application settings.</returns>
        public static ISettingsStore GetHawkeyeSettings(this IHawkeyeHost host)
        {
            return host.GetSettings(string.Empty);
        }
    }
}

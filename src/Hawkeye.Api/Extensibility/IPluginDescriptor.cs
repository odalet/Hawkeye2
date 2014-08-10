using System;

namespace Hawkeye.Extensibility
{
    /// <summary>
    /// Interface that needs be implemented to declare plugins
    /// </summary>
    public interface IPluginDescriptor
    {
        /// <summary>
        /// Gets this plugin name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets this plugin version.
        /// </summary>
        Version Version { get; }

        /// <summary>
        /// Creates an instance of the plugin passing it the specified host.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <returns>Created plugin instance.</returns>
        IPlugin Create(IHawkeyeHost host);
    }
}

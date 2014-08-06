using System;

namespace Hawkeye
{
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

        IPlugin Create(IHawkeyeHost host);
    }
}

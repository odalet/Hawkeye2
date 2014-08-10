namespace Hawkeye.Extensibility
{
    /// <summary>
    /// Base interface for Hawkeye plugins
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Gets the descriptor instance that created this plugin.
        /// </summary>
        IPluginDescriptor Descriptor { get; }

        /// <summary>
        /// Initializes this plugin passing it the specified host.
        /// </summary>
        /// <param name="host">The host.</param>
        void Initialize(IHawkeyeHost host);
    }
}

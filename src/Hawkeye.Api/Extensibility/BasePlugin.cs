using System;
using System.Collections.Generic;
using System.Text;

namespace Hawkeye.Extensibility
{
    /// <summary>
    /// Base class helping in building plugins
    /// </summary>
    public abstract class BasePlugin : IPlugin
    {
        private readonly IPluginDescriptor descriptor;
        private bool initialized = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePlugin"/> class.
        /// </summary>
        /// <param name="pluginDescriptor">The plugin descriptor.</param>
        /// <exception cref="System.ArgumentNullException">pluginDescriptor</exception>
        public BasePlugin(IPluginDescriptor pluginDescriptor)
        {
            if (pluginDescriptor == null)
                throw new ArgumentNullException("pluginDescriptor");
            descriptor = pluginDescriptor;
        }

        #region IPlugin Members

        /// <summary>
        /// Gets the descriptor instance that created this plugin.
        /// </summary>
        public IPluginDescriptor Descriptor
        {
            get { return descriptor; }
        }

        /// <summary>
        /// Initializes this plugin passing it the specified host.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <exception cref="System.ArgumentNullException">host</exception>
        public void Initialize(IHawkeyeHost host)
        {
            if (host == null) throw new ArgumentNullException("host");
            Host = host;
            initialized = true;
            OnInitialized();
        }

        #endregion

        /// <summary>
        /// Gets the host for this plugin.
        /// </summary>
        protected IHawkeyeHost Host { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the plugin is initialized.
        /// </summary>
        /// <value>
        ///   <c>true</c> if initialized; otherwise, <c>false</c>.
        /// </value>
        protected bool IsInitialized
        {
            get { return initialized; }
        }

        /// <summary>
        /// Called when the plugin has just been initialized.
        /// </summary>
        protected virtual void OnInitialized() { }

        /// <summary>
        /// Ensures this plugin is initialized; throws if not.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">The plugin is not initialized.</exception>
        protected void EnsureInitialized()
        {
            if (!initialized)
                throw new InvalidOperationException("The plugin is not initialized.");
        }
    }
}

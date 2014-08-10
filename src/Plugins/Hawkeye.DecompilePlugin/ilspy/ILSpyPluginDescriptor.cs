using System;
using Hawkeye.Extensibility;

namespace Hawkeye.DecompilePlugin
{
    internal class ILSpyPluginDescriptor : IPluginDescriptor
    {
        private static readonly Version version = new Version(AssemblyVersionInfo.Version);

        #region IPluginDescriptor Members

        public string Name
        {
            get { return "ILSpy Plugin"; }
        }

        public Version Version
        {
            get { return version; }
        }

        public IPlugin Create(IHawkeyeHost host)
        {
            var plugin = new ILSpyPluginCore(this);
            plugin.Initialize(host);
            return plugin;
        }

        #endregion
    }
}

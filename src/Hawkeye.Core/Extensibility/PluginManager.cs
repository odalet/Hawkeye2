using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Hawkeye.Logging;

namespace Hawkeye.Extensibility
{
    internal class PluginManager
    {
        private static readonly ILogService log = LogManager.GetLogger<PluginManager>();
        private const string pluginsSubDirectory = "plugins";

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginManager"/> class.
        /// </summary>
        public PluginManager()
        {
            // Just so that it does not return null;
            PluginDescriptors = new IPluginDescriptor[0]; 
        }

        /// <summary>
        /// Gets the discovered plugin descriptors.
        /// </summary>
        public IPluginDescriptor[] PluginDescriptors { get; private set; }

        /// <summary>
        /// Gets the loaded plugin instances.
        /// </summary>
        public IPlugin[] Plugins { get; private set; }

        /// <summary>
        /// Discovers the available plugins.
        /// </summary>
        public void DiscoverAll()
        {
            var directory = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), pluginsSubDirectory);

            if (!Directory.Exists(directory))
            {
                log.Debug(string.Format("No Plugins directory ({0}).", directory));
                return;
            }

            var descriptors = new List<IPluginDescriptor>();

            foreach (var file in Directory.GetFiles(directory, "*.dll"))
            {
                log.Debug(string.Format("Examining File {0} for plugins:", file));
                try
                {
                    var assy = Assembly.LoadFile(file);
                    var types = assy.GetTypes().Where(t => t.IsA<IPluginDescriptor>()).ToArray();

                    if (types == null || types.Length == 0)
                        log.Debug("--> No plugins in this assembly");
                    else
                    {
                        log.Debug(string.Format("--> {0} plugins were found in this assembly", types.Length));
                        foreach (var type in types)
                        {
                            try
                            {
                                var instance = (IPluginDescriptor)type.CreateInstance();
                                descriptors.Add(instance);
                            }
                            catch (Exception ex)
                            {
                                log.Error(string.Format("----> Could not create an instance of type {0}: {1}", type, ex.Message), ex);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Debug(string.Format("--> Not an assembly: {0}", ex.Message));
                }
            }

            PluginDescriptors = descriptors.ToArray();
        }

        /// <summary>
        /// Loads all the plugins (passing them the specified host) 
        /// from previously discovered plugin descriptors.
        /// </summary>
        /// <param name="host">The host.</param>
        public void LoadAll(IHawkeyeHost host)
        {
            var plugins = new List<IPlugin>();

            foreach (var descriptor in PluginDescriptors)
            {
                var pluginName = "?"; // Unlikely, but descriptor.Name could throw...
                try
                {
                    pluginName = descriptor.Name;
                    log.Debug(string.Format("Loading plugin '{0}':", pluginName));
                    var instance = descriptor.Create(host);
                    if (instance == null)
                        log.Warning("--> Created plugin is null. Nothing to load.");
                    else
                    {
                        plugins.Add(instance);
                        log.Debug("--> OK");
                    }
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("--> Plugin '{0}' could not be loaded: {1}", pluginName, ex.Message), ex);
                }
            }

            Plugins = plugins.ToArray();
        }
    }
}

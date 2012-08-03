using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

namespace Hawkeye.Logging.log4net
{
    /// <summary>
    /// This class allows for the creation of a trace service instance based
    /// on the log4net framework.
    /// </summary>
    internal class Log4NetServiceFactory : ILogServiceFactory, ILogServiceAppendable
    {
        public const string ConfigurationFileKey = "configurationFile";

        public static string InitializeApplicationName()
        {
            // We add a property giving the current application name, for use in log4net.config files.
            var entryAssembly = GetEntryAssembly();
            var appname = entryAssembly != null ?
                entryAssembly.GetName().Name : "unknown-app";
            global::log4net.GlobalContext.Properties["ApplicationName"] = appname;
            return appname;
        }

        #region ILogServiceFactory Members

        public ILogService GetLogger(Type type, IDictionary<string, object> additionalData = null)
        {
            if (!log4netConfiguredYet)
            {
                InitializeApplicationName();
                if (additionalData != null && additionalData.ContainsKey(ConfigurationFileKey))
                {
                    var file = additionalData[ConfigurationFileKey] as FileInfo;
                    if (file != null)
                    {
                        global::log4net.Config.XmlConfigurator.Configure(file);
                        log4netConfiguredYet = true;
                    }
                }


                if (!log4netConfiguredYet)
                {
                    global::log4net.Config.XmlConfigurator.Configure();
                    log4netConfiguredYet = true;
                }

                var initialLogger = Log4NetServiceFactory.CreateService(null);
            }

            return Log4NetServiceFactory.CreateService(type);
        }

        public ILogLevelThresholdSelector AppendLogService(ILogService logService, IDictionary<string, object> additionalData = null)
        {
            if (logService == null) throw new ArgumentNullException("logService");

            if (RootLog4NetService == null) return null;
            return RootLog4NetService.AppendLogService(logService, additionalData);
        }

        #endregion

        private static bool log4netConfiguredYet = false;

        private static Log4NetService rootLog4NetService = null;

        private static Log4NetService RootLog4NetService
        {
            get
            {
                if (rootLog4NetService == null)
                {
                    var rootLogger = GetRootLogger();
                    if (rootLogger == null) return null;
                    rootLog4NetService = new Log4NetService(rootLogger);
                }

                return rootLog4NetService;
            }
        }

        /// <summary>
        /// Creates the logging service and return the newly created instance.
        /// </summary>
        /// <param name="type">The type that requests the creation of a log service.</param>
        /// <returns>
        /// An instance of <see cref="Toaster.Logging.ILogService"/>.
        /// </returns>
        /// <remarks>
        /// The configuration file used by log4net will be the application's configuration
        /// file (<b>web.config</b> ou <b>app.exe.config</b>) which will have to contain a
        /// <b>&lt;log4net&gt;</b> section.
        /// </remarks>
        private static ILogService CreateService(Type type)
        {
            if (type == null) return RootLog4NetService;
            else return new Log4NetService(type);
        }

        private static global::log4net.Core.ILogger GetRootLogger()
        {
            var hierarchy = (global::log4net.Repository.Hierarchy.Hierarchy)global::log4net.LogManager.GetRepository();
            return hierarchy.Root;
        }

        private static Assembly GetEntryAssembly()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null) return entryAssembly;
            
            return typeof(Log4NetServiceFactory).Assembly; // fallback: should never happen.
        }
    }
}

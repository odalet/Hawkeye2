using System;
using System.Collections.Generic;

namespace Hawkeye.Logging.log4net
{
    /// <summary>
    /// Wraps the logging Log4Net logging framework into an <see cref="Toaster.Logging.ILogService"/>
    /// so that it can be used as any logging service of the Sopra framework.
    /// </summary>
    internal partial class Log4NetService : BaseLogService, ILogServiceAppendable
    {
        public const string PatternParameter = "PATTERN";

        private global::log4net.Core.ILogger currentLogger = null;

        private Type sourceType = null;
        private static Type thisServiceType = typeof(Log4NetService);
        
        /// <summary>
        /// Gets the current log object.
        /// </summary>
        /// <value>The current log object.</value>
        private global::log4net.Core.ILogger CurrentLogger
        {
            get { return currentLogger ?? global::log4net.LogManager.GetLogger(SourceType).Logger; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetService"/> class.
        /// </summary>
        /// <param name="type">The type that requests the creation of a log service.</param>
        /// <remarks>
        /// As we don't state anything, log4net will look for its configuration inside
        /// the application's configuration file (<b>web.config</b> or <b>app.exe.config</b>)
        /// which will have to contain a <b>&lt;log4net&gt;</b> section.
        /// </remarks>
        public Log4NetService(Type type)
        {
            sourceType = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetService"/> class.
        /// </summary>
        /// <param name="log">An existing Log4Net <see cref="log4net.ILog"/> object.</param>
        internal Log4NetService(global::log4net.Core.ILogger logger)
        {
            currentLogger = logger;
        }
        
        /// <summary>
        /// Logs the specified log entry.
        /// </summary>
        /// <param name="entry">The entry to log.</param>
        public override void Log(ILogEntry entry)
        {
            if (entry == null) throw new ArgumentNullException("entry");

            CurrentLogger.Log(SourceType,
                Log4NetHelper.LogLevelToLog4NetLevel(entry.Level),
                entry.Message, entry.Exception);
        }

        #region ILogServiceAppendable Members

        public ILogLevelThresholdSelector AppendLogService(ILogService logService, IDictionary<string, object> additionalData = null)
        {
            if (CurrentLogger == null) return null;

            var appenderAttachable = CurrentLogger as global::log4net.Core.IAppenderAttachable;
            if (appenderAttachable == null) return null;

            var appender = new LogServiceAppender(logService);

            // let's examine potential parameters
            if (additionalData != null && additionalData.ContainsKey(PatternParameter))
            {
                var parameter = additionalData[PatternParameter];
                if (parameter != null && parameter is string)
                {
                    var pattern = (string)parameter;
                    appender.Layout = new global::log4net.Layout.PatternLayout(pattern);
                }
            }

            appender.LogLevelThreshold = LogLevel.All;
            appenderAttachable.AddAppender(appender);
            return appender;
        }

        #endregion

        /// <summary>
        /// Gets the type this logger is attached to.
        /// </summary>
        /// <value>The type this logger is attached to.</value>
        protected override Type SourceType
        {
            get { return sourceType ?? thisServiceType; }
        }
    }
}

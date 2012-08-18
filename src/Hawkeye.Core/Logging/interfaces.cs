using System;
using System.Collections.Generic;

namespace Hawkeye.Logging
{
    /// <summary>
    /// This interface is implemented by classes providing string formatting to a log entry.
    /// </summary>
    internal interface ILogEntryFormatter
    {
        /// <summary>
        /// Formats the specified log entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <returns>The log entry as a string</returns>
        string FormatEntry(LogEntry entry);
    }

    /// <summary>
    /// Interface implemented by classes that can specify the maximum log level to trace.
    /// </summary>
    internal interface ILogLevelThresholdSelector : IDisposable
    {
        /// <summary>
        /// Gets or sets the log level threshold.
        /// </summary>
        LogLevel LogLevelThreshold { get; set; }
    }

    /// <summary>
    /// Implemented by logging services to which a textbox (or any other output implementation) can be attached.
    /// </summary>
    internal interface ILogServiceAppendable
    {
        /// <summary>
        /// Appends the specified log service to a root log service (the current instance).
        /// </summary>
        /// <param name="logService">The log service.</param>
        /// <param name="additionalData">The optional additional data.</param>
        /// <returns>An implementation of <see cref="ILogLevelThresholdSelector"/> allowing to set a maximum log level to trace.</returns>
        ILogLevelThresholdSelector AppendLogService(ILogService logService, IDictionary<string, object> additionalData = null);
    }

    /// <summary>
    /// Interface implemented by logging service factories
    /// </summary>
    internal interface ILogServiceFactory
    {
        /// <summary>
        /// Obtains an instance of the logger service for the specified type and optional additional data.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="additionalData">The additional data.</param>
        /// <returns>An implementation of <see cref="ILogService"/>.</returns>
        ILogService GetLogger(Type type, IDictionary<string, object> additionalData = null);

        /// <summary>
        /// Closes all the resources held by the various loggers.
        /// </summary>
        void Shutdown();
    }
}

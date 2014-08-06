using System;

namespace Hawkeye.Logging
{
    /// <summary>
    /// Abstract base class allowing for a quick implementation of a logging service.
    /// </summary>
    /// <seealso cref="Hawkeye.Logging.ILogService"/>
    internal abstract class BaseLogService : ILogService
    {
        /// <summary>
        /// Gets the type this logger is attached to.
        /// </summary>
        /// <value>The type this logger is attached to.</value>
        protected abstract Type SourceType { get; }

        #region ILogService Members
        
        /// <summary>
        /// Logs the specified log entry.
        /// </summary>
        /// <param name="entry">The entry to log.</param>
        public abstract void Log(ILogEntry entry);

        /// <summary>
        /// Makes a <see cref="ILogEntry"/> object from the specified parameters.
        /// </summary>
        /// <param name="level">The trace level.</param>
        /// <param name="message">The message object.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>
        /// A <see cref="ILogEntry"/> object ready to be logged.
        /// </returns>
        public virtual ILogEntry MakeLogEntry(LogLevel level, string message, Exception exception)
        {
            return new LogEntry() { Level = level, Message = message, Exception = exception };
        }

        #endregion
    }
}

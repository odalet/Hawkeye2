using System;

// Minimal logging interfaces accessible to plugins
namespace Hawkeye.Logging
{
    /// <summary>Represents a logging service.</summary>
    public interface ILogService
    {
        /// <summary>
        /// Logs the specified log entry.
        /// </summary>
        /// <param name="entry">The entry to log.</param>
        void Log(ILogEntry entry);

        /// <summary>
        /// Makes a <see cref="ILogEntry"/> object from the specified parameters.
        /// </summary>
        /// <param name="level">The trace level.</param>
        /// <param name="message">The message object.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>A <see cref="ILogEntry"/> object ready to be logged.</returns>
        ILogEntry MakeLogEntry(LogLevel level, string message, Exception exception);
    }

    /// <summary>
    /// Represents a log entry; ie. what a logged message is made of.
    /// </summary>
    public interface ILogEntry
    {
        /// <summary>
        /// Gets or sets the trace level for this entry.
        /// </summary>
        LogLevel Level { get; set; }

        /// <summary>
        /// Gets or sets the message object for this entry.
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// Gets or sets the exception for this entry.
        /// </summary>
        Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets the source name for this entry.
        /// </summary>
        string Source { get; set; }
    }
}

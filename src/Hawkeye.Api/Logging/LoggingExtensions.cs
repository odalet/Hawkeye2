using System;

namespace Hawkeye.Logging
{
    /// <summary>
    /// Adds shortcut methods to the <see cref="Hawkeye.Logging.ILogService"/> interface.
    /// </summary>
    public static class LoggingExtensions
    {
        #region Log

        /// <summary>
        /// Logs the specified message with the specified trace level.
        /// </summary>
        /// <param name="log">The logging service used to output the trace.</param>
        /// <param name="level">The trace level.</param>
        /// <param name="message">The message to trace.</param>
        public static void Log(this ILogService log, LogLevel level, string message)
        {
            log.Log(level, message, null);
        }

        /// <summary>
        /// Logs the specified exception with the specified trace level.
        /// </summary>
        /// <param name="log">The logging service used to output the trace.</param>
        /// <param name="level">The trace level.</param>
        /// <param name="exception">The exception to trace.</param>
        public static void Log(this ILogService log, LogLevel level, Exception exception)
        {
            log.Log(level, string.Empty, exception);
        }

        /// <summary>
        /// Logs the specified message and exception with the specified trace level.
        /// </summary>
        /// <param name="log">The logging service used to output the trace.</param>
        /// <param name="level">The trace level.</param>
        /// <param name="message">The message to trace.</param>
        /// <param name="exception">The exception to trace.</param>
        public static void Log(this ILogService log, LogLevel level, string message, Exception exception)
        {
            log.Log(log.MakeLogEntry(level, message, exception));
        }

        #endregion

        #region Verbose

        /// <summary>
        /// Logs the specified message with the <see cref="LogLevel.Verbose"/> trace level.
        /// </summary>
        /// <param name="log">The logging service used to output the trace.</param>
        /// <param name="message">The message to trace.</param>
        public static void Verbose(this ILogService log, string message) { log.Log(LogLevel.Verbose, message); }

        /// <summary>
        /// Logs the specified exception with the <see cref="LogLevel.Verbose"/> trace level.
        /// </summary>
        /// <param name="log">The logging service used to output the trace.</param>
        /// <param name="exception">The exception to trace.</param>
        public static void Verbose(this ILogService log, Exception exception) { log.Log(LogLevel.Verbose, exception); }

        /// <summary>
        /// Logs the specified message and exception with the <see cref="LogLevel.Verbose"/> trace level.
        /// </summary>
        /// <param name="log">The logging service used to output the trace.</param>
        /// <param name="message">The message to trace.</param>
        /// <param name="exception">The exception to trace.</param>
        public static void Verbose(this ILogService log, string message, Exception exception) { log.Log(LogLevel.Verbose, message, exception); }

        #endregion

        #region Debug

        /// <summary>
        /// Logs the specified message with the <see cref="LogLevel.Debug"/> trace level.
        /// </summary>
        /// <param name="log">The logging service used to output the trace.</param>
        /// <param name="message">The message to trace.</param>
        public static void Debug(this ILogService log, string message) { log.Log(LogLevel.Debug, message); }

        /// <summary>
        /// Logs the specified exception with the <see cref="LogLevel.Debug"/> trace level.
        /// </summary>
        /// <param name="log">The logging service used to output the trace.</param>
        /// <param name="exception">The exception to trace.</param>
        public static void Debug(this ILogService log, Exception exception) { log.Log(LogLevel.Debug, exception); }

        /// <summary>
        /// Logs the specified message and exception with the <see cref="LogLevel.Debug"/> trace level.
        /// </summary>
        /// <param name="log">The logging service used to output the trace.</param>
        /// <param name="message">The message to trace.</param>
        /// <param name="exception">The exception to trace.</param>
        public static void Debug(this ILogService log, string message, Exception exception) { log.Log(LogLevel.Debug, message, exception); }

        #endregion

        #region Info

        /// <summary>
        /// Logs the specified message with the <see cref="LogLevel.Info"/> trace level.
        /// </summary>
        /// <param name="log">The logging service used to output the trace.</param>
        /// <param name="message">The message to trace.</param>
        public static void Info(this ILogService log, string message) { log.Log(LogLevel.Info, message); }

        /// <summary>
        /// Logs the specified exception with the <see cref="LogLevel.Info"/> trace level.
        /// </summary>
        /// <param name="log">The logging service used to output the trace.</param>
        /// <param name="exception">The exception to trace.</param>
        public static void Info(this ILogService log, Exception exception) { log.Log(LogLevel.Info, exception); }

        /// <summary>
        /// Logs the specified message and exception with the <see cref="LogLevel.Info"/> trace level.
        /// </summary>
        /// <param name="log">The logging service used to output the trace.</param>
        /// <param name="message">The message to trace.</param>
        /// <param name="exception">The exception to trace.</param>
        public static void Info(this ILogService log, string message, Exception exception) { log.Log(LogLevel.Info, message, exception); }

        #endregion

        #region Warning

        /// <summary>
        /// Logs the specified message with the <see cref="LogLevel.Warning"/> trace level.
        /// </summary>
        /// <param name="log">The logging service used to output the trace.</param>
        /// <param name="message">The message to trace.</param>
        public static void Warning(this ILogService log, string message) { log.Log(LogLevel.Warning, message); }

        /// <summary>
        /// Logs the specified exception with the <see cref="LogLevel.Warning"/> trace level.
        /// </summary>
        /// <param name="log">The logging service used to output the trace.</param>
        /// <param name="exception">The exception to trace.</param>
        public static void Warning(this ILogService log, Exception exception) { log.Log(LogLevel.Warning, exception); }

        /// <summary>
        /// Logs the specified message and exception with the <see cref="LogLevel.Warning"/> trace level.
        /// </summary>
        /// <param name="log">The logging service used to output the trace.</param>
        /// <param name="message">The message to trace.</param>
        /// <param name="exception">The exception to trace.</param>
        public static void Warning(this ILogService log, string message, Exception exception) { log.Log(LogLevel.Warning, message, exception); }

        #endregion

        #region Error

        /// <summary>
        /// Logs the specified message with the <see cref="LogLevel.Error"/> trace level.
        /// </summary>
        /// <param name="log">The logging service used to output the trace.</param>
        /// <param name="message">The message to trace.</param>
        public static void Error(this ILogService log, string message) { log.Log(LogLevel.Error, message); }

        /// <summary>
        /// Logs the specified exception with the <see cref="LogLevel.Error"/> trace level.
        /// </summary>
        /// <param name="log">The logging service used to output the trace.</param>
        /// <param name="exception">The exception to trace.</param>
        public static void Error(this ILogService log, Exception exception) { log.Log(LogLevel.Error, exception); }

        /// <summary>
        /// Logs the specified message and exception with the <see cref="LogLevel.Error"/> trace level.
        /// </summary>
        /// <param name="log">The logging service used to output the trace.</param>
        /// <param name="message">The message to trace.</param>
        /// <param name="exception">The exception to trace.</param>
        public static void Error(this ILogService log, string message, Exception exception) { log.Log(LogLevel.Error, message, exception); }

        #endregion

        #region Fatal

        /// <summary>
        /// Logs the specified message with the <see cref="LogLevel.Fatal"/> trace level.
        /// </summary>
        /// <param name="log">The logging service used to output the trace.</param>
        /// <param name="message">The message to trace.</param>
        public static void Fatal(this ILogService log, string message) { log.Log(LogLevel.Fatal, message); }

        /// <summary>
        /// Logs the specified exception with the <see cref="LogLevel.Fatal"/> trace level.
        /// </summary>
        /// <param name="log">The logging service used to output the trace.</param>
        /// <param name="exception">The exception to trace.</param>
        public static void Fatal(this ILogService log, Exception exception) { log.Log(LogLevel.Fatal, exception); }

        /// <summary>
        /// Logs the specified message and exception with the <see cref="LogLevel.Fatal"/> trace level.
        /// </summary>
        /// <param name="log">The logging service used to output the trace.</param>
        /// <param name="message">The message to trace.</param>
        /// <param name="exception">The exception to trace.</param>
        public static void Fatal(this ILogService log, string message, Exception exception) { log.Log(LogLevel.Fatal, message, exception); }

        #endregion
    }
}

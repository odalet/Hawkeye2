using log4net.Core;

namespace Hawkeye.Logging.log4net
{
    internal static class Log4NetHelper
    {
        /// <summary>
        /// Converts a Sopra flavor trace level (<see cref="Siti.Logging.LogLevel"/>)
        /// into a log4net like level (<see cref="E:log4net.Core.Level"/>).
        /// </summary>
        /// <param name="level">The trace level to convert.</param>
        /// <returns>log4net converted trace level.</returns>
        public static Level LogLevelToLog4NetLevel(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.All: return Level.All;
                case LogLevel.Verbose: return Level.Verbose;
                case LogLevel.Debug: return Level.Debug;
                case LogLevel.Info: return Level.Info;
                case LogLevel.Warning: return Level.Warn;
                case LogLevel.Error: return Level.Error;
                case LogLevel.Fatal: return Level.Fatal;
                case LogLevel.Off: return Level.Off;
            }

            return Level.All;
        }

        /// <summary>
        /// Converts a log4net flavor trace level (<see cref="E:log4net.Core.Level"/>)
        /// into a Sopra like level (<see cref="Siti.Logging.LogLevel"/>).
        /// </summary>
        /// <param name="level">The log4net level to convert.</param>
        /// <returns>Sopra converted trace level.</returns>
        public static LogLevel Log4NetLevelToLogLevel(Level level)
        {
            if (level == Level.All) return LogLevel.All;
            if (level == Level.Verbose) return LogLevel.Verbose;
            if (level == Level.Debug) return LogLevel.Debug;
            if (level == Level.Info) return LogLevel.Info;
            if (level == Level.Warn) return LogLevel.Warning;
            if (level == Level.Error) return LogLevel.Error;
            if (level == Level.Fatal) return LogLevel.Fatal;
            if (level == Level.Off) return LogLevel.Off;
            return LogLevel.All;
        }

        /// <summary>
        /// Loggings a log4net flavor logging event (<see cref="log4net.Core.LoggingEvent"/>)
        /// into a Sopra Log entry (<see cref="Siti.Logging.LogEntry"/>).
        /// </summary>
        /// <param name="loggingEvent">The logging event.</param>
        /// <returns></returns>
        public static LogEntry LoggingEventToLogEntry(LoggingEvent loggingEvent)
        {
            if (loggingEvent == null) return null;
            
            var message = loggingEvent.MessageObject == null ?
                loggingEvent.RenderedMessage : loggingEvent.MessageObject.ToString();

            return new LogEntry()
            {
                Exception = loggingEvent.ExceptionObject,
                Level = Log4NetLevelToLogLevel(loggingEvent.Level),
                Message = message,
                Source = loggingEvent.LoggerName,
                Tag = loggingEvent,
                TimeStamp = loggingEvent.TimeStamp
            };
        }

        public static LoggingEvent LogEntryToLoggingEvent(LogEntry entry)
        {
            if (entry == null) return null;
            if (entry.Tag is LoggingEvent) return (LoggingEvent)entry.Tag;

            return new LoggingEvent(
                new LoggingEventData()
                {
                    ExceptionString = entry.Exception.ToFormattedString(),
                    Level = LogLevelToLog4NetLevel(entry.Level),                    
                    Message = entry.Message,
                    LoggerName = entry.Source,
                    TimeStamp = entry.TimeStamp
                });
        }
    }
}

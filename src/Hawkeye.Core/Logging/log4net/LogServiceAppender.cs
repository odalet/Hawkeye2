using System;

using log4net.Core;
using log4net.Appender;

namespace Hawkeye.Logging.log4net
{
    internal class LogServiceAppender : AppenderSkeleton, ILogLevelThresholdSelector
    {
        private ILogService logService = null;
        private bool closed = false;

        public LogServiceAppender(ILogService logServiceToAppend)
        {
            if (logServiceToAppend == null) throw new ArgumentNullException("logServiceToAppend");
            logService = logServiceToAppend;
        }

        #region ILogLevelThresholdSelector Members

        /// <summary>
        /// Gets or sets the log level threshold.
        /// </summary>
        /// <value>The log level threshold.</value>
        public LogLevel LogLevelThreshold
        {
            get { return Log4NetHelper.Log4NetLevelToLogLevel(base.Threshold); }
            set { base.Threshold = Log4NetHelper.LogLevelToLog4NetLevel(value); }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            base.Close();
        }

        #endregion

        /// <summary>
        /// Subclasses of <see cref="T:log4net.Appender.AppenderSkeleton"/> should implement this method
        /// to perform actual logging.
        /// </summary>
        /// <param name="loggingEvent">The event to append.</param>
        /// <remarks>
        /// 	<para>
        /// A subclass must implement this method to perform
        /// logging of the <paramref name="loggingEvent"/>.
        /// </para>
        /// 	<para>This method will be called by <see cref="M:log4net.Appender.AppenderSkeleton.DoAppend(log4net.Core.LoggingEvent)"/>
        /// if all the conditions listed for that method are met.
        /// </para>
        /// 	<para>
        /// To restrict the logging of events in the appender
        /// override the <see cref="M:log4net.Appender.AppenderSkeleton.PreAppendCheck"/> method.
        /// </para>
        /// </remarks>
        protected override void Append(LoggingEvent loggingEvent)
        {
            if (closed) return;

            // Only log if event's level is >= threshold
            var level = Log4NetHelper.Log4NetLevelToLogLevel(loggingEvent.Level);
            if (level < LogLevelThreshold) return;
            
            var entry = Log4NetHelper.LoggingEventToLogEntry(loggingEvent);
            if (Layout != null) entry.Formatter = new Log4NetLayoutFormatter(Layout);
            logService.Log(entry);
        }

        /// <summary>
        /// Is called when the appender is closed. Derived classes should override
        /// this method if resources need to be released.
        /// </summary>
        /// <remarks>
        /// 	<para>
        /// Releases any resources allocated within the appender such as file handles,
        /// network connections, etc.
        /// </para>
        /// 	<para>
        /// It is a programming error to append to a closed appender.
        /// </para>
        /// </remarks>
        protected override void OnClose()
        {
            base.OnClose();
            closed = true;
        }
    }
}

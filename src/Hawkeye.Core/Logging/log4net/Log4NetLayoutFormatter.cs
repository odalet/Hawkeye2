using System;
using System.IO;
using System.Text;

namespace Hawkeye.Logging.log4net
{
    internal class Log4NetLayoutFormatter : ILogEntryFormatter
    {
        private global::log4net.Layout.ILayout layout = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetLayoutFormatter"/> class.
        /// </summary>
        /// <param name="log4netLayout">The log4net layout.</param>
        public Log4NetLayoutFormatter(global::log4net.Layout.ILayout log4netLayout)
        {
            if (log4netLayout == null) throw new ArgumentNullException("log4netLayout");
            layout = log4netLayout;
        }

        #region ILogEntryFormatter Members

        /// <summary>
        /// Formats the specified log entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <returns>
        /// The log entry as a string
        /// </returns>
        public string FormatEntry(LogEntry entry)
        {
            var loggingEvent = Log4NetHelper.LogEntryToLoggingEvent(entry);
            if (loggingEvent == null) return string.Empty;

            var builder = new StringBuilder();
            using (var writer = new StringWriter(builder))
            {
                layout.Format(writer, loggingEvent);
                writer.Close();
            }

            return builder.ToString();
        }

        #endregion
    }
}

using System;
using System.IO;
using System.Text;

namespace Hawkeye.Logging.log4net
{
    internal class Log4NetLayoutFormatter : ILogEntryFormatter
    {
        private global::log4net.Layout.ILayout layout = null;

        public Log4NetLayoutFormatter(global::log4net.Layout.ILayout log4netLayout)
        {
            if (log4netLayout == null) throw new ArgumentNullException("log4netLayout");
            layout = log4netLayout;
        }

        #region ILogEntryFormatter Members

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

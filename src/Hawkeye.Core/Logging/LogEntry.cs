using System;
using System.Text;

namespace Hawkeye.Logging
{
    /// <summary>
    /// This class stores the properties a logging message is made of.
    /// </summary>
    internal class LogEntry : ILogEntry
    {
        private class DefaultFormatter : ILogEntryFormatter
        {
            public static readonly DefaultFormatter instance = new DefaultFormatter();
            
            #region ILogEntryFormatter Members

            public string FormatEntry(LogEntry entry)
            {
                if (entry == null) return string.Empty;

                var sb = new StringBuilder();

                var source = entry.Source;
                if (string.IsNullOrEmpty(entry.Source)) source = "Default source";

                sb.AppendFormat("[{0}] {1}",
                    source, entry.Level.ToString().ToUpperInvariant());

                if ((entry.TimeStamp != DateTime.MinValue) && (entry.TimeStamp != DateTime.MaxValue))
                    sb.AppendFormat(" [{0}]", entry.TimeStamp.ToLongInvariantString());

                sb.Append(": ");

                var sbText = new StringBuilder();

                if (!string.IsNullOrEmpty(entry.Message))
                {
                    sbText.Append(entry.Message);
                    if (entry.Exception != null) sbText.Append(" - ");
                }

                if (entry.Exception != null) sbText.Append(entry.Exception.ToFormattedString());

                var text = sbText.ToString();
                sb.Append(string.IsNullOrEmpty(text) ? "No message" : text);

                return sb.ToString();
            }

            #endregion
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ILogEntry"/> class.
        /// </summary>
        public LogEntry() 
        {
            TimeStamp = DateTime.Now;
            Formatter = DefaultFormatter.instance; 
        }

        /// <summary>
        /// Gets or sets the source name for this entry.
        /// </summary>
        /// <value>The source name.</value>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the trace level for this entry.
        /// </summary>
        public LogLevel Level { get; set; }

        /// <summary>
        /// Gets or sets the message object for this entry.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the exception for this entry.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets the creation time of this entry.
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Gets or sets additional data related to this log entry.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Gets or sets the formatter for this entry.
        /// </summary>
        /// <value>The formatter.</value>
        internal ILogEntryFormatter Formatter { get; set; }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the default string representation of this entry.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            var formatter = Formatter ?? DefaultFormatter.instance;
            return formatter.FormatEntry(this);
        }
    }
}

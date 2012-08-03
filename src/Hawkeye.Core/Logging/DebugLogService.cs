using System;
using System.Diagnostics;

namespace Hawkeye.Logging
{
    /// <summary>
    /// This Logging service implementation logs its entries to the Visual Studio output Window.
    /// </summary>
    internal class DebugLogService : BaseLogService
    {
        private Type sourceType = null;
        private static Type thisServiceType = typeof(DebugLogService);

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugLogService"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public DebugLogService(Type type) 
        {
            sourceType = type; 
        }

        /// <summary>
        /// Logs the specified log entry.
        /// </summary>
        /// <param name="entry">The entry to log.</param>
        public override void Log(LogEntry entry)
        {
            entry.Source = SourceType.Name;            
            Debug.WriteLine(entry.ToString());
        }

        /// <summary>
        /// Gets the type this logger is attached to.
        /// </summary>
        /// <value>The type this logger is attached to.</value>
        protected override Type SourceType
        {
            get { return sourceType ?? thisServiceType; }
        }
    }

    public class DebugLogServiceFactory : ILogServiceFactory
    {

        #region ILogServiceFactory Members

        public ILogService GetLogger(Type type, System.Collections.Generic.IDictionary<string, object> additionalData = null)
        {
            return new DebugLogService(type);
        }

        #endregion
    }
}

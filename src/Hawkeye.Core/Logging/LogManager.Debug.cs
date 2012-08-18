using System;
using System.Diagnostics;

namespace Hawkeye.Logging
{
    partial class LogManager
    {
        /// <summary>
        /// This Logging service implementation logs its entries to the Visual Studio output Window.
        /// </summary>
        /// <remarks>
        /// It is used by the LogManager so that an implementation of ILogServiceFactory is available in design mode;
        /// otherwise, Visual Studio crashes (because of static readonly log initializations in forms and controls).
        /// </remarks>
        private class DebugLogger : BaseLogService, ILogServiceFactory
        {
            private Type sourceType = null;
            private static Type thisServiceType = typeof(DebugLogger);

            /// <summary>
            /// Initializes a new instance of the <see cref="DebugLogger"/> class.
            /// </summary>
            /// <param name="type">The type.</param>
            public DebugLogger(Type type)
            {
                if (type == null) type = GetType();
                sourceType = type;
            }
            
            #region ILogServiceFactory Members

            public ILogService GetLogger(Type type, System.Collections.Generic.IDictionary<string, object> additionalData = null)
            {
                return new DebugLogger(type);
            }

            /// <summary>
            /// Closes all the resources held by the various loggers.
            /// </summary>
            public void Shutdown()
            {
                // Nothing to do here!
            }

            #endregion

            /// <summary>
            /// Logs the specified log entry.
            /// </summary>
            /// <param name="entry">The entry to log.</param>
            public override void Log(ILogEntry entry)
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
    }
}

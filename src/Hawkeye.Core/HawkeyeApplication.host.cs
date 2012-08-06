using System;
using Hawkeye.Logging;

namespace Hawkeye
{
    partial class HawkeyeApplication
    {
        private class HawkeyeHostImplementation : HawkeyeHost.IHost
        {
            private static readonly ILogService log = LogManager.GetLogger<HawkeyeHostImplementation>();

            private IHawkeyeApplicationImplementation implementation = null;

            public HawkeyeHostImplementation(IHawkeyeApplicationImplementation app)
            {
                if (app == null) throw new ArgumentNullException("app", 
                    "You must provide an implementation of HawkeyeApplication.IHawkeyeApplicationImplementation");
                implementation = app;
                log.Debug("A Hawkeye host implementation has just been created.");
            }

            #region IHost Members

            /// <summary>
            /// Gets a logger instance for the specified type.
            /// </summary>
            /// <param name="type">The type for which to log.</param>
            /// <returns>
            /// An instance of an object implementing <see cref="ILogService"/>.
            /// </returns>
            public ILogService GetLogger(Type type)
            {
                return LogManager.GetLogger(type);
            }

            #endregion
        }
    }
}

using System;

namespace Hawkeye.Logging
{
    /// <summary>Trace levels</summary>
    public enum LogLevel
    {
        /// <summary>All logging levels.</summary>
        All = 0,

        /// <summary>Minimal trace logging level.</summary>
        Verbose = 1,

        /// <summary>Debug trace logging level: used for debugging purpose.</summary>
        Debug = 2,

        /// <summary>Low trace logging level: used for information purpose.</summary>
        Info = 3,

        /// <summary>Middle logging trace level: used to log warnings.</summary>
        Warning = 4,

        /// <summary>High logging trace level: used to log errors.</summary>
        Error = 5,

        /// <summary>Highest logging trace level: used to log serious and fatal errors.</summary>
        Fatal = 6,

        /// <summary>Do not log anything.</summary>
        Off = 7,
    }
}

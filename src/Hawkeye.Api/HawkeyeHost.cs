using System;
using Hawkeye.Logging;

namespace Hawkeye
{
    /// <summary>
    /// Exposes the Hawkeye application API to plugins
    /// </summary>
    public static class HawkeyeHost
    {
        /// <summary>
        /// Provides a Hawkeye application host implementation 
        /// to the <see cref="HawkeyeHost"/> public facade.
        /// </summary>
        internal interface IHost
        {
            /// <summary>
            /// Gets a logger instance for the specified type.
            /// </summary>
            /// <param name="type">The type for which to log.</param>
            /// <returns>
            /// An instance of an object implementing <see cref="ILogService"/>.
            /// </returns>
            ILogService GetLogger(Type type);
        }

        private static IHost implementation = null;

        /// <summary>
        /// Gets a value indicating whether the Hawkeye Host is initialized.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the Hawkeye Host is initialized; otherwise, <c>false</c>.
        /// </value>
        public static bool IsInitialized
        {
            get { return implementation != null; }
        }

        /// <summary>
        /// Gets the default logger.
        /// </summary>
        /// <returns>An instance of an object implementing <see cref="ILogService"/>.</returns>
        public static ILogService GetLogger()
        {
            return GetLogger(null);
        }

        /// <summary>
        /// Gets a logger instance for the specified type.
        /// </summary>
        /// <typeparam name="T">The type for which to log.</typeparam>
        /// <returns>
        /// An instance of an object implementing <see cref="ILogService"/>.
        /// </returns>
        public static ILogService GetLogger<T>()
        {
            return GetLogger(typeof(T));
        }

        /// <summary>
        /// Gets a logger instance for the specified type.
        /// </summary>
        /// <param name="type">The type for which to log.</param>
        /// <returns>
        /// An instance of an object implementing <see cref="ILogService"/>.
        /// </returns>
        public static ILogService GetLogger(Type type)
        {
            EnsureInitialized();
            return implementation.GetLogger(type);
        }

        internal static void Initialize(IHost host)
        {
            implementation = host;
        }

        private static void EnsureInitialized()        
        {
            if (!IsInitialized) throw new ApplicationException(
                "The Hawkeye application Host is not initialized.");
        }
    }

    //internal interface IThisImplementation
    //{
    //    Clr CurrentClr { get; }
    //    Bitness CurrentBitness { get; }
    //    IWindowInfo GetWindowInfo(IntPtr hwnd);
    //    ILogServiceFactory GetLogServiceFactory();
    //}

    ///// <summary>
    ///// Central point for Hawkeye API access.
    ///// </summary>
    //public static class This
    //{
    //    private static IThisImplementation implementation = null;
    //    private static bool initialized = false;

    //    /// <summary>
    //    /// Gets a value indicating whether the running instance is x64.
    //    /// </summary>
    //    /// <value>
    //    ///   <c>true</c> if x64; otherwise, <c>false</c>.
    //    /// </value>
    //    public static bool IsX64
    //    {
    //        get { return IntPtr.Size == 8; }
    //    }

    //    internal static bool IsInitialized
    //    {
    //        get { return initialized; }
    //    }

    //    internal static void InitializeApi(IThisImplementation impl)
    //    {
    //        if (impl == null) throw new ArgumentNullException("impl");
    //        implementation = impl;
    //        initialized = true;
    //    }

    //    public static Clr CurrentClr
    //    {
    //        get 
    //        {
    //            EnsureInitialized();
    //            return implementation.CurrentClr; 
    //        }
    //    }

    //    public static Bitness CurrentBitness
    //    {
    //        get
    //        {
    //            EnsureInitialized();
    //            return implementation.CurrentBitness;
    //        }
    //    }

    //    /// <summary>
    //    /// Gets window information given its handle.
    //    /// </summary>
    //    /// <param name="hwnd">The Window handle.</param>
    //    /// <returns>Window information</returns>
    //    public static IWindowInfo GetWindowInfo(IntPtr hwnd)
    //    {
    //        EnsureInitialized();
    //        return implementation.GetWindowInfo(hwnd);
    //    }

    //    /// <summary>
    //    /// Returns a new instance of the default logging service factory.
    //    /// </summary>
    //    /// <returns>An implementation of <see cref="ILogServiceFactory"/>.</returns>
    //    public static ILogServiceFactory GetLogServiceFactory()
    //    {
    //        EnsureInitialized();
    //        return implementation.GetLogServiceFactory();
    //    }

    //    private static void EnsureInitialized()
    //    {
    //        if (!initialized) throw new ApplicationException(
    //            "The core API is not initialized. You should call This.InitializeApi first.");
    //    }
    //}
}

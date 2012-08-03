using System;
using System.Text;
using System.Collections.Generic;

namespace Hawkeye
{
    internal interface IThisImplementation
    {
        IWindowInfo GetWindowInfo(IntPtr hwnd);
    }

    /// <summary>
    /// Central point for Hawkeye API access.
    /// </summary>
    public static class This
    {
        private static IThisImplementation implementation = null;
        private static bool initialized = false;

        /// <summary>
        /// Gets a value indicating whether the running instance is x64.
        /// </summary>
        /// <value>
        ///   <c>true</c> if x64; otherwise, <c>false</c>.
        /// </value>
        public static bool IsX64
        {
            get { return IntPtr.Size == 8; }
        }

        internal static void InitializeApi(IThisImplementation impl)
        {
            if (impl == null) throw new ArgumentNullException("impl");
            implementation = impl;
            initialized = true;
        }

        /// <summary>
        /// Gets window information given its handle.
        /// </summary>
        /// <param name="hwnd">The Window handle.</param>
        /// <returns>Window information</returns>
        public static IWindowInfo GetWindowInfo(IntPtr hwnd)
        {
            EnsureInitialized();
            return implementation.GetWindowInfo(hwnd);
        }

        private static void EnsureInitialized()
        {
            if (!initialized) throw new ApplicationException(
                "The core API is not initialized. You should call This.InitializeApi first.");
        }
    }
}

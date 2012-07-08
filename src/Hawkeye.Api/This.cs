using System;
using System.Collections.Generic;
using System.Text;

namespace Hawkeye
{
    internal interface IThisImplementation
    {
        IWindowInfo GetWindowInfo(IntPtr hwnd);
    }

    public static class This
    {
        private static IThisImplementation implementation = null;
        private static bool initialized = false;
        
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

        public static IWindowInfo GetWindowInfo(IntPtr hwnd)
        {
            return implementation.GetWindowInfo(hwnd);
        }
    }
}

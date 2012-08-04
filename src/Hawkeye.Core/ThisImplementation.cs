using System;
using Hawkeye.WinApi;
using Hawkeye.Logging;
using Hawkeye.Logging.log4net;

namespace Hawkeye
{
    internal class ThisImplementation : IThisImplementation
    {
        private static readonly Bitness currentBitness;
        private static readonly Clr currentClr;

        /// <summary>
        /// Initializes the <see cref="ThisImplementation"/> class.
        /// </summary>
        static ThisImplementation()
        {
            currentBitness = IntPtr.Size == 8 ? 
                Bitness.x64 : Bitness.x86;
            currentClr = typeof(int).Assembly.GetName().Version.Major == 4 ? 
                Clr.Net4 : Clr.Net2;
        }

        #region IThisImplementation Members

        public IWindowInfo GetWindowInfo(IntPtr hwnd)
        {
            return new WindowInfo(hwnd);
        }

        public ILogServiceFactory GetLogServiceFactory()
        {
            return new Log4NetServiceFactory();
        }

        public Clr CurrentClr
        {
            get { return currentClr; }
        }

        public Bitness CurrentBitness
        {
            get { return currentBitness; }
        }

        #endregion
    }
}

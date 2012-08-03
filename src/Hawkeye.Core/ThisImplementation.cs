using System;
using Hawkeye.WinApi;
using Hawkeye.Logging;
using Hawkeye.Logging.log4net;

namespace Hawkeye
{
    internal class ThisImplementation : IThisImplementation
    {
        #region IThisImplementation Members

        public IWindowInfo GetWindowInfo(IntPtr hwnd)
        {
            return new WindowInfo(hwnd);
        }

        #endregion

        #region IThisImplementation Members
        
        public ILogServiceFactory GetLogServiceFactory()
        {
            return new Log4NetServiceFactory();            
        }

        #endregion
    }
}

using System;
using Hawkeye.WinApi;

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
    }
}

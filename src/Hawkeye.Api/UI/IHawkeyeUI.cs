using System;

namespace Hawkeye.UI
{
    /// <summary>
    /// Gives access to the Hawkeye User interface to plugins
    /// </summary>
    public interface IHawkeyeUI
    {
        /// <summary>
        /// Tells the main Hawkeye Form what Window to inspect.
        /// </summary>
        /// <param name="hwnd">The Window Handle.</param>
        void SetTarget(IntPtr hwnd);
    }
}

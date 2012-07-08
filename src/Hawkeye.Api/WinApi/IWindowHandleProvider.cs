using System;

namespace Hawkeye
{
    public interface IWindowHandleProvider
    {
        /// <summary>
        /// Gets the Window Handle.
        /// </summary>
        IntPtr Handle { get; }
    }
}

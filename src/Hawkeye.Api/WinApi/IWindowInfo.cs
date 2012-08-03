using System;

namespace Hawkeye
{
    /// <summary>
    /// Represents a Win32 Window.
    /// </summary>
    public interface IWindowInfo
    {
        /// <summary>
        /// Gets this Window handle.
        /// </summary>
        IntPtr Handle { get; }

        /// <summary>
        /// Gets this Window's owner process bitness.
        /// </summary>
        Bitness Bitness { get; }

        /// <summary>
        /// Gets this Window's owner process CLR version.
        /// </summary>
        Clr Clr { get; }

        /// <summary>
        /// Gets the list of modules loaded by this Window's owner process.
        /// </summary>
        IModuleInfo[] Modules { get; }

        /// <summary>
        /// Dumps the content of this object in a text form.
        /// </summary>
        /// <returns>A string with the dumped data.</returns>
        string Dump();
    }
}

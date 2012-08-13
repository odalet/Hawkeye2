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
        /// Get this Window owner's thread Id 
        /// </summary>
        int ThreadId { get; }

        /// <summary>
        /// Get this Window owner's process Id 
        /// </summary>
        int ProcessId { get; }

        /// <summary>
        /// Gets this window class name
        /// </summary>
        string ClassName { get; }

        /// <summary>
        /// Gets the Windows Forms Control info associated with this window.
        /// </summary>
        IControlInfo ControlInfo { get; }

        /// <summary>
        /// Dumps the content of this object in a text form.
        /// </summary>
        /// <returns>A string with the dumped data.</returns>
        string Dump();

        /// <summary>
        /// Returns a short string representation of the window information
        /// </summary>
        /// <returns>A string</returns>
        string ToShortString();
    }
}

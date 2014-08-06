using System;

namespace Hawkeye
{
    /// <summary>
    /// This interface represents a Win32 module.
    /// </summary>
    public interface IModuleInfo
    {
        /// <summary>
        /// Gets the identifier of the process who owns this module.
        /// </summary>
        uint ProcessId { get; }

        /// <summary>
        /// Gets the base address of the module in the context of the owning process.
        /// </summary>
        IntPtr BaseAddress { get; }

        /// <summary>
        /// Gets the size of the module, in bytes.
        /// </summary>
        uint BaseSize { get; }

        /// <summary>
        /// Gets a handle to the module in the context of the owning process.
        /// </summary>
        IntPtr Handle { get; }

        /// <summary>
        /// Gets the module name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the module path.
        /// </summary>
        string Path { get; }
    }
}

using System;
using System.ComponentModel;

using Hawkeye.WinApi;

namespace Hawkeye.ComponentModel
{
    /// <summary>
    /// This class represents a Win32 module.
    /// </summary>
    [TypeConverter(typeof(ModuleInfoConverter))]
    internal class ModuleInfo : IModuleInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleInfo"/> class.
        /// </summary>
        /// <param name="module">The Win32 module entry data.</param>
        public ModuleInfo(MODULEENTRY32 module)
        {
            ProcessId = module.th32ProcessID;
            BaseAddress = module.modBaseAddr;
            BaseSize = module.modBaseSize;
            Handle = module.hModule;
            Name = module.szModule;
            Path = module.szExePath;

            ModuleId = module.th32ModuleID;
            StructureSize = module.dwSize;
            GlblcntUsage = module.GlblcntUsage;
            ProccntUsage = module.ProccntUsage;
        }

        #region IModuleInfo Members
        
        /// <summary>
        /// Gets the identifier of the process who owns this module.
        /// </summary>
        public uint ProcessId { get; private set; }

        /// <summary>
        /// Gets the base address of the module in the context of the owning process.
        /// </summary>
        public IntPtr BaseAddress { get; private set; }

        /// <summary>
        /// Gets the size of the module, in bytes.
        /// </summary>
        public uint BaseSize { get; private set; }

        /// <summary>
        /// Gets a handle to the module in the context of the owning process.
        /// </summary>
        public IntPtr Handle { get; private set; }

        /// <summary>
        /// Gets the module name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the module path.
        /// </summary>
        public string Path { get; private set; }

        #endregion

        public uint ModuleId { get; private set; }
        public uint StructureSize { get; private set; }
        public uint GlblcntUsage { get; private set; }
        public uint ProccntUsage { get; private set; }
    }
}

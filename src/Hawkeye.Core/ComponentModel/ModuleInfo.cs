using System;
using System.ComponentModel;

using Hawkeye.WinApi;

namespace Hawkeye.ComponentModel
{
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


        public uint ProcessId { get; private set; }

        public IntPtr BaseAddress { get; private set; }

        public uint BaseSize { get; private set; }

        public IntPtr Handle { get; private set; }

        public string Name { get; private set; }

        public string Path { get; private set; }

        #endregion

        public uint ModuleId { get; private set; }
        public uint StructureSize { get; private set; }
        public uint GlblcntUsage { get; private set; }
        public uint ProccntUsage { get; private set; }
    }
}

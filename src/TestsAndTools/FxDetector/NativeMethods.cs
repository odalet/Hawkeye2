using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;

using Microsoft.Win32.SafeHandles;

/////////////////////////////////////////////////////////////////////
// Large parts and overall inspiration for this code come from the 
// Snoop project (http://xxx). Cory Plotts and Xxx should be
// granted credit for this. 
/////////////////////////////////////////////////////////////////////
namespace FxDetector
{
    internal static class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int processId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static public extern ToolHelpHandle CreateToolhelp32Snapshot(SnapshotFlags dwFlags, int th32ProcessID);

        [DllImport("kernel32.dll")]
        static public extern bool Module32First(ToolHelpHandle hSnapshot, ref MODULEENTRY32 lpme);

        [DllImport("kernel32.dll")]
        static public extern bool Module32Next(ToolHelpHandle hSnapshot, ref MODULEENTRY32 lpme);

        [DllImport("kernel32.dll", SetLastError = true)]
        static public extern bool CloseHandle(IntPtr hHandle);

        /// <summary>
        /// Safe handle wrapper for Module32* and CreateToolhelp32Snapshot Win32 functions.
        /// </summary>
        public class ToolHelpHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ToolHelpHandle"/> class.
            /// </summary>
            private ToolHelpHandle() : base(true) { }

            /// <summary>
            /// When overridden in a derived class, 
            /// executes the code required to free the handle.
            /// </summary>
            /// <returns>
            /// true if the handle is released successfully; 
            /// otherwise, in the event of a catastrophic failure, false. 
            /// In this case, it generates a releaseHandleFailed MDA 
            /// Managed Debugging Assistant.
            /// </returns>
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
            override protected bool ReleaseHandle()
            {
                return CloseHandle(handle);
            }
        }

        [Flags]
        public enum SnapshotFlags : uint
        {
            HeapList = 0x00000001,
            Process = 0x00000002,
            Thread = 0x00000004,
            Module = 0x00000008,
            Module32 = 0x00000010,
            Inherit = 0x80000000,
            All = 0x0000001F
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        [TypeConverter(typeof(ModuleEntryConverter))]
        public struct MODULEENTRY32
        {
            public uint dwSize;
            public uint th32ModuleID;
            public uint th32ProcessID;
            public uint GlblcntUsage;
            public uint ProccntUsage;
            IntPtr modBaseAddr;
            public uint modBaseSize;
            IntPtr hModule;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szModule;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szExePath;
        }
    }
}

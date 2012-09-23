using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;

using Microsoft.Win32.SafeHandles;

namespace Hawkeye.WinApi
{
    ///////////////////////////////////////////////////////////////////////
    //// Large parts and overall inspiration for the module detection code 
    //// come from the Snoop project (http://snoopwpf.codeplex.com/). 
    //// Cory Plotts should be granted credit for this. 
    ///////////////////////////////////////////////////////////////////////
    internal static class NativeMethods
    {
        #region Imported methods

        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int processId);

        [DllImport("user32.dll")]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern ToolHelpHandle CreateToolhelp32Snapshot(SnapshotFlags dwFlags, int th32ProcessID);

        [DllImport("kernel32.dll")]
        public static extern bool Module32First(ToolHelpHandle hSnapshot, ref MODULEENTRY32 lpme);

        [DllImport("kernel32.dll")]
        public static extern bool Module32Next(ToolHelpHandle hSnapshot, ref MODULEENTRY32 lpme);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hHandle);
        
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr LoadCursorFromFile(string path);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr CopyImage(IntPtr hImage, int uType, int cxDesired, int cyDesired, int fuFlags);

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(POINT Point);

        [DllImport("user32.dll")]
        public static extern IntPtr ChildWindowFromPoint(IntPtr hwndParent, POINT Point);

        [DllImport("user32.dll")]
        public static extern bool ScreenToClient(IntPtr hwnd, ref POINT lpPoint);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hwnd, ref RECT rc);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetParent(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hwnd, IntPtr hDC);

        [DllImport("user32.dll")]
        public static extern bool InvalidateRect(IntPtr hwnd, IntPtr lpRect, bool bErase);

        [DllImport("user32.dll")]
        public static extern bool UpdateWindow(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern bool RedrawWindow(IntPtr hwnd, IntPtr lpRect, IntPtr hrgnUpdate, uint flags);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hwnd, uint msg, IntPtr wparam, IntPtr lparam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool EnableWindow(IntPtr hwnd, bool enable);

        #endregion

        #region Helpers

        public static string GetWindowClassName(IntPtr hwnd)
        {
            const int max = 256;
            var buffer = new StringBuilder(max);
            GetClassName(hwnd, buffer, max);
            return buffer.ToString();
        }

        #endregion

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
    }
}

using System;
using System.Text;
using System.Runtime.InteropServices;

namespace Hawkeye.DecompilePlugin
{
    /// <summary>
    /// Allows an external program to remotely control a decompiler (Reflector or ILSpy).
    /// This class is adapted from RemoteController.cs found in 
    /// .NET Reflector addins project (http://www.codeplex.com/reflectoraddins)
    /// </summary>
    /// <remarks>
    /// ILSpy supports a way of being remotely controlled very similar to Reflector's
    /// See https://github.com/icsharpcode/ILSpy/blob/master/doc/Command%20Line.txt.
    /// </remarks>
    internal abstract class BaseDecompilerController : IDecompilerController
    {
        private IntPtr targetWindow = IntPtr.Zero;

        public BaseDecompilerController()
        {
            WindowMessage = WM_COPYDATA;
        }

        #region Win32 Interop

        private const int WM_COPYDATA = 0x4A;

        private delegate bool EnumWindowsCallback(IntPtr hwnd, int lparam);

        [DllImport("user32.dll")]
        private static extern int EnumWindows(EnumWindowsCallback callback, int lparam);

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder title, int size);

        [DllImport("user32.dll")]
        private static extern bool SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref CopyDataStruct lParam);

        [StructLayout(LayoutKind.Sequential)]
        private struct CopyDataStruct
        {
            public IntPtr Padding;
            public int Size;
            public IntPtr Buffer;

            public CopyDataStruct(IntPtr padding, int size, IntPtr buffer)
            {
                Padding = padding;
                Size = size;
                Buffer = buffer;
            }
        }

        #endregion

        #region IDecompilerController Members

        /// <summary>
        /// Gets a value indicating whether a decompiler instance is running.
        /// </summary>
        /// <value>
        /// <c>true</c> if a running decompiler instance could be found; otherwise, <c>false</c>.
        /// </value>
        public abstract bool IsRunning { get; }

        /// <summary>
        /// Loads the type's assembly then selects the specified type declaration in the decompiler;
        /// </summary>
        /// <param name="type">The type to decompile.</param>
        /// <returns>
        ///   <c>true</c> if the action succeeded; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public abstract bool GotoType(Type type);

        #endregion

        /// <summary>
        /// Gets the target window (each call re-enumerates to make sure the target exists).
        /// </summary>
        protected IntPtr TargetWindow
        {
            get
            {
                targetWindow = IntPtr.Zero;
                EnumWindows(new EnumWindowsCallback(EnumWindow), 0);
                return targetWindow;
            }
        }

        /// <summary>
        /// Determines wether the specified window title matches the actual decompiler.
        /// </summary>
        /// <param name="title">The window title.</param>
        /// <returns><c>true</c> if matches; otherwise, <c>false</c>.</returns>
        protected abstract bool DoesWindowTitleMatches(string title);

        /// <summary>
        /// Gets or sets the window message used to communicate with the decompiler instance.
        /// </summary>
        /// <value>
        /// Default value is WM_COPYDATA.
        /// </value>
        protected virtual int WindowMessage { get; set; }

        protected bool Send(string message)
        {
            targetWindow = IntPtr.Zero;

            // We can't use a simple FindWindow, because the decompiler window title 
            // can vary: we must detect its window title starts with a known value; 
            // not simply it is equal to a known value. See the EnumWindow method.
            EnumWindows(new EnumWindowsCallback(EnumWindow), 0);

            if (targetWindow != IntPtr.Zero)
            {
                var chars = message.ToCharArray();
                var data = new CopyDataStruct();
                data.Padding = IntPtr.Zero;
                data.Size = chars.Length * 2;
                data.Buffer = Marshal.AllocHGlobal(data.Size);
                Marshal.Copy(chars, 0, data.Buffer, chars.Length);

                var result = SendMessage(targetWindow, WindowMessage, IntPtr.Zero, ref data);
                Marshal.FreeHGlobal(data.Buffer);

                return result;
            }

            return false;
        }

        private bool EnumWindow(IntPtr handle, int lparam)
        {
            var titleBuilder = new StringBuilder(256);
            GetWindowText(handle, titleBuilder, 256);

            var title = titleBuilder.ToString();
            if (DoesWindowTitleMatches(title))
            {
                targetWindow = handle;
                return false; // No need to enumerate other windows
            }
            else return true; // Try again
        }
    }
}

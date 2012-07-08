using System;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Hawkeye.WinApi
{
    internal class WindowInfo : IWindowInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowInfo"/> class.
        /// </summary>
        /// <param name="hwnd">The target window handle.</param>
        public WindowInfo(IntPtr hwnd)
        {
            Handle = hwnd;
            RunDetection();
        }

        #region IWindowInfo Members

        /// <summary>
        /// Gets this Window handle.
        /// </summary>
        public IntPtr Handle { get; private set; }

        /// <summary>
        /// Gets this Window's owner process bitness.
        /// </summary>
        public Bitness Bitness { get; private set; }

        /// <summary>
        /// Gets this Window's owner process CLR version.
        /// </summary>
        public Clr Clr { get; private set; }

        /// <summary>
        /// Gets the list of modules loaded by this Window's owner process.
        /// </summary>
        public IModuleInfo[] Modules { get; private set; }

        /// <summary>
        /// Dumps the content of this object in a text form.
        /// </summary>
        /// <returns>
        /// A string with the dumped data.
        /// </returns>
        public string Dump()
        {

            var builder = new StringBuilder();
            builder.AppendFormattedLine("Hwnd = {0}", Handle);
            builder.AppendFormattedLine("Bitness = {0}", Bitness);
            builder.AppendFormattedLine("Clr = {0}", Clr);
            builder.AppendFormattedLine("Modules ({0}):", Modules.Length);

            int index = 0;
            foreach (var module in Modules)
            {
                builder.AppendFormattedLine(1, "Module {0}:", index);
                module.DumpTo(builder, 2);
                index++;
            }

            return builder.ToString();
        }

        #endregion

        private void RunDetection()
        {
            Modules = GetModules().Select(m => new ModuleInfo(m)).ToArray();

            Clr = DetectFramework();
            Bitness = DetectBitness();
        }

        private Clr DetectFramework()
        {
            var mscorlibs = Modules
                .Where(m => m.Name.Contains("mscorlib")).ToArray();
            if (mscorlibs.Length == 0) return Clr.Net2;
            if (mscorlibs
                .Select(m => FileVersionInfo.GetVersionInfo(m.Path).FileMajorPart)
                .Any(v => v == 4)) return Clr.Net4;
            if (mscorlibs
                .Select(m => FileVersionInfo.GetVersionInfo(m.Path).FileMajorPart)
                .Any(v => v >= 2 && v < 4)) return Clr.Net2;
            return Clr.Unknown;
        }

        private Bitness DetectBitness()
        {
            // In case we are a x64 process, we detect wether the inspected
            // window is running in Wow64 mode; if this is the case, the 
            // inspected app is a 32bits application running on a x64 box.
            // Hope nobody will have the good idea to name one of its dll
            // wow64 in a 32bits app... ;)
            if (This.IsX64)
            {
                var isWow64 = Modules.Any(
                    m => m.Name.ToLowerInvariant().Contains("wow64"));
                return isWow64 ? Bitness.x86 : Bitness.x64;
            }

            // Otherwise, the test is simple: if we can detect modules,
            // it means the app is x86 (as we are).
            return Modules.Length > 0 ? Bitness.x86 : Bitness.x64;
        }


        /// <summary>
        /// Similar to System.Diagnostics.WinProcessManager.GetModuleInfos,
        /// except that we include 32 bit modules when we run in x64 mode.
        /// See http://blogs.msdn.com/b/jasonz/archive/2007/05/11/code-sample-is-your-process-using-the-silverlight-clr.aspx
        /// </summary>
        private IEnumerable<MODULEENTRY32> GetModules()
        {
            int processId;
            NativeMethods.GetWindowThreadProcessId(Handle, out processId);

            var me32 = new MODULEENTRY32();
            var hModuleSnap = NativeMethods.CreateToolhelp32Snapshot(
                SnapshotFlags.Module | SnapshotFlags.Module32,
                processId);

            if (hModuleSnap.IsInvalid) yield break;

            using (hModuleSnap)
            {
                me32.dwSize = (uint)Marshal.SizeOf(me32);
                if (NativeMethods.Module32First(hModuleSnap, ref me32))
                {
                    do
                    {
                        yield return me32;
                    } while (NativeMethods.Module32Next(hModuleSnap, ref me32));
                }
            }
        }
    }
}

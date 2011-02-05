using System;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;

/////////////////////////////////////////////////////////////////////
// Large parts and overall inspiration for this code come from the 
// Snoop project (http://xxx). Cory Plotts and Xxx should be
// granted credit for this. 
/////////////////////////////////////////////////////////////////////
namespace FxDetector
{
    internal enum Bitness
    {
        Unknown, X86, X64
    }

    internal enum FxVersion
    {
        Unknown, V2, V4
    }

    internal class WindowInfo
    {
        public WindowInfo(IntPtr hwnd)
        {
            Hwnd = hwnd;
            RunDetection();
        }

        public IntPtr Hwnd { get; private set; }

        public Bitness Bitness { get; private set; }

        public FxVersion FxVersion { get; private set; }

        public NativeMethods.MODULEENTRY32[] Modules { get; private set; }

        public string Dump()
        {

            var builder = new StringBuilder();
            builder.AppendFormattedLine("Hwnd = {0}", Hwnd);
            builder.AppendFormattedLine("Bitness = {0}", Bitness);
            builder.AppendFormattedLine("FxVersion = {0}", FxVersion);
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

        private void RunDetection()
        {
            Modules = GetModules().ToArray();

            FxVersion = DetectFramework();
            Bitness = DetectBitness();
        }

        private FxVersion DetectFramework()
        {
            var mscorlibs = Modules
                .Where(m => m.szModule.Contains("mscorlib")).ToArray();
            if (mscorlibs.Length == 0) return FxVersion.Unknown;
            if (mscorlibs
                .Select(m => FileVersionInfo.GetVersionInfo(m.szExePath).FileMajorPart)
                .Any(v => v == 4)) return FxVersion.V4;
            if (mscorlibs
                .Select(m => FileVersionInfo.GetVersionInfo(m.szExePath).FileMajorPart)
                .Any(v => v >= 2 && v < 4)) return FxVersion.V2;
            return FxVersion.Unknown;
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
                    m => m.szModule.ToLowerInvariant().Contains("wow64"));
                return isWow64 ? Bitness.X86 : Bitness.X64;
            }
         
            // Otherwise, the test is simple: if we can detect modules,
            // it means the app is x86 (as we are).
            return Modules.Length > 0 ? Bitness.X86 : Bitness.X64;
        }
        
        /// <summary>
        /// Similar to System.Diagnostics.WinProcessManager.GetModuleInfos,
        /// except that we include 32 bit modules when we run in x64 mode.
        /// See http://blogs.msdn.com/b/jasonz/archive/2007/05/11/code-sample-is-your-process-using-the-silverlight-clr.aspx
        /// </summary>
        private IEnumerable<NativeMethods.MODULEENTRY32> GetModules()
        {
            int processId;
            NativeMethods.GetWindowThreadProcessId(Hwnd, out processId);

            var me32 = new NativeMethods.MODULEENTRY32();
            var hModuleSnap = NativeMethods.CreateToolhelp32Snapshot(
                NativeMethods.SnapshotFlags.Module | NativeMethods.SnapshotFlags.Module32, 
                processId);

            if (!hModuleSnap.IsInvalid)
            {
                using (hModuleSnap)
                {
                    me32.dwSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(me32);
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
}

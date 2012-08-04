using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;

using Hawkeye.UI;
using Hawkeye.WinApi;
using Hawkeye.Logging;
using Hawkeye.Reflection;

namespace Hawkeye
{
    internal static class HawkeyeRunner
    {
        private static readonly Guid runnerId = Guid.NewGuid();
        private static MainForm mainForm = null;

        public static bool IsInjected
        {
            get;
            private set;
        }

        public static void Run()
        {
            InitializeHawkeye();
            LogDebug(string.Format("{0} - Running Hawkeye", runnerId));

            if (mainForm != null) mainForm.Close();
            mainForm = new MainForm();
            Application.Run(mainForm);
        }
        
        public static void Inject(IWindowInfo info)
        {
            if (info == null) throw new ArgumentNullException("info");
            var handle = info.Handle;
            var bootstrapExecutable = GetBootstrap(info.Clr, info.Bitness);
            var arguments = new string[]
            {
                handle.ToString(),                                          // Target window
                mainForm.Handle.ToString(),                                 // Original Hawkeye 
                "\"" + Assembly.GetExecutingAssembly().Location + "\"",     // This assembly
                "\"" + typeof(HawkeyeRunner).FullName + "\"",                           // This class
                "Attach"                                                    // Attach method
            };

            var pinfo = new ProcessStartInfo(
                bootstrapExecutable, string.Join(" ", arguments));
            Process.Start(pinfo);
        }

        //TODO: private static void Attach(IntPtr target, IntPtr origin)
        private static void Attach(string target, string hawkeye, string assembly, string type, string method)
        {
            // Because when injected the base probing path is the one of the injected app, not the hawkeye root path.
            AppDomain.CurrentDomain.AssemblyResolve += (s, r) => 
                AssemblyResolver.Resolve(r.Name);

            InitializeHawkeye();

            IntPtr targetHandle = StringToIntPtr(target);
            IntPtr hawkeyeHandle = StringToIntPtr(hawkeye);

            IsInjected = true;
            LogDebug(string.Format("{0} - Attached to {1}", runnerId, Application.ProductName));

            if (mainForm != null) mainForm.Close();
            mainForm = new MainForm();
            mainForm.SetTarget(targetHandle);

            // close original window
            NativeMethods.SendMessage(hawkeyeHandle, WindowMessages.WM_CLOSE, IntPtr.Zero, IntPtr.Zero); // close
            
            // Show new window
            mainForm.Show();
        }

        private static void InitializeHawkeye()
        {
            This.InitializeApi(new ThisImplementation());
            var log = LogManager.GetLogger(typeof(HawkeyeRunner));
        }


        private static void LogDebug(string message)
        {
            var log = LogManager.GetLogger(typeof(HawkeyeRunner));
            log.Debug(message);
        }

        #region Helpers

        private static IntPtr StringToIntPtr(string ptr)
        {
            return IntPtr.Size == 8 ?
                new IntPtr(long.Parse(ptr)) : new IntPtr(int.Parse(ptr));
        }

        private static string GetBootstrap(Clr clr, Bitness bitness)
        {
            var clrVersion = string.Empty;
            switch (clr)
            {
                case Clr.Net2: clrVersion = "N2"; break;
                case Clr.Net4: clrVersion = "N4"; break;
                default: throw new ArgumentException(string.Format(
                    "Clr Value {0} is invalid.", clr), "clr");
            }

            var bitnessVersion = string.Empty;
            switch (bitness)
            {
                case Bitness.x86: bitnessVersion = "x86"; break;
                case Bitness.x64: bitnessVersion = "x64"; break;
                default: throw new ArgumentException(string.Format(
                    "Bitness Value {0} is invalid.", bitness), "bitness");
            }

            var exe = string.Format(
                "HawkeyeBootstrap{0}{1}.exe", clrVersion, bitnessVersion);
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(directory, exe);
        }

        #endregion
    }
}

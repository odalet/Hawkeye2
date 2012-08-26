using System;
using System.IO;
using ManagedInjector;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace HawkeyeBootstrap
{
    internal static class Program
    {
        private static readonly string logFileName;

        static Program()
        {
            //SimpleLogManager.Initialize(

            // Determine where to log; should be C:\ProgramData\Hawkeye\HawkeyeInjector.log on Win7
            var logFileDirectory = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.CommonApplicationData), "Hawkeye");
            if (!Directory.Exists(logFileDirectory))
                Directory.CreateDirectory(logFileDirectory);

            logFileName = Path.Combine(logFileDirectory, "HawkeyeInjector.log");
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        [STAThread]
        private static void Main(string[] args)
        {
            Log("Starting the injection process...", false);

            var windowHandle = (IntPtr)Int64.Parse(args[0]);
            var originalHandle = (IntPtr)Int64.Parse(args[1]);
            var assemblyName = args[2];
            var className = args[3];
            var methodName = args[4];

            Injector.Launch(windowHandle, originalHandle, assemblyName, className, methodName, logFileName);

            //check to see that it was injected, and if not, retry with the main window handle.
            var process = GetProcessFromWindowHandle(windowHandle);
            if (process != null && !CheckInjectedStatus(process) && process.MainWindowHandle != windowHandle)
            {
                Log("Could not inject with current handle... retrying with MainWindowHandle");
                Injector.Launch(process.MainWindowHandle, originalHandle, assemblyName, className, methodName, logFileName);
                CheckInjectedStatus(process);
            }
        }

        private static Process GetProcessFromWindowHandle(IntPtr windowHandle)
        {
            int processId;
            GetWindowThreadProcessId(windowHandle, out processId);
            if (processId == 0)
            {
                Log(string.Format("could not get process for window handle {0}", windowHandle));
                return null;
            }

            var process = Process.GetProcessById(processId);
            if (process == null)
            {
                Log(string.Format("could not get process for PID = {0}", processId));
                return null;
            }
            return process;
        }

        private static bool CheckInjectedStatus(Process process)
        {
            bool containsFile = false;
            process.Refresh();
            foreach (ProcessModule module in process.Modules)
            {
                if (module.FileName.Contains("HawkeyeInjector"))
                    containsFile = true;
            }

            if (containsFile) Log(string.Format(
                "Successfully injected Hawkeye for process {0} (PID = {1})", process.ProcessName, process.Id));
            else Log(string.Format(
                "Failed to inject for process {0} (PID = {1})", process.ProcessName, process.Id));

            return containsFile;
        }

        [DllImport("user32.dll")]
        private static extern int GetWindowThreadProcessId(IntPtr hwnd, out int processId);

        private static void Log(string message, bool append = true)
        {
            Injector.LogMessage(logFileName, message, append);
        }
    }
}

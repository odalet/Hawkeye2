using System;
using System.Collections.Generic;
using System.Text;
using Hawkeye.Logging;
using Hawkeye.WinApi;
using Hawkeye.UI;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using Hawkeye.Logging.log4net;

namespace Hawkeye
{
    partial class HawkeyeApplication
    {
        private class Implementation : IHawkeyeApplicationImplementation
        {
            private readonly Guid hawkeyeId = Guid.NewGuid();
            private MainForm mainForm = null;
            private ILogServiceFactory logFactory = null;

            #region IHawkeyeApplicationImplementation Members

            public bool IsInjected
            {
                get;
                private set;
            }

            /// <summary>
            /// Runs the Hawkeye application.
            /// </summary>
            public void Run()
            {
                LogDebug(string.Format("{0} - Running Hawkeye in its own process.", hawkeyeId.GetHashCode()));
                Initialize();
                LogDebug(string.Format("{0} - Hawkeye initialization is complete", hawkeyeId.GetHashCode()));

                if (mainForm != null) mainForm.Close();
                mainForm = new MainForm();
                Application.Run(mainForm);
            }

            /// <summary>
            /// Attaches the specified info.
            /// </summary>
            /// <param name="info">The info.</param>
            public void Inject(IWindowInfo info)
            {
                if (info == null) throw new ArgumentNullException("info");

                var handle = info.Handle;
                var bootstrapExecutable = GetBootstrap(info.Clr, info.Bitness);
                var hawkeyeAttacherType = typeof(__HawkeyeAttacherNamespace__.HawkeyeAttacher);
                var arguments = new string[]
                {
                    handle.ToString(),                                          // Target window
                    mainForm.Handle.ToString(),                                 // Original Hawkeye 
                    "\"" + hawkeyeAttacherType.Assembly.Location + "\"",     // This assembly
                    "\"" + hawkeyeAttacherType.FullName + "\"",          // The main hawkeye application class
                    "Attach"                                                    // Attach method
                };

                var pinfo = new ProcessStartInfo(
                    bootstrapExecutable, string.Join(" ", arguments));
                Process.Start(pinfo);
            }

            /// <summary>
            /// Injects the specified target window.
            /// </summary>
            /// <param name="targetWindow">The target window.</param>
            /// <param name="originalHawkeyeWindow">The original hawkeye window.</param>
            public void Attach(IntPtr targetWindow, IntPtr originalHawkeyeWindow)
            {
                // Because native c++ code called Attach, we now know we are injected.
                IsInjected = true;

                // close original window: must be done before we try to log anything because the log file is locked
                // by the previous Hawkeye instance.
                NativeMethods.SendMessage(originalHawkeyeWindow, WindowMessages.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);

                // Let's get the target window associated pid.
                int pid;
                NativeMethods.GetWindowThreadProcessId(targetWindow, out pid);

                LogDebug(string.Format("{0} - Running Hawkeye attached to application {1} (pid={2})", 
                    hawkeyeId.GetHashCode(), Application.ProductName, pid));
                Initialize();
                LogDebug(string.Format("{0} - Hawkeye initialization is complete", hawkeyeId.GetHashCode()));


                if (mainForm != null) mainForm.Close();
                mainForm = new MainForm();
                mainForm.SetTarget(targetWindow);
                
                // Show new window
                mainForm.Show();
            }

            /// <summary>
            /// Gets the default log service factory.
            /// </summary>
            /// <returns>
            /// An instance of <see cref="ILogServiceFactory"/>.
            /// </returns>
            public ILogServiceFactory GetLogServiceFactory()
            {
                if (logFactory == null)
                {
                    // when injected, log4net won't find its configuration where it expects it to be
                    // so we suppos we have a log4net.config file in the root directory of hawkeye.
                    var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    var log4netConfigFile = Path.Combine(directory, "log4net.config");
                    logFactory = new Log4NetServiceFactory(log4netConfigFile);
                }

                return logFactory;
            }

            #endregion

            private void Initialize()
            { 
                // Initialize the hawheye API host
                HawkeyeHost.Initialize(new HawkeyeApplication.HawkeyeHostImplementation(this));
                LogDebug("The Hawkeye host has been initialized; The Hawkeye API is now available to plugins.");

                // Now discover then load plugins
                
                LogDebug("Discovering Hawkeye plugins.");
                //DiscoverPlugins
                int discoveredCount = 0;
                
                LogDebug("Loading Hawkeye plugins.");
                //LoadPlugins
                int loadedCount = 0;

                LogDebug(string.Format("{0}/{1} Hawkeye plugins were successfully loaded.", loadedCount, discoveredCount));
            }

            private static void LogDebug(string message)
            {
                var log = LogManager.GetLogger<HawkeyeApplication.Implementation>();
                log.Debug(message);
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

        }
    }
}

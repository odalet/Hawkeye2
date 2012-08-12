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
                Run(IntPtr.Zero, IntPtr.Zero);
            }

            /// <summary>
            /// Runs the Hawkeye application.
            /// </summary>
            /// <param name="windowToSpy">The window to spy.</param>
            /// <param name="windowToKill">The window to kill.</param>
            public void Run(IntPtr windowToSpy, IntPtr windowToKill)
            {
                // close original window: must be done before we try to log anything because the log file is locked
                // by the previous Hawkeye instance.
                if (windowToKill != IntPtr.Zero) NativeMethods.SendMessage(
                    windowToKill, WindowMessages.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);

                var appId = hawkeyeId.GetHashCode();
                LogDebug("Running Hawkeye in its own process.", appId);
                LogDebug(string.Format("Parameters: {0}, {1}.", windowToSpy, windowToKill), appId);
                Initialize();
                LogDebug("Hawkeye initialization is complete", appId);

                if (mainForm != null) mainForm.Close();
                mainForm = new MainForm();
                if (windowToSpy != IntPtr.Zero)
                    mainForm.SetTarget(windowToSpy);

                Application.Run(mainForm);
            }
            
            /// <summary>
            /// Determines whether Hawkeye can be injected given the specified window info.
            /// </summary>
            /// <param name="info">The window info.</param>
            /// <returns>
            ///   <c>true</c> if Hawkeye can be injected; otherwise, <c>false</c>.
            /// </returns>
            public bool CanInject(IWindowInfo info)
            {
                if (info == null) return false;
                     
                // Same process, don't inject.
                if (info.ProcessId == Process.GetCurrentProcess().Id) return false;

                // Not a .NET process
                if (info.Clr == Clr.None) return false;

                // Not a supported CLR
                if (info.Clr == Clr.Unsupported) return false;
                
                // Don't know! But maybe this is because we triedto inspect a x64 process and we are x86...
                if (info.Clr == Clr.Undefined) 
                    return HawkeyeApplication.CurrentBitness == Bitness.x86 && info.Bitness == Bitness.x64;
                
                // Otherwise, ok
                return true;
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

                var appId = hawkeyeId.GetHashCode();
                LogDebug(string.Format("Running Hawkeye attached to application {0} (pid={1})",
                    Application.ProductName, pid), appId);
                Initialize();
                LogDebug("Hawkeye initialization is complete", appId);


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

            private static void LogDebug(string message, int appId = 0)
            {
                var log = LogManager.GetLogger<HawkeyeApplication.Implementation>();

                if (appId == 0) log.Debug(message);
                else log.Debug(string.Format(
                    "{0} - {1}", appId.GetHashCode(), message));
            }

            private static string GetBootstrap(Clr clr, Bitness bitness)
            {
                var bitnessVersion = string.Empty;
                switch (bitness)
                {
                    case Bitness.x86: bitnessVersion = "x86"; break;
                    case Bitness.x64: bitnessVersion = "x64"; break;
                    default: throw new ArgumentException(string.Format(
                        "Bitness Value {0} is invalid.", bitness), "bitness");
                }

                var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                // Very special case: Hawkeye is x86 and the spied process is x64: we can't know for sure
                // whether the process is .NET 2 or 4 or none.
                // So, we must simply re-run Hawkeye.exe (which is compiled as Any Cpu and therefore
                // Will run as x64 in a x64 environment.
                if (clr == Clr.Undefined && HawkeyeApplication.CurrentBitness == Bitness.x86 && bitness == Bitness.x64)
                    return Path.Combine(directory, "Hawkeye.exe");

                var clrVersion = string.Empty;
                switch (clr)
                {
                    case Clr.Net2: clrVersion = "N2"; break;
                    case Clr.Net4: clrVersion = "N4"; break;
                    default: throw new ArgumentException(
                        string.Format("Clr Value {0} is invalid.", clr), "clr");
                }

                var exe = string.Format("HawkeyeBootstrap{0}{1}.exe", clrVersion, bitnessVersion);                
                return Path.Combine(directory, exe);
            }
        }
    }
}

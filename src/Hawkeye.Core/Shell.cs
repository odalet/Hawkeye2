using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;

using Hawkeye.UI;
using Hawkeye.WinApi;
using Hawkeye.Logging;
using Hawkeye.Logging.log4net;
using Hawkeye.Configuration;
using Hawkeye.Extensibility;

namespace Hawkeye
{
    internal class Shell : IHawkeyeHost
    {
        private readonly Guid hawkeyeId;

        private MainForm mainForm = null;
        private MainControl mainControl = null;
        private ILogServiceFactory logFactory = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Shell"/> class.
        /// </summary>
        public Shell()
        {
            hawkeyeId = Guid.NewGuid();
            ApplicationInfo = new HawkeyeApplicationInfo();
            
            // Do nothing else here, otherwise, HawkeyeApplication static constructor may fail.
        }

        #region IHawkeyeHost Members

        public event EventHandler CurrentWindowInfoChanged;

        public ILogService GetLogger(Type type)
        {
            return LogManager.GetLogger(type);
        }

        public ISettingsStore GetSettings(string key)
        {
            if (string.IsNullOrEmpty(key) || key == SettingsManager.HawkeyeStoreKey)
            {
                // Let's get a read-only version of Hawkeye settings
                var hawkeyeSettings = SettingsManager.GetHawkeyeStore();
                return new SettingsManager.ReadOnlyStoreWrapper(hawkeyeSettings);
            }

            return SettingsManager.GetStore(key);
        }

        public IHawkeyeApplicationInfo ApplicationInfo { get; private set; }

        public IWindowInfo CurrentWindowInfo
        {
            get 
            {
                return mainControl == null ? 
                    null : mainControl.CurrentInfo; 
            }
        }

        #endregion

        public PluginManager PluginManager { get; private set;  }

        public bool IsInjected { get; private set; }

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
            LogInfo("Running Hawkeye in its own process.", appId);
            LogDebug(string.Format("Parameters: {0}, {1}.", windowToSpy, windowToKill), appId);
            Initialize();
            LogDebug("Hawkeye initialization is complete", appId);
            
            InitializeMainForm(windowToSpy);
            LogDebug("Hawkeye Main Form initialization is complete", appId);

            Application.Run(mainForm);
        }

        /// <summary>
        /// Operations that should be realized before we close Hawkeye.
        /// </summary>
        public void Close()
        {
            LogDebug(new string('-', 80));

            // Save settings & layout
            SettingsManager.Save();

            // Release resources (the log file for example) held by log4net.                
            LogManager.Shutdown();
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

            // Don't know! But maybe this is because we tried to inspect a x64 process and we are x86...
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
                    handle.ToString(),                                      // Target window
                    mainForm.Handle.ToString(),                             // Original Hawkeye 
                    "\"" + hawkeyeAttacherType.Assembly.Location + "\"",    // This assembly
                    "\"" + hawkeyeAttacherType.FullName + "\"",             // The name of the class responsible for attaching to the process
                    "Attach"                                                // Attach method
                };

            var args = string.Join(" ", arguments);
            var pinfo = new ProcessStartInfo(bootstrapExecutable, args);

            LogInfo(string.Format("Starting a new instance of Hawkeye: {0}", bootstrapExecutable));
            LogDebug(string.Format("Command is: {0} {1}", bootstrapExecutable, args));

            // Close Hawkeye; i.e. clean everything beore it is really killed in the Attach method.
            Close();

            Process.Start(pinfo);
        }

        /// <summary>
        /// Injects the specified target window.
        /// </summary>
        /// <param name="windowToSpy">The target window.</param>
        /// <param name="originalHawkeyeWindow">The original hawkeye window.</param>
        public void Attach(IntPtr windowToSpy, IntPtr originalHawkeyeWindow)
        {
            // Because native c++ code called Attach, we now know we are injected.
            IsInjected = true;

            // close original window: must be done before we try to log anything because the log file is locked
            // by the previous Hawkeye instance.
            NativeMethods.SendMessage(originalHawkeyeWindow, WindowMessages.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);

            // Let's get the target window associated pid.
            int pid;
            NativeMethods.GetWindowThreadProcessId(windowToSpy, out pid);

            var appId = hawkeyeId.GetHashCode();
            LogInfo(string.Format("Running Hawkeye attached to application {0} (pid={1})",
                Application.ProductName, pid), appId);
            Initialize();
            LogDebug("Hawkeye initialization is complete", appId);

            InitializeMainForm(windowToSpy);
            LogDebug("Hawkeye Main Form initialization is complete", appId);

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
                // so we suppose we have a log4net.config file in the root directory of hawkeye.
                var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var log4netConfigFile = Path.Combine(directory, "log4net.config");
                logFactory = new Log4NetServiceFactory(log4netConfigFile);
            }

            return logFactory;
        }
        
        private void Initialize()
        {
            // Load Hawkeye settings
            SettingsManager.Initialize();

            // Now discover then load plugins
            PluginManager = new PluginManager();

            LogDebug("Discovering Hawkeye plugins.");
            PluginManager.DiscoverAll();
            var discoveredCount = PluginManager.PluginDescriptors.Length;

            LogDebug("Loading Hawkeye plugins.");
            PluginManager.LoadAll(this);
            int loadedCount = PluginManager.Plugins.Length;

            LogDebug(string.Format("{0}/{1} Hawkeye plugins were successfully loaded.", loadedCount, discoveredCount));
        }

        private void InitializeMainForm(IntPtr windowToSpy)
        {
            if (mainForm != null) mainForm.Close();
            mainForm = new MainForm();
            if (windowToSpy != IntPtr.Zero)
                mainForm.SetTarget(windowToSpy);

            mainControl = mainForm.MainControl;
            mainControl.CurrentInfoChanged += (s, _) =>
                RaiseCurrentWindowInfoChanged();

            RaiseCurrentWindowInfoChanged();
        }

        private void RaiseCurrentWindowInfoChanged()
        {
            if (CurrentWindowInfoChanged != null)
                CurrentWindowInfoChanged(this, EventArgs.Empty);
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
            // will run as x64 in a x64 environment) passing it the handle of the spied window so that another 
            // detection is achieved, this time from a x64 process.
            // Note that because we run Hawkeye.exe, we won't inject anything.
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

        #region Logging

        private static void LogInfo(string message, int appId = 0)
        {
            Log(LogLevel.Info, message, appId);
        }

        private static void LogDebug(string message, int appId = 0)
        {
            Log(LogLevel.Debug, message, appId);
        }

        private static void Log(LogLevel level, string message, int appId)
        {
            var log = LogManager.GetLogger<Shell>();
            log.Log(level, appId == 0 ? message :
                string.Format("{0} - {1}", appId.GetHashCode(), message));
        }

        #endregion
    }
}

using System;
using Hawkeye.Logging;

namespace Hawkeye
{
    /// <summary>
    /// The root class representing the Hawkeye Application.
    /// </summary>
    internal static partial class HawkeyeApplication
    {
        private static readonly HawkeyeApplicationInfo applicationInfo;
        private static readonly Bitness currentBitness;
        private static readonly Clr currentClr;

        /// <summary>
        /// Initializes the <see cref="HawkeyeApplication"/> class.
        /// </summary>
        static HawkeyeApplication()
        {
            applicationInfo = new HawkeyeApplicationInfo();
                        
            var clrVersion = typeof(int).Assembly.GetName().Version;
            currentClr = clrVersion.Major == 4 ? Clr.Net4 : Clr.Net2;
            currentBitness = IntPtr.Size == 8 ? Bitness.x64 : Bitness.x86;

            Shell = new Shell();
        }

        /// <summary>
        /// Gets the Hawkeye application shell.
        /// </summary>
        public static Shell Shell { get; private set; }

        /// <summary>
        /// Gets Hawkeye current CLR.
        /// </summary>
        public static Clr CurrentClr
        {
            get { return currentClr; }
        }

        /// <summary>
        /// Gets Hawkeye current bitness.
        /// </summary>
        public static Bitness CurrentBitness
        {
            get { return currentBitness; }
        }

        /// <summary>
        /// Runs the Hawkeye application.
        /// </summary>
        /// <remarks>
        /// Use this method to run Hawkeye in its own process.
        /// </remarks>
        public static void Run()
        {
            Shell.Run();
        }

        /// <summary>
        /// Runs the Hawkeye application.
        /// </summary>
        /// <param name="windowToSpy">The window to spy.</param>
        /// <param name="windowToKill">The window to kill.</param>
        /// <remarks>
        /// Use this method to run Hawkeye in its own process.
        /// </remarks>
        public static void Run(IntPtr windowToSpy, IntPtr windowToKill)
        {
            Shell.Run(windowToSpy, windowToKill);
        }

        /// <summary>
        /// Operations that should be realized before we close Hawkeye.
        /// </summary>
        public static void Close()
        {
            Shell.Close();
        }

        /// <summary>
        /// Determines whether Hawkeye can be injected given the specified window info.
        /// </summary>
        /// <param name="info">The window info.</param>
        /// <returns>
        ///   <c>true</c> if Hawkeye can be injected; otherwise, <c>false</c>.
        /// </returns>
        public static bool CanInject(IWindowInfo info)
        {
            return Shell.CanInject(info);
        }

        /// <summary>
        /// Injects the Hawkeye application into the process owning the specified window.
        /// </summary>
        /// <param name="info">The target window information.</param>
        public static void Inject(IWindowInfo info)
        {
            Shell.Inject(info);
        }

        /// <summary>
        /// Attaches the (injected) Hawkeye application to the specified target window
        /// (and destroys the original Hawkeye window).
        /// </summary>
        /// <param name="targetWindowHandle">The target window.</param>
        /// <param name="hawkeyeWindowHandle">The original Hawkeye window.</param>
        public static void Attach(IntPtr targetWindowHandle, IntPtr hawkeyeWindowHandle)
        {
            Shell.Attach(targetWindowHandle, hawkeyeWindowHandle);
        }
    }
}

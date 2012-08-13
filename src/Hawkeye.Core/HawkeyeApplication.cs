using System;
using Hawkeye.Logging;

namespace Hawkeye
{
    /// <summary>
    /// The root class representing the Hawkeye Application.
    /// </summary>
    internal static partial class HawkeyeApplication
    {
        /// <summary>
        /// Interface one must implement to provide real logic behind the <see cref="HawkeyeApplication"/>
        /// static facade.
        /// </summary>
        public interface IHawkeyeApplicationImplementation
        {
            /// <summary>
            /// Gets a value indicating whether the Hawkeye application is running injected in a 
            /// host process or in its original process.
            /// </summary>
            /// <value>
            /// 	<c>true</c> if the application is injected; otherwise, <c>false</c>.
            /// </value>
            bool IsInjected { get; }

            /// <summary>
            /// Runs the Hawkeye application.
            /// </summary>
            /// <remarks>
            /// Use this method to run Hawkeye in its own process.
            /// </remarks>
            void Run();
            
            /// <summary>
            /// Runs the Hawkeye application.
            /// </summary>
            /// <param name="windowToSpy">The window to spy.</param>
            /// <param name="windowToKill">The window to kill.</param>
            /// <remarks>
            /// Use this method to run Hawkeye in its own process.
            /// </remarks>
            void Run(IntPtr windowToSpy, IntPtr windowToKill);

            /// <summary>
            /// Determines whether Hawkeye can be injected given the specified window info.
            /// </summary>
            /// <param name="info">The window info.</param>
            /// <returns>
            ///   <c>true</c> if Hawkeye can be injected; otherwise, <c>false</c>.
            /// </returns>
            bool CanInject(IWindowInfo info);

            /// <summary>
            /// Injects the Hawkeye application into the process owning the specified window.
            /// </summary>
            /// <param name="info">The target window information.</param>
            void Inject(IWindowInfo info);

            /// <summary>
            /// Attaches the (injected) Hawkeye application to the specified target window 
            /// (and destroys the original Hawkeye window).
            /// </summary>
            /// <param name="targetWindow">The target window.</param>
            /// <param name="originalHawkeyeWindow">The original Hawkeye window.</param>
            void Attach(IntPtr targetWindow, IntPtr originalHawkeyeWindow);
            
            /// <summary>
            /// Gets the default log service factory.
            /// </summary>
            /// <returns>An instance of <see cref="ILogServiceFactory"/>.</returns>
            ILogServiceFactory GetLogServiceFactory();
        }

        private static readonly Bitness currentBitness;
        private static readonly Clr currentClr;
        private static readonly IHawkeyeApplicationImplementation implementation =
            new Implementation();
        
        /// <summary>
        /// Initializes the <see cref="HawkeyeApplication"/> class.
        /// </summary>
        static HawkeyeApplication()
        {
            currentBitness = IntPtr.Size == 8 ?
                Bitness.x64 : Bitness.x86;
            currentClr = typeof(int).Assembly.GetName().Version.Major == 4 ?
                Clr.Net4 : Clr.Net2;
        }

        /// <summary>
        /// Gets the default log service factory.
        /// </summary>
        public static ILogServiceFactory LogFactory
        {
            get { return implementation.GetLogServiceFactory(); }
        }
        
        /// <summary>
        /// Gets a value indicating whether the Hawkeye application is running injected in a 
        /// host process or in its original process.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the application is injected; otherwise, <c>false</c>.
        /// </value>
        public static bool IsInjected
        {
            get { return implementation.IsInjected; }
        }

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
            implementation.Run();
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
            implementation.Run(windowToSpy, windowToKill);
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
            return implementation.CanInject(info);
        }

        /// <summary>
        /// Injects the Hawkeye application into the process owning the specified window.
        /// </summary>
        /// <param name="info">The target window information.</param>
        public static void Inject(IWindowInfo info)
        {
            implementation.Inject(info);
        }

        /// <summary>
        /// Attaches the (injected) Hawkeye application to the specified target window
        /// (and destroys the original Hawkeye window).
        /// </summary>
        /// <param name="targetWindowHandle">The target window.</param>
        /// <param name="hawkeyeWindowHandle">The original Hawkeye window.</param>
        public static void Attach(IntPtr targetWindowHandle, IntPtr hawkeyeWindowHandle)
        {
            implementation.Attach(targetWindowHandle, hawkeyeWindowHandle);
        }
    }
}

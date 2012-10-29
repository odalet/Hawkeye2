using System;
using System.Windows.Forms;

using Hawkeye;
using System.Threading;
using System.Globalization;

namespace HawkeyeApplication
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Force American English
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");

            if (args.Length == 0)
                global::Hawkeye.HawkeyeApplication.Run();
            else
            {
                var windowHandle = (IntPtr)Int64.Parse(args[0]);
                var originalHandle = (IntPtr)Int64.Parse(args[1]);

                global::Hawkeye.HawkeyeApplication.Run(windowHandle, originalHandle);
            }
        }
    }
}

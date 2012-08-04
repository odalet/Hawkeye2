using System;
using System.Windows.Forms;

using Hawkeye;

namespace HawkeyeApplication
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            HawkeyeRunner.Run();
        }
    }
}

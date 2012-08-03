using System;
using System.Windows.Forms;

using Hawkeye;
using Hawkeye.Logging;

namespace HawkeyeApplication
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            This.InitializeApi(new ThisImplementation());

            // Can't initialize the logger before the API!
            var logService = LogManager.GetLogger(typeof(Program));
            logService.Debug("Running Hawkeye.");

            //Application.Run(new Form1());
            Application.Run(new Hawkeye.UI.MainForm());
            logService.Debug("Closing Hawkeye.");
        }
    }
}

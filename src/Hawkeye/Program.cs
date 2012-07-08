using System;
using System.Windows.Forms;
using System.Collections.Generic;

using Hawkeye;

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

            //Application.Run(new Form1());
            Application.Run(new Hawkeye.UI.MainForm());
        }
    }
}

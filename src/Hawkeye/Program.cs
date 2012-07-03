using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Hawkeye
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

            Application.Run(new Form1());
        }
    }
}

using System;
using System.Windows.Forms;

namespace Hawkeye.UI
{
    /// <summary>
    /// The application form.
    /// </summary>
    internal partial class MainForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            UpdateTitle();
        }

        public void SetTarget(IntPtr hwnd)
        {
            mainControl.SetTarget(hwnd);
        }

        private void UpdateTitle()
        {
            base.Text = string.Format("Hawkeye {0} - {1}", 
                HawkeyeApplication.CurrentClr, 
                HawkeyeApplication.CurrentBitness.ToString().ToLowerInvariant());
        }

        private void Inject(Clr clr, Bitness bitness)
        {
        }

        private void injectN2x86Button_Click(object sender, EventArgs e)
        {
            Inject(Clr.Net2, Bitness.x86);
        }

        private void injectN2X64Button_Click(object sender, EventArgs e)
        {
            Inject(Clr.Net2, Bitness.x64);
        }

        private void injectN4x86Button_Click(object sender, EventArgs e)
        {
            Inject(Clr.Net4, Bitness.x86);
        }

        private void injectN4x64Button_Click(object sender, EventArgs e)
        {
            Inject(Clr.Net4, Bitness.x64);
        }

        private static int logCount = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            logCount++;
            var l = Hawkeye.Logging.LogManager.GetLogger(GetType());
            Hawkeye.Logging.LoggingExtensions.Verbose(l, "TEST LOGGING: " + logCount);
        }
    }
}

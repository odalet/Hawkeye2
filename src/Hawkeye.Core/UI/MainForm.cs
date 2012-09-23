using System;
using System.Drawing;
using System.Windows.Forms;
using Hawkeye.WinApi;

namespace Hawkeye.UI
{
    /// <summary>
    /// The application form.
    /// </summary>
    internal partial class MainForm : Form
    {
        private IntPtr currentSpiedWindow = IntPtr.Zero;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            UpdateTitle();

            base.Parent = null;
        }

        /// <summary>
        /// Gets or sets this form's settings.
        /// </summary>
        public MainFormSettings Settings
        {
            get
            {
                return new MainFormSettings()
                {

                    SpiedWindow = mainControl.Target,
                    Location = Location,
                    Size = Size,
                    WindowState = WindowState,
                    Screen = Screen.FromControl(this)
                };
            }
            set
            {
                if (value == null) return;
                SetWindowSettings(value);
            }
        }

        public void SetTarget(IntPtr hwnd)
        {
            mainControl.Target = hwnd;
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

        private void SetWindowSettings(MainFormSettings settings)
        {
            //TODO: handle multiple-screens (and config changes!)
            Location = settings.Location;
            Size = settings.Size;
            WindowState = settings.WindowState;

            SetTarget(settings.SpiedWindow);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InformationBox.Show(this, "About Hawkeye!");
        }

        /// <summary>
        /// Handles this window messages
        /// </summary>
        /// <param name="m">The message.</param>
        protected override void WndProc(ref Message m)
        {
            bool eat = false;
            switch (m.Msg)
            {
                case (int)WindowMessages.WM_CANCELMODE:
                    // We eat this one; due to modal dialogs. See below
                    eat = true;
                    break;

                case (int)WindowMessages.WM_ENABLE:
                    var wparam = m.WParam.ToInt32();
                    if (wparam == 0)
                    {
                        // This means the window was disabled. 
                        // Whenever we are disabled, let's re-enable ourself
                        // This is needed if we want to be able to spy modal windows
                        NativeMethods.EnableWindow(Handle, true);
                        eat = true;
                    }
                    break;
            }
            
            if (!eat) base.WndProc(ref m);
        }
    }
}

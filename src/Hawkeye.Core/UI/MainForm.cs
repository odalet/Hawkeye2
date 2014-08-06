using System;
using System.Drawing;
using System.Windows.Forms;

using Hawkeye.WinApi;
using Hawkeye.Configuration;

namespace Hawkeye.UI
{
    /// <summary>
    /// The application form.
    /// </summary>
    internal partial class MainForm : Form, IDefaultLayoutProvider
    {
        private IntPtr currentSpiedWindow = IntPtr.Zero;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            UpdateTitle();
            LayoutManager.RegisterForm("HawkeyeMainForm", this);
            base.Parent = null;

            mainControl.CurrentInfoChanged += (s, e) =>
            {
                var info = mainControl.CurrentInfo;
                var name = string.Empty;
                if (info != null && info.ControlInfo != null)
                    name = info.ControlInfo.Name;
                UpdateTitle(name);
            };
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

        internal MainControl MainControl
        {
            get { return mainControl; }
        }

        public void SetTarget(IntPtr hwnd)
        {
            mainControl.Target = hwnd;
        }
        
        #region IDefaultLayoutProvider Members

        public Rectangle GetDefaultBounds()
        {
            var screenWorkingAreaSize = Screen.PrimaryScreen.WorkingArea.Size;
            var size = new Size(384, 476);
            if (size.Width > screenWorkingAreaSize.Width)
                size.Width = screenWorkingAreaSize.Width - 40;
            if (size.Height > screenWorkingAreaSize.Height)
                size.Height = screenWorkingAreaSize.Height - 40;

            var location = new Point(
                20,
                screenWorkingAreaSize.Height - size.Height - 20);

            return new Rectangle(location, size);
        }

        #endregion

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            HawkeyeApplication.Close();
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

        private void UpdateTitle(string controlName = "")
        {
            var clr = HawkeyeApplication.CurrentClr.GetLabel();
            var bitness = HawkeyeApplication.CurrentBitness.ToString().ToLowerInvariant();
            var clrAndBitness = string.IsNullOrEmpty(clr) ? bitness : clr + " " + bitness;

            var title = string.Format("Hawkeye {0}", clrAndBitness);
            if (string.IsNullOrEmpty(controlName))
                base.Text = title;
            else base.Text = string.Format("{0} - {1}", controlName, title);
        }

        private void SetWindowSettings(MainFormSettings settings)
        {
            //TODO: handle multiple-screens (and config changes!)
            Location = settings.Location;
            Size = settings.Size;
            WindowState = settings.WindowState;

            SetTarget(settings.SpiedWindow);
        }

        /// <summary>
        /// Handles the Click event of the aboutToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var about = new AboutForm())
                about.ShowDialog(this);
        }

        /// <summary>
        /// Handles the Click event of the exitToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

using System;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ScreenInfo
{
    public partial class MainForm : Form
    {
        #region System Menu

        // P/Invoke constants
        private const int WM_SYSCOMMAND = 0x112;
        private const int MF_STRING = 0x0;
        private const int MF_SEPARATOR = 0x800;

        // P/Invoke declarations
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool AppendMenu(IntPtr hMenu, int uFlags, int uIDNewItem, string lpNewItem);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InsertMenu(IntPtr hMenu, int uPosition, int uFlags, int uIDNewItem, string lpNewItem);

        // ID for the About item on the system menu
        private int SYSMENU_TRACKING_MENU_ID = 0x1;

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            // Get a handle to a copy of this form's system (window) menu
            IntPtr hSysMenu = GetSystemMenu(Handle, false);
            // Add a separator
            AppendMenu(hSysMenu, MF_SEPARATOR, 0, string.Empty);

            // Add the About menu item
            AppendMenu(hSysMenu, MF_STRING, SYSMENU_TRACKING_MENU_ID, "Switch Tracking");
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_SYSCOMMAND && (int)m.WParam == SYSMENU_TRACKING_MENU_ID)
            {
                tracking = !tracking;
                trackCheckBox.Checked = tracking;
                MessageBox.Show(this, "Tracking is now " + (tracking ? "on" : "off"));
            }
        }

        #endregion

        private bool tracking = true;
        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm" /> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DesignMode) return;

            tracking = trackCheckBox.Checked;
            FillScreensList();
        }

        /// <summary>
        /// Fills the screens list.
        /// </summary>
        private void FillScreensList()
        {
            lvScreens.Items.Clear();
            lvScreens.Items.AddRange(Screen.AllScreens.Select(s =>
            {
                var item = new ListViewItem(s.Primary ? "P" : string.Empty);
                item.SubItems.Add(s.DeviceName);
                item.SubItems.Add(s.BitsPerPixel.ToString());
                item.SubItems.Add(s.Bounds.ToString());
                item.SubItems.Add(s.WorkingArea.ToString());
                return item;
            }).ToArray());

        }

        private void UpdateTrackedInfo()
        {
            boundsTextBox.Text = Bounds.ToString();
            maximizedCheckBox.Checked = WindowState == FormWindowState.Maximized;
            minimizedCheckBox.Checked = WindowState == FormWindowState.Minimized;
            clientRectangleTextBox.Text = ClientRectangle.ToString();
            clientRectangleInScreenCoordinatesTextBox.Text = RectangleToScreen(ClientRectangle).ToString();
            
            currentScreenTextBox.Text = Screen.FromControl(this).DeviceName;            
        }

        /// <summary>
        /// Handles the Click event of the closeButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the Click event of the refreshScreensButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void refreshScreensButton_Click(object sender, EventArgs e)
        {
            FillScreensList();
        }

        /// <summary>
        /// Handles the CheckedChanged event of the trackCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void trackCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            tracking = trackCheckBox.Checked;
            if (tracking) 
                UpdateTrackedInfo();
        }

        /// <summary>
        /// Handles the Move event of the MainForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void MainForm_Move(object sender, EventArgs e)
        {
            if (tracking)
                UpdateTrackedInfo();
        }

        /// <summary>
        /// Handles the Resize event of the MainForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (tracking)
                UpdateTrackedInfo();
        }
    }
}

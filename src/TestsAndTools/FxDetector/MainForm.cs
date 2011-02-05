using System;
using System.IO;
using System.Windows.Forms;

namespace FxDetector
{
    /// <summary>
    /// Application's main form
    /// </summary>
    public partial class MainForm : Form
    {
        private WindowInfo currentInfo = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DesignMode) return;

            hwndBox.Text = Handle.ToString();
            OnCurrentInfoChanged();
        }

        private WindowInfo CurrentInfo
        {
            get { return currentInfo; }
            set
            {
                if (currentInfo == value) return;
                currentInfo = value;
                OnCurrentInfoChanged();
            }
        }

        private void OnCurrentInfoChanged()
        {
            pgrid.SelectedObject = CurrentInfo;
            pgrid.ExpandAllGridItems();

            dumpButton.Enabled = CurrentInfo != null;
        }

        private void Detect()
        {
            CurrentInfo = null;
            IntPtr hwnd = IntPtr.Zero;
            if (This.IsX64)
            {
                long windowHandle;
                if (!long.TryParse(hwndBox.Text, out windowHandle)) MessageBox.Show(
                    this, string.Format("{0} is not a valid window handle.", hwndBox.Text), "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                else hwnd = new IntPtr(windowHandle);
            }
            else
            {
                int windowHandle;
                if (!int.TryParse(hwndBox.Text, out windowHandle)) MessageBox.Show(
                    this, string.Format("{0} is not a valid window handle.", hwndBox.Text), "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                else hwnd = new IntPtr(windowHandle);
            }
            
            CurrentInfo = new WindowInfo(hwnd);
        }

        private void Dump()
        {
            if (CurrentInfo == null) throw new InvalidOperationException(
                "Can't dump if no window selected.");

            var filename = GetFileName();
            if (string.IsNullOrEmpty(filename)) return;

            try
            {
                var text = CurrentInfo.Dump();
                using (var writer = File.CreateText(filename))
                {
                    writer.Write(text);
                    writer.Flush();
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ex.ToString());
            }
        }

        private string GetFileName()
        {
            if (CurrentInfo == null) throw new InvalidOperationException(
                "Can't dump if no window selected.");

            using (var dialog = new SaveFileDialog()
            {
                //FileName = string.Format("dump_{0}.log", CurrentInfo.Hwnd),
                FileName = "dump.log",
                Filter = "Log files|*.log|All files|*.*"
            })
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                    return dialog.FileName;
                else return string.Empty;
            }
        }

        /// <summary>
        /// Handles the Click event of the detectButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void detectButton_Click(object sender, EventArgs e) { Detect(); }

        /// <summary>
        /// Handles the Click event of the dumpButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void dumpButton_Click(object sender, EventArgs e) { Dump(); }
    }
}

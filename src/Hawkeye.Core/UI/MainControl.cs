using System;
using System.IO;
using System.Windows.Forms;
using Hawkeye.WinApi;

namespace Hawkeye.UI
{
    public partial class MainControl : UserControl
    {
        private IWindowInfo currentInfo = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainControl"/> class.
        /// </summary>
        public MainControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.UserControl.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (DesignMode) return;

            hwndBox.Text = Handle.ToString();
            OnCurrentInfoChanged();

            windowFinderControl1.ActiveWindowChanged += (s, _) =>
                hwndBox.Text = windowFinderControl1.ActiveWindowHandle.ToString();
            windowFinderControl1.WindowSelected += (s, _) =>
            {
                var hwnd = windowFinderControl1.ActiveWindowHandle;
                if (hwnd == IntPtr.Zero) CurrentInfo = null;
                else CurrentInfo = new WindowInfo(hwnd);
            };
        }

        private IWindowInfo CurrentInfo
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

        private IWindowInfo CreateWindowInfo(IntPtr hwnd)
        {
            return This.GetWindowInfo(hwnd);
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

            CurrentInfo = CreateWindowInfo(hwnd);
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
                //ErrorBox.Show(ex.ToString());
                MessageBox.Show(ex.ToString());
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

        private void detectButton_Click(object sender, EventArgs e) { Detect(); }

        private void dumpButton_Click(object sender, EventArgs e) { Dump(); }
    }
}
